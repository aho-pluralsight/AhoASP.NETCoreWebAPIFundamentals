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
    context.Response.WriteAsync("01. Outbound\n");
    await next(context);
    context.Response.WriteAsync("01. Return\n");
});

app.Use(async (context, next) =>
{
    context.Response.WriteAsync("02. Outbound\n");
    await next(context);
    context.Response.WriteAsync("02. Return\n");
});

app.Use(async (context, next) =>
{
    context.Response.WriteAsync("03. Outbound\n");
    await next(context);
    context.Response.WriteAsync("03. Return\n");
});

app.Run(async context =>
{
    context.Response.WriteAsync("-> X. Bullseye!\n");
});

app.Run();
