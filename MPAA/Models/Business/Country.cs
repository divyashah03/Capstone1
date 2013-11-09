using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPAA.Models.Business
{
    public class Country
    {
        public String countryName;
        public Dictionary<String, long> countryTotalPerDate = new Dictionary<string, long>();
        public LinkedList<String> piracyType = new LinkedList<string>();
        public Dictionary<String, PiracyType> piracyTypeForCountry = new Dictionary<String, PiracyType>();

        public Country(String name)
        {
            countryName = name;
        }

        public void addPiracyTypeMap(String ptName, Dictionary<String, long> datePTMapping)
        {
            piracyType.AddLast(ptName);
            piracyTypeForCountry.Add(ptName, new PiracyType(ptName, datePTMapping));
            populateCountryTotalPerDate(datePTMapping);

            //populateTotalPerPiracyType(ptName.Split(':')[0]+" Total", datePTMapping);
        }

        private void populateTotalPerPiracyType(String ptTotal, Dictionary<String, long> datePTMapping)
        {
            if (piracyTypeForCountry.ContainsKey(ptTotal))
            {
                PiracyType pt;
                piracyTypeForCountry.TryGetValue(ptTotal, out pt);

            }
            else
            {
                PiracyType pt = new PiracyType(ptTotal, datePTMapping);
            }

        }

        private void populateCountryTotalPerDate(Dictionary<String, long> datePTMapping)
        {
            foreach (KeyValuePair<String, long> dateValuePair in datePTMapping)
            {
                String date = dateValuePair.Key;
                long total=0;
                countryTotalPerDate.TryGetValue(date, out total);
                countryTotalPerDate[date] = total + dateValuePair.Value;
            }
            
        }

        public class PiracyType
        {
            String piracyType;
            Dictionary<String, long> piracyPerDate { get; set; }

            public PiracyType(String name, Dictionary<String, long> datePTMapping)
            {
                piracyType = name;
                piracyPerDate = datePTMapping;
            }
        }
    }
}