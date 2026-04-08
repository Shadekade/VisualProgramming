using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Lab_rab_Kirichenko_2
{
    public class SortResult
    {
        public int[] Array { get; set; } = new int[0]; // инициализирую пустым, чтобы избежать эррорки
        public long Comparisons { get; set; }
        public double Time { get; set; }
    }

    public class ArraySorter
    {
        private long _totalComparisons; // объект для лок, чтобы при одновременно
        private readonly object _lock = new object();

        public event Action<SortResult>? BubbleCompleted;
        public event Action<SortResult>? QuickCompleted;
        public event Action<SortResult>? InsertionCompleted;
        public event Action<SortResult>? ShakerCompleted;

        public long TotalComparisons => _totalComparisons;

        public void ResetTotal() { lock (_lock) _totalComparisons = 0; }

        public int[] Generate(int size)
        {
            var r = new Random();
            var arr = new int[size];
            for (int i = 0; i < size; i++) arr[i] = r.Next(0, 10000);
            return arr;
        }

        public void RunBubbleThread(int[] data, IProgress<double> progress, CancellationToken ct)
            => InvokeThreadSort(data, d => InternalBubbleSort(d, progress, ct), BubbleCompleted);

        public void RunQuickThread(int[] data, IProgress<double> progress, CancellationToken ct)
            => InvokeThreadSort(data, d => InternalQuickSort(d, progress, ct), QuickCompleted);

        public void RunInsertionThread(int[] data, IProgress<double> progress, CancellationToken ct)
            => InvokeThreadSort(data, d => InternalInsertionSort(d, progress, ct), InsertionCompleted);

        public void RunShakerThread(int[] data, IProgress<double> progress, CancellationToken ct)
            => InvokeThreadSort(data, d => InternalShakerSort(d, progress, ct), ShakerCompleted);

        private void InvokeThreadSort(int[] data, Func<int[], SortResult?> sortFunc, Action<SortResult>? completedEvent)
        {
            var res = sortFunc(data);
            if (res != null)
            {
                lock (_lock) { _totalComparisons += res.Comparisons; }
                completedEvent?.Invoke(res);
            }
        }

        public async Task<SortResult?> RunBubbleTaskAsync(int[] data, IProgress<double> progress, CancellationToken ct)
            => await RunTaskSort(data, d => InternalBubbleSort(d, progress, ct), ct);

        public async Task<SortResult?> RunQuickTaskAsync(int[] data, IProgress<double> progress, CancellationToken ct)
            => await RunTaskSort(data, d => InternalQuickSort(d, progress, ct), ct);

        public async Task<SortResult?> RunInsertionTaskAsync(int[] data, IProgress<double> progress, CancellationToken ct)
            => await RunTaskSort(data, d => InternalInsertionSort(d, progress, ct), ct);

        public async Task<SortResult?> RunShakerTaskAsync(int[] data, IProgress<double> progress, CancellationToken ct)
            => await RunTaskSort(data, d => InternalShakerSort(d, progress, ct), ct);

        private async Task<SortResult?> RunTaskSort(int[] data, Func<int[], SortResult?> sortFunc, CancellationToken ct)
        {
            return await Task.Run(() =>
            {
                var res = sortFunc(data);
                if (res != null)
                {
                    lock (_lock) { _totalComparisons += res.Comparisons; }
                }
                return res;
            }, ct);
        }

        private SortResult? InternalBubbleSort(int[] source, IProgress<double> progress, CancellationToken ct)
        {
            int[] arr = (int[])source.Clone();
            long count = 0;
            int n = arr.Length;
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < n - 1; i++)
            {
                if (ct.IsCancellationRequested) return null;
                if (i % 10 == 0) progress?.Report((double)i / n * 100);
                for (int j = 0; j < n - i - 1; j++)
                {
                    count++;
                    if (arr[j] > arr[j + 1]) (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                }
            }
            progress?.Report(100);
            return new SortResult { Array = arr, Comparisons = count, Time = sw.Elapsed.TotalMilliseconds };
        }

        private SortResult? InternalQuickSort(int[] source, IProgress<double> progress, CancellationToken ct)
        {
            int[] arr = (int[])source.Clone();
            long count = 0;
            var sw = Stopwatch.StartNew();
            QuickSortRecursive(arr, 0, arr.Length - 1, ref count, ct);
            if (ct.IsCancellationRequested) return null;
            progress?.Report(100);
            return new SortResult { Array = arr, Comparisons = count, Time = sw.Elapsed.TotalMilliseconds };
        }

        private void QuickSortRecursive(int[] a, int low, int high, ref long count, CancellationToken ct)
        {
            if (ct.IsCancellationRequested || low >= high) return;
            int p = Partition(a, low, high, ref count);
            QuickSortRecursive(a, low, p, ref count, ct);
            QuickSortRecursive(a, p + 1, high, ref count, ct);
        }

        private int Partition(int[] a, int low, int high, ref long count)
        {
            int pivot = a[(low + high) / 2];
            int i = low - 1; int j = high + 1;
            while (true)
            {
                do { i++; count++; } while (a[i] < pivot);
                do { j--; count++; } while (a[j] > pivot);
                if (i >= j) return j;
                (a[i], a[j]) = (a[j], a[i]);
            }
        }

        private SortResult? InternalInsertionSort(int[] source, IProgress<double> progress, CancellationToken ct)
        {
            int[] arr = (int[])source.Clone();
            long count = 0;
            int n = arr.Length;
            var sw = Stopwatch.StartNew();
            for (int i = 1; i < n; i++)
            {
                if (ct.IsCancellationRequested) return null;
                if (i % 10 == 0) progress?.Report((double)i / n * 100);
                int key = arr[i]; int j = i - 1;
                while (j >= 0 && arr[j] > key) { count++; arr[j + 1] = arr[j]; j--; }
                arr[j + 1] = key;
            }
            progress?.Report(100);
            return new SortResult { Array = arr, Comparisons = count, Time = sw.Elapsed.TotalMilliseconds };
        }

        private SortResult? InternalShakerSort(int[] source, IProgress<double> progress, CancellationToken ct)
        {
            int[] arr = (int[])source.Clone();
            long count = 0;
            var sw = Stopwatch.StartNew();
            int left = 0, right = arr.Length - 1, size = arr.Length;
            while (left < right)
            {
                if (ct.IsCancellationRequested) return null;
                bool swapped = false;
                for (int i = left; i < right; i++)
                {
                    count++;
                    if (arr[i] > arr[i + 1]) { (arr[i], arr[i + 1]) = (arr[i + 1], arr[i]); swapped = true; }
                }
                right--;
                if (!swapped) break;
                swapped = false;
                for (int i = right; i > left; i--)
                {
                    count++;
                    if (arr[i - 1] > arr[i]) { (arr[i - 1], arr[i]) = (arr[i], arr[i - 1]); swapped = true; }
                }
                left++;
                progress?.Report((double)(size - (right - left)) / size * 100);
                if (!swapped) break;
            }
            progress?.Report(100);
            return new SortResult { Array = arr, Comparisons = count, Time = sw.Elapsed.TotalMilliseconds };
        }
    }
}