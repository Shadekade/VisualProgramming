using System;
using System.Collections.Generic;
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

namespace Lab_rab_1._2_Kirichenko
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        private Circle GetCircle(TextBlock errorBlock)
        {
            if (string.IsNullOrWhiteSpace(CircleRadiusTextBox.Text))
            {
                errorBlock.Text = "Введите радиус.";
                return null;
            }

            try
            {
                double radius = double.Parse(CircleRadiusTextBox.Text.Replace('.', ','));
                return new Circle(radius);
            }
            catch (ArgumentException ex)
            {
                errorBlock.Text = $"Ошибка: {ex.Message}";
                return null;
            }
            catch (FormatException)
            {
                errorBlock.Text = "Ошибка ввода: Введите корректное число.";
                return null;
            }
        }

        
        private void Circle_CalculateArea_Click(object sender, RoutedEventArgs e)
        {
            Circle circle = GetCircle(CircleAreaResultsTextBlock);
            if (circle != null)
            {
                CircleAreaResultsTextBlock.Text = $"Площадь круга: {circle.CalculateArea():F3}";
            }
            else
            {

            }
        }

        private void Circle_CalculatePerimeter_Click(object sender, RoutedEventArgs e)
        {
            Circle circle = GetCircle(CirclePerimeterResultsTextBlock);
            if (circle != null)
            {
                CirclePerimeterResultsTextBlock.Text = $"Периметр (длина окружности): {circle.CalculatePerimeter():F3}";
            }
            else
            {
                
            }
        }

        
        private Rectangle GetRectangle(TextBlock errorBlock)
        {
            if (string.IsNullOrWhiteSpace(RectWidthTextBox.Text) || string.IsNullOrWhiteSpace(RectHeightTextBox.Text))
            {
                errorBlock.Text = "Введите ширину и высоту.";
                return null;
            }

            try
            {
                double width = double.Parse(RectWidthTextBox.Text.Replace('.', ','));
                double height = double.Parse(RectHeightTextBox.Text.Replace('.', ','));
                return new Rectangle(width, height);
            }
            catch (ArgumentException ex)
            {
                errorBlock.Text = $"Ошибка: {ex.Message}";
                return null;
            }
            catch (FormatException)
            {
                errorBlock.Text = "Ошибка ввода: Введите корректные числа.";
                return null;
            }
        }

        
        private void Rectangle_CalculateArea_Click(object sender, RoutedEventArgs e)
        {
            Rectangle rectangle = GetRectangle(RectangleAreaResultsTextBlock);
            if (rectangle != null)
            {
                RectangleAreaResultsTextBlock.Text = $"Площадь прямоугольника: {rectangle.CalculateArea():F3}";
            }
        }

        private void Rectangle_CalculatePerimeter_Click(object sender, RoutedEventArgs e)
        {
            Rectangle rectangle = GetRectangle(RectanglePerimeterResultsTextBlock);
            if (rectangle != null)
            {
                RectanglePerimeterResultsTextBlock.Text = $"Периметр: {rectangle.CalculatePerimeter():F3}";
            }
        }

        
        private Triangle GetTriangle(TextBlock errorBlock)
        {
            if (string.IsNullOrWhiteSpace(TriSideATextBox.Text) || string.IsNullOrWhiteSpace(TriSideBTextBox.Text) || string.IsNullOrWhiteSpace(TriSideCTextBox.Text))
            {
                errorBlock.Text = "Введите длины всех трех сторон.";
                return null;
            }

            try
            {
                double a = double.Parse(TriSideATextBox.Text.Replace('.', ','));
                double b = double.Parse(TriSideBTextBox.Text.Replace('.', ','));
                double c = double.Parse(TriSideCTextBox.Text.Replace('.', ','));
                return new Triangle(a, b, c);
            }
            catch (ArgumentException ex)
            {
                
                errorBlock.Text = $"Ошибка: {ex.Message}";
                return null;
            }
            catch (FormatException)
            {
                errorBlock.Text = "Ошибка ввода: Введите корректные числа.";
                return null;
            }
        }

        
        private void Triangle_CalculateArea_Click(object sender, RoutedEventArgs e)
        {
            Triangle triangle = GetTriangle(TriangleAreaResultsTextBlock);
            if (triangle != null)
            {
                TriangleAreaResultsTextBlock.Text = $"Площадь треугольника: {triangle.CalculateArea():F3}";
            }
        }

        private void Triangle_CalculatePerimeter_Click(object sender, RoutedEventArgs e)
        {
            Triangle triangle = GetTriangle(TrianglePerimeterResultsTextBlock);
            if (triangle != null)
            {
                TrianglePerimeterResultsTextBlock.Text = $"Периметр: {triangle.CalculatePerimeter():F3}";
            }
        }

        

        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1) && e.Text != "," && e.Text != "." && e.Text != "-")
            {
                e.Handled = true;
            }
            if ((e.Text == "," || e.Text == ".") && ((TextBox)sender).Text.Contains(e.Text == "," ? "," : "."))
            {
                e.Handled = true;
            }
        }

        private void Number_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}