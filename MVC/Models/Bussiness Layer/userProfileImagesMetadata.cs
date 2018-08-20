using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    [MetadataType(typeof(userProfileImagesMetadata))]
    public partial class userProfileImages
    {
        public HttpPostedFileBase File { get; set; }
    }
    public class userProfileImagesMetadata
    {
            public int ImageId { get; set; }
            public Nullable<int> UserId { get; set; }
            public byte[] ImageData { get; set; }
            public string FileName { get; set; }
            public Nullable<int> ImageSize { get; set; }
    }
}