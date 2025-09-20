using System;
using System.Windows;
using System.Windows.Input;

namespace Lab_rab_1._1_Kirichenko
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
           
            if (string.IsNullOrWhiteSpace(BookPagesTextBox.Text) || string.IsNullOrWhiteSpace(BookPriceTextBox.Text))
            {
                
                return;
            }
            string name = BookNameTextBox.Text;
            int pages = int.Parse(BookPagesTextBox.Text);
            decimal price = decimal.Parse(BookPriceTextBox.Text);
            bool isProgrammingBook = name.StartsWith("Программирование", StringComparison.OrdinalIgnoreCase);
            if (isProgrammingBook)
            {
                price *= 2;
            }
            decimal averagePageCost = (pages > 0) ? price / pages : 0;

            string result = "";
            if (isProgrammingBook)
            {
                result += $"Цена удвоена.\nНовая цена: {price:C}\n";
            }

            result += $"Средняя стоимость страницы: {averagePageCost:C}";

            ResultsTextBlock.Text = result;
        }
        private void BookPagesTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }
        private void BookPriceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1) && e.Text != "," && e.Text != ".")
            {
                e.Handled = true;
            }
        }
        private void BookPagesTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
        private void BookPriceTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}