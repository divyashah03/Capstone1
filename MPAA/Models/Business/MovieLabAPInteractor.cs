using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;



namespace MPAA.Models.Business
{
    public class MovieLabAPInteractor
    {
        public static void populateCountryList(ReportModel model)
        {
            var reader = new StreamReader(File.OpenRead(HttpContext.Current.Server.MapPath("/Content/countrycodes.csv")));
            // new DataContractJsonSerializer();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                model.addCountry(values[0], values[1]);
            }
        }

        public static void populatePiracyTypeInformation(String countryCode, ReportModel model, ChartInfo cinfo)
        {
            if(cinfo.countries.ContainsKey(countryCode))
            {
                return;
            }

            String cc = countryCode;

            if (countryCode == "WORLD")
            {
                cc = "";
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://mlmap.com/api/charts.php?data=serieskind&country="+cc+"&split=subkind");
            string credentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("cmu:d9SS7eu2"));
            request.Headers.Add("Authorization", "Basic " + credentials);
            request.AddRange("bytes", 1024);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string jsonResponse = string.Empty;
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                jsonResponse = sr.ReadToEnd();
            }
            Regex rgx = new Regex("\"data\":({.*?}})");
            MatchCollection matches = rgx.Matches(jsonResponse);
            if (matches.Count > 0)
            {
                jsonResponse = matches[0].Groups[1].Value;
            }
            
            var serializer = new JavaScriptSerializer();
            Dictionary<String, Dictionary<String, long>> dict1 = serializer.Deserialize<Dictionary<String, Dictionary<String, long>>>(jsonResponse);
            Dictionary<String, Dictionary<String, long>>.KeyCollection keys1 = dict1.Keys;
            Dictionary<String, long> piracyDates = new Dictionary<String, long>();

            Country c = new Country(countryCode);

            foreach (string piracyt in keys1)
            {
                dict1.TryGetValue(piracyt, out piracyDates);
                c.addPiracyTypeMap(piracyt, piracyDates);
            }

            cinfo.addCountry(countryCode, c);
            
            if (model.piracyTypes.Count==0)
            {
                model.addPiracyTypes(keys1);
            }
        }
    }
}