using System.Collections.Immutable;
using Autofac.Core;
using PhoneDumpClient.View;
using XamlingCore.Windows8.Glue;
using XamlingCore.Windows8.Shared.Glue;
using Autofac;
using System.Reflection;
using PhoneDump.Services.Auth;

namespace PhoneDumpClient.UWP.Glue
{
    public class ProjectGlue : Windows8Glue
    {
        public override void Init()
        {
            base.Init();

            XCoreAutoRegistration.RegisterAssembly(Builder, typeof(HomeViewModel));
            XCoreAutoRegistration.RegisterAssembly(Builder, typeof(ProjectGlue));

            Builder.RegisterAssemblyTypes(typeof(ProjectGlue).GetTypeInfo().Assembly)
             .Where(_ => _.FullName.Contains("Service") || _.FullName.Contains("Repo"))
             .AsImplementedInterfaces()
             .SingleInstance();

            Builder.RegisterAssemblyTypes(typeof(TokenService).GetTypeInfo().Assembly)
            .Where(_ => _.FullName.Contains("Service") || _.FullName.Contains("Repo"))
            .AsImplementedInterfaces()
            .SingleInstance();

            // Builder.RegisterType<WorkflowExamples>();
            Container = Builder.Build();
        }
    }
}
