using Cloud.GoogleDrive.WebApplication.Helpers;
using Google.Apis.Drive.v3;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Cloud.GoogleDrive.WebApplication.Controllers
{
    public class CloudController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (file != null)
            {
                GoogleDriveAPIHelper.UploadFileOnDrive(file);
                ViewBag.Success = "File Uploaded on Google Drive";
            }
            else
            {
                ViewBag.Failed = "null or empty!!!!!!";
            }
            return View();
        }

        [HttpGet]
        public ActionResult GoogleAuthCallback(string code, string state)
        {
            return RedirectToAction("Index");
        }
        public ActionResult ListFiles()
        {
            var service = GoogleDriveAPIHelper.GetService();

            FilesResource.ListRequest request = service.Files.List();
            request.Fields = "files(id, name)";
            var result = request.Execute();

            return View(result.Files);
        }
        public ActionResult Download(string id)
        {
            try
            {
                var filePath = GoogleDriveAPIHelper.DownloadGoogleFile(id);
                var fileName = System.IO.Path.GetFileName(filePath);
                var mimeType = MimeMapping.GetMimeMapping(fileName);
                return File(filePath, mimeType, fileName);
            }
            catch
            {
                TempData["Error"] = "File not found or cannot be downloaded.";
                return RedirectToAction("ListFiles");
            }
        }







    }
}
