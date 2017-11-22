using System.Web.Mvc;
using Composite.AspNet.MvcFunctions;
using Composite.Core.Application;
using Controllers;

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

        functions.RegisterAction<EBookController>("UploadFile", "Demo.Ebook.UploadFile");
        functions.RegisterAction<EBookController>("UploadFiles", "Demo.Ebook.Upload File List");
        functions.RegisterAction<EBookController>("Detail", "Demo.Ebook.Detail");
    }

    public static void OnInitialized()
    { }
}