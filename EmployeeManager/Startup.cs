using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EmployeeManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using EmployeeManager.Security;
using Microsoft.Extensions.Options;

namespace EmployeeManager
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(
            options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
            

            services.Configure<IdentityOptions>(options => 
            {
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            });

            // Set [Authorize] attribute globally
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy", 
                    policy => policy.RequireClaim("Delete Role"));
                options.AddPolicy("EditRolePolicy",
                    policy => policy.RequireClaim("Edit Role"));
                options.AddPolicy("EditRoleCustomPolicy",
                    policy => policy.RequireAssertion(context => AuthorizeAction(context)));
                options.AddPolicy("EditRoleCustomComplexPolicy",
                    policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));
            });

            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "955381776449-4jdcspf39nfdd7is98hvbv222f36vmdi.apps.googleusercontent.com";
                options.ClientSecret = "rv5mLR9D6A8vyXfhd6CNPY4d";
            });

            services.AddAuthentication().AddFacebook(options =>
            {
                options.AppId = "176980770385359";
                options.AppSecret = "ab38ca8e994218549c1fa98d6c9e65f3";
            });

            //services.AddMvc().AddXmlSerializerFormatters();
            services.AddScoped<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddScoped<IEmployeeRepository, SqlEmployeeRepository>();
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
                app.UseHsts();
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            // app.UseMvcWithDefaultRoute();

            //app.UseMvc(routes => {
            //    routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            //});

            // Test
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // app.UseEndpoints(endpoints =>
            // {
            //     endpoints.MapGet("/", async context =>
            //     {
            //         await context.Response.WriteAsync("Hello world");
            //     });
            // });
            // app.Run(async (context) =>{
            //     await context.Response.WriteAsync("Hello world !!");  
            // });
        }

        private bool AuthorizeAction(AuthorizationHandlerContext context)
        {
            return (context.User.IsInRole("Admin") &&
            context.User.HasClaim(claim => claim.Type == "Edit Role") ||
            context.User.IsInRole("Super Admin"));
        }
    }
}
