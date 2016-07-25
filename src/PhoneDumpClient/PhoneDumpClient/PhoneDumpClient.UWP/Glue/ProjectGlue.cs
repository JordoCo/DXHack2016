using System.Collections.Immutable;
using Autofac.Core;
using PhoneDumpClient.View;
using XamlingCore.Windows8.Glue;
using XamlingCore.Windows8.Shared.Glue;

namespace PhoneDumpClient.UWP.Glue
{
    public class ProjectGlue : Windows8Glue
    {
        public override void Init()
        {
            base.Init();

            XCoreAutoRegistration.RegisterAssembly(Builder, typeof(HomeViewModel));
            XCoreAutoRegistration.RegisterAssembly(Builder, typeof(ProjectGlue));

            // Builder.RegisterType<WorkflowExamples>();
            Container = Builder.Build();
        }
    }
}
