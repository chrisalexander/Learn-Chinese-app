using Microsoft.Practices.Prism.Mvvm;
using Windows.UI.Xaml.Controls;

namespace LevelEditor.Views
{
    /// <summary>
    /// The view code for the main page.
    /// </summary>
    public sealed partial class MainPage : Page, IView
    {
        /// <summary>
        /// Constructor for the main page.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
        }
    }
}
