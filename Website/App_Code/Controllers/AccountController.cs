using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using FluentValidation.Results;
using Models;

/// <summary>
/// Summary description for AccountController
/// </summary>
public class AccountController : Controller
{
    public AccountController()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public ActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    public ActionResult Login(LoginViewModel loginViewModel)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("");
        var validator = new LoginViewModelValidator();
        ValidationResult result = validator.Validate(loginViewModel);

        if (ModelState.IsValid)
        {
            
        }

        return View(loginViewModel);
    }
}