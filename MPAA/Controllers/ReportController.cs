using MPAA.Models;
using MPAA.Models.Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MPAA.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/

        public ActionResult Index()
        {
            ReportModel model;
            ChartInfo chartInfo;

            if (Session["model"] == null)
            {
                model = new ReportModel();
                chartInfo = new ChartInfo();
                MovieLabAPInteractor.populateCountryList(model);
                MovieLabAPInteractor.populatePiracyTypeInformation("US", model, chartInfo);
                Session["model"] = model;
                Session["ChartInfo"] = chartInfo;
            }
            else
            {
                model = (ReportModel)Session["model"];
                model.selectedCountries.Clear();
                model.selectedPT.Clear();
            }
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ReportModel model)
        {
            ChartInfo chartInfo;

            if (Session["ChartInfo"] == null)
            {
                chartInfo = new ChartInfo();
                Session["ChartInfo"] = chartInfo;
            }
            else
            {
                chartInfo = (ChartInfo)Session["ChartInfo"];
            }

            ReportModel rm = (ReportModel)Session["model"];
            rm.selectedCountries.Clear();

            foreach(String country in model.selectedCountries){
                String countryCode;
                rm.countries.TryGetValue(country, out countryCode);
                rm.selectedCountries.AddLast(country);
                MovieLabAPInteractor.populatePiracyTypeInformation(countryCode, rm, chartInfo);
            }
            
            Session["model"] = rm;
            return View(rm);
        }

        public ActionResult GetPiracyTypes(String country)
        {
            ReportModel model;
            ChartInfo chartInfo;

            if (Session["model"] == null)
            {
                model = new ReportModel();
            }
            else
            {
                model = (ReportModel)Session["model"];
            }

            if (Session["ChartInfo"] == null)
            {
                chartInfo = new ChartInfo();
                Session["ChartInfo"] = chartInfo;
            }
            else
            {
                chartInfo = (ChartInfo)Session["ChartInfo"];
            }

            String countryCode;
            model.countries.TryGetValue(country, out countryCode);
            MovieLabAPInteractor.populatePiracyTypeInformation(countryCode, model, chartInfo);
            
            return View(model);
        }

    }
}
