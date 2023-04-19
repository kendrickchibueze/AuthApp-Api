using AuthApp_Api.Data;
using AuthApp_Api.Extensions;
using AuthApp_Api.Services;
using AuthApp_Api.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace AuthApp_Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.



            builder.Services.AddDbContext<AuthenticationDbContext>(opts =>
            {


                var defaultConn = builder.Configuration.GetSection("ConnectionString")["DefaultConn"];

                opts.UseSqlServer(defaultConn);

            });


            builder.Services.Configure<IdentityOptions>(opts => opts.SignIn.RequireConfirmedEmail = true);
            builder.Services.ConfigureEmail(builder.Configuration);
            builder.Services.AddEmailService(builder.Configuration);


            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {

                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;

            }).AddEntityFrameworkStores<AuthenticationDbContext>().AddDefaultTokenProviders();

            builder.Services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Authentication:Audience"],
                    ValidIssuer = builder.Configuration["Authentication:Issuer"],
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:Key"])),
                    ValidateIssuerSigningKey = true
                };

            });






             builder.Services.AddScoped<IUserService, UserService>();

              builder.Services.AddScoped<ISecurityService, JWTSecurityService>();


             builder.Services.AddSingleton<IMailService, SendGridMailService>();

            //fake mailing
            //builder.Services.AddScoped<IEmailService, EmailService>();




            builder.Services.AddControllers();
            builder.Services.AddRazorPages();









            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(sw =>

            sw.SwaggerDoc("v1",
            new OpenApiInfo { Title = "dotnet6JWTAuthentication", Version = "1.0" }));



            builder.Services.AddSwaggerGen(s =>
                    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Insert JWT Token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "bearer"

                    }));


            builder.Services.AddSwaggerGen(w =>
                w.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            } ,
                            new string[]{}

                    
                        }
                    }
                 ));



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllers();

            app.Run();
        }
    }
}