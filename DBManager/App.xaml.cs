using Microsoft.Practices.Prism.Mvvm;
using System.Composition;
using System.Composition.Hosting;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace DBManager
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : MvvmAppBase
    {
        /// <summary>
        /// The container for MEF.
        /// </summar>y
        private CompositionHost container;

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
            var configuration = new ContainerConfiguration().WithAssembly(typeof(App).GetTypeInfo().Assembly);

            this.container = configuration.CreateContainer();
            this.container.SatisfyImports(this);

            NavigationService.Navigate("Main", null);
            return Task.FromResult<object>(null);
        }
    }
}
