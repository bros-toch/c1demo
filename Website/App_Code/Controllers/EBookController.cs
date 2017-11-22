using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Composite.Data;
using Demo.Ebook;
using eBdb.EpubReader;
using Models;

namespace Controllers
{
    public class EBookController : Controller
    {
        [HttpGet]
        public ActionResult UploadFile()
        {
            var viewModel = new EbookFileUploadViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult UploadFile(EbookFileUploadViewModel viewModel)
        {
            if (Request.Files.Count != 1 || Request.Files[0].ContentLength == 0)
            {
                ModelState.AddModelError("uploadError", "File's length is zero, or no files found");
                return View(viewModel);
            }

            // check the file size (max 4 Mb)

            if (Request.Files[0].ContentLength > 1024 * 1024 * 4)
            {
                ModelState.AddModelError("uploadError", "File size can't exceed 4 MB");
                return View(viewModel);
            }

            // check the file size (min 100 bytes)

            if (Request.Files[0].ContentLength < 100)
            {
                ModelState.AddModelError("uploadError", "File size is too small");
                return View(viewModel);
            }

            // check file extension

            string extension = Path.GetExtension(Request.Files[0].FileName).ToLower();

            if (extension != ".epub")
            {
                ModelState.AddModelError("uploadError", "Supported file extensions: epub");
                return View(viewModel);
            }

            // extract only the filename
            var fileName = Path.GetFileName(Request.Files[0].FileName);

            // store the file inside ~/App_Data/uploads folder
            var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);

            try
            {
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                Request.Files[0].SaveAs(path);
            }
            catch (Exception)
            {
                ModelState.AddModelError("uploadError", "Can't save file to disk");
            }

            if (ModelState.IsValid)
            {
                using (var data = new DataConnection())
                {
                    var fileUpload = data.CreateNew<FileUpload>();
                    fileUpload.FileName = fileName;
                    data.Add(fileUpload);
                }
            }
            
            return View(viewModel);
        }

        public ActionResult UploadFiles()
        {
            var viewModel = new FileUploadListViewModel();

            viewModel.Items = DataFacade.GetData<FileUpload>().Select(x=> new FileUploadListItemViewModel()
            {
                Id = x.Id,
                Name = x.FileName
            });
            return View(viewModel);
        }

        public ActionResult Detail()
        {
            var detailIdStr = Request.QueryString["id"];
            Guid id;

            if (Guid.TryParse(detailIdStr, out id))
            {
                
                var book = DataFacade.GetData<FileUpload>(x => x.Id == id).FirstOrDefault();
                if (book != null)
                {
                    var viewModel = new BookDetailViewModel();

                    var file = Server.MapPath("~/App_Data/Uploads/" + book.FileName);
                    Epub epub = new Epub(file);

                    viewModel.Title = epub.Title[0];
                    viewModel.Author = string.Join(", ", epub.Creator);
                    
                    viewModel.Epub = epub;
                    return View(viewModel);
                }
                            }

            return View(new BookDetailViewModel());
        }
    }
}