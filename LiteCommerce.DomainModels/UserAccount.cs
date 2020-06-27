using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DomainModels
{
    public class UserAccount
    {
        public string UserID { get; set; }
        public string FullName { get; set; }
        public string Photo { get; set; }
        public string GroupName { get; set; }
    }
}
