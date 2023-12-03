using CallAppTask.DTO;
using CallAppTask.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CallAppTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), b => b.MigrationsAssembly("CallAppTask")));

            builder.Services.AddSwaggerGen(options =>
                {
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Name = "Authorization",
                        Description = "Provide Bearer Token",
                        Type = SecuritySchemeType.Http
                    });

                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },

                            new List<string>()
                        }
                    });
                });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateActor = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });
            builder.Services.AddAuthorization();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddScoped<IUserService, UserService>();

            var app = builder.Build();

            app.UseSwagger();
            app.UseAuthorization();
            app.UseAuthentication();

            app.MapGet("/", () => "Hello World");

            app.MapPost("/login",
                (string username, string password) => Login(username, password));

            app.MapPost("/create",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
            (User user, IUserService service) => Create(user, service));

            app.MapGet("/get",
            (int id, IUserService repository) => Get(id, repository));

            app.MapGet("/getall",
                (IUserService service) => GetAll(service));

            app.MapPut("/update",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
            (User newUser, IUserService service) => Update(newUser, service));

            app.MapDelete("/delete",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
            (int id, IUserService service) => Delete(id, service));

            IResult Create(User user, IUserService service)
            {
                var result = service.Create(user);
                return Results.Ok(result);
            }

            IResult Get(int id, IUserService repository)
            {
                var result = repository.Get(id);

                if (result is null) return Results.NotFound("Not found");

                return Results.Ok(result);
            }

            IResult GetAll(IUserService service)
            {
                var result = service.GetAll();
                return Results.Ok(result);
            }

            IResult Update(User newUser, IUserService service)
            {
                var result = service.Update(newUser);

                if (result is null) Results.NotFound("Not found");

                return Results.Ok(result);
            }

            IResult Delete(int id, IUserService service)
            {
                var result = service.Delete(id);

                if(result) return Results.Ok(result);

                return Results.BadRequest("Bad Request");
            }

            IResult Login(string username, string password)
            {
                if (username is not null && password is not null && username == "Admin" && password == "Admin123")
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, "Luke"),
                        new Claim(ClaimTypes.Email, "SkyWalker"),
                    };

                    var token = new JwtSecurityToken
                        (
                            issuer: builder.Configuration["Jwt:Issuer"],
                            audience: builder.Configuration["Jwt:Audience"],
                            claims: claims,
                            expires: DateTime.UtcNow.AddDays(15),
                            notBefore: DateTime.UtcNow,
                            signingCredentials: new SigningCredentials(
                                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"])), SecurityAlgorithms.HmacSha256)
                        );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                    return Results.Ok(tokenString);
                }

                return Results.NotFound("Invalid Credentials");
            }

            app.UseSwaggerUI();

            app.Run();
        }
    }
}