using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace LiteCommerce.Admin.Codes
{
    public class ConvertMD5
    {
        public static string GetMD5(string str)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            byte[] data = md5.ComputeHash(Encoding.Default.GetBytes(str));
            StringBuilder sb = new StringBuilder();
            for(int i=0; i< data.Length; i++)
            {
                sb.AppendFormat("{0:x2}", data[i]);
            }
            return sb.ToString();
        }
        public static bool IsMD5(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            else
            {
                return Regex.IsMatch(str, "^[0-9a-fA-F]{32}$", RegexOptions.Compiled);
            }
        }
    }
}