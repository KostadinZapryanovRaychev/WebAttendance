using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebAttendance.Areas.Identity.Data;
using WebAttendance.Data.Models;

[assembly: HostingStartup(typeof(WebAttendance.Areas.Identity.IdentityHostingStartup))]
namespace WebAttendance.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {

        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}