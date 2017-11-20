using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using FluentValidation;
using FluentValidation.Mvc;
using FluentValidation.Resources;
using FluentValidation.Results;
using Models;

/// <summary>
/// Summary description for AccountController
/// </summary>
public class AccountController : Controller
{
    public class MyResources
    {
        public static string email_error
        {
            get { return "{PropertyName}"; }
        }
        public static string equal_error
        {
            get { return "{PropertyName}"; }
        }
        public static string exact_length_error
        {
            get { return "{PropertyName}"; }
        }
        public static string exclusivebetween_error
        {
            get { return "{PropertyName}"; }
        }
        public static string greaterthan_error
        {
            get { return "{PropertyName}"; }
        }
        public static string greaterthanorequal_error
        {
            get { return "{PropertyName}"; }
        }
        public static string inclusivebetween_error
        {
            get { return "{PropertyName}"; }
        }
        public static string length_error
        {
            get { return "{PropertyName}"; }
        }
        public static string lessthan_error
        {
            get { return "{PropertyName}"; }
        }
        public static string lessthanorequal_error
        {
            get { return "{PropertyName}"; }
        }
        public static string notempty_error
        {
            get { return "{PropertyName}"; }
        }
        public static string notequal_error
        {
            get { return "{PropertyName}"; }
        }
        public static string notnull_error
        {
            get { return "{PropertyName}"; }
        }
        public static string predicate_error
        {
            get { return "{PropertyName}"; }
        }
        public static string regex_error
        {
            get { return "{PropertyName}"; }
        }
    }

    //public class CustomLanguageManager : FluentValidation.Resources.LanguageManager
    //{
    //    public CustomLanguageManager()
    //    {
    //        AddTranslation("en", "NotNullValidator", "'{PropertyName}' is required.");
    //    }
    //}

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
    public ActionResult Login([CustomizeValidator]LoginViewModel loginViewModel)
    {
        //ValidatorOptions.LanguageManager = new CustomLanguageManager();
        if (ModelState.IsValid)
        {
            
        }
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(string.Format("{0}-{1}", "zh", "CN"));

        Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
        //ValidatorOptions.LanguageManager = new LanguageManager() {Culture = Thread.CurrentThread.CurrentCulture};
        ValidatorOptions.LanguageManager.Enabled = true;
        ValidatorOptions.ResourceProviderType = typeof(MyResources);

        var validator = new LoginViewModelValidator();
        ValidationResult result = validator.Validate(loginViewModel);
        result.AddToModelState(ModelState, null);

        return View(loginViewModel);
    }
}