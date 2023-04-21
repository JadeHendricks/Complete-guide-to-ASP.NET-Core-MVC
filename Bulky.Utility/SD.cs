using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Utility
{
    //SD = Static Details
    public static class SD
    {
        //user roles - these roles needs to be added to our database - we are doing that in register.cshtml.cs
        public const string Role_Customer = "Customer";
        //company users don't have to make payment right away they can pay within 30 days and can be created by a admin user
        public const string Role_Company = "Company";
        public const string Role_Admin = "Admin";
        //employee users will have access to modify shipping details of a order
        public const string Role_Employee = "Employee";
    }
}
