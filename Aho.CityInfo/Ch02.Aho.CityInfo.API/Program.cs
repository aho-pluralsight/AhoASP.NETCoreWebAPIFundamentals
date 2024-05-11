using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.Use(async (context, next) =>
{
    if (context.Request.Path.Value == "/aho.js")
    {
        string execDir = AppDomain.CurrentDomain.BaseDirectory;
        await context.Response.WriteAsync(System.IO.File.ReadAllText($"{execDir}\\scripts\\aho.js"));
    }
    else if (context.Request.Path.Value == "/aho.css")
    {
        string execDir = AppDomain.CurrentDomain.BaseDirectory;
        await context.Response.WriteAsync(System.IO.File.ReadAllText($"{execDir}\\scripts\\aho.css"));
    }
    else
    {
        await next(context);
    }
});

app.Use(async (context, next) =>
{
    if (context.Request.Path.Value == "/pretty")
    {
        await context.Response.WriteAsync(
            "<!DOCTYPE html>" +
            "<html lang=\"en\" class=\"notranslate\" translate=\"no\"");
        await context.Response.WriteAsync(
            "<head>" +
            "<link rel='stylesheet' href='aho.css'>" +
            "<meta name=\"google\" content=\"notranslate\" />" +
            "</head>");
        await context.Response.WriteAsync(
            "<body>");
        await context.Response.WriteAsync("<div id='pipeContent'>");
        await next(context);
        await context.Response.WriteAsync("</div>");
        await context.Response.WriteAsync("<script type='text/javascript' src='aho.js'></script>");
        await context.Response.WriteAsync("<script>jolifying();</script>");
        await context.Response.WriteAsync(
            "</body>" +
            "</html>");
    }
    else
    {
        await next(context);
    }
});

app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("P03. Outbound\n");
    await next(context);
    await context.Response.WriteAsync("P03. Return\n");
});

app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("P02. Outbound\n");
    await next(context);
    await context.Response.WriteAsync("P02. Return\n");
});

app.Use(async (context, next) =>
{
    await context.Response.WriteAsync("P01. Outbound\n");
    await next(context);
    await context.Response.WriteAsync("P01. Return\n");
});

app.Run(async context =>
{
    await context.Response.WriteAsync("P00. Bullseye\n");
});

app.Run();
