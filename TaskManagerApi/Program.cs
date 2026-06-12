var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAngularDev", policy =>
	{
		policy.WithOrigins("http://localhost:4200") // Angular default local URL
			  .AllowAnyMethod()                    // Allows GET, POST, PUT, DELETE
			  .AllowAnyHeader()                    // Allows headers like Content-Type
			  .AllowCredentials();                 // Required if you send cookies/tokens
	});
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularDev");
app.UseAuthorization();

app.MapControllers();

app.Run();
