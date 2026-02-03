using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Lab_rab_4Kirichenko.Helper;
using Lab_rab_4Kirichenko.Model;
using Lab_rab_4Kirichenko.View;


namespace Lab_rab_4Kirichenko.ViewModel
{
    public class PersonViewModel : NotifyPropertyChanged
    {

        private static ObservableCollection<Person> ListPersonStatic { get; set; } = new ObservableCollection<Person>();


        private ObservableCollection<PersonDPO> listPersonDPO = new ObservableCollection<PersonDPO>();


        private PersonDPO selectedPersonDPO;

        public ObservableCollection<Role> ListRole { get; set; } = new RoleViewModel().ListRole;

        public ObservableCollection<PersonDPO> ListPersonDPO
        {
            get { return listPersonDPO; }
            set { listPersonDPO = value; OnPropertyChanged(nameof(ListPersonDPO)); }
        }

        public PersonDPO SelectedPersonDPO
        {
            get { return selectedPersonDPO; }
            set
            {
                selectedPersonDPO = value;
                OnPropertyChanged(nameof(SelectedPersonDPO));
                ((RelayCommand)EditPerson).RaiseCanExecuteChanged();
                ((RelayCommand)DeletePerson).RaiseCanExecuteChanged();
            }
        }


        public ICommand AddPerson { get; }
        public ICommand EditPerson { get; }
        public ICommand DeletePerson { get; }

        public PersonViewModel()
        {

            if (ListPersonStatic.Count == 0)
            {
                ListPersonStatic.Add(new Person { Id = 1, RoleId = 1, FirstName = "Иван", LastName = "Иванов", Birthday = new DateTime(1980, 02, 28) });
                ListPersonStatic.Add(new Person { Id = 2, RoleId = 2, FirstName = "Петр", LastName = "Петров", Birthday = new DateTime(1981, 03, 20) });
                ListPersonStatic.Add(new Person { Id = 3, RoleId = 3, FirstName = "Виктор", LastName = "Викторов", Birthday = new DateTime(1982, 04, 15) });
                ListPersonStatic.Add(new Person { Id = 4, RoleId = 3, FirstName = "Сидор", LastName = "Сидоров", Birthday = new DateTime(1983, 05, 10) });
            }


            UpdateListPersonDPO();


            AddPerson = new RelayCommand(OnAddPersonExecute);
            EditPerson = new RelayCommand(OnEditPersonExecute, OnEditDeleteCanExecute);
            DeletePerson = new RelayCommand(OnDeletePersonExecute, OnEditDeleteCanExecute);
        }


        private bool OnEditDeleteCanExecute(object arg)
        {
            return SelectedPersonDPO != null;
        }


        private void OnAddPersonExecute(object obj)
        {
            PersonDPO newPersonDPO = new PersonDPO();

            newPersonDPO.Id = ListPersonStatic.Any() ? ListPersonStatic.Max(p => p.Id) + 1 : 1;
            newPersonDPO.Birthday = DateTime.Now; 

            WindowPerson winPerson = new WindowPerson(newPersonDPO, ListRole);
            if (winPerson.ShowDialog() == true)
            {
                ListPersonStatic.Add(PersonDPO.DPOToPerson(newPersonDPO));
                UpdateListPersonDPO();
            }
        }


        private void OnEditPersonExecute(object obj)
        {

            PersonDPO personToEdit = SelectedPersonDPO.Clone(); 

            WindowPerson winPerson = new WindowPerson(personToEdit, ListRole);
            if (winPerson.ShowDialog() == true)
            {

                Person oldPerson = ListPersonStatic.FirstOrDefault(p => p.Id == personToEdit.Id);

                if (oldPerson != null)
                {

                    int index = ListPersonStatic.IndexOf(oldPerson);
                    ListPersonStatic.RemoveAt(index);
                    ListPersonStatic.Insert(index, PersonDPO.DPOToPerson(personToEdit));


                    UpdateListPersonDPO();
                    SelectedPersonDPO = ListPersonDPO.FirstOrDefault(p => p.Id == personToEdit.Id);
                }
            }
        }


        private void OnDeletePersonExecute(object obj)
        {
            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите удалить сотрудника {SelectedPersonDPO.LastName} {SelectedPersonDPO.FirstName}?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Person personToRemove = ListPersonStatic.FirstOrDefault(p => p.Id == SelectedPersonDPO.Id);
                if (personToRemove != null)
                {
                    ListPersonStatic.Remove(personToRemove);
                    UpdateListPersonDPO();
                }
            }
        }

 
        public void UpdateListPersonDPO()
        {

            ListPersonDPO.Clear();
            List<Role> roles = new List<Role>(ListRole);

            foreach (var p in ListPersonStatic)
            {
                Role rol = roles.FirstOrDefault(r => r.Id == p.RoleId);

                ListPersonDPO.Add(new PersonDPO
                {
                    Id = p.Id,
                    Role = rol != null ? rol.NameRole : "Неизвестно",
                    RoleId = p.RoleId,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Birthday = p.Birthday
                });
            }
        }
    }
}