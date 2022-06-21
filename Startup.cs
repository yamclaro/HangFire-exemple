
using Hangfire;
using Hangfire.SqlServer;
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
         string connStr = (Configuration.GetConnectionString("DBConnection"));
         services.AddScoped<IjobTestService, JobTestService>();
         services.AddHangfireServer();
         services.AddHangfire(x =>
            x.UseColouredConsoleLogProvider()
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseResultsInContinuations()
                .UseSqlServerStorage(connStr, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(1),
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true,
                    EnableHeavyMigrations = true
                }));
      
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