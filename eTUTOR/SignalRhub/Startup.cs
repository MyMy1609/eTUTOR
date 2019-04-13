﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(whiteboardEtutor.SignalRhub.Startup))]

namespace whiteboardEtutor.SignalRhub
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                map.MapSignalR();
                map.MapHubs();
                var hubConfiguration = new HubConfiguration { };
                map.RunSignalR(hubConfiguration);
            });
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
