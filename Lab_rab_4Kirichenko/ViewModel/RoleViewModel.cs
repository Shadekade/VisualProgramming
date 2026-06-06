using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Lab_rab_4Kirichenko.Helper;
using Lab_rab_4Kirichenko.Model;
using Lab_rab_4Kirichenko.View;
using System.IO;
using Newtonsoft.Json;

namespace Lab_rab_4Kirichenko.ViewModel
{
    public class RoleViewModel : NotifyPropertyChanged
    {
        private string path;

        private ObservableCollection<Role> listRole;
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
                if (EditRole is RelayCommand editCommand) editCommand.RaiseCanExecuteChanged();
                if (DeleteRole is RelayCommand deleteCommand) deleteCommand.RaiseCanExecuteChanged();
            }
        }

        public ICommand AddRole { get; }
        public ICommand EditRole { get; }
        public ICommand DeleteRole { get; }

        public RoleViewModel()
        {

            path = PathHelper.GetActualPath("RoleData.json");

            ListRole = LoadRole();

            AddRole = new RelayCommand(OnAddRoleExecute);
            EditRole = new RelayCommand(OnEditRoleExecute, OnEditDeleteCanExecute);
            DeleteRole = new RelayCommand(OnDeleteRoleExecute, OnEditDeleteCanExecute);
        }

        public ObservableCollection<Role> LoadRole()
        {
            try
            {
                if (!File.Exists(path)) return new ObservableCollection<Role>();

                string json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<ObservableCollection<Role>>(json) ?? new ObservableCollection<Role>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки ролей: {ex.Message}");
                return new ObservableCollection<Role>();
            }
        }

        private void SaveRole()
        {
            try
            {
                string json = JsonConvert.SerializeObject(ListRole, Formatting.Indented);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения ролей: {ex.Message}");
            }
        }

        private bool OnEditDeleteCanExecute(object arg) => SelectedRole != null;

        private void OnAddRoleExecute(object obj)
        {
            Role newRole = new Role();
            newRole.Id = ListRole.Any() ? ListRole.Max(r => r.Id) + 1 : 1;
            WindowRoleEdit winRole = new WindowRoleEdit(newRole);
            if (winRole.ShowDialog() == true)
            {
                ListRole.Add(newRole);
                SaveRole();
            }
        }

        private void OnEditRoleExecute(object obj)
        {
            Role roleToEdit = SelectedRole.Clone();
            WindowRoleEdit winRole = new WindowRoleEdit(roleToEdit);
            if (winRole.ShowDialog() == true)
            {
                Role oldRole = ListRole.FirstOrDefault(r => r.Id == roleToEdit.Id);
                if (oldRole != null)
                {
                    oldRole.NameRole = roleToEdit.NameRole;
                    SaveRole();
                    OnPropertyChanged(nameof(ListRole));
                }
            }
        }

        private void OnDeleteRoleExecute(object obj)
        {
            if (MessageBox.Show($"Удалить '{SelectedRole.NameRole}'?", "Удаление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ListRole.Remove(SelectedRole);
                SaveRole();
                SelectedRole = null;
            }
        }
    }
}