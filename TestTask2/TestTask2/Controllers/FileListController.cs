using Microsoft.AspNetCore.Mvc;

namespace TestTask2.Controllers
{
    public class FileListController : Controller
    {
        public IActionResult FileList()
        {
            var items = GetFiles();
            return View(items);
        }

        //get all uploaded files
        private List<string> GetFiles()
        {
            string dirpath = @"C:\Users\апро\Desktop\TestTask\TestTask2\TestTask2\Upload";
            DirectoryInfo dir = new DirectoryInfo(dirpath);
            System.IO.FileInfo[] fileItems = dir.GetFiles("*.*");
            List<string> files = new List<string>();
            foreach (var file in fileItems)
            {
                files.Add(file.Name);
            }
            return files;

        }
    }
}
