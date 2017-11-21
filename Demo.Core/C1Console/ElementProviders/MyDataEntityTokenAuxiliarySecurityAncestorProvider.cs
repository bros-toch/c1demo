using System;
using System.Collections.Generic;
using Composite.C1Console.Security;
using Composite.Core.Types;
using Composite.Data;
using Composite.Data.Types;
using Demo.Core.C1Console.ElementProviders.EntityTokens;

namespace Demo.Core.C1Console.ElementProviders
{
    public sealed class MyDataEntityTokenAuxiliarySecurityAncestorProvider :
        IAuxiliarySecurityAncestorProvider
    {
        public Dictionary<EntityToken, IEnumerable<EntityToken>> GetParents(
            IEnumerable<EntityToken> entityTokens)
        {
            Dictionary<EntityToken, IEnumerable<EntityToken>> result =
                new Dictionary<EntityToken, IEnumerable<EntityToken>>();
            foreach (EntityToken entityToken in entityTokens)
            {
                DataEntityToken dataEntityToken = entityToken as DataEntityToken;
                if (dataEntityToken.Data == null) continue;
                Type interfaceType = TypeManager.GetType(dataEntityToken.Type);
                if (interfaceType != typeof(IPage)) continue;
                IPage page = dataEntityToken.Data as IPage;
                if (page.PublicationStatus == "awaitingApproval")
                {
                    result.Add(entityToken, new MyEntityToken[] { new MyEntityToken("Approval") });
                }
                else if (page.PublicationStatus == "awaitingPublication")
                {
                    result.Add(entityToken, new MyEntityToken[] { new MyEntityToken("Publication") });
                }
            }
            return result;
        }
    }
}
