using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using CRUD_Img_Upload.Models;
using System.IO;

namespace CRUD_Img_Upload.Controllers
{
    public class FileUploadCrudController : Controller
    {
        private readonly ApplicationDbContext _adb;
        private readonly IWebHostEnvironment _iweb;

        public FileUploadCrudController(ApplicationDbContext adb,IWebHostEnvironment iweb)
        {
            _adb = adb;
            _iweb = iweb;
        }
        public IActionResult Index()
        {
            var displayimages = _adb.Saveimg.ToList();
            return View(displayimages);
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile fileobj,ImageCrudClass icc)
        {
            var imgext = Path.GetExtension(fileobj.FileName);
            if(imgext==".jpg" || imgext==".gif") //Here i am allowing only image files.
            {
                var uploading = Path.Combine(_iweb.WebRootPath, "Images", fileobj.FileName);
                var stream = new FileStream(uploading, FileMode.Create);
                await fileobj.CopyToAsync(stream);
                stream.Close();

                icc.Imgname = fileobj.FileName;
                icc.Imgpath = uploading;
                await _adb.Saveimg.AddAsync(icc); // Here Saveimg is table name.
                await _adb.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id==null)
            {
                return RedirectToAction("Index");
            }
            var displayimgdetails = await _adb.Saveimg.FindAsync(id);
            return View(displayimgdetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IFormFile fileobj, ImageCrudClass icc, string fname, int id)
        {
            if(ModelState.IsValid)
            {
                var getimagedetails = await _adb.Saveimg.FindAsync(id);
                _adb.Saveimg.Remove(getimagedetails);
                fname = Path.Combine(_iweb.WebRootPath, "Images", getimagedetails.Imgname);
                FileInfo fi = new FileInfo(fname);
                if(fi.Exists)
                {
                    System.IO.File.Delete(fname);
                    fi.Delete();
                }

            var imgext = Path.GetExtension(fileobj.FileName);
            if (imgext == ".jpg" || imgext == ".gif") //Here i am allowing only image files.
            {
                var uploading = Path.Combine(_iweb.WebRootPath, "Images", fileobj.FileName);
                var stream = new FileStream(uploading, FileMode.Create);
                await fileobj.CopyToAsync(stream);
                stream.Close();

                icc.Imgname = fileobj.FileName;
                icc.Imgpath = uploading;
                _adb.Update(icc);
                await _adb.SaveChangesAsync();
            }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var displayimgdetails = await _adb.Saveimg.FindAsync(id);
            return View(displayimgdetails);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var displayimgdetails = await _adb.Saveimg.FindAsync(id);
            return View(displayimgdetails);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string fname, int id)
        {
            var getimagedetails = await _adb.Saveimg.FindAsync(id);
            _adb.Saveimg.Remove(getimagedetails);
            fname = Path.Combine(_iweb.WebRootPath, "Images", getimagedetails.Imgname);
            FileInfo fi = new FileInfo(fname);
            if (fi.Exists)
            {
                System.IO.File.Delete(fname);
                fi.Delete();
            }
            await _adb.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
