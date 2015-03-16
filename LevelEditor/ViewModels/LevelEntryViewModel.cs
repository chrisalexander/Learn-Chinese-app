using CourseDB;
using Microsoft.Practices.Prism.Mvvm;
using System.Composition;

namespace LevelEditor.ViewModels
{
    /// <summary>
    /// View model for level entries.
    /// </summary>
    [Export(typeof(LevelEntryViewModel))]
    public class LevelEntryViewModel : ViewModel
    {
        /// <summary>
        /// The ID of the entry in the entry database.
        /// </summary>
        private EntryId entryId;

        /// <summary>
        /// The selected translation to be used in the level.
        /// </summary>
        private string selectedTranslation;

        /// <summary>
        /// Construct a new view model from a source.
        /// </summary>
        /// <param name="source">The source entry.</param>
        public LevelEntryViewModel(ILevelEntry source)
        {
            this.EntryId = source.Id;
            this.SelectedTranslation = source.SelectedTranslation;
        }

        /// <summary>
        /// The ID of the entry in the entry database.
        /// </summary>
        public EntryId EntryId
        {
            get
            {
                return this.entryId;
            }

            set
            {
                this.SetProperty(ref this.entryId, value);
            }
        }

        /// <summary>
        /// The selected translation to be used in the level.
        /// </summary>
        public string SelectedTranslation
        {
            get
            {
                return this.selectedTranslation;
            }

            set
            {
                this.SetProperty(ref this.selectedTranslation, value);
            }
        }

        /// <summary>
        /// Convert the view model to the model class.
        /// </summary>
        /// <returns>The view model data in model format.</returns>
        public LevelEntry ToSource()
        {
            var source = new LevelEntry();
            source.Id = this.EntryId;
            source.SelectedTranslation = this.SelectedTranslation;
            return source;
        }
    }
}
