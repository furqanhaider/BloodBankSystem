using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodBankSystem.Models
{
    public class DonorViewModel
    {
        [DisplayName("Name :")]
        [Required]
        public string Name { get; set; }


        [DisplayName("Contact No. :")]
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string ContactNo { get; set; }

        [DisplayName("Blood Group :")]
        [Required]
        public SelectList BloodGroupsList { get; set; }

        [Key]
        public string bloodGroup { get; set; }
        public string gender { get; set; }

        [DisplayName("D.O.B :")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime dob { get; set; }

        [DisplayName("CNIC :")]
        [RegularExpression("^[0-9+]{5}-[0-9+]{7}-[0-9]{1}$", ErrorMessage = "Must be in form 12345-7891231-1")]
        [Required]
        public string cnic { get; set; }

        [DisplayName("Gender :")]
        [Required]
        public SelectList GenderList { get; set; }

        [DisplayName("Address :")]
        [Required]
        public string Address { get; set; }

        [DisplayName("City :")]
        [Required]
        public string City { get; set; }

        [DisplayName("Upload Image :")]
        [Required]
        public HttpPostedFileBase Photo { get; set; }

        public DonorViewModel()
        {
            List<SelectListItem> gender = new List<SelectListItem>();
            gender.Add(new SelectListItem { Text = "Male", Value = "Male" });
            gender.Add(new SelectListItem { Text = "Female", Value = "Female" });

            List<SelectListItem> bloodGroups = new List<SelectListItem>();
            bloodGroups.Add(new SelectListItem { Text = "A-", Value = "A-" });
            bloodGroups.Add(new SelectListItem { Text = "A+", Value = "A+" });
            bloodGroups.Add(new SelectListItem { Text = "AB-", Value = "AB-" });
            bloodGroups.Add(new SelectListItem { Text = "AB+", Value = "AB+" });
            bloodGroups.Add(new SelectListItem { Text = "B-", Value = "B-" });
            bloodGroups.Add(new SelectListItem { Text = "B+", Value = "B+" });
            bloodGroups.Add(new SelectListItem { Text = "O-", Value = "O-" });
            bloodGroups.Add(new SelectListItem { Text = "O+", Value = "O+" });

            this.GenderList = new SelectList(gender, "Value", "Text");
            this.BloodGroupsList = new SelectList(bloodGroups, "Value", "Text");

        }
    }
}