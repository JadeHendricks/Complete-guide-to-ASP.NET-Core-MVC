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

		public const string StatusPending = "Pending";
		public const string StatusApproved = "Approved";
		public const string StatusInProcess = "Processing";
		public const string StatusShipped = "Shipped";
		public const string StatusCancelled = "Cancelled";
		public const string StatusRefunded = "Refunded";

		public const string PaymentStatusPending = "Pending";
		public const string PaymentStatusApproved = "Approved";
		public const string PaymentStatusDelayedPayment = "ApprovedForDelayedPayment";
		public const string PaymentStatusRejected = "Rejected";

		public const string SessionCart = "SessionShoppingCart";
	}
}
