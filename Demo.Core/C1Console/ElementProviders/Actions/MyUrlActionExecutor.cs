using System;
using Composite.C1Console.Actions;
using Composite.C1Console.Events;
using Composite.C1Console.Security;
using Composite.Data;
using Composite.Data.Types;

namespace Demo.Core.C1Console.ElementProviders.Actions
{
    public sealed class MyUrlActionExecutor : IActionExecutor
    {
        public FlowToken Execute(
            EntityToken entityToken,
            ActionToken actionToken,
            FlowControllerServicesContainer flowControllerServicesContainer)
        {
            string currentConsoleId = flowControllerServicesContainer.
                GetService<IManagementConsoleMessageService>().CurrentConsoleId;
            DataEntityToken dataEntityToken = entityToken as DataEntityToken;
            IPage page = dataEntityToken.Data as IPage;
            string url = string.Format("/MyUrlAction.aspx?pageId={0}", page.Id);
            ConsoleMessageQueueFacade.Enqueue(new OpenViewMessageQueueItem
            {
                Url = url,
                ViewId = Guid.NewGuid().ToString(),
                ViewType = ViewType.Main,
                Label = "My URL Action"
            }, currentConsoleId);
            return null;
        }
    }
}