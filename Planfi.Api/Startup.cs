﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using HotChocolate.AspNetCore.Playground;
using WebApi.GraphQl;
using HotChocolate.AspNetCore;
using HotChocolate;
using Microsoft.AspNetCore.Http.Features;
using WebApi.Interfaces;
using WebApi.Services.Account;
using WebApi.Services.Chat;
using WebApi.Services.exercises;
using WebApi.Services.Exercises;
using WebApi.Services.Organizations;
using WebApi.Services.Payment.PaypalIntegration;
using WebApi.Services.users;
using AccountService = WebApi.Services.Account.AccountService;
using PlanService = WebApi.Services.Plans.PlanService;

namespace WebApi
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddCors();
            services.AddControllers().AddNewtonsoftJson();

            services.AddCors(options => 
                options.AddPolicy(name: "AllowSetOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .WithOrigins("http://localhost:3000");
                    }));
            services.AddSignalR();

            // Use a PostgreSQL database
            var sqlConnectionString = Configuration.GetConnectionString("WebApiDatabase");

            services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(sqlConnectionString));

            services.Configure<FormOptions>(options => options.ValueCountLimit = 20000); 

            // todo
            services.AddIdentityCore<IdentityUser>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<DataContext>();

            // AutoMapper
            services.AddAutoMapper(typeof(Startup));

            // Swagger
            services.AddSwaggerGen(c =>
            {
               c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlanFi", Version = "v1" });
            });

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });


            // email configuration
            services.AddSingleton(Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
            services.AddTransient<IEmailService, EmailService>();
            services.AddSession();
            
            //payment conf
            //todo - for what clients are able to pay?
            //add products and plans
            //StripeConfiguration.SetApiKey(Configuration["Stripe:SecretKey"]);
            
            //chat module
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(ChatIdentityServer.GetIdentityResources())
                .AddInMemoryApiResources(ChatIdentityServer.GetApiResources())
                .AddInMemoryClients(ChatIdentityServer.GetClients())
                .AddTestUsers(ChatIdentityServer.GetUsers());
            
            services.AddScoped<IChatRoomService, ChatRoomService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IExerciseService, ExerciseService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPayPalProcesesing, PayPalProcessing>();
            //services.AddScoped<IStripeProcessing, StripeProcessing>();
            
            services.AddMvc().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });

            services.AddScoped<Query>();
            services.AddGraphQL(SchemaBuilder.New()
                .AddQueryType<Query>()
                //.AddMutationType<Mutation>()
                .Create());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataContext dataContext)
        {
                dataContext.Database.Migrate();
                app.UseCors("AllowSetOrigins");
                app.UseRouting();
                app.UseSwagger();
                app.UseSession();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Planfi");
                });
            
                //chat module
                app.UseIdentityServer();
                app.UseAuthorization();
                app.UseEndpoints(routes =>
                {
                    routes.MapHub<ChatHub>("chat");
                    routes.MapControllers();
                });
                
            
                app.UseGraphQL("/graphql");
                app.UsePlayground(new PlaygroundOptions { QueryPath = "/graphql", Path = "/playground" });
                app.UseAuthentication();
        }
    }
}
