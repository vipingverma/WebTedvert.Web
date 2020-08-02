﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime.Internal.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAdvert.Web.Models.Accounts;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAdvert.Web.Controllers
{
    public class Accounts : Controller
    {
	    private readonly SignInManager<CognitoUser> _signInManager;
	    private readonly UserManager<CognitoUser> _userManager;
	    private readonly CognitoUserPool _pool;

	    public Accounts(SignInManager<CognitoUser> signInManager, UserManager<CognitoUser> userManager,
		    CognitoUserPool pool)
	    {
		    _signInManager = signInManager;
		    _userManager = userManager;
		    _pool = pool;
	    }
		public async Task<IActionResult> Signup()
	    {
			var model = new SignupModel();
			return View(model);
	    }

		[HttpPost]
	    public async Task<IActionResult> Signup(SignupModel model)
	    {
		    if (ModelState.IsValid)
		    {
			    var user = _pool.GetUser(model.Email);
			    if (user.Status != null)
			    {
				    ModelState.AddModelError("UserExists", "User with this email id already exists");

			    }
			    
				user.Attributes.Add(CognitoAttribute.Name.AttributeName, model.Email);
			    var createdUser = await _userManager.CreateAsync(user, model.Password);

			    if (createdUser.Succeeded)
			    {
				    return RedirectToAction("Confirm");
			    }
		    }
		    return View(model);
	    }

	    public async Task<IActionResult> Confirm()
	    {
		    var model = new ConfirmModel();
		    return View(model);
	    }

		[HttpPost]
		[ActionName("Confirm")]
	    public async Task<IActionResult> Confirm_Post(ConfirmModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user == null)
				{
					ModelState.AddModelError("NotFound", "A user with this email not found");
					return View(model);
				}
				//var result = await ((CognitoUserManager<CognitoUser>)_userManager).ConfirmSignUpAsync(user, model.Code, true);
				var result = await ((CognitoUserManager<CognitoUser>)_userManager).ConfirmSignUpAsync(user, model.Code, true);

				if (result.Succeeded)
				{
					return RedirectToAction("Index", "Home");
				}
				else
				{
					foreach (var item in result.Errors)
					{
						ModelState.AddModelError(item.Code, item.Description);
					}

					return View(model);
				}
			}

			return View(model);
		}

		[HttpGet]
	    public async Task<IActionResult> Login(LoginModel model)
	    {
		    return View(model);

	    }

	    [HttpPost]
	    [ActionName("Login")]
	    public async Task<IActionResult> LoginPost(LoginModel model)
	    {
		    if (ModelState.IsValid)
		    {
			    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false)
				    .ConfigureAwait(false);
			    if (result.Succeeded)
			    {
				    return RedirectToAction("Index", "Home");
			    }
			    else
			    {
				    ModelState.AddModelError("LoginError", "Email or password don't match");
			    }
		    }
		    return View("Login",model);

	    }



	}
}
