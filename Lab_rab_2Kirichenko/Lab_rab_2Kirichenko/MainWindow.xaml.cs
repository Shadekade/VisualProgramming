using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab_rab_2Kirichenko
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeComboBoxes();
            LoadImagesFromResources();
        }

        private void LoadImagesFromResources()
        {
            try
            {
                R1Image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/formula1.png"));
                R2Image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/formula2.png"));
                R3Image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/formula3.png"));
                R4Image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/formula4.png"));
                R5Image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/formula5.png"));
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
    }
}