using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dragon.Models
{
    public class Supplier
    {
        public int SupplierID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OwnerID { get; set; }
        public string Phone { get; set; }
        public int CategoryID { get; set; }
        public int Capacity { get; set; }
        public string Address { get; set; }
        public decimal Cost { get; set; }
        public string SocialMedia { get; set; }
        public string VideoLink { get; set; }
        public int BackgroundImageID { get; set; }
        public string City { get; set; }
        public string Brand { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
    }
}