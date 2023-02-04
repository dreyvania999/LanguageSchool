using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageSchool
{
    public partial class Client
    {
        public string FullName
        {
            get
            {
                string fullName = FirstName + " " + LastName + " " + Patronymic;
                return fullName;
            }
        }
    }
}
