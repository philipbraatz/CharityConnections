using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Doorfail.Connections.BL;

namespace Doorfail.Connections.WebUI.Model
{
    public class CharitySignup : Charity
    {
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password required")]
        public Password Password { get; set; }
        [Display(Name = "Confirm new password")]
        [Required(ErrorMessage = "Enter Confirm Password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public Password confirmPassword { get; set; }
        //maybe add location 
        //and preferences here

        public List<SelectListItem> possibleCatergories { get; } = new List<SelectListItem>();
        
        [Display(Name = "Select a Category")]
        [Required(ErrorMessage = "You must choose a category")]
        public string Value { get; set; }

        public CharitySignup()
        {
            Location = new Location();
            Password = new Password();
            Category = new Category();
            CategoryCollection.INSTANCE.ForEach(c => 
            possibleCatergories.Add(new SelectListItem { 
                Text= c.Desc,
                Value= c.ID.ToString()
            }));

            //TEMPERARY
            Value = Guid.Empty.ToString();
        }
        //take a prefilled charity
        public CharitySignup(Charity c)
        {
            base.setCharity(c);
        }
    }
}