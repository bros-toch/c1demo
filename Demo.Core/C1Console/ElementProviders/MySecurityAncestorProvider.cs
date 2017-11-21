using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Composite.C1Console.Security;
using Demo.Core.C1Console.ElementProviders.EntityTokens;

namespace Demo.Core.C1Console.ElementProviders
{
    public sealed class MySecurityAncestorProvider : ISecurityAncestorProvider
    {
        public IEnumerable<EntityToken> GetParents(EntityToken entityToken)
        {
            yield return new MyRootEntityToken();
        }
    }
}
