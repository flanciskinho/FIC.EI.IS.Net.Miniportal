using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;

namespace Es.Udc.DotNet.MiniPortal.Test
{
    public class TestManager
    {

        public static IUnityContainer ConfigureUnityContainer(string sectionName)
        {
            IUnityContainer container = new UnityContainer();

            UnityConfigurationSection section =
                (UnityConfigurationSection)ConfigurationManager.GetSection(sectionName);

            section.Configure(container, section.Containers.Default.Name);

            return container;
        }


        public static void ClearUnityContainer(IUnityContainer container) {

            container.Dispose();
        }

    }
}
