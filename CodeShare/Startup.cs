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
using CodeShare.Services.SessionsManager;
using CodeShare.Services.DatabaseInteractor.Test;
using CodeShare.SignalR;
using System.Collections.Generic;
using System;

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
            //services.AddLogging(builder => {
            //    builder.ClearProviders();
            //    builder.AddConsole();
            //    builder.SetMinimumLevel(LogLevel.None);
            //});
            services.AddLogging();
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
            services.AddHttpContextAccessor();
            services.AddSingleton<ISessionsManager, SessionsManager>();
            services.AddSingleton<IDatabaseInteractor, TestInteractor>();
            //services.AddSingleton<IDatabaseInteractor, MongoInteractor>(
            //    provider => new MongoInteractor("mongodb://localhost:27017", "codeshare")
            //    );

            IMapper mapper = CreateConfiguredMapper();
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
                endpoints.MapHub<TextEditorHub>("/editor/{sessionId}", options =>
                {
                    options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
                    //options.WebSockets.CloseTimeout = TimeSpan.FromSeconds(5);
                });
            });
        }

        private static IMapper CreateConfiguredMapper()
        {
            var mapperCfg = new MapperConfiguration(
                cfg =>
                {
                    ConfigureTasksMapping(cfg);
                    ConfigureSolutionsMapping(cfg);
                    ConfigureUsersMapping(cfg);
                    ConfigureSessionsMapping(cfg);
                }
            );
            var mapper = mapperCfg.CreateMapper();
            return mapper;
        }

        private static void ConfigureSessionsMapping(IMapperConfigurationExpression cfg)
        {
            cfg
            .CreateMap<SessionDTO, SessionEntity>()
            .ForMember(
                entity => entity.Collaborators,
                opt => opt.NullSubstitute(new List<UserEntity>())
                )
            .ForMember(
                entity => entity.EditorInstance,
                opt => opt.MapFrom<EditorInstanceResolver>()
                )
            .ForMember(
                entity => entity.CurrentTask,
                opt => opt.MapFrom(dto => dto.Task)
                );
            cfg
            .CreateMap<SessionEntity, SessionDTO>()
            .ForMember(
                dto => dto.CurrentSolution,
                opt => opt.MapFrom(entity => entity.EditorInstance.Solution)
                )
            .ForMember(
                dto => dto.Task,
                opt => opt.MapFrom(entity => entity.CurrentTask)
                );
        }

        private static void ConfigureUsersMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<UserDTO, UserEntity>();
            cfg.CreateMap<UserEntity, UserDTO>();
        }

        private static void ConfigureSolutionsMapping(IMapperConfigurationExpression cfg)
        {
            cfg
            .CreateMap<SolutionEntity, SolutionDTO>()
            .ForMember(
                dto => dto.ProgrammingLanguageName,
                opt => opt.MapFrom(entity => entity.Language.Name)
                );
            cfg
            .CreateMap<SolutionDTO, SolutionEntity>()
            .ForMember(
                entity => entity.Language,
                opt => opt.MapFrom(dto =>
                    ProgrammingLanguage.GetByName(dto.ProgrammingLanguageName)
                    )
                );
        }

        private static void ConfigureTasksMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<TaskDTO, TaskEntity>();
            cfg.CreateMap<TaskEntity, TaskDTO>();
        }

        private class EditorInstanceResolver : IValueResolver<SessionDTO, SessionEntity, CollaborativeEditor?>
        {
            public CollaborativeEditor Resolve(SessionDTO source, SessionEntity destination, CollaborativeEditor? destMember, ResolutionContext context) =>
                new CollaborativeEditor(source.CurrentSolution.SourceCode);
        }
    }
}
