using Asp.Versioning.ApiExplorer;
using Ch06.Aho.CityInfo.API.DbContexts;
using Ch06.Aho.CityInfo.API.Services;
using Ch06.Aho.CityInfo.API.Services.Repository;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

//[AHO] configuring Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfologs.txt", rollingInterval: RollingInterval.Hour)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
//[AHO] clear default providers
builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
//[AHO] register Serilog logger
builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers(options =>
{
    //[AHO] Not acceptable (Status code 406) when the requested format (Accept header) is not available
    options.ReturnHttpNotAcceptable = true;
    //[AHO] Explicit Output Formatterd
    options.OutputFormatters.Clear();
    options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter()); // The first registered Formatter => Default Formatter
    options.OutputFormatters.Add(new SystemTextJsonOutputFormatter(new JsonSerializerOptions
    {
        TypeInfoResolver = new DefaultJsonTypeInfoResolver()
    }));
}).AddNewtonsoftJson();

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = ctxt =>
    {
        ctxt.ProblemDetails.Extensions.Add("customInfo", "Take it or leave it!");
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
builder.Services.AddTransient<SimpleNotificationService>();
builder.Services.AddKeyedScoped<INotificationService, FancyNotificationService>("notifFancy");
builder.Services.AddKeyedScoped<INotificationService, ConfigurableNotificationService>("notifConfig");
builder.Services.AddDbContext<AhoCityInfoContext>(dbContextOptions
    => dbContextOptions.UseSqlite(builder.Configuration["ConnectionStrings:AhoCityInfoDbConnectionString"])
);
builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"]))
        };
    }
    );
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OnlyDevelopers", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("Role", "Dev");
    });
});
builder.Services.AddApiVersioning(setup =>
{
    setup.ReportApiVersions = true;
    setup.AssumeDefaultVersionWhenUnspecified = true;
    setup.DefaultApiVersion = new Asp.Versioning.ApiVersion(3.0);
}).AddMvc()
.AddApiExplorer(setup =>
{
    setup.SubstituteApiVersionInUrl = true;
});
var apiVersionDescriptionProvider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
builder.Services.AddSwaggerGen(setup =>
{
    foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
    {
        setup.SwaggerDoc(description.GroupName,
            new()
            {
                Title = "AHO - City Info API",
                Version = $"[ {description.ApiVersion.ToString()} ]",
                Description = "The best API in the universe"
            });
    }
    var xmlCommentsFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlCommentsFileFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFileName);
    setup.IncludeXmlComments(xmlCommentsFileFullPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}
//app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(setup =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            setup.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"version: aho-{description.GroupName}");
        }
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

//app.MapControllers();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();
