using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPAA.Models.Business
{
    public class ChartInfo
    {
        public Dictionary<String, Country> countries = new Dictionary<String, Country>();

        public void addCountry(String code, Country c)
        {
            countries.Add(code, c);
        }
    }
}