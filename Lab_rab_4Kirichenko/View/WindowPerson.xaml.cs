using System.Windows;
using Lab_rab_4Kirichenko.Model;
using System.Collections.ObjectModel;
using Lab_rab_4Kirichenko.ViewModel;

namespace Lab_rab_4Kirichenko.View
{

    public partial class WindowPerson : Window
    {

        public WindowPerson(PersonDPO personDPO, ObservableCollection<Role> roles)
        {
            InitializeComponent();


            DataContext = personDPO;


            CmbRole.ItemsSource = roles;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {

            this.DialogResult = true;
        }
    }
}