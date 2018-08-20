using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public partial class userFilesCollection
    {
        
        public HttpPostedFileBase[] Files { get; set; }
    }
}