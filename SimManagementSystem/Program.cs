using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SimManagementSystem.Controllers;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.Services;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SimManagementSystemContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SimManagementSystem"));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(2),
        ValidIssuer = "https://localhost:5272",
        ValidAudience = "https://localhost:5272",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("7m348xB438xfxM3xfizi3hxeDfniwz<sbirbxhcnjymFeSuinjwvehu3rjicbuyiuxwb2iuwqhbcbinwx29828hc9nexbncbinncashjcimjncuh83ejdc3byicnjbc2ecjxnh2ejce2hjcujuj58nfximmewmxfemmxfiekwfxow"))
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-expired", "true");
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDevicesService, DevicesService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IInspectionsService, InspectionsService>();
builder.Services.AddScoped<IMaintenancesService, MaintenanceService>();
builder.Services.AddScoped<IMaintenanceTypesService, MaintenanceTypesService>();
builder.Services.AddScoped<IMalfunctionsService, MalfunctionsService>();
builder.Services.AddScoped<IPredefinedSessionsService, PredefinedSessionsService>();
builder.Services.AddScoped<IRecoveryActionsService, RecoveryActionsService>();
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<ISessionCategoriesService, SessionCategoriesService>();
builder.Services.AddScoped<ISimulatorSessionsService, SimulatorSessionsService>();
builder.Services.AddScoped<ISimulatorStatesService, SimulatorStatesService>();
builder.Services.AddScoped<ITestQTGsService, TestQTGsService>();
builder.Services.AddScoped<ITestResultsService, TestResultsService>();


var app = builder.Build();

// Enable CORS
app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
