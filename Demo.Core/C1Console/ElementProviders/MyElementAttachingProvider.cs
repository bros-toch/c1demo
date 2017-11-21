using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Composite.C1Console.Elements;
using Composite.C1Console.Elements.Plugins.ElementAttachingProvider;
using Composite.C1Console.Security;
using Composite.Core.ResourceSystem.Icons;
using Demo.Core.C1Console.ElementProviders.EntityTokens;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Demo.Core.C1Console.ElementProviders
{
    [ConfigurationElementType(typeof(NonConfigurableElementAttachingProvider))]
    public sealed class MyElementAttachingProvider : IElementAttachingProvider
    {
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
                    Label = "Page state overview",
                    ToolTip = "Page state overview",
                    HasChildren = true,
                    Icon = CommonElementIcons.Folder
                }
            };
        }
        public IEnumerable<Element> GetChildren(EntityToken parentEntityToken,
            Dictionary<string, string> piggybag)
        {
            throw new NotImplementedException("Will be later in the tutorial");
        }
    }
}
