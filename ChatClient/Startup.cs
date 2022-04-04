using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ChatClient
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services
                .AddSingleton<ChatRoom>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    System.Reflection.AssemblyName current = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                    string appVersion = $"{current.Name} v.{current.Version}";
                    await context.Response.WriteAsync(appVersion);
                });
            });
        }
    }
}
