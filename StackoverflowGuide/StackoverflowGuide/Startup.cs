using AspNetCore.Identity.Mongo;
using AspNetCore.Identity.Mongo.Model;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using StackoverflowGuide.API.Mapping;
using StackoverflowGuide.BLL.Models.User;
using StackoverflowGuide.BLL.Services;
using StackoverflowGuide.BLL.Services.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace StackoverflowGuide
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
            var passwordOptions = new PasswordOptions()
            {
                RequiredLength = 8,
                RequireLowercase = false,
                RequireUppercase = false,
                RequireNonAlphanumeric = false,
                RequireDigit = false
            };
            
            services.AddIdentityMongoDbProvider<User, MongoRole>
                (
                    identityOptions => { identityOptions.Password = passwordOptions; },
                    mongoIdentityOptions =>
                    {
                        mongoIdentityOptions.ConnectionString = Configuration.GetConnectionString("MongoDbDatabase");
                    });

            // Add Jwt Authentication
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services.AddAuthentication(options =>
            {
                //Set default Authentication Schema as Bearer
                options.DefaultAuthenticateScheme =
                           JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme =
                           JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme =
                           JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters =
                       new TokenValidationParameters
                       {
                           ValidIssuer = Configuration["JwtIssuer"],
                           ValidAudience = Configuration["JwtIssuer"],
                           IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                           ClockSkew = TimeSpan.Zero // remove delay of token when expire
                       };
            });

            services.AddControllersWithViews();

            var serviceProvider = services.BuildServiceProvider();
            var userManager = serviceProvider.GetService<UserManager<User>>();
            var signInManager = serviceProvider.GetService<SignInManager<User>>();

            services.AddSingleton<IAuthService>(
                new AuthService(
                    Configuration.GetValue<string>("JwtKey"),
                    Configuration.GetValue<int>("JwtExpireDays"),
                    userManager,
                    signInManager
                )
            );

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            var mappingConfig = new MapperConfiguration(mc =>
               mc.AddProfile(new MappingProfile())
            );
            services.AddSingleton(mappingConfig.CreateMapper());

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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
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
