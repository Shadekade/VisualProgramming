using System.Collections.ObjectModel;
using Lab_rab_4Kirichenko.Model;


namespace Lab_rab_4Kirichenko.ViewModel
{

    public class EditPersonViewModel : NotifyPropertyChanged
    {

        public PersonDPO EditedPerson { get; set; }


        public ObservableCollection<Role> ListRole { get; set; }

        public EditPersonViewModel(PersonDPO person, ObservableCollection<Role> roleList)
        {

            EditedPerson = person;


            ListRole = roleList;
        }
    }
}