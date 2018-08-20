using System;
using System.Net.Http;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using IdentityService.IdentityServerProviders;
using IdentityService.IdentityServerProviders.Interfaces;
using IdentityService.IdentityServices;
using IdentityService.Models;
using IdentityService.Repository;
using IdentityService.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Identity
            var connectionString = $"{Configuration["ConnectionStrings:ServerAddress"]}{Configuration["ConnectionStrings:DatabaseName"]}";

            services.AddIdentity<ApplicationUser, Models.IdentityRole>(config =>
            {
                config.Password.RequiredLength = 8;
                config.User.RequireUniqueEmail = true;
                config.SignIn.RequireConfirmedEmail = true;
                config.SignIn.RequireConfirmedPhoneNumber = false;
            }).RegisterMongoStores<ApplicationUser, Models.IdentityRole>(connectionString).AddDefaultTokenProviders();

            var usesFacebook = !string.IsNullOrEmpty(Configuration["Authentication:Facebook:AppId"]) &&
                               !string.IsNullOrEmpty(Configuration["Authentication:Facebook:AppSecret"]);
            var usesGoogle = false;

            if (usesFacebook)
            {
                services.AddAuthentication()
                    .AddFacebook(facebookOptions =>
                    {
                        facebookOptions.AppId = Configuration["Authentication:Facebook:AppId"];
                        facebookOptions.AppSecret = Configuration["Authentication:Facebook:AppSecret"];
                        facebookOptions.SaveTokens = true;
                    });
            }
            else
                services.AddAuthentication();
            #endregion

            services.AddMvc()
                .AddViewLocalization(
                    LanguageViewLocationExpanderFormat.Suffix,
                    opts => { opts.ResourcesPath = "Resources"; })
                .AddDataAnnotationsLocalization();

            services.AddCors();

            #region IdentityServer
            IdentityConfig.Configuration = Configuration;

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(IdentityConfig.GetIdentityResources())
                .AddInMemoryApiResources(IdentityConfig.GetApiResources())
                .AddInMemoryClients(IdentityConfig.GetClients())
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<ProfileService>();

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddTransient<IProfileService, ProfileService>();

            services.AddTransient<IRepository<string, IdentityServicePersistedGrant>, Repository<string, IdentityServicePersistedGrant>>();
            services.AddScoped<IPersistedGrantStore, PersistedGrantStore>();

            services.AddScoped<IExtensionGrantValidator, ExternalAuthenticationGrant<ApplicationUser>>();
            services.AddSingleton<HttpClient>();

            services.AddScoped<IProviderRepository, ProviderRepository>();

            if (usesFacebook)
                services.AddTransient<IFacebookAuthProvider, FacebookAuthProvider<ApplicationUser>>();

            if (usesGoogle)
                services.AddTransient<IGoogleAuthProvider, GoogleAuthProvider<ApplicationUser>>();

            services.AddTransient<ICustomTokenRequestValidator, DefaultClientClaimsAdder>();
            #endregion

            #region Session
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
            });
            #endregion

            #region MongoDB
            var connection = Configuration.GetConnectionString("IdentityService");

            services.AddSingleton<IMongoDbContext>(new MongoDbContext(Configuration["ConnectionStrings:ServerAddress"], Configuration["ConnectionStrings:DatabaseName"]));
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();

                app.UseCors(builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseStaticFiles();
            app.UseIdentityServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
