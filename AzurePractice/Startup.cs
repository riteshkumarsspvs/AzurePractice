using Azure.Identity;
using AzurePractice.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace AzurePractice
{
    public class Startup
    {       
        private const string keyVaultUrl = "https://keyvaultpractice.vault.azure.net/";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var clientId = Configuration.GetValue<string>("ClientId");
            var tenantId = Configuration.GetValue<string>("TenantId");
            var secretKey = Configuration.GetValue<string>("SecretKey");

            services.AddAuthentication(o =>
               {
                   o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                   o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
               })
               .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));

            services.AddAzureClients(option=> {
                option.AddSecretClient(new System.Uri(keyVaultUrl));
                //option.UseCredential(new EnvironmentCredential());
                //option.UseCredential(new DefaultAzureCredential());
                option.UseCredential(new ClientSecretCredential(tenantId, clientId, secretKey));
            });

            services.AddTransient<IKeyVaultManager, KeyVaultManager>();

            services.AddControllersWithViews()
                .AddMicrosoftIdentityUI(); 
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
