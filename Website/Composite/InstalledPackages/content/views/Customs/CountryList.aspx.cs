using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Composite.Data;
using Composite.Data.DynamicTypes;
using Composite.Data.Plugins.DataProvider;
using Composite.Data.Types;
using Composite.C1Console.Security;
using Composite.Core.Types;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Composite.Core.Xml;
using Composite.Core.Configuration;
using Composite.C1Console.Events;
using System.IO;
using Composite.Core.IO;
using Composite.Core.Application;
using Composite;
using Composite.C1Console.Users;
using System.Configuration;
using System.Web.Configuration;
using System.Xml.Xsl;
using System.Xml;
using System.Web.Hosting;
using Composite.C1Console.Workflow;
using Composite.Core.WebClient;
using Consoles;
using Consoles.EntityTokens;
using Demo;

public partial class CountryList : System.Web.UI.Page
{
	public class EditLink
	{
	    public string Link { get; set; }
        public string Label { get; set; }
	}
	private static object _lock = new object();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!UserValidationFacade.IsLoggedIn())
        {
            Response.Redirect("/Composite/Login.aspx?ReturnUrl=" + Request.Url.PathAndQuery);
            return;
        }
        ScriptLoader.Render("sub");


        rptCountryList.DataSource = DataFacade.GetData<Country>().Take(10)
            .ToArray()
            .Select(x =>
            {
                var editLink = new EditLink()
                {
                    Label = x.Name
                };

                if (DataScopeManager.CurrentDataScope != DataScopeIdentifier.Administrated)
                {
                    editLink.Link = String.Empty;
                    return editLink;
                }

                var teaser = x;

                //var editWorkflowAttribute = teaser.DataSourceId.InterfaceType.GetCustomAttributes(true).OfType<EditWorkflowAttribute>().FirstOrDefault();
                //if (editWorkflowAttribute == null)
                //{
                //    editLink.Link = string.Empty;
                //    return editLink;
                //}

                
                var page = PageManager.GetPageById(Guid.Parse("63ec1a73-b1ed-4ec8-923f-2840448c43ce"));
                var entityToken = new PageTeaserInstanceEntityToken(page, teaser);
                var serializedEntityToken = EntityTokenSerializer.Serialize(entityToken, true);

                var editActionToken = new WorkflowActionToken(typeof(EditWorkflowAttribute));
                var serializedActionToken = ActionTokenSerializer.Serialize(editActionToken, true);

                var html = String.Format("<a href=\"#\" data-providername='{0}' data-entitytoken='{1}' data-actiontoken=\"{2}\" data-piggybag='{3}' data-piggybaghash='{4}' onclick=\"executeAction(this)\">{5}</a>",
                    teaser.DataSourceId.ProviderName,
                    serializedEntityToken,
                    serializedActionToken,
                    String.Empty,
                    HashSigner.GetSignedHash(string.Empty).Serialize(),
                    "edit");

                editLink.Link = html;
                return editLink;
            });


        rptCountryList.DataBind();


    }

    protected void rptCountryList_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
                ((Literal) e.Item.FindControl("ltLink")).Text = ((EditLink) e.Item.DataItem).Link;
        }
    }
}