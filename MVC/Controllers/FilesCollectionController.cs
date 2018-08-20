using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Models;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MVC.Controllers
{
    public class FilesCollectionController : Controller
    {
        //To present a view to upload files 
        public async Task<ActionResult> UploadFiles(int? id)
        {
            userFilesCollection uploadFileCollection = new userFilesCollection();
            uploadFileCollection.UserId = id;
            using(DataquadEntities db = new DataquadEntities())
            {
                var watch = new Stopwatch();
                var files = await GetAllFiles.GetAllFilesByUserId(id);
                ViewBag.files = files;
                ViewBag.userImages = ImageFiles(files);
            }
            return View(uploadFileCollection);
        }

        [HttpPost]
        //Upload files
        public async Task<ActionResult> UploadFiles(userFilesCollection uploadedFiles, int? userId)
        {
            string fileNameString, fileExtensionString;
            userFilesCollection uploadFileCollection = new userFilesCollection();
            if (uploadedFiles.Files[0] != null)
            {
                foreach (var file in uploadedFiles.Files)
                {
                    //Images image = new Images();
                    fileNameString = (file.FileName.Split('.'))[0];
                    fileExtensionString = (file.FileName.Split('.'))[1];
                    uploadFileCollection.FileName = fileNameString;
                    uploadFileCollection.FileExtension = fileExtensionString;
                    uploadFileCollection.FileSize = file.ContentLength;
                    uploadFileCollection.FileContentType = file.ContentType;
                    byte[] data = new byte[file.ContentLength];
                    file.InputStream.Read(data, 0, file.ContentLength);
                    uploadFileCollection.FileData = data;
                    uploadFileCollection.UserId = userId;
                    DataquadEntities db = new DataquadEntities();
                    db.userFilesCollections.Add(uploadFileCollection);
                    db.SaveChanges();
                }
                return RedirectToAction("ViewProfile", "Account", new { id = userId });
            }
            else
            {
                ModelState.AddModelError("FileName", "Please select atleast one file to upload");
                userFilesCollection uploadFile = new userFilesCollection();
                uploadFileCollection.UserId = uploadedFiles.UserId;
                using (DataquadEntities db = new DataquadEntities())
                {
                    var files = await GetAllFiles.GetAllFilesByUserId(uploadedFiles.UserId);
                    ViewBag.files = files;

                    ViewBag.userImages = ImageFiles(files);
                }
                return View("UploadFiles", uploadFileCollection);
            }
        }

        //To download a file
        public FileResult DownloadFile(int? fileId)
        {
            byte[] bytes;
            string fileName, contentType;
            using (DataquadEntities db = new DataquadEntities())
            {
                var file = GetFileById(fileId);
                bytes = file.FileData;
                contentType = file.FileContentType;
                fileName = file.FileName + "." + file.FileExtension;
            }
            return File(bytes, contentType, fileName);
        }

        //To present a view to rename a file
        public ActionResult RenameFile(int? fileId)
        {
            using (DataquadEntities db = new DataquadEntities())
            {
                var file = GetFileById(fileId);
                return View("Test",file);
            }
        }

        [HttpPost]
        //To rename a file
        public ActionResult RenameFile(userFilesCollection file)
        {
            using (DataquadEntities db = new DataquadEntities())
            {
                var fileFromDB = db.userFilesCollections.Where(x => x.FileId == file.FileId).FirstOrDefault();
                fileFromDB.FileName = file.FileName;
                db.SaveChanges();
                return RedirectToAction("ViewProfile","Account", new { id = fileFromDB.UserId });
                
            }
        }

        //To view a Image using the File Id
        public ActionResult ViewImage(int? fileId)
        {
            ViewBag.userImages = GetFileById(fileId);
            var file = ViewBag.userImages;
            return View(file);
        }

        //To present a view specifying the file to delete
        public ActionResult DeleteFile(int? fileId)
        {

            var file = GetFileById(fileId);
            ViewBag.userImages = file;
            ViewBag.DeleteImage = fileId;
            return View("ViewImage", file);
        }
        [HttpPost]
        [ActionName("DeleteFile")]
        //To delete a file
        public ActionResult Delete(userFilesCollection deleteFile)
        {
            //int id = Convert.ToInt32(TempData["Id"]);
            using (DataquadEntities db = new DataquadEntities())
            {
                var recordFromDb = db.userFilesCollections.Find(deleteFile.FileId);
                var userId = recordFromDb.UserId;
                db.userFilesCollections.Remove(recordFromDb);
                db.SaveChanges();
                return RedirectToAction("ViewProfile", "Account", new { id = userId });
            }
        }

        //To get the File using the FileId
        private static userFilesCollection GetFileById(int? fileId)
        {
            using (DataquadEntities db = new DataquadEntities())
            {
                var fileFromDB = db.userFilesCollections.Where(x => x.FileId == fileId).FirstOrDefault();
                return fileFromDB;
            }
        }


        //To obtain the Image files from all the uploaded files.
        private static IEnumerable<userFilesCollection> ImageFiles(List<userFilesCollection> files)
        {
            List<userFilesCollection> images = new List<userFilesCollection>();
            foreach (var file in files)
            {
                if (file.FileContentType == "image/jpeg" || file.FileContentType == "image/png")
                {
                    images.Add(file);
                }
            }
            return images;
        }

    }
}