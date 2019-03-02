using DinkToPdf;
using DinkToPdf.Contracts;
using GeradorPDFAspNetCore.Models;
using GeradorPDFAspNetCore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GeradorPDFAspNetCore.Controllers
{
    [Route("api/sample")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        private readonly IViewRenderService _viewRenderService;
        private IConverter _converter;
        private IHttpContextAccessor _context;

        public SampleController(IViewRenderService viewRenderService, IConverter converter, IHttpContextAccessor context)
        {
            _viewRenderService = viewRenderService;
            _converter = converter;
            _context = context;
        }

        [Route("pdf")]
        public async Task<IActionResult> DinkToPDF()
        {
            var request = _context.HttpContext.Request;
            GerarContratoViewModel person = new GerarContratoViewModel()
            {
                UrlBase = $"{request.Scheme}://{request.Host.ToString()}" ,
                DataGeracao = DateTime.Now
            };


            var html = await _viewRenderService.RenderToStringAsync("DinkToPDF", person);
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                ColorMode = ColorMode.Grayscale,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4
                
                },
                Objects = {
                    new ObjectSettings() {
                        HtmlContent = html,
                        WebSettings = { DefaultEncoding = "utf-8", MinimumFontSize = 12, EnableIntelligentShrinking =false, LoadImages = true  }
                    }
                }
            };

            byte[] pdf = _converter.Convert(doc);

            return File(pdf, "application/pdf");
        }
    }
}