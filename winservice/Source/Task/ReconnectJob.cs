using Aula_Dagtilbud_AD_Integration.WebSockets;
using Quartz;

namespace Aula_Dagtilbud_AD_Integration.Task
{
    [DisallowConcurrentExecution]
    public class ReconnectJob : IJob
    {
        public WSCommunication webSocket { get; set; }

        public System.Threading.Tasks.Task Execute(IJobExecutionContext context)
        {
            webSocket.Connect();

            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}
