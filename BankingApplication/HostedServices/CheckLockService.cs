using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BankingApplication.Data;
using BankingApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RepositoryWrapper;

namespace BankingApplication.HostedServices {
    //IHostedService example referenced from Microsoft Documentation
    //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-3.1&tabs=visual-studio
    public class CheckLockService : IHostedService, IDisposable {
        private int executionCount = 0;
        private readonly ILogger<BillPayService> _logger;
        private readonly IServiceScopeFactory _scopedFactory;
        private Wrapper _repo;
        private List<BillPay> bills = new List<BillPay>();
        private List<Login> logins = new List<Login>();
        private Timer _timer;
        private Task task;

        public CheckLockService (ILogger<BillPayService> logger, IServiceScopeFactory scopedFactory) {
            _logger = logger;
            _scopedFactory = scopedFactory;
        }

        public Task StartAsync (CancellationToken stoppingToken) {
            _logger.LogInformation ("Timed Hosted Service running.");

            _timer = new Timer (DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds (15));

            return Task.CompletedTask;
        }
        //DbContext scoping learnt from StackOverflow user alsami:
        //https://stackoverflow.com/questions/51572637/access-dbcontext-service-from-background-task
        private void DoWork (object state) {

            task = CheckLocks();
           
            var count = Interlocked.Increment (ref executionCount);
            _logger.LogInformation (
                "CheckLocks completed. Count: {Count}", count);
        }

        //Timed method for checking for locked accounts
        //Will unlock accounts if they exceed their set locked time (1 minute)
        private async Task CheckLocks()
        {
            using (var scope = _scopedFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<Wrapper>();
                _repo = repo;
                logins = await _repo.Login.GetLocked();
                foreach(var login in logins)
                {
                    if(login.LockoutTime < DateTime.UtcNow)
                    {
                        login.Locked = false;
                    }
                }
                await _repo.SaveChanges();
            }
        }

        public Task StopAsync (CancellationToken stoppingToken) {
            _logger.LogInformation ("Timed Hosted Service is stopping.");

            _timer?.Change (Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose () {
            _timer?.Dispose ();
        }
    }
}