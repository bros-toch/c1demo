using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Composite.C1Console.Elements;
using Composite.C1Console.Elements.Plugins.ElementAttachingProvider;
using Composite.C1Console.Security;
using Composite.C1Console.Workflow;
using Composite.Core.ResourceSystem;
using Composite.Core.ResourceSystem.Icons;
using Composite.Data;
using Composite.Data.Types;
using Composite.Plugins.Elements.ElementProviders.PageElementProvider;
using Demo.Core.C1Console.ElementProviders.Actions;
using Demo.Core.C1Console.ElementProviders.EntityTokens;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Demo.Core.C1Console.ElementProviders
{
    [ConfigurationElementType(typeof(NonConfigurableElementAttachingProvider))]
    public sealed class MyElementAttachingProvider : IElementAttachingProvider
    {
        private static ResourceHandle DataAwaitingApproval
        {
            get
            {
                return GetIconHandle("page-awaiting-approval");
            }
        }
        private static ResourceHandle DataAwaitingPublication
        {
            get
            {
                return GetIconHandle("page-awaiting-publication");
            }
        }

        public ElementProviderContext Context
        {
            get;
            set;
        }
        public bool HaveCustomChildElements(EntityToken parentEntityToken,
            Dictionary<string, string> piggybag)
        {
            if (ElementAttachingPointFacade.IsAttachingPoint(parentEntityToken,
                    AttachingPoint.ContentPerspective) == false) return false;
            return true;
        }
        public ElementAttachingProviderResult GetAlternateElementList(
            EntityToken parentEntityToken,
            Dictionary<string, string> piggybag)
        {
            if (ElementAttachingPointFacade.IsAttachingPoint(parentEntityToken,
                    AttachingPoint.ContentPerspective) == false) return null;
            ElementAttachingProviderResult result = new ElementAttachingProviderResult()
            {
                Elements = GetRootElements(piggybag),
                Position = ElementAttachingProviderPosition.Bottom,
                PositionPriority = 0
            };
            return result;
        }
        private IEnumerable<Element> GetRootElements(Dictionary<string, string> piggybag)
        {
            yield return new Element(this.Context.CreateElementHandle(new MyRootEntityToken()))
            {
                VisualData = new ElementVisualizedData
                {
                    Label = "Page State Overview",
                    ToolTip = "Page State Overview",
                    HasChildren = true,
                    Icon = CommonElementIcons.Folder
                }
            };

            //yield return new Element(this.Context.CreateElementHandle(new UsersRootEntityToken()))
            //{
            //    VisualData = new ElementVisualizedData
            //    {
            //        Label = "Users",
            //        ToolTip = "Users",
            //        HasChildren = true,
            //        Icon = CommonElementIcons.Folder
            //    }
            //};
        }
        public IEnumerable<Element> GetChildren(EntityToken parentEntityToken,
            Dictionary<string, string> piggybag)
        {
            using (DataConnection connection = new DataConnection())
            {
                if ((parentEntityToken is MyRootEntityToken) == true)
                {
                    yield return new Element(this.Context.CreateElementHandle(new MyEntityToken("Approval")))
                    {
                        VisualData = new ElementVisualizedData
                        {
                            Label = "Pages awaiting approval",
                            ToolTip = "Pages awaiting approval",
                            HasChildren = connection.
                                Get<IPage>().
                                Any(f => f.PublicationStatus == "awaitingApproval"),
                            Icon = CommonElementIcons.Search
                        }
                    };
                    yield return new Element(this.Context.CreateElementHandle(
                        new MyEntityToken("Publication")))
                    {
                        VisualData = new ElementVisualizedData
                        {
                            Label = "Pages awaiting publication",
                            ToolTip = "Pages awaiting publication",
                            HasChildren = connection.
                                Get<IPage>().
                                Any(f => f.PublicationStatus == "awaitingPublication"),
                            Icon = CommonElementIcons.Search
                        }
                    };
                }
                else
                {
                    MyEntityToken myEntityToken = parentEntityToken as MyEntityToken;
                    if (myEntityToken == null) throw new NotImplementedException();
                    IEnumerable<IPage> pages;
                    ResourceHandle icon;
                    if (myEntityToken.Id == "Approval")
                    {
                        pages = connection.Get<IPage>().
                            Where(f => f.PublicationStatus == "awaitingApproval");
                        icon = DataAwaitingApproval;
                    }
                    else if (myEntityToken.Id == "Publication")
                    {
                        pages = connection.Get<IPage>().
                            Where(f => f.PublicationStatus == "awaitingPublication");
                        icon = DataAwaitingPublication;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                    foreach (IPage page in pages)
                    {
                        Element element =
                            new Element(this.Context.CreateElementHandle(page.GetDataEntityToken()))
                            {
                                VisualData = new ElementVisualizedData
                                {
                                    Label = page.Title,
                                    ToolTip = page.Title,
                                    HasChildren = false,
                                    Icon = icon
                                }
                            };
                        element.AddAction(new ElementAction(new ActionHandle(new MyUrlActionToken()))
                        {
                            VisualData = new ActionVisualizedData
                            {
                                Label = "My Url Action",
                                ToolTip = "My Url Action",
                                Icon = CommonCommandIcons.ShowReport,
                                ActionLocation = new ActionLocation
                                {
                                    ActionType = ActionType.Other,
                                    IsInFolder = false,
                                    IsInToolbar = true,
                                    ActionGroup = new ActionGroup(ActionGroupPriority.PrimaryHigh)
                                }
                            }
                        });
                        
                        element.AddAction(
                            new ElementAction(new ActionHandle(new WorkflowActionToken(
                                WorkflowFacade.GetWorkflowType(typeof(EditPageWorkflow).FullName),
                                new PermissionType[] { PermissionType.Administrate })))
                            {
                                VisualData = new ActionVisualizedData
                                {
                                    Label = "Edit page",
                                    ToolTip = "Eidt page",
                                    Icon = icon,
                                    Disabled = false,
                                    ActionLocation = new ActionLocation
                                    {
                                        ActionType = ActionType.Edit,
                                        IsInFolder = false,
                                        IsInToolbar = true,
                                        ActionGroup = new ActionGroup(ActionGroupPriority.PrimaryHigh)
                                    }
                                }
                            });
                        yield return element;
                    }
                }
            }
        }

        private static ResourceHandle GetIconHandle(string name)
        {
            return new ResourceHandle(BuildInIconProviderName.ProviderName, name);
        }
    }
}
