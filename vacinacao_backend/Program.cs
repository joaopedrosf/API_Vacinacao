using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using vacinacao_backend.Repositories;
using vacinacao_backend.Services;

namespace vacinacao_backend {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => {
                x.TokenValidationParameters = new TokenValidationParameters {
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };  
            });

            builder.Services.AddAuthorization(options => {
                options.AddPolicy("AdminPolicy", p => {
                    p.RequireClaim("isAdmin", "true");
                });
            });

            builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("postgre"));
            builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Vacinacao", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
            });

            builder.Services.AddScoped<UsuarioService>();
            builder.Services.AddScoped<AlergiaService>();
            builder.Services.AddScoped<VacinaService>();
            builder.Services.AddScoped<AgendaService>();
            builder.Services.AddScoped<LoginService>();
            builder.Services.AddEntityFrameworkNpgsql().AddDbContext<VacinacaoContext>(options => { options.UseNpgsql(builder.Configuration.GetConnectionString("postgre")); });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapHealthChecks("/_health", new HealthCheckOptions {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}