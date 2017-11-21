using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Composite.C1Console.Security;
using Composite.Data;
using Composite.Data.Types;
using Composite.C1Console.Users;
using Composite.C1Console.Workflow;
using Composite.Plugins.Elements.ElementProviders.GeneratedDataTypesElementProvider;
using Composite.Plugins.Elements.ElementProviders.PageElementProvider;
using Consoles;
using Consoles.EntityTokens;
using Demo.Core.C1Console.ElementProviders.EntityTokens;


namespace AttachingCustomElementsSample
{
    public partial class MyUrlAction : System.Web.UI.Page
    {
        public string providerName = "";
        public string entityToken = "";
        public string actionToken = "";
        public string piggybag = "";
        public string piggybagHash = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string pageIdString = this.Request.QueryString["pageId"];

            if (pageIdString == null) throw new InvalidOperationException();

            Guid pageId = new Guid(pageIdString);

            //var editWorkflowAttribute = teaser.DataSourceId.InterfaceType.GetCustomAttributes(true).OfType<EditWorkflowAttribute>().FirstOrDefault();
            //if (editWorkflowAttribute == null)
            //{
            //    editLink.Link = string.Empty;
            //    return editLink;
            //}


            var pagex = PageManager.GetPageById(pageId);
            var _entityToken = new PageDataEntityToken(pagex);

            var serializedEntityToken = EntityTokenSerializer.Serialize(_entityToken, true);

            var editActionToken = new WorkflowActionToken(typeof(EditDataWorkflow));

            var token = new WorkflowActionToken(WorkflowFacade.GetWorkflowType(typeof(EditPageWorkflow).FullName));


            var serializedActionToken = ActionTokenSerializer.Serialize(token, true);
            
            //var m = ActionTokenSerializer.Deserialize(k);
            var x = ActionTokenSerializer.Deserialize(serializedActionToken);

            providerName = pagex.DataSourceId.ProviderName;
            entityToken = serializedEntityToken;
            actionToken = serializedActionToken;
            piggybag = string.Empty;
            piggybagHash = HashSigner.GetSignedHash(string.Empty).Serialize();

            using (new DataScope(DataScopeIdentifier.Administrated, UserSettings.ActiveLocaleCultureInfo))
            {
                 IPage page = PageManager.GetPageById(pageId);

                 this.MyPlaceHolder.Controls.Add(new LiteralControl("Title: " + page.Title + "<br />"));
            }
        }
    }
}
