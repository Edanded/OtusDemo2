using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Otus.Signalr.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;

namespace Otus.Signalr
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();


            // Добавляем SignalR
            services.AddSignalR();


            services.AddSingleton<IUserIdProvider, SignalUserProvider>();


            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

              services.AddAuthentication(options =>
        {
            // Identity made Cookie authentication the default.
            // However, we want JWT Bearer Auth to be the default.
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            // Configure the Authority to the expected value for your authentication provider
            // This ensures the token is appropriately validated

            // We have to hook the OnMessageReceived event in order to
            // allow the JWT authentication handler to read the access
            // token from the query string when a WebSocket or 
            // Server-Sent Events request comes in.

            // Sending the access token in the query string is required due to
            // a limitation in Browser APIs. We restrict it to only calls to the
            // SignalR hub in this code.
            // See https://docs.microsoft.com/aspnet/core/signalr/security#access-token-logging
            // for more information about security considerations when using
            // the query string to transmit the access token.
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                     var accessToken = context.Request.Query["access_token"];
                    

                    // If the request is for our hub...
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) &&
                        (path.StartsWithSegments("/hubs/chat")))
                    {
                        // Read the token out of the query string
                        context.Token = accessToken;
                    }
                    
                    return Task.CompletedTask;
                }
            };
        });


            services.AddCors(options =>
                            {
                                options.AddPolicy("ClientPermission", policy =>
                                {
                                    policy.AllowAnyHeader()
                                        .AllowAnyMethod()
                                        .WithOrigins("http://localhost:3000","http://localhost:5000")
                                        .AllowCredentials();
                                });
                            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
              //  app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseCors("ClientPermission");


            

            app.UseRouting();
app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapHub<ChatHub>("/hubs/chat");
            });



            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
