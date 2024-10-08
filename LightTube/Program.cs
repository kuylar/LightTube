using System.Globalization;
using InnerTube;
using LightTube;
using LightTube.Chores;
using LightTube.Database;
using LightTube.Localization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Events;


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Configuration.InitConfig();
LocalizationManager.Init();
try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());

    // Add services to the container.
    builder.Services
        .AddControllersWithViews()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.Converters.Add(new StringEnumConverter(new DefaultNamingStrategy(), false));
        });

    InnerTubeAuthorization? auth = Configuration.InnerTubeAuthorization;
    builder.Services.AddSingleton(new SimpleInnerTubeClient(new InnerTubeConfiguration
    {
        Authorization = auth,
        CacheSize = Configuration.CacheSize,
        CacheExpirationPollingInterval = default
    }));
    builder.Services.AddSingleton(new HttpClient());

    await JsCache.DownloadLibraries();
    ChoreManager.RegisterChores();
    DatabaseManager.Init(Configuration.ConnectionString);

    WebApplication app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
        app.UseHttpsRedirection();
    }

    app.UseStaticFiles(new StaticFileOptions
    {
        OnPrepareResponse = ctx =>
        {
            // Cache static files for 3 days
            ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=259200");
            ctx.Context.Response.Headers.Append("Expires",
                DateTime.UtcNow.AddDays(3).ToString("R", CultureInfo.InvariantCulture));
        }
    });

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}