using System;

namespace Lab_rab_1._1_Kirichenko
{
    public class Book
    {

        public string Name { get; }
        public int Pages { get; }
        public decimal Price { get; private set; }


        public Book(string name, int pages, decimal price)
        {
            Name = name;
            Pages = pages;
            Price = price;
        }


        public bool DoublePriceIfProgramming(string programmingKeyword = "Программирование")
        {
            if (Name.StartsWith(programmingKeyword, StringComparison.OrdinalIgnoreCase))
            {
                Price *= 2;
                return true;
            }
            return false;
        }

        public decimal CalculateAvgPageCost()
        {
            if (Pages == 0)
            {
                return 0;
            }
            return Price / Pages;
        }
    }
}