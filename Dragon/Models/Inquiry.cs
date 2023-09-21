using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dragon.Models
{
    public class Inquiry
    {
        public int SupplierID { get; set; }
        public string UserName { get; set;}
        public string UserEmail { get; set; }
        
        public string Subject { get; set; }
        public string Message { get; set; }

    }
}