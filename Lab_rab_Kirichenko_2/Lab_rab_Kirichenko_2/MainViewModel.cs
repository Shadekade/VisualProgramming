using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lab_rab_Kirichenko_2
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly ArraySorter _sorter = new ArraySorter();
        private readonly SynchronizationContext? _uiContext = SynchronizationContext.Current; // запоминает главный поток, чтобы фоновые могли его обновлять
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private SemaphoreSlim? _semaphore;
        private int[]? _originalArray;

        // [ObservableProperty] автоматически создает публичное свойство с вызовом OnPropertyChanged, чтобы интерфейс реагировал на изменения.
        [ObservableProperty] private int _arraySize = 1000;
        [ObservableProperty] private int _maxThreads = 2;
        [ObservableProperty] private bool _useSharedArray = false;
        [ObservableProperty] private bool _useTasks = true;

        [ObservableProperty] private string _originalArrayString = "Массив не сгенерирован";
        [ObservableProperty] private string _bubbleSortResult = string.Empty;
        [ObservableProperty] private string _quickSortResult = string.Empty;
        [ObservableProperty] private string _insertionSortResult = string.Empty;
        [ObservableProperty] private string _shakerSortResult = string.Empty;
        [ObservableProperty] private string _totalComparisons = "Общее число сравнений: 0";

        [ObservableProperty] private double _bubbleProgress;
        [ObservableProperty] private double _quickProgress;
        [ObservableProperty] private double _insertionProgress;
        [ObservableProperty] private double _shakerProgress;

        public MainViewModel()
        {
            _sorter.BubbleCompleted += (res) => UpdateFromThread(res, "Bubble");
            _sorter.QuickCompleted += (res) => UpdateFromThread(res, "Quick");
            _sorter.InsertionCompleted += (res) => UpdateFromThread(res, "Insertion");
            _sorter.ShakerCompleted += (res) => UpdateFromThread(res, "Shaker");
        }

        [RelayCommand]
        private void GenerateArray()
        {
            ResetAllInternal();
            _originalArray = _sorter.Generate(ArraySize);
            _sorter.ResetTotal();
            OriginalArrayString = $"Сгенерирован массив из {ArraySize} элементов";
            ClearResults();
            UpdateTotal();
        }

        [RelayCommand]
        private void AbortAll()
        {
            ResetAllInternal();
            BubbleSortResult = QuickSortResult = InsertionSortResult = ShakerSortResult = "Прервано";
        }

        private void ResetAllInternal()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();

            
            _semaphore?.Dispose(); // сбрасываем симафор, чтобы при изменении maxthreads в интерфейсе при следующем запуске он создавался заново
            _semaphore = null;

            BubbleProgress = QuickProgress = InsertionProgress = ShakerProgress = 0;
        }

        [RelayCommand] private async Task BubbleSort() => await ExecuteSort("Bubble");
        [RelayCommand] private async Task QuickSort() => await ExecuteSort("Quick");
        [RelayCommand] private async Task InsertionSort() => await ExecuteSort("Insertion");
        [RelayCommand] private async Task ShakerSort() => await ExecuteSort("Shaker");

        private async Task ExecuteSort(string type)
        {
            if (_originalArray == null) return;

            
            if (_semaphore == null) _semaphore = new SemaphoreSlim(MaxThreads);

            var currentToken = _cts.Token;
            var progress = new Progress<double>(val => {
                if (!currentToken.IsCancellationRequested)
                {
                    _uiContext?.Post(_ => { 
                        switch (type)
                        {
                            case "Bubble": BubbleProgress = val; break;
                            case "Quick": QuickProgress = val; break;
                            case "Insertion": InsertionProgress = val; break;
                            case "Shaker": ShakerProgress = val; break;
                        }
                    }, null);
                }
            });

            SetStatus(type, "В очереди...");

            try
            {
                await _semaphore.WaitAsync(currentToken);
            }
            catch (OperationCanceledException)
            {
                SetStatus(type, "Отменено");
                return;
            }

            
            int[] dataToSort = UseSharedArray ? _originalArray : (int[])_originalArray.Clone();

            if (UseTasks)
            {
                _ = Task.Run(async () => {
                    try
                    {
                        SetStatus(type, "Сортировка...");
                        SortResult? res = type switch
                        {
                            "Bubble" => await _sorter.RunBubbleTaskAsync(dataToSort, progress, currentToken),
                            "Quick" => await _sorter.RunQuickTaskAsync(dataToSort, progress, currentToken),
                            "Insertion" => await _sorter.RunInsertionTaskAsync(dataToSort, progress, currentToken),
                            "Shaker" => await _sorter.RunShakerTaskAsync(dataToSort, progress, currentToken),
                            _ => null
                        };
                        if (res != null && !currentToken.IsCancellationRequested) UpdateUI(res, type);
                        else SetStatus(type, "Прервано");
                    }
                    finally { _semaphore?.Release(); }
                }, currentToken);
            }
            else
            {
                new Thread(() => {
                    try
                    {
                        SetStatus(type, "Сортировка...");
                        switch (type)
                        {
                            case "Bubble": _sorter.RunBubbleThread(dataToSort, progress, currentToken); break;
                            case "Quick": _sorter.RunQuickThread(dataToSort, progress, currentToken); break;
                            case "Insertion": _sorter.RunInsertionThread(dataToSort, progress, currentToken); break;
                            case "Shaker": _sorter.RunShakerThread(dataToSort, progress, currentToken); break;
                        }
                        if (currentToken.IsCancellationRequested) SetStatus(type, "Прервано");
                    }
                    finally { _semaphore?.Release(); }
                }).Start();
            }
        }

        private void UpdateFromThread(SortResult? res, string type)
        {
            if (res == null) return;
            _uiContext?.Post(_ => UpdateUI(res, type), null);
        }

        private void UpdateUI(SortResult res, string type)
        {
            string info = $"{res.Time:F2} мс | Сравн: {res.Comparisons}";
            switch (type)
            {
                case "Bubble": BubbleSortResult = info; BubbleProgress = 100; break;
                case "Quick": QuickSortResult = info; QuickProgress = 100; break;
                case "Insertion": InsertionSortResult = info; InsertionProgress = 100; break;
                case "Shaker": ShakerSortResult = info; ShakerProgress = 100; break;
            }
            UpdateTotal();
        }

        private void SetStatus(string type, string status)
        {
            _uiContext?.Post(_ => { // отправка задачи в главную очередь потока
                switch (type)
                {
                    case "Bubble": BubbleSortResult = status; break;
                    case "Quick": QuickSortResult = status; break;
                    case "Insertion": InsertionSortResult = status; break;
                    case "Shaker": ShakerSortResult = status; break;
                }
            }, null);
        }

        private void ClearResults()
        {
            BubbleSortResult = QuickSortResult = InsertionSortResult = ShakerSortResult = string.Empty;
            BubbleProgress = QuickProgress = InsertionProgress = ShakerProgress = 0;
        }

        private void UpdateTotal() => TotalComparisons = $"Общее число сравнений: {_sorter.TotalComparisons}";
    }
}