﻿using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Data
{
    //this is config that needs to be setup to use entity framework core
    //DbContext is basically the root class of entity framework core and it is with this that we will be accessing Entity Framework
    public class ApplicationDbContext : DbContext
    {
        //here we need to pass in some config when we create of a instance of ApplicationDbContext - and when we do, we pass that config onwards to the DbContext class
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
