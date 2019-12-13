using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CC.Connections.BL;

namespace CC.Connections.WebUI.Model
{
    public class CharitySignup : Charity
    {
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password required")]
        public new Password Password { get; set; }
        [Display(Name = "Confirm new password")]
        [Required(ErrorMessage = "Enter Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public Password confirmPassword { get; set; }
        //maybe add location 
        //and preferences here
        public CharitySignup()
        {
            Location = new Location();
            Password = new Password();
            Category = new Category();
        }
        //take a prefilled charity
        public CharitySignup(Charity c)
        {
            base.setCharityInfo(c);
        }
    }
}