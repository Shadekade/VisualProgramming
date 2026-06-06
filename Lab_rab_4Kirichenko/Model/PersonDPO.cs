using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_rab_4Kirichenko.ViewModel;

namespace Lab_rab_4Kirichenko.Model
{
    public class PersonDPO : NotifyPropertyChanged
    {

        private int id;
        private int roleId;
        private string role;
        private string firstName;
        private string lastName;
        private DateTime birthday;

        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(); }
        }


        public int RoleId
        {
            get => roleId;
            set { roleId = value; OnPropertyChanged(); }
        }


        public string Role
        {
            get => role;
            set { role = value; OnPropertyChanged(); }
        }

        public string FirstName
        {
            get => firstName;
            set { firstName = value; OnPropertyChanged(); }
        }

        public string LastName
        {
            get => lastName;
            set { lastName = value; OnPropertyChanged(); }
        }

        public DateTime Birthday
        {
            get => birthday;
            set { birthday = value; OnPropertyChanged(); }
        }


        public PersonDPO() { }

        public PersonDPO(int id, int roleId, string role, string firstName, string lastName, DateTime birthday)
        {
            this.Id = id;
            this.RoleId = roleId;
            this.Role = role;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Birthday = birthday;
        }


        public PersonDPO Clone()
        {
            return new PersonDPO
            {
                Id = this.Id,
                RoleId = this.RoleId,
                Role = this.Role,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Birthday = this.Birthday
            };
        }


        public static Person DPOToPerson(PersonDPO pDPO)
        {
            return new Person
            {
                Id = pDPO.Id,
                RoleId = pDPO.RoleId,
                FirstName = pDPO.FirstName,
                LastName = pDPO.LastName,
                Birthday = pDPO.Birthday
            };
        }
    }
}