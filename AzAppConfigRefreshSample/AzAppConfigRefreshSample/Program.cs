using AzAppConfigRefreshSample;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>(optional: true);
builder.Configuration.AddEnvironmentVariables();

var connectionString = builder.Configuration.GetConnectionString("AzureAppConfiguration");

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    //Connect to your App Config Store using the connection string
    config.AddAzureAppConfiguration(options =>
        options.Connect(connectionString)
            // Load configuration values with no label
            .Select("Config:*", LabelFilter.Null)
            // Override with any configuration values specific to current hosting env
            .Select("Config:*", hostingContext.HostingEnvironment.EnvironmentName)
            .ConfigureRefresh(refresh =>
            {
                refresh.Register("Config:Update", refreshAll: true)
                    .SetCacheExpiration(new System.TimeSpan(0, 0, 5));
            }));
    }
);

// Add services to the container.
builder.Services.AddRazorPages();

//var conf = new Config();
//builder.Configuration.GetSection("Config").Bind(conf);

builder.Services.Configure<Config>(
    builder.Configuration.GetSection("Config"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
