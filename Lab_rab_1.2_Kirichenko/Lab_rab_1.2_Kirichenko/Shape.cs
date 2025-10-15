using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_rab_1._2_Kirichenko
{
    
    public abstract class Shape
    {
        public abstract double CalculateArea();
        public abstract double CalculatePerimeter();
    }

    
    public class Circle : Shape
    {
        public double Radius { get; set; }

        public Circle(double radius)
        {
            if (radius <= 0) throw new ArgumentException("Радиус должен быть положительным числом.");
            Radius = radius;
        }

        public override double CalculateArea() => Math.PI * Radius * Radius;
        public override double CalculatePerimeter() => 2 * Math.PI * Radius;
    }

    
    public class Rectangle : Shape
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public Rectangle(double width, double height)
        {
            if (width <= 0 || height <= 0) throw new ArgumentException("Ширина и высота должны быть положительными числами.");
            Width = width;
            Height = height;
        }

        public override double CalculateArea() => Width * Height;
        public override double CalculatePerimeter() => 2 * (Width + Height);
    }

    
    public class Triangle : Shape
    {
        public double SideA { get; set; }
        public double SideB { get; set; }
        public double SideC { get; set; }

        public Triangle(double a, double b, double c)
        {
            if (a <= 0 || b <= 0 || c <= 0) throw new ArgumentException("Все стороны должны быть положительными числами.");
            
            if (a + b <= c || a + c <= b || b + c <= a)
            {
                throw new ArgumentException("Невозможно создать треугольник с заданными длинами сторон.");
            }
            SideA = a;
            SideB = b;
            SideC = c;
        }

        public override double CalculatePerimeter() => SideA + SideB + SideC;

        public override double CalculateArea()
        {
            double s = CalculatePerimeter() / 2; 
            return Math.Sqrt(s * (s - SideA) * (s - SideB) * (s - SideC));
        }
    }
}