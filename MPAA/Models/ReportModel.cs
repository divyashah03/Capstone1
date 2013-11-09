using MPAA.Models.Business;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MPAA.Models
{
    public class ReportModel
    {
        public LinkedList<String> countryName = new LinkedList<String>();
        public Dictionary<String,String> countries = new Dictionary<String,String>();
        public HashSet<String> piracyTypes = new HashSet<string>();
        public LinkedList<String> selectedPT { get; set; }
        public LinkedList<String> selectedCountries { get; set; }

        public ReportModel()
        {
            selectedPT = new LinkedList<string>();
            selectedCountries = new LinkedList<string>();
        }

        public void addCountry(String name, String code)
        {
            countryName.AddLast(name);
            countries.Add(name,code);
        }

        public void addPiracyTypes(Dictionary<String, Dictionary<String, long>>.KeyCollection pT)
        {
            
            foreach (String s in pT)
            {
                piracyTypes.Add(s);
                piracyTypes.Add(s.Split(':')[0]+" Total");
            }
        }
    }
}