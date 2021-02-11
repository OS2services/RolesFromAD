using System;
using Aula_Dagtilbud_AD_Integration.Task;
using Quartz;
using StructureMap;
using Topshelf;
using Topshelf.Quartz.StructureMap;
using Topshelf.StructureMap;

namespace Aula_Dagtilbud_AD_Integration
{
    class Program
    {
        static void Main(string[] args)
        {
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            HostFactory.Run(configure =>
            {
                configure.UseStructureMap(new Container(c =>
                {
                    c.AddRegistry(new DefaultRegistry());
                }));

                configure.Service<Service>(service =>
                {
                    service.ConstructUsingStructureMap();
                    service.UseQuartzStructureMap();

                    service.ScheduleQuartzJob(q =>
                        q.WithJob(() =>
                            JobBuilder.Create<ReconnectJob>().Build())
                            .AddTrigger(() => TriggerBuilder.Create()
                                                            .WithSimpleSchedule(b => b.WithIntervalInSeconds(30).RepeatForever())
                                                            .Build())
                    );
                });

                configure.RunAsLocalSystem();
                configure.SetServiceName("Aula Dagtilbud AD Integration");
                configure.SetDisplayName("Aula Dagtilbud AD Integration");
                configure.SetDescription("Lookup of AD group memberships for AULA Dagtilbud");
            });
        }
    }
}
