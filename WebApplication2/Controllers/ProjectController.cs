﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.DAL;
using WebApplication2.ModelBinders;
using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    public class ProjectController : Controller
    {
        private object dal;

        // GET: Project
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Enter() {

            return View();
        }
        public ActionResult imanagerp()
        {
            ProjectsDal pdal = new ProjectsDal();
            ProjectMv pm = new ProjectMv();
             pm.project = (from x in pdal.projects where (AccountInfo.userName).Equals(x.UserName) select x).ToList<Project>();
           
            return View(pm);
        }


        [HttpPost]
        public ActionResult Submit([ModelBinder(typeof(ProjectBinder))] Project project)
        {
            
            ProjectsDal pdal = new ProjectsDal();

            // if exists
            if (pdal.AddProject(project) == false)
            {
                TempData["ExistUser"] = "Your project does Exists in DataBase! try other project";
                return View("Enter");
            }


            // if not exists
            return View("Project", project);

        }

        public ActionResult Requests()
        {
            return View();
        }

        public ActionResult New_Request()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Submit_Request()
        {
            var pdal = new ProjectsDal();
            string typee = Request.Form["types"];

            Project project = SetProjectByType(Session["Username"].ToString(), Request.Form["To_user"], Request.Form["Projectname"], typee);


            if (pdal.IsNotExists(project) == false)  // if project does exist then
            {
                var membdal = new ProjectMembersDal();
                var member = SetMember(Session["Username"].ToString(), Request.Form["To_user"], typee);

                TempData["Error"] = AddRequestsToDb(pdal,membdal,project,member,typee, Request.Form["To_user"], Session["Username"].ToString(), Request.Form["Discription"]);

                if (TempData["Error"].Equals(""))
                    return View(); // Added Seccefuly
                return View("New_Request");   // Faild to add
            }
            else         // if project does not exist then 
            {
                TempData["Error"] = "Project name && Member username does not exist togther!!";
                return View("New_Request");
            }
        }


        private string AddRequestsToDb(ProjectsDal pdal,ProjectMembersDal membdal,Project project,string member,string typee,string touser,string fromsuer,string discrip)
        {
            if (!typee.Equals("Leave_Project"))
            {
                if (membdal.IsNotExists(pdal.GetProjectId(project), member) == true)
                {
                    var req = SetRequest(touser, fromsuer, discrip, project.ProjectName, typee);
                    var reqDal = new RequestsDal();
                    if (reqDal.AddRequest(req) == true)        // addes sucssefuly
                        return "";

                    if (typee.Equals("Add_Member"))
                        return "You have been sent request to add new membert!!";
                    return "You have been sent request to Join to Project!!";
                }
                else
                {
                    if (typee.Equals("Add_Member"))
                        return "You are Exists in your project!!";
                    return "Member Exists in your project!!";
                }
            }
            else
            {
                if (membdal.IsNotExists(pdal.GetProjectId(project), member) == false)
                {
                    var req = SetRequest(touser, fromsuer, discrip, project.ProjectName, typee);
                    var reqDal = new RequestsDal();
                    if (reqDal.AddRequest(req) == true)        // addes sucssefuly
                        return "";

                    
                        return "You have been sent request to Leave Project!!";
                }
                else
                    return "Your are not exists in project";

            }
        }

        private Project SetProjectByType(string user1,string user2,string projectname,string type)
        {
            Project project;

            if (type.Equals("Add_Member"))
                project = SetProject(user1, projectname);
            else
                project = SetProject(user2, projectname);

            return project;
        }

        private string SetMember(string user1, string user2,string type)
        {
            if (type.Equals("Add_Member"))
                return user2;
            else
                return user1;

        }
        public ActionResult My_Requests()
        {
            return View();
        }

        public ActionResult View_Recent_Requests()
        {
            return View();
        }

        public ActionResult View_Requests()
        {
            return View();
        }


    
        private Request SetRequest(string Touser,string Fromuser,string Discription,string Projectname,string type)
        {
            var newRequest = new Request {
                discription = Discription,
                from_user =Fromuser,
                Projectname =Projectname,
                status =0,
                to_user =Touser,
                request_type =type}
            ;
            
            return newRequest;
        }

        private Project SetProject(string user,string projname)
        {
            var p = new Project{ProjectName=projname,UserName=user };
            return p;
        }

        
      
    }
}