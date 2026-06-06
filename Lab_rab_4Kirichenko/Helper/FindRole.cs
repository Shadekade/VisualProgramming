using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab_rab_4Kirichenko.Model;

namespace Lab_rab_4Kirichenko.Helper
{
    public class FindRole
    {
        int id;

        public FindRole(int id)
        {
            this.id = id;
        }

        
        public bool RolePredicate(Role role)
        {
            return role.Id == id;
        }
    }
}
