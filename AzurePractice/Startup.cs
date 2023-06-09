using Azure.Core;
using Azure.Identity;
using AzurePractice.Services;
using AzureSubscription;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
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
        private const string keyVaultKeyUrl = "https://keyvaultpractice.vault.azure.net/keys/AzurePracKey/d2c09ba1b4fa4d118119db94c37a40aa";
        private const string keyStorageUrl = "https://storagepracticerits.blob.core.windows.net";

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

            services.AddAzureClients(option =>
            {
                option.AddSecretClient(new System.Uri(keyVaultUrl));
                option.AddKeyClient(new System.Uri(keyVaultUrl));
                option.AddBlobServiceClient(new System.Uri(keyStorageUrl));
                //option.AddServiceBusClient("Endpoint=sb://servicebusrits.servicebus.windows.net/;SharedAccessKeyName=ListenQueue;SharedAccessKey=K73rYO734f0YmiM1qvv8o1FB4CVYktaxn+ASbJr8lx4=;EntityPath=servicebusqueue");
                option.AddServiceBusClientWithNamespace("ServiceBusRits.servicebus.windows.net");
                //option.UseCredential(new EnvironmentCredential());
                //option.UseCredential(new DefaultAzureCredential());
                option.UseCredential(new ClientSecretCredential(tenantId, clientId, secretKey));
            });

            //Dependency Injection
            services.AddTransient<IKeyVaultManager, KeyVaultManager>();
            services.AddTransient<IBlobManager, BlobManager>();
            services.AddTransient<IServiceBusQueue, ServiceBusQueue>();
            services.AddTransient<IServiceBusTopics, ServiceBusTopics>();
            services.AddSingleton<TokenCredential, ClientSecretCredential>((serviceProvider =>
            {
                return new ClientSecretCredential(tenantId, clientId, secretKey);
            }));

            /////Register Subscription
            ///
            var subscription = new Subscription(new Azure.Messaging.ServiceBus.ServiceBusClient("ServiceBusRits.servicebus.windows.net", new ClientSecretCredential(tenantId, clientId, secretKey)));

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                              ForwardedHeaders.XForwardedProto;
                // Only loopback proxies are allowed by default.
                // Clear that restriction because forwarders are enabled by explicit
                // configuration.
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

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
            app.UseForwardedHeaders();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseRouting();
            //app.UseAuthentication();
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
