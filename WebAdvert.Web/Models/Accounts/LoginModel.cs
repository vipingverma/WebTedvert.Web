using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdvert.Web.Models.Accounts
{
    public class LoginModel
    {
		[Required(ErrorMessage = "Email is Required")]
		[EmailAddress]
		[Display(Name = "Email")]
	    public string Email { get; set; }

		[Required(ErrorMessage = "Password is Required")]
		[DataType(DataType.Password)]
	    public string Password { get; set; }

		[DisplayName("Remember Me")]
	    public bool RememberMe { get; set; }
    }
}
