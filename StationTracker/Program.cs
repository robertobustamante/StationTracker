using System;
using Topshelf;

namespace StationTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            var exitCode = HostFactory.Run(x =>
           {
               x.Service<ServiceTask>(s =>
               {
                   s.ConstructUsing(servicetask => new ServiceTask());
                   s.WhenStarted(servicetask => servicetask.Start());
                   s.WhenStopped(servicetask => servicetask.Stop());
               });

               x.RunAsLocalSystem();

               x.SetServiceName("StationTracker");
               x.SetDisplayName("Station Tracker");
               x.SetDescription("Servicio que sincroniza las bases de datos de acces con el servicio en nube de Azure de las estaciones de servicio");
           });
        }
    }
}
