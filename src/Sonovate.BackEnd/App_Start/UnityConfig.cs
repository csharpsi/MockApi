using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Sonovate.BackEnd.Repository;
using Unity.Mvc5;

namespace Sonovate.BackEnd
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<ISettingRepository, SettingRepository>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}