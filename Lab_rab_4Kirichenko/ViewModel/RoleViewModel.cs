using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_rab_4Kirichenko.Helper;
using System.Windows.Input;
using System.Windows;
using Lab_rab_4Kirichenko.Model;
using Lab_rab_4Kirichenko.View;
using Lab_rab_4Kirichenko.ViewModel;

namespace Lab_rab_4Kirichenko.ViewModel
{

    public class RoleViewModel : NotifyPropertyChanged
    {

        private static ObservableCollection<Role> ListRoleStatic { get; set; } = new ObservableCollection<Role>();

        private ObservableCollection<Role> listRole = ListRoleStatic;
        public ObservableCollection<Role> ListRole
        {
            get { return listRole; }
            set { listRole = value; OnPropertyChanged(nameof(ListRole)); }
        }


        private Role selectedRole;

        public Role SelectedRole
        {
            get { return selectedRole; }
            set
            {
                selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));


                if (EditRole is RelayCommand editCommand)
                {
                    editCommand.RaiseCanExecuteChanged();
                }
                if (DeleteRole is RelayCommand deleteCommand)
                {
                    deleteCommand.RaiseCanExecuteChanged();
                }
            }
        }


        public ICommand AddRole { get; }
        public ICommand EditRole { get; }
        public ICommand DeleteRole { get; }

        public RoleViewModel()
        {

            if (ListRoleStatic.Count == 0)
            {
                ListRoleStatic.Add(new Role { Id = 1, NameRole = "Директор" });
                ListRoleStatic.Add(new Role { Id = 2, NameRole = "Бухгалтер" });
                ListRoleStatic.Add(new Role { Id = 3, NameRole = "Менеджер" });
            }


            AddRole = new RelayCommand(OnAddRoleExecute);
            EditRole = new RelayCommand(OnEditRoleExecute, OnEditDeleteCanExecute);
            DeleteRole = new RelayCommand(OnDeleteRoleExecute, OnEditDeleteCanExecute);
        }

  
        private bool OnEditDeleteCanExecute(object arg)
        {
            
            return SelectedRole != null;
        }

   
        private void OnAddRoleExecute(object obj)
        {
            Role newRole = new Role();
          
            newRole.Id = ListRoleStatic.Any() ? ListRoleStatic.Max(r => r.Id) + 1 : 1;

           
            WindowRoleEdit winRole = new WindowRoleEdit(newRole);
            if (winRole.ShowDialog() == true)
            {
                ListRoleStatic.Add(newRole);
            }
        }


        private void OnEditRoleExecute(object obj)
        {
            
            Role roleToEdit = SelectedRole.Clone();

            WindowRoleEdit winRole = new WindowRoleEdit(roleToEdit);
            if (winRole.ShowDialog() == true)
            {
       
                Role oldRole = ListRoleStatic.FirstOrDefault(r => r.Id == roleToEdit.Id);

                if (oldRole != null)
                {
                    
                    oldRole.NameRole = roleToEdit.NameRole;
                }
            }
        }


        private void OnDeleteRoleExecute(object obj)
        {
            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите удалить должность '{SelectedRole.NameRole}'?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                ListRoleStatic.Remove(SelectedRole);
                SelectedRole = null; 
            }
        }
    }
}