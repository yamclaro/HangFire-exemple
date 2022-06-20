
using Hangfire;
using HangFire2.Models;
using Microsoft.OpenApi.Models;
using System;

namespace HangFire2;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
          services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "HangfireApplication", Version = "v1" });
        });
         services.AddScoped<IjobTestService, JobTestService>();
        services.AddHangfire(x =>
        {
            x.UseSqlServerStorage(Configuration.GetConnectionString("DBConnection"));
        });
        services.AddHangfireServer();
    }
    

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobs, IServiceProvider serviceProvider)
    {
        
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HangfireApplication v1"));
        }
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        app.UseHangfireDashboard();
    }
}