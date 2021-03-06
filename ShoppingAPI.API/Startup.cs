using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ShoppingAPI.API.Identity;
using ShoppingAPI.Database.Data;
using ShoppingAPI.Database.Data.Services;
using ShoppingAPI.API.Pages;
using System;
using ShoppingAPI.API.Middlewares;
using ShoppingAPI.Domain.AggregateRoots.AppUserAggregate;
using ShoppingAPI.Domain.AggregateRoots.OrderAggregate;
using ShoppingAPI.Domain.AggregateRoots.ProductAggregate;

namespace ShoppingAPI
{
  public class Startup
  {
    public IConfiguration Configuration { get; }
    private readonly IWebHostEnvironment _env;
    private readonly OptionConfiguration _optionConfig;

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
      Configuration = configuration;
      _env = env;
      _optionConfig = new OptionConfiguration(configuration, env);
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddHttpContextAccessor();
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddSwaggerGen(options =>
      {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "ShoppingAPI", Version = "v1" });
      });
      services.AddDbContext<ApplicationContext>(builder =>
      {
        builder.UseSqlServer(Configuration.GetConnectionString("ShoppingAPIDB"), opt => opt.EnableRetryOnFailure());
      });
      services.AddDefaultIdentity<AppUser>(_optionConfig.ConfigureIdentityOptions)
          .AddEntityFrameworkStores<ApplicationContext>()
          .AddClaimsPrincipalFactory<AppUserClaimsPrincipalFactory>()
          .AddSignInManager<AppSignInManager<AppUser>>();

      // DI Services
      services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, AppUserClaimsPrincipalFactory>();
      services.AddScoped<SignInManager<AppUser>, AppSignInManager<AppUser>>();
      //services.AddScoped<IClaimsTransformation, AppClaimsTransformation>();
      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
      services.AddScoped<ExceptionHandling>();

      // Repositories
      services.AddScoped<IProductService, ProductService>();
      services.AddScoped<IOrderService, OrderService>();
      services.AddScoped<IAppUserService, AppUserService>();

      services.AddAuthentication()
          .AddGoogle(_optionConfig.ConfigureGoogleOptions);
      services.ConfigureApplicationCookie(options =>
      {
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
      });
      services.AddAuthorization(options =>
      {
        options.AddPolicy("test", policy => policy.RequireClaim("test"));
      });
      services.AddControllers()
        .AddNewtonsoftJson(x =>
        {
          x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        });
      services.AddRazorPages();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShoppingAPI v1"));
      }

      app.UseHttpsRedirection();

      app.UseStaticFiles();

      app.UseRouting();

      app.UseMiddleware<ExceptionHandling>();

      app.UseAuthentication();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapRazorPages();
      });
    }
  }
}
