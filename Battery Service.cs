using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace BatteryManagement
{

    public class Battery_Management : IHostedService, IDisposable
    {
        
        private readonly ILogger<Battery_Management> _logger;
        private System.Threading.Timer _timer;

        public Battery_Management(ILogger<Battery_Management> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            var notification = new System.Windows.Forms.NotifyIcon()
            {
                Visible = true,
                Icon = System.Drawing.SystemIcons.Information,
                // optional - BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info,
                BalloonTipTitle = "Battery Management",
                BalloonTipText = "The battery is being monitorized",
            };

            // Display for 5 seconds.
            notification.ShowBalloonTip(5000);

            // This will let the balloon close after it's 5 second timeout
            // for demonstration purposes. Comment this out to see what happens
            // when dispose is called while a balloon is still visible.
            Thread.Sleep(10000);

            // The notification should be disposed when you don't need it anymore,
            // but doing so will immediately close the balloon if it's visible.
            notification.Dispose();

            _timer = new Timer(CheckBattery, null, TimeSpan.Zero,TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }
        

        private void CheckBattery(object state)
        {
            System.Windows.Forms.PowerStatus pw = System.Windows.Forms.SystemInformation.PowerStatus;

            if (pw.BatteryLifeRemaining >= 80)
            {
                var notification = new System.Windows.Forms.NotifyIcon()
                {
                    Visible = true,
                    Icon = System.Drawing.SystemIcons.Information,
                    // optional - BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info,
                    BalloonTipTitle = "Battery Management",
                    BalloonTipText = "The battery is fully charged",
                };

                // Display for 5 seconds.
                notification.ShowBalloonTip(5000);

                // This will let the balloon close after it's 5 second timeout
                // for demonstration purposes. Comment this out to see what happens
                // when dispose is called while a balloon is still visible.
                Thread.Sleep(10000);

                // The notification should be disposed when you don't need it anymore,
                // but doing so will immediately close the balloon if it's visible.
                notification.Dispose();

            }
            else {
                _logger.LogInformation("Battery hasn't reached the required charge level.");
            }
            

        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
