using System;
using System.Collections.Generic;
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
    public class PersonViewModel : NotifyPropertyChanged
    {
        private string path;

        private static ObservableCollection<Person> ListPersonStatic { get; set; }
        private ObservableCollection<PersonDPO> listPersonDPO = new ObservableCollection<PersonDPO>();
        private PersonDPO selectedPersonDPO;

        public ObservableCollection<Role> ListRole { get; set; }

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
                if (EditPerson is RelayCommand editCommand) editCommand.RaiseCanExecuteChanged();
                if (DeletePerson is RelayCommand deleteCommand) deleteCommand.RaiseCanExecuteChanged();
            }
        }

        public ICommand AddPerson { get; }
        public ICommand EditPerson { get; }
        public ICommand DeletePerson { get; }

        public PersonViewModel()
        {

            ListRole = new RoleViewModel().ListRole;


            path = GetActualPath("PersonData.json");

            LoadPerson();
            UpdateListPersonDPO();

            AddPerson = new RelayCommand(OnAddPersonExecute);
            EditPerson = new RelayCommand(OnEditPersonExecute, OnEditDeleteCanExecute);
            DeletePerson = new RelayCommand(OnDeletePersonExecute, OnEditDeleteCanExecute);
        }


        private string GetActualPath(string fileName)
        {

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo dir = new DirectoryInfo(baseDir);


            while (dir != null && !dir.GetFiles("*.csproj").Any() && !dir.GetFiles("*.sln").Any())
            {
                dir = dir.Parent;
            }


            if (dir != null)
            {

                string projectFilePath = Path.Combine(dir.FullName, "Model", fileName);


                if (File.Exists(projectFilePath))
                {
                    return projectFilePath;
                }
            }


            return Path.Combine(baseDir, fileName);
        }

        private void LoadPerson()
        {
            try
            {
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    ListPersonStatic = JsonConvert.DeserializeObject<ObservableCollection<Person>>(json) ?? new ObservableCollection<Person>();
                }
                else
                {
                    ListPersonStatic = new ObservableCollection<Person>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки сотрудников: {ex.Message}");
                ListPersonStatic = new ObservableCollection<Person>();
            }
        }

        private void SavePerson()
        {
            try
            {
                string json = JsonConvert.SerializeObject(ListPersonStatic, Formatting.Indented);
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        private bool OnEditDeleteCanExecute(object arg) => SelectedPersonDPO != null;

        private void OnAddPersonExecute(object obj)
        {
            PersonDPO newPersonDPO = new PersonDPO { Birthday = DateTime.Now };
            newPersonDPO.Id = ListPersonStatic.Any() ? ListPersonStatic.Max(p => p.Id) + 1 : 1;

            WindowPerson winPerson = new WindowPerson(newPersonDPO, ListRole);
            if (winPerson.ShowDialog() == true)
            {
                ListPersonStatic.Add(PersonDPO.DPOToPerson(newPersonDPO));
                SavePerson();
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
                    SavePerson();
                    UpdateListPersonDPO();
                }
            }
        }

        private void OnDeletePersonExecute(object obj)
        {
            if (MessageBox.Show($"Удалить {SelectedPersonDPO.LastName}?", "Удаление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Person personToRemove = ListPersonStatic.FirstOrDefault(p => p.Id == SelectedPersonDPO.Id);
                if (personToRemove != null)
                {
                    ListPersonStatic.Remove(personToRemove);
                    SavePerson();
                    UpdateListPersonDPO();
                }
            }
        }

        public void UpdateListPersonDPO()
        {
            ListPersonDPO.Clear();
            foreach (var p in ListPersonStatic)
            {
                Role rol = ListRole.FirstOrDefault(r => r.Id == p.RoleId);
                ListPersonDPO.Add(new PersonDPO
                {
                    Id = p.Id,
                    Role = rol != null ? rol.NameRole : "Не задано",
                    RoleId = p.RoleId,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Birthday = p.Birthday
                });
            }
        }
    }
}