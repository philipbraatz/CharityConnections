using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CC.Connections.BL;

namespace CC.Connections.WebUI.Model
{
    public class CharitySignup : AbsContact
    {
        [Display(Name = "Charity Email")]
        [DataType(DataType.EmailAddress)]
        public string contactInfoEmail
        {
            get
            {
                return this.contactInfoEmail;
            }

            set
            {
                this.contactInfoEmail = ContactInfo_Email;
            }
        }

        [Display(Name = "Charity Name")]
        [DataType(DataType.Text)]
        public string lastName
        {
            get
            {
                return this.lastName;
            }
            set
            {
                this.lastName = ContactInfo_LName;
            }
        }

        [Display(Name = "Charity")]
        [DataType(DataType.Text)]
        public string firstName
        {
            get
            {
                return this.firstName;
            }
            set
            {
                this.firstName = "Charity Business";
            }
        }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password required")]
        public Password password { get; set; }
        [Display(Name = "Confirm new password")]
        [Required(ErrorMessage = "Enter Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public Password confirmPassword { get; set; }
        //maybe add location 
        //and preferences here
        public CharitySignup()
        {
            password = new Password();
        }
    }
}