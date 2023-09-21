using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dragon.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public int SupplierId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime CreatedTime { get; set; }
        public string UserID { get; set; }
        public int Rating { get; set; }
    }
}