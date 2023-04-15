using Bulky.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.DataAccess.Data
{
    //this is config that needs to be setup to use entity framework core
    //DbContext is basically the root class of entity framework core and it is with this that we will be accessing Entity Framework
    public class ApplicationDbContext : DbContext
    {
        //here we need to pass in some config when we create of a instance of ApplicationDbContext - and when we do, we pass that config onwards to the DbContext class
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //when we need to create a table we have to create something call a DbSet inside of ApplicationDbContext + add-migration "name of migration" + update-database
        //DbSet<ModelName> TableName
        public DbSet<Category> Categories { get; set; }

        //seeding data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Entity<WhatTableEntityWillBeWorkingOn>
            //HasData expects an array of categories that we want to seed into the Table
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 }
            );
        }
    }
}
