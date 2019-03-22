﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
using WebApplication2.DAL;
using WebApplication2.ModelBinders;
using System.Web.Security;

namespace WebApplication2.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Submit([ModelBinder(typeof(UserBinder))] User user) { 

            UserDal udal = new UserDal();
            udal.users.Add(user);
            udal.SaveChanges();

            return View("User", user);
        }


        public ActionResult Login(User U)
        {
            if (ModelState.IsValid)
            {
                UserDal dal = new UserDal();
                //check if the user is in the users DB (not admin) 
                Console.WriteLine("start");
                List<User> userValid = (from u in dal.users where (u.Password == U.Password) && (u.UserName == U.UserName) select u).ToList<User>();
                if (userValid.Count == 1)
                {
                    Console.WriteLine("dfs");
                    myRole.setUser(U.UserName, "user", userValid[0].UserName);//set the user role 
                    FormsAuthentication.SetAuthCookie(U.UserName, true);
                    return RedirectToAction("Index", "Home");
                }
               /* else
                {
                    //check if the user is admin in the librarian BD
                    List<librarian> adminValid = (from u in dal.librarians where (u.password == U.password) && (u.id == U.id) select u).ToList<librarian>();
                    if (adminValid.Count == 1)
                    {
                        myRole.setUser(U.id, "admin", adminValid[0].name);//set the user role  as admin 
                        FormsAuthentication.SetAuthCookie(U.id, true);
                        return RedirectToAction("Index", "Userpage");
                    }
                }*/

                ViewBag.result = "1-User name or password is inccorect!";//the user inter invalid password or username
                ViewBag.signup = "2- this username is not exist, please sign up  ";   //or he is not a user 
                Console.WriteLine("start1");

            }

            return View("Index");

        }
    }
}