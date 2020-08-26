using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using UserAuthManager.API.Data;
using UserAuthManager.API.Extentions;
using UserAuthManager.API.Mapping;
using UserAuthManager.API.Models;
using UserAuthManager.API.Services;
using UserAuthManager.API.Services.Interfaces;
using UserAuthManager.Core.Models;

namespace UserAuthManager.API
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
            services.AddDbContext<UserManagerDbContext>(options =>
                options.UseMySql(Configuration["Databases:MySql:ConnectionString"]));
            
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password = new PasswordOptions()
                    {
                        RequireDigit = false,
                        RequiredLength = 4,
                        RequireLowercase = false,
                        RequireUppercase = false,
                        RequireNonAlphanumeric = false
                    };
                })
                .AddEntityFrameworkStores<UserManagerDbContext>()
                .AddDefaultTokenProviders();
            
            var authOptionsConfig = Configuration.GetSection("Auth");
            
            services.AddServices();
            
            services.AddAuthJwt(authOptionsConfig);
            
            services.AutoMapperConfig();
            
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
            
            services.AddControllers().AddFluentValidation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();
 
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            //create db
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<UserManagerDbContext>();
            context.Database.EnsureCreated();
            
            //test data
            InitData.SeedData(serviceProvider);
        }
    }
}