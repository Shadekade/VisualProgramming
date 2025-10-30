using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_rab_4Kirichenko.ViewModel;

namespace Lab_rab_4Kirichenko.Model
{
    public class Role : NotifyPropertyChanged
    {
        private int id;
        private string nameRole;

        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(); }
        }

        public string NameRole
        {
            get => nameRole;
            set { nameRole = value; OnPropertyChanged(); }
        }

        public Role() { }

        public Role(int id, string nameRole)
        {
            this.Id = id;
            this.NameRole = nameRole;
        }

        public Role Clone()
        {
            return new Role
            {
                Id = this.Id,
                NameRole = this.NameRole
            };
        }
        public override string ToString()
        {
            return NameRole;
        }
    }
}