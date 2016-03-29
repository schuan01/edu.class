using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace EduClass.Web.Infrastructure.Helpers
{
    public class StringHelper
    {
        public static int ReturnNumbers(string text) 
        {
            int num; 
            var number = Regex.Replace(text, @"\D", String.Empty, RegexOptions.None);

            if (Int32.TryParse(number, out num))
	        {
		        return num;
	        }
            else
	        {
                throw new FormatException("No hay números");
	        }
        }
    }
}