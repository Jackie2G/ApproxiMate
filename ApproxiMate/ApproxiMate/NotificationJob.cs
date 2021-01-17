using Shiny.Jobs;
using Shiny.Notifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ApproxiMate
{
    //[XamlCompilation]
    public class NotificationJob : IJob
    {
        readonly INotificationManager notifications;
        IAuth auth;

        public NotificationJob(INotificationManager notifications)
        {
            this.notifications = notifications;
            //Xamarin.Forms.DependencyService.Register<IAuth>();
            //auth = DependencyService.Get<IAuth>();
        }

        public async Task<bool> Run(JobInfo jobInfo, CancellationToken cancelToken)
        {
            //var counter = await auth.GetMatches();
            //int counter = 0;
            auth = DependencyService.Get<IAuth>();
            var runJob = false;
            if (jobInfo.LastRunUtc == null)
            {
                runJob = true;
            }
            else if (DateTime.UtcNow > jobInfo.LastRunUtc.Value.AddMinutes(1))
            {
                runJob = true;
            }
            if (runJob)
            {
                await Task.Delay(10000);
                int counter = await auth.GetMatches();
                if (counter == 0)
                    await this.notifications.Send("New Matches", "No new matches.");
                else
                    await this.notifications.Send("New Matches", "You've got " + counter.ToString() + " new matches!");
                return true;
            }
            return true;
        }
    }
}
