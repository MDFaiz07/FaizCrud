using FaizCrud.DB_Context;
using FaizCrud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FaizCrud.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(User_Login obj)
        {
            FInfoEntities dobj = new FInfoEntities();
            var UserRes = dobj.Log_In.Where(m => m.email == obj.email).FirstOrDefault();
            if(UserRes == null)
            {
                TempData["Invalid"] = "Email not found";
            }
            else
            {
                if(UserRes.email == obj.email && UserRes.password==obj.password)
                {
                    FormsAuthentication.SetAuthCookie(UserRes.email, false);
                    Session["UserName"] = UserRes.name;
                    return RedirectToAction("Dashboard", "Home");
                }
                else
                {
                    TempData["Wrong"] = "Wrong Password";
                    return View();
                }
            }
            return View();
        }
        [Authorize]
        public ActionResult Dashboard()
        {
            return View();
        }
        [Authorize]
        public ActionResult Tables()
        {
            FInfoEntities dobj = new FInfoEntities();
            List<F_Model> mobj = new List<F_Model>();
            var res = dobj.F_tb.ToList();
            foreach (var item in res)
            {
                mobj.Add(new F_Model
                {
                    Id = item.Id,
                    Name = item.Name,
                    Age = (int)item.Age,
                    Technology = item.Technology,
                    Email = item.Email

                });

            }
            return View(mobj);
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Authorize]
        [HttpGet]
        public ActionResult Form()
        {

            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Form(F_Model mobj)
        {
            FInfoEntities dobj = new FInfoEntities();
            F_tb tobj = new F_tb();
            tobj.Id = mobj.Id;
            tobj.Name = mobj.Name;
            tobj.Age = mobj.Age;
            tobj.Email = mobj.Email;
            tobj.Technology = mobj.Technology;
            if (mobj.Id == 0)
            {
                dobj.F_tb.Add(tobj);
                dobj.SaveChanges();
            }
            else
            {
                dobj.Entry(tobj).State = System.Data.Entity.EntityState.Modified;
                dobj.SaveChanges();
            }
            return RedirectToAction("Tables");
        }
        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [Authorize]
        public ActionResult edit(int Id)
        {
            F_Model mobj = new F_Model();
            FInfoEntities dobj = new FInfoEntities();
            var eobj = dobj.F_tb.Where(m => m.Id == Id).First();
            mobj.Id = eobj.Id;
            mobj.Name = eobj.Name;
            mobj.Age = (int)eobj.Age;
            mobj.Technology = eobj.Technology;
            mobj.Email = eobj.Email;

            return View("Form", mobj);
        }
        [Authorize]
        public ActionResult delete(int Id)
        {
            FInfoEntities dobj = new FInfoEntities();
            var ditem = dobj.F_tb.Where(m => m.Id == Id).First();
            dobj.F_tb.Remove(ditem);
            dobj.SaveChanges();

            return RedirectToAction("Tables");
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index");
        }
    }
}