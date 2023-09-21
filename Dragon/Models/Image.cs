using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dragon.Models
{
    public class Image
    {
        public int ImageID { get; set; }
        public int SupplierID { get; set; }
        public byte[] ImageContent { get; set; }
    }
}