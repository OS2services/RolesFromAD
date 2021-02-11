using Aula_Dagtilbud_AD_Integration.ActiveDirectory;
using Aula_Dagtilbud_AD_Integration.Task;
using Aula_Dagtilbud_AD_Integration.WebSockets;
using StructureMap;

namespace Aula_Dagtilbud_AD_Integration
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            Scan(_ =>
            {
                _.WithDefaultConventions();
            });

            For<WSCommunication>().Singleton();

            Policies.FillAllPropertiesOfType<WSCommunication>();
            Policies.FillAllPropertiesOfType<ADStub>();
            Policies.FillAllPropertiesOfType<ReconnectJob>();

            Policies.Add<LoggingForClassPolicy>();
        }
    }
}