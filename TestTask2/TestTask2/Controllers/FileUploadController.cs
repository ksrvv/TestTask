using Microsoft.AspNetCore.Mvc;
using System.Web;
using TestTask2.Models;

namespace TestTask2.Controllers
{ 
    public class FileUploadController : Controller
    {
       
        
        ExelToDBExporter exporter = new ExelToDBExporter();
        
        public IActionResult FileUpload()
        {
         

            return View();
        }
        [HttpPost]

        public async  Task <ActionResult>  FileUpload(IFormFile file)
        {
            await UploadFile(file);
            TempData["msg"] = $"File {file.FileName} uploaded!";
            //uploading in database here 
            exporter.Export( file.FileName);

            return View();
        }

        //upload file on server
        public async Task <bool> UploadFile (IFormFile file)
        {
            string path = "";
            bool iscopied = false;
            try
            {
                if (file.Length > 0)
                {
                    string filename = file.FileName;
                    path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(),"Upload"));
                    using (FileStream filestream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                    {
                       await file.CopyToAsync(filestream);
                    }
                    iscopied = true;
                }
                else iscopied = false;
            }
            catch (Exception)
            {
                throw;
            }
            return iscopied;
        }

     
    }
}