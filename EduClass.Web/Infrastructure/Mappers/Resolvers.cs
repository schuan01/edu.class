using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace EduClass.Web.Infrastructure.Mappers
{
    public class StringToDateTimeResolver : ValueResolver<string, DateTime>
    {
        protected override DateTime ResolveCore(string source)
        {
            try
            {

                object objDateTime = source;
                DateTime dateTime;

                if (objDateTime == null)
                {
                    return default(DateTime);
                }

                if (!source.Contains(':'))
                {
                    source += " 00:00";
                }

                dateTime = DateTime.ParseExact(source, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                return dateTime;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}