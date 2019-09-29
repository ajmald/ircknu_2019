using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using IrkcnuApi.Services;
using Newtonsoft.Json;
using System.Text;


namespace IrkcnuApi.Controllers
{
	public class UploadController : Controller
	{
        public IHostingEnvironment HostingEnvironment { get; set; }
        private readonly ImportService _importService;
        public UploadController(IHostingEnvironment hostingEnvironment, ImportService importService)
        {
            HostingEnvironment = hostingEnvironment;
            _importService = importService;
        }

        
        public ActionResult Index()
		{
			return View();
		}

        public async Task<ActionResult> Save(IEnumerable<IFormFile> files)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(fileContent.FileName.ToString().Trim('"'));
                    var physicalPath = Path.Combine(HostingEnvironment.WebRootPath, "App_Data", fileName);

                    using (var fileStream = new FileStream(physicalPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    string data = _importService.ConvertCsvFileToJsonObject(physicalPath);
                    physicalPath = Path.Combine(HostingEnvironment.WebRootPath, "App_Data", fileName + ".json");
                    System.IO.File.WriteAllText(physicalPath,data);
                    _importService.InsertDocumentsInCollection(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
        }
        
        public ActionResult Remove(string[] fileNames)
		{
			// The parameter of the Remove action must be called "fileNames"
			
			if (fileNames != null)
			{
				foreach (var fullName in fileNames)
				{
					var fileName = Path.GetFileName(fullName);
					var physicalPath = Path.Combine(HostingEnvironment.WebRootPath, "App_Data", fileName);

					// TODO: Verify user permissions

					if (System.IO.File.Exists(physicalPath))
					{
					
						 System.IO.File.Delete(physicalPath);
					}
				}
			}

			// Return an empty string to signify success
			return Content("");
		}
	}
}