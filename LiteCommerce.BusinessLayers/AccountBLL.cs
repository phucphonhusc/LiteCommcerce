using LiteCommerce.DataLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.BusinessLayers
{
    public static class AccountBLL
    {
        private static IAccountDAL AccountDB { get; set; }
        public static void Initialize(string connectionString)
        {
            AccountDB = new DataLayers.SqlServer.AccountDAL(connectionString);
        }
        public static Account Account_Get(string email)
        {
            return AccountDB.Get(email);
        }
        public static bool CheckLogin(string email, string password)
        {
            return AccountDB.CheckLogin(email, password);
        }
        public static bool UpdatePass(string password, string email)
        {
            return AccountDB.UpdatePass(password, email);
        }
        public static Account GetPassByEmail(string email)
        {
            return AccountDB.GetPassByEmail(email);
        }
        public static bool UpdateProfile(Account data)
        {
            return AccountDB.UpdateProfile(data);
        }
    }
}
