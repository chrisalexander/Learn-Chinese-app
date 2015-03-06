using CourseDB;
using DBUtils;
using LangDB;
using LongRunningProcess;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace LevelEditor
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : MvvmAppBase
    {
        /// <summary>
        /// The container for MEF.
        /// </summary>
        private CompositionHost container;

        /// <summary>
        /// List of the assembly modules to include in the container.
        /// </summary>
        private readonly IEnumerable<Type> assemblyModules = new[] { typeof(App), typeof(LangDBModule), typeof(LongRunningProcessModule), typeof(DBUtilsModule), typeof(CourseDBModule) };

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Application launch callback.
        /// </summary>
        /// <param name="args">Launch args.</param>
        /// <returns>When complete.</returns>
        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            var configuration = new ContainerConfiguration().WithAssemblies(this.assemblyModules.Select(t => t.GetTypeInfo().Assembly));

            this.container = configuration.CreateContainer();
            this.container.SatisfyImports(this);

            ViewModelLocationProvider.SetDefaultViewModelFactory(t => this.container.GetExport(t));

            NavigationService.Navigate("Main", null);
            return Task.FromResult<object>(null);
        }
    }
}
