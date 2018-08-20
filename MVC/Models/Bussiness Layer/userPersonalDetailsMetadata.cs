using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MVC.Models
{
    [MetadataType(typeof(userPersonalDetailsMetadata))]
    public partial class userPersonalDetails
    {
        public string Comments { get; set; }
        public string selectedGender { get; set; }
        public List<SelectListItem> GenderList { get; set; }
        public List<SelectListItem> RaceList { get; set; }
        [Required(AllowEmptyStrings = false,ErrorMessage ="Please select a value")]
        public int selectedRaceId { get; set; }
        public string selectedRaceName { get; set; }
        public userPersonalDetails()
        {
            this.GenderList = new List<SelectListItem>();
            this.RaceList = new List<SelectListItem>();
        }
    }
    public class userPersonalDetailsMetadata
    {

    }
}