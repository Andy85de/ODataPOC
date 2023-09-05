using Marten;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddMarten(
    options =>
    {
        options.Connection("User ID=root;Password=1;Host=localhost;Port=5432;Database=QueryTests;");
        options.DatabaseSchemaName = "test";
        options.Logger(new ConsoleMartenLogger());
    }).ApplyAllDatabaseChangesOnStartup();

var app = builder.Build();


app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());

app.UseSwagger();
app.UseSwaggerUI();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});


app.Run();