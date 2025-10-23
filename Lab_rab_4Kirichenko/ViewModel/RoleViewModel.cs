using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_rab_4Kirichenko.Model;

namespace Lab_rab_4Kirichenko.ViewModel
{
    public class RoleViewModel
    {
        public ObservableCollection<Role> ListRole { get; set; } = new ObservableCollection<Role>();

        public RoleViewModel()
        {
            this.ListRole.Add(new Role { Id = 1, NameRole = "Директор" });
            this.ListRole.Add(new Role { Id = 2, NameRole = "Бухгалтер" });
            this.ListRole.Add(new Role { Id = 3, NameRole = "Менеджер" });
        }
    }
}