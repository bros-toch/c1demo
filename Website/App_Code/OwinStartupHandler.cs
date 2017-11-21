using System;
using System.Threading.Tasks;
using Composite.Core;
using CompositeC1Contrib.Email;
using CompositeC1Contrib.ScheduledTasks;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(OwinStartupHandler))]

public class OwinStartupHandler
{
    public void Configuration(IAppBuilder app)
    {
        app.UseCompositeC1ContribScheduledTasks(options =>
        {
            options.JobStorage = new SqlServerStorage("c1");
        });
        app.UseHangfireDashboard();
        
        app.UseCompositeC1ContribEmail();
    }
}
