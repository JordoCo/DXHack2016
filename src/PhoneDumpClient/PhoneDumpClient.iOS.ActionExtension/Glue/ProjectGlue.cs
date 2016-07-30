using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac;
using PhoneDumpClient.View;
using XamlingCore.iOS.Unified.Glue;
using XamlingCore.Platform.Shared.Glue;
using PhoneDump.Services.Auth;

namespace PhoneDumpClient.iOS.ActionExtension.Glue
{
    public class ProjectGlue : iOSGlue
    {
        public override void Init()
        {
            base.Init(); //ensure you call this first so the builder and container are available

            //Place the type of one of your view models here, so we can find its assembly and auto register all views and view models there.
            //do this for any assemblies where you need to resolve views and view models.
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

            //you can also do Builder.RegisterModule<> etc just like with Autofac - look it up :)

            Container = Builder.Build();
        }


    }
}
