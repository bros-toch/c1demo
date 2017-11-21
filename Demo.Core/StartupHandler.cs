using Composite.C1Console.Security;
using Composite.Core.Application;
using Composite.Data;
using Demo.Core.C1Console.ElementProviders;
using Demo.Core.C1Console.ElementProviders.EntityTokens;

namespace Demo.Core
{
    [ApplicationStartup]
    internal static class StartupHandler
    {
        public static void OnBeforeInitialize()
        {
        }

        public static void OnInitialized()
        {
            AuxiliarySecurityAncestorFacade.AddAuxiliaryAncestorProvider<MyRootEntityToken>(
                new MyRootEntityTokenAuxiliarySecurityAncestorProvider());

            AuxiliarySecurityAncestorFacade.AddAuxiliaryAncestorProvider<DataEntityToken>(
                new MyDataEntityTokenAuxiliarySecurityAncestorProvider());
        }
    }
}