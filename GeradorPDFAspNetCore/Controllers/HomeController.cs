using GeradorPDFAspNetCore.Models;
using jsreport.AspNetCore;
using jsreport.Types;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using System;

namespace GeradorPDFAspNetCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RotativaPDF()
        {
            GerarContratoViewModel person = new GerarContratoViewModel()
            {
               
            };

            var pdf = new ViewAsPdf(person);

            return pdf;
        }

        [MiddlewareFilter(typeof(JsReportPipeline))]
        public IActionResult JSReportPDF()
        {
            GerarContratoViewModel person = new GerarContratoViewModel()
            {
               
            };

            HttpContext.JsReportFeature().Recipe(Recipe.PhantomPdf);

            return View(person);
        }
    }
}