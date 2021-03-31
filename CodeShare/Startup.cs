using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using CodeShare.Services.DatabaseInteractor;
using CodeShare.Services.DatabaseInteractor.MongoDB;
using Microsoft.Extensions.Logging;
using AutoMapper;

using TaskDTO = CodeShare.Model.DTOs.Task;
using SolutionDTO = CodeShare.Model.DTOs.Solution;
using SessionDTO = CodeShare.Model.DTOs.Session;
using UserDTO = CodeShare.Model.DTOs.User;
using TaskEntity = CodeShare.Model.Entities.Task;
using SolutionEntity = CodeShare.Model.Entities.Solution;
using SessionEntity = CodeShare.Model.Entities.Session;
using UserEntity= CodeShare.Model.Entities.User;
using CodeShare.Model.Entities;

namespace CodeShare
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSignalR();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                    {
                        options.LoginPath = "/Login";
                        options.LogoutPath = "/Logout";
                        options.ReturnUrlParameter = "returnRoute";
                    }
                );
            //services.AddLogging();
            services.AddHttpContextAccessor();
            //services.AddSingleton<ISessionsManager, CollabManager>();
            services.AddSingleton<IDatabaseInteractor, MongoInteractor>(
                provider => new MongoInteractor("mongodb://localhost:27017", "codeshare")
                );

            var mapperCfg = new MapperConfiguration(
                cfg =>
                {
                    cfg.CreateMap<TaskDTO, TaskEntity>();
                    cfg.CreateMap<TaskEntity, TaskDTO>();
                    cfg
                    .CreateMap<SolutionEntity, SolutionDTO>()
                    .ForMember(
                        dst => dst.ProgrammingLanguageName,
                        opt => opt.MapFrom(src => src.Language.Name)
                        );
                    cfg
                    .CreateMap<SolutionDTO, SolutionEntity>()
                    .ForMember(
                        dst => dst.Language,
                        opt => opt.MapFrom(src =>
                            ProgrammingLanguage.GetByName(src.ProgrammingLanguageName)
                            )
                        );
                    cfg.CreateMap<UserDTO, UserEntity>();
                    cfg.CreateMap<UserEntity, UserDTO>();
                }
            );
            var mapper = mapperCfg.CreateMapper();
            services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            loggerFactory.CreateLogger<Startup>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

        }
    }
}
