using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Shiny;
using Shiny.Jobs;

namespace ApproxiMate
{
    public class MyStartup : Shiny.ShinyStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.UseNotifications(true);
            services.RegisterJob(new JobInfo(typeof(NotificationJob), nameof(NotificationJob))
            {
                RequiredInternetAccess = InternetAccess.Any
            });
        }
    }
}
