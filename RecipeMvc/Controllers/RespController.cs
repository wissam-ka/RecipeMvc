using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using RecipeMvc.Models;
using System.Net;
using System.IO;

namespace RecipeMvc.Controllers
{
    public class RespController : Controller
    {
        //
        // GET: /Resp/
        [AllowAnonymous]
        public ActionResult Index()
        {
            var db = new RespDataContext();
            var resp = db.RespComps.ToArray();
            //var resp = db.RespComps;
            return View(resp);
        }
        
        public ActionResult Resipe(long? id)
        {  
            var db = new RespDataContext();
            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }
           
            
                //var db = new RespDataContext();
                var resp = db.RespComps.Find(id);
                if (resp == null)
                {
                    return View(db.RespComps.FirstOrDefault());
                    //ViewData["Resipe"] = resp;
                }  
                return View(resp);
            
        }
        [HttpGet]
        public ActionResult Create()
        {
           
            //var meatlist = new SelectList(new[] { "chiken", "Beef", "Lamb" });
            //ViewBag.MeatList = meatlist;
            int i = 0;
            ViewBag.ListId = i;
        
            return View();
        }
        [HttpPost]
        [Authorize(Users = "wk") ]
        public ActionResult Create(Models.RespComp respcomp,HttpPostedFileBase file)
        {
          
            if (file != null && file.ContentLength > 0 && file.ContentType.StartsWith("image/"))
            {
                try
                {

                    string temppath = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images/Recipes"), temppath);

                    file.SaveAs(path);
                    ViewBag.Message = "File uploaded successfully";
                    respcomp.FilePath = Path.Combine("~/Images/Recipes", temppath);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            }
            else
            {
                ModelState.AddModelError("file", "You have not specified a file // wrong file type");
                // ViewBag.Message = "You have not specified a file // wrong file type";
            }


            respcomp.RNum = 0;
            respcomp.FRate = 0;
            if (ModelState.IsValid)

            {
                var db = new RespDataContext();
               
                db.RespComps.Add(respcomp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
                  return View(respcomp);
             
        }
        
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Rate(Rate rate)
        {

            var db = new RespDataContext();
            var i=rate.ReId;
           var resp = db.RespComps.Find(rate.ReId);
            if (rate.Comm > 10 || rate.Comm < 0 || rate.Comm==0)
            {
                ModelState.AddModelError("comm", "out of range");
                }
                else if (resp == null)
                {
                    ModelState.AddModelError("ReId", "not found");
                }
                else
                {
                    resp.Rates.Add(rate);
                    resp.RNum = resp.RNum + 1;
                    resp.FRate =(double)Math.Round(((resp.FRate * (resp.RNum-1) + rate.Comm) / (resp.RNum)),2);
                    //resp.FRate = ((resp.FRate*(resp.RNum-1) + rate.Comm)/(resp.RNum));
                    db.SaveChanges();
                }   
           if (!Request.IsAjaxRequest())
                return RedirectToAction("Resipe", new { id = rate.ReId });
            //var httpStatus = ModelState.IsValid ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            //return new HttpStatusCodeResult(httpStatus);
            //return PartialView("_CurrentRate", resp);
            return Json(new
            {
                FRate1=resp.FRate,
                NNum=resp.RNum
                
            });
        }
        [HttpGet]
        public ActionResult Search()
        {
            var searchlist  = new SelectList(new[] { "Meat", "Food Rate", "people number" });
            ViewBag.SearchList = searchlist;
            return View();
        }
        [HttpPost]
        public ActionResult Search(string category, string searchPattern)
        {
            var searchlist = new SelectList(new[] { "Food Rate", "people number" });
            ViewBag.SearchList = searchlist;
            if (searchPattern.IsEmpty())
            {
                ModelState.AddModelError("searchPattern", "Shouldn't be empty");
            }
            if (ModelState.IsValid)
            {
                var db = new RespDataContext();
                IQueryable<RespComp> results = db.RespComps;
               
               if (category == "Food Rate")
                {
                    results = results.Where(recipe => recipe.FRate.Equals(double.Parse(searchPattern)));
                }
                else
                {int i = int.Parse(searchPattern);
                    results = results.Where(recipe => recipe.Number == i);
                }
                return View("Index", results);

            }
            return View();
        }





    }
}
