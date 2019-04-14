﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.DAL
{
    public class ProjectMembersDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProjectMembers>().ToTable("ProjectMembers");
        }
        public DbSet<ProjectMembers> projectMembers { get; set; }

        public bool IsNotExists(int projectid,string memb)
        {
            var result = (from x in projectMembers
                          where x.ProjectId == projectid && x.Member.Equals(memb)
                          select x).ToList<ProjectMembers>();
            if (result.Count == 0)
                return true;
            return false;
        }

        public bool AddMember(ProjectMembers mem)
        {
            if (IsNotExists(mem.ProjectId, mem.Member)==true)
            {
                projectMembers.Add(mem);
                SaveChanges();
                return true;
            }
            return false;
        }

    }
}