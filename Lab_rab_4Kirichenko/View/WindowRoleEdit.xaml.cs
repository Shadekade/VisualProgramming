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
using System.Windows.Shapes;
using Lab_rab_4Kirichenko.Model;

namespace Lab_rab_4Kirichenko.View
{
    /// <summary>
    /// Логика взаимодействия для WindowRoleEdit.xaml
    /// </summary>
    public partial class WindowRoleEdit : Window
    {
        public WindowRoleEdit(Role role)
        {
            InitializeComponent();
            DataContext = role;
        }

        
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}