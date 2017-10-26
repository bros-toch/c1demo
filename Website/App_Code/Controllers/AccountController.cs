﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
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
        return View(new LoginViewModel() { Username = "test"});
    }

    [HttpPost]
    public ActionResult Login(LoginViewModel loginViewModel)
    {

        if (ModelState.IsValid)
        {
            
        }

        return View(loginViewModel);
    }
}