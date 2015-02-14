using Microsoft.Practices.Prism.Mvvm;
using System.Composition;

namespace LevelEditor.ViewModels
{
    /// <summary>
    /// View model for the main page.
    /// </summary>
    [Export(typeof(MainPageViewModel))]
    public class MainPageViewModel : ViewModel
    {
        /// <summary>
        /// MEF importing constructor.
        /// </summary>
        [ImportingConstructor]
        public MainPageViewModel()
        {
        }
    }
}
