using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace LiteCommerce.Admin.Codes
{
    public static class AppSetting
    {
        public static int DefaultPageSize
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["DefaultPageSize"]);
            }
        }
    }
}