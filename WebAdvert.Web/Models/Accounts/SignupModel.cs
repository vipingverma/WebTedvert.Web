﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace WebAdvert.Web.Models.Accounts
{
    public class SignupModel
    {
		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
	    public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[StringLength(6, ErrorMessage = "Password must be 6 characters long")]
		[Display(Name = "Password")]
		public string Password { get; set; }

	    [Required]
	    [DataType(DataType.Password)]
	    [StringLength(6, ErrorMessage = "Password must be 6 characters long")]
		[Compare("Password", ErrorMessage = "Passwords don't match")]
		public string ConfirmPassword { get; set; }

		

    }
}
