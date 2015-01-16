using System.Composition;

namespace DBManager.ViewModels
{
    /// <summary>
    /// View model for the main page.
    /// </summary>
    [Export(typeof(MainPageViewModel))]
    public class MainPageViewModel
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
