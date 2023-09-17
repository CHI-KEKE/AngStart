using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using API.MiddleWare;
using Microsoft.AspNetCore.HttpOverrides;
using API.Domain;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Core;
using Infrastructure.Services;
using StackExchange.Redis;
using Core.Interface;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using API.Errors;
using Infrastructure.Repository;
using API.Domain.IService;
using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;
using Serilog;
using Serilog.Sinks.AwsCloudWatch;
using API.Helpers;
using API.Domain.AccountService;

var builder = WebApplication.CreateBuilder(args);


//AWS Logger

builder.Logging.AddAWSProvider();


// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});


//ConnectionString for SQL

DotNetEnv.Env.Load();
string DefaultConnection = Environment.GetEnvironmentVariable("DB_CONNECTIONSTRING");
builder.Services.AddDbContext<StoreContext>(options => options.UseSqlServer(DefaultConnection));


//Identity

string IdentityConnection = Environment.GetEnvironmentVariable("IDENTITY_CONNECTIONSTRING");

// builder.Services.AddDbContext<AppIdentityDbContext>(options =>options.UseSqlServer(IdentityConnection));

builder.Services.AddIdentityCore<AppUser>(opt => {})
.AddRoles<IdentityRole>()                ////////////////////////Role
.AddEntityFrameworkStores<AppIdentityDbContext>()
.AddSignInManager<SignInManager<AppUser>>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
});



//Redis Cache


// StackExchabge Redis Cache
// var config = builder.Configuration;
// builder.Services.AddSingleton<IConnectionMultiplexer>(c => {
//     var options = ConfigurationOptions.Parse(config.GetConnectionString("Redis"));
//     var connection =  ConnectionMultiplexer.Connect(options);
//     if (connection.IsConnected)
//     {
//         Console.WriteLine("Redis connection established.");
//     }
//     else
//     {
//         Console.WriteLine("Failed to connect to Redis.");
//     }

//     return connection;
// });


//AWS Redis Cache


builder.Services.AddSingleton<IConnectionMultiplexer>(c => {
    var options = ConfigurationOptions.Parse("master.redis.cyg7e4.apse2.cache.amazonaws.com:6379");
    options.Password = "Ag1314579j468kkk";
    options.Ssl = true;
    options.AllowAdmin = true; // 如果需要進行管理操作，可以設置為 true
    options.AbortOnConnectFail = false; // 如果要允許重試連接，可以設置為 false

    var connection =  ConnectionMultiplexer.Connect(options);
    if (connection.IsConnected)
    {
        Console.WriteLine("ElasticRedis connection established.");
    }
    else
    {
        Console.WriteLine("Failed to connect to Redis.");
    }

    return connection;
});






builder.Services.AddSingleton<IResponseCacheService,ResponseCacheService>();


//S3!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

builder.Services.AddScoped<IStorageService,StorageService>();



//Authentication of JWT

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(optoins =>{
    var config = builder.Configuration;
    optoins.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
        ValidIssuer = config["Token:Issuer"],
        ValidateIssuer = true,
        ValidateAudience = false,
    };
});

//FB Auth
builder.Services.AddAuthentication()
    .AddFacebook(options =>
    {
        options.AppId = "629483562400919";
        options.AppSecret = "df041939538481f9207c1b35127597a2";
    });


//Token Service
builder.Services.AddScoped<ITokenService,TokenService>();


builder.Services.AddAuthentication();
builder.Services.AddAuthorization();


//Custom Services

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderDataService>();
builder.Services.AddScoped<FacebookLoginService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<CampaignRepository>();

//CORS

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

//SignalR

builder.Services.AddSignalR();


//AutoMapper

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//handle 400

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        var errors = actionContext.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .SelectMany(x => x.Value.Errors)
            .Select(x => x.ErrorMessage).ToArray();

        var errorResponse = new ApiValidationErrorResponse
        {
            Errors = errors
        };

        return new BadRequestObjectResult(errorResponse);
    };
});


//in-memory cache
builder.Services.AddMemoryCache();



//Repository

builder.Services.AddScoped<IProductRepository, ProductRepository>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseForwardedHeaders();


//CORS

app.UseCors();

// app.UseMiddleware<ErrorHandlingMiddleware>();
// app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

//handle 500
app.UseMiddleware<ExceptionMiddleware>();

//if meet 404 notfound will invoked firstly
app.UseStatusCodePagesWithReExecute("/errors/{0}");



//Handle OrderHub
// app.UseEndpoints(endpoints =>
// {
//     // ...
//     endpoints.MapHub<OrderHub>("/orderHub");
// });


// Trusted Cetificate

// app.UseHttpsRedirection();
// app.UseRouting();
// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapControllers();
// });



//map controller

app.MapControllers();

// using var scope = app.Services.CreateScope();
// var services = scope.ServiceProvider;
// var context = services.GetRequiredService<StoreContext>();
// var identityContext = services.GetRequiredService<AppIdentityDbContext>();
// var userManager = services.GetRequiredService<UserManager<AppUser>>();

// var logger = services.GetRequiredService<ILogger<Program>>();


//Seed Identity

// try{

//     await identityContext.Database.MigrateAsync();
//     await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
//     await context.Database.MigrateAsync();
//     await StoreContextSeed.SeedAsync(context);

    //seeding udentity with role
    // string email = "testRole@gmail.com";
    // string password = "testPassword";

    // if(await userManager.FindByEmailAsync(email) == null)
    // {
    //     var user = new AppUser()
    //     {
    //         Name = "NewRole",
    //         Email = email,
    //         UserName = email,
    //         PictureUrl = "https://api.appworks-school.tw/assets/201807202157/0.jpg",
    //         Address = new Address()
    //         {
    //             FirstName = "New",
    //             LastName = "Role",
    //             Street = "Coco Road",
    //             City = "XinPei",
    //             State = "Florida",
    //             ZipCode = "11111",
    //         }
    //     };

    //     await userManager.CreateAsync(user,password);

    //     await userManager.AddToRoleAsync(user,"Admin");
    // }
}

// catch(Exception ex)
// {
//     logger.LogError(ex, "An error occurred during Migration!!");
// }


// using(var Rolescope = app.Services.CreateScope())
// {
//     var roleManager = Rolescope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

//     var roles = new []{"Admin","Manager","Member"};

//     foreach(var role in roles)
//     {
//         if(! await roleManager.RoleExistsAsync(role))
//         {
//             await roleManager.CreateAsync(new IdentityRole(role));
//         }
//     }
// }




app.Run();