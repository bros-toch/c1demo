using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Composite.AspNet.MvcFunctions;
using Composite.Core.Application;

[ApplicationStartup]
internal static class StartupHandler
{
    public static void OnBeforeInitialize()
    {
        var functions = MvcFunctionRegistry.NewFunctionCollection();

        functions.RouteCollection.MapMvcAttributeRoutes();
        functions.RouteCollection.MapRoute(
            "Default",
            "{controller}/{action}/{id}",
            new { action = "Index", id = UrlParameter.Optional }
        );

        functions.RegisterAction<AccountController>("Login", "Demo.Account.Login");
    }

    public static void OnInitialized()
    { }
}