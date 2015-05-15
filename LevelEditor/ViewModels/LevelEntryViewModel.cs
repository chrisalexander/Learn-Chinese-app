using System;
using CourseDB;
using LangDB;
using LanguageModel;
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
        /// The search service.
        /// </summary>
        private readonly ILanguageSearchService searchService;

        /// <summary>
        /// The ID of the entry in the entry database.
        /// </summary>
        private EntryId entryId;

        /// <summary>
        /// The chosen language entry ID.
        /// </summary>
        private LanguageEntryId languageEntryId;

        /// <summary>
        /// The selected translation to be used in the level.
        /// </summary>
        private string selectedTranslation;

        /// <summary>
        /// Construct a new view model from a source.
        /// </summary>
        /// <param name="searchService">The search service.</param>
        /// <param name="source">The source entry.</param>
        [ImportingConstructor]
        public LevelEntryViewModel(ILanguageSearchService searchService, ILevelEntry source)
        {
            this.searchService = searchService;

            this.LevelSearchViewModel = new LevelSearchViewModel(searchService, (entryId, translation) =>
            {
                this.LanguageEntryId = entryId;
                this.SelectedTranslation = translation;
            });

            this.EntryId = source.Id;
            this.LanguageEntryId = source.LanguageEntryId;
            this.SelectedTranslation = source.SelectedTranslation;
        }

        public LevelSearchViewModel LevelSearchViewModel { get; private set; }

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
        /// The ID of the corresponding language entry.
        /// </summary>
        public LanguageEntryId LanguageEntryId
        {
            get
            {
                return this.languageEntryId;
            }

            set
            {
                this.SetProperty(ref this.languageEntryId, value);
                this.OnPropertyChanged("LanguageEntry");
            }
        }

        /// <summary>
        /// Helper to retrieve the associated language entry.
        /// </summary>
        public ILanguageEntry LanguageEntry
        {
            get
            {
                try
                {
                    return this.searchService.ObjectFromKey(this.languageEntryId);
                }
                catch (Exception)
                {
                    return null;
                }
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
            source.LanguageEntryId = this.LanguageEntryId;
            source.SelectedTranslation = this.SelectedTranslation;
            return source;
        }
    }
}
