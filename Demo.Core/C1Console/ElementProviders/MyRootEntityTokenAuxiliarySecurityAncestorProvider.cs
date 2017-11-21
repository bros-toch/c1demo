using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Composite.C1Console.Elements;
using Composite.C1Console.Security;
using Demo.Core.C1Console.ElementProviders.EntityTokens;

namespace Demo.Core.C1Console.ElementProviders
{
    public sealed class MyRootEntityTokenAuxiliarySecurityAncestorProvider : IAuxiliarySecurityAncestorProvider
    {
        public Dictionary<EntityToken, IEnumerable<EntityToken>>
            GetParents(IEnumerable<EntityToken> entityTokens)
        {
            Dictionary<EntityToken, IEnumerable<EntityToken>> result =
                new Dictionary<EntityToken, IEnumerable<EntityToken>>();
            foreach (EntityToken entityToken in entityTokens)
            {
                if (entityToken.GetType() == typeof(MyRootEntityToken))
                {
                    // Here we specify that the Content perspective element is the parent of our root element
                    result.Add(entityToken, new EntityToken[]
                    {
                        AttachingPoint.ContentPerspective.EntityToken
                    });
                }
            }
            return result;
        }
    }
}
