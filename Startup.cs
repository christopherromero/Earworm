using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Earworm
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
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            services.AddMvc();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "Spotify";
            })
              .AddCookie(options =>
              {
                  options.Cookie.Name = "EarwormCookie";
                  options.ExpireTimeSpan = TimeSpan.FromHours(24);
                  options.SlidingExpiration = true;
              })
              .AddOAuth("Spotify", options =>
              {
                  options.ClientId = Configuration["Spotify:ClientId"];
                  options.ClientSecret = Configuration["Spotify:ClientSecret"];
                  options.CallbackPath = new PathString("/callback");

                  options.AuthorizationEndpoint = "https://accounts.spotify.com/authorize";
                  options.TokenEndpoint = "https://accounts.spotify.com/api/token";
                  options.Scope.Add("user-read-email");
                  options.Scope.Add("user-read-private");
                  options.Scope.Add("user-top-read");
                  options.Scope.Add("user-read-playback-state");
                  options.Scope.Add("user-read-currently-playing");
                  options.Scope.Add("user-modify-playback-state");
                  options.Scope.Add("streaming");
                  options.SaveTokens = true;
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
