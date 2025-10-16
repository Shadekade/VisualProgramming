using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Lab_rab_2Kirichenko
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeComboBoxes();
            InitializeThemeComboBox();
            LoadImagesFromResources();
            ApplyTheme("Светлая");
        }

        private void InitializeThemeComboBox()
        {
            ThemeComboBox.ItemsSource = new List<string> { "Светлая", "Темная" };
            ThemeComboBox.SelectedIndex = 0;
        }

        private void LoadImagesFromResources()
        {
            try
            {

                string assemblyName = "Lab_rab_2Kirichenko";
                R1Image.Source = new BitmapImage(new Uri($"pack://application:,,/{assemblyName};component/Resources/formula1.png"));
                R2Image.Source = new BitmapImage(new Uri($"pack://application:,,/{assemblyName};component/Resources/formula2.png"));
                R3Image.Source = new BitmapImage(new Uri($"pack://application:,,/{assemblyName};component/Resources/formula3.png"));
                R4Image.Source = new BitmapImage(new Uri($"pack://application:,,/{assemblyName};component/Resources/formula4.png"));
                R5Image.Source = new BitmapImage(new Uri($"pack://application:,,/{assemblyName};component/Resources/formula5.png"));
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Ошибка загрузки изображений из ресурсов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitializeComboBoxes()
        {
            R1ComboF.ItemsSource = new List<double> { 4, 5, 6, 7, 8, 9 };
            R1ComboF.SelectedIndex = 0;

            R2ComboF.ItemsSource = new List<double> { 10, 20, 30, 40 };
            R2ComboF.SelectedIndex = 0;

            R3ComboC.ItemsSource = new List<double> { 0, 1 };
            R3ComboC.SelectedIndex = 0;
            R3ComboD.ItemsSource = new List<double> { -1, 0, 1 };
            R3ComboD.SelectedIndex = 0;

            R4ComboC.ItemsSource = new List<double> { 0, 1, 2, 3, 4, 5 };
            R4ComboC.SelectedIndex = 0;
        }

 
        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ThemeComboBox.SelectedItem is string selectedTheme)
            {
                ApplyTheme(selectedTheme);
            }
        }

        private void ApplyTheme(string themeName)
        {
            Style newStackPanelStyle = null;
            SolidColorBrush newBorderBrush;
            SolidColorBrush newForegroundBrush; 

            
            if (Application.Current.Resources.Contains($"{themeName}Style"))
            {
                newStackPanelStyle = (Style)Application.Current.Resources[$"{themeName}Style"];
            }

            if (themeName == "Темная")
            {
                
                ThisWindow.Background = new SolidColorBrush(Color.FromRgb(30, 30, 30)); 
                
                newBorderBrush = new SolidColorBrush(Color.FromRgb(68, 68, 68)); 
                
                newForegroundBrush = new SolidColorBrush(Colors.White); 
            }
            else 
            { 
                
                ThisWindow.Background = new SolidColorBrush(Colors.White);
                
                newBorderBrush = new SolidColorBrush(Color.FromRgb(204, 204, 204)); 

                newForegroundBrush = new SolidColorBrush(Colors.Black); 
            }


            if (newStackPanelStyle != null)
            {
                MainContentStackPanel.Style = newStackPanelStyle;
            }


            TextElement.SetForeground(MainContentStackPanel, newForegroundBrush);


            ApplyBorderColor(MainContentStackPanel, newBorderBrush);
        }


        private void ApplyBorderColor(DependencyObject parent, SolidColorBrush brush)
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is Border border)
                {
                    border.BorderBrush = brush;
                }

                ApplyBorderColor(child, brush);
            }
        }



        private int SafeParseInt(string text)
        {
            if (int.TryParse(text, out int result))
            {
                return result;
            }
            return 0;
        }

        private double SafeParseDouble(string text)
        {
            string normalizedText = text.Replace(',', '.');
            if (double.TryParse(normalizedText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double result))
            {
                return result;
            }
            return 0.0;
        }

        private void Calc_Click(object sender, RoutedEventArgs e)
        {
            double result = 0.0;
            TextBlock activeResultBlock = null;

            try
            {
                if (Radio1.IsChecked == true)
                {
                    activeResultBlock = R1ResultTextBlock;
                    double a = SafeParseDouble(R1TextA.Text);
                    if (R1ComboF.SelectedItem == null) throw new InvalidOperationException("Не выбран параметр 'f' для Формулы 1.");
                    double f = (double)R1ComboF.SelectedItem;
                    result = Formula1.Calculate(a, f);
                }
                else if (Radio2.IsChecked == true)
                {
                    activeResultBlock = R2ResultTextBlock;
                    double a = SafeParseDouble(R2TextA.Text);
                    double b = SafeParseDouble(R2TextB.Text);
                    if (R2ComboF.SelectedItem == null) throw new InvalidOperationException("Не выбран параметр 'f' для Формулы 2.");
                    double f = (double)R2ComboF.SelectedItem;
                    result = Formula2.Calculate(a, b, f);
                }
                else if (Radio3.IsChecked == true)
                {
                    activeResultBlock = R3ResultTextBlock;
                    double a = SafeParseDouble(R3TextA.Text);
                    double b = SafeParseDouble(R3TextB.Text);
                    if (R3ComboC.SelectedItem == null || R3ComboD.SelectedItem == null) throw new InvalidOperationException("Не выбраны параметры 'c' или 'd' для Формулы 3.");
                    double c = (double)R3ComboC.SelectedItem;
                    double d = (double)R3ComboD.SelectedItem;
                    result = Formula3.Calculate(a, b, c, d);
                }
                else if (Radio4.IsChecked == true)
                {
                    activeResultBlock = R4ResultTextBlock;
                    double a = SafeParseDouble(R4TextA.Text);
                    if (R4ComboC.SelectedItem == null) throw new InvalidOperationException("Не выбран параметр 'c' для Формулы 4.");
                    double c = (double)R4ComboC.SelectedItem;
                    int d_limit = (int)SafeParseDouble(R4TextD.Text);
                    result = Formula4.Calculate(a, c, d_limit);
                }
                else if (Radio5.IsChecked == true)
                {
                    activeResultBlock = R5ResultTextBlock;
                    double p = SafeParseDouble(R5TextP.Text);
                    double y = SafeParseDouble(R5TextY.Text);
                    int N = SafeParseInt(R5TextN.Text);
                    int K = SafeParseInt(R5TextK.Text);
                    result = Formula5.Calculate(p, y, N, K);
                }

                if (activeResultBlock != null)
                {
                    activeResultBlock.Text = $"Ответ: {result:F4}";
                }
            }
            catch (Exception ex)
            {
                if (activeResultBlock != null)
                {
                    activeResultBlock.Text = "Ответ: Ошибка!";
                }
                MessageBox.Show($"Ошибка вычисления: {ex.Message}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static class Formula1
        {
            public static double Calculate(double a, double f) => Math.Sin(f * a);
        }

        private static class Formula2
        {
            public static double Calculate(double a, double b, double f) => Math.Cos(f * a) + Math.Sin(f * b);
        }

        private static class Formula3
        {
            public static double Calculate(double a, double b, double c, double d) => c * Math.Pow(a, 2) + d * Math.Pow(b, 2);
        }

        private static class Formula4
        {
            public static double Calculate(double a, double c, int d_limit)
            {
                double sum = 0;
                for (int i = 0; i <= d_limit; i++)
                {
                    sum += Math.Pow(c + a, i);
                }
                return sum;
            }
        }

        private static class Formula5
        {
            public static double Calculate(double p, double y, int N, int K)
            {
                return p * y + N / (double)K;
            }
        }
    }
}
