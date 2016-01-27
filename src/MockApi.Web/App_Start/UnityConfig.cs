using System.Web.Hosting;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using MockApi.Web.Data;
using MockApi.Web.Repository;
using Unity.Mvc5;

namespace MockApi.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container
                .RegisterType<IDataContext, DataContext>(new InjectionConstructor(HostingEnvironment.MapPath("~/App_Data/MockApi.db")))
                .RegisterType<IMockRepository, MockRepository>(new InjectionConstructor(typeof(IDataContext)));
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}