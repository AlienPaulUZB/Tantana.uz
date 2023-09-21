using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dragon.Models
{
    public class CategoryImage
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public byte[] ImageContent { get; set; }
    }
}