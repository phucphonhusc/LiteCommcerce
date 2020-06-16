using LiteCommerce.BusinessLayers;
using LiteCommerce.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiteCommerce.Admin
{
    public static class SelectListHelper
    {
        public static List<Country> ListOfCountries()
        {
            List<Country> listCountries = CatalogBLL.Country_List();
            return listCountries;
        }
    }
}