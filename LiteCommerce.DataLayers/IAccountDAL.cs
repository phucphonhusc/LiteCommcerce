using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface IAccountDAL
    {
        bool UpdatePass(string password, string email);
        Account Get(string email);
        bool CheckLogin(string email, string password);
        Account GetPassByEmail(string email);
        bool UpdateProfile(Account data);
        bool CheckEmail(string email);
    }
}
