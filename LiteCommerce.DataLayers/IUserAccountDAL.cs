using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteCommerce.DataLayers
{
    public interface IUserAccountDAL
    {
        /// <summary>
        /// kiem tra thong tin dang nhap co hop le hay khong neu hop le tra ve thon tin cua user nguoc la ham tra ve null
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserAccount Authorize(string userName, string password);
    }
}
