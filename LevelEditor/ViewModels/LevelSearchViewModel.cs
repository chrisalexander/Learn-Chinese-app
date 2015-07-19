using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using LangDB;
using LanguageModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;

namespace LevelEditor.ViewModels
{
    /// <summary>
    /// View model for searching levels.
    /// </summary>
    public class LevelSearchViewModel : ViewModel
    {
        /// <summary>
        /// The search service.
        /// </summary>
        private readonly ILanguageSearchService searchService;

        /// <summary>
        /// The action to call when a result has been chosen.
        /// </summary>
        private readonly Action<LanguageEntryId, string> resultChosen;

        /// <summary>
        /// The search term.
        /// </summary>
        private string searchTerm;

        /// <summary>
        /// Search results.
        /// </summary>
        private List<ILanguageEntry> results;

        /// <summary>
        /// The selected result.
        /// </summary>
        private ILanguageEntry selectedResult;

        /// <summary>
        /// The selected translation.
        /// </summary>
        private string selectedTranslation;

        /// <summary>
        /// Creates a new level search view model.
        /// </summary>
        /// <param name="searchService">The search service.</param>
        /// <param name="resultChosen">The details of the chosen result.</param>
        public LevelSearchViewModel(ILanguageSearchService searchService, Action<LanguageEntryId, string> resultChosen)
        {
            this.searchService = searchService;
            this.resultChosen = resultChosen;

            this.SearchCommand = new DelegateCommand(this.ExecuteSearch);
            this.SelectCommand = new DelegateCommand(this.SelectResult);
        }

        /// <summary>
        /// Command to execute a search.
        /// </summary>
        public ICommand SearchCommand { get; private set; }

        /// <summary>
        /// Command to select one of the results.
        /// </summary>
        public ICommand SelectCommand { get; private set; }

        /// <summary>
        /// The search term.
        /// </summary>
        public string SearchTerm
        {
            get
            {
                return this.searchTerm;
            }

            set
            {
                this.SetProperty(ref this.searchTerm, value);
            }
        }

        /// <summary>
        /// The results of the last search.
        /// </summary>
        public List<ILanguageEntry> Results
        {
            get
            {
                return this.results;
            }

            private set
            {
                this.SetProperty(ref this.results, value);
            }
        }

        /// <summary>
        /// The selected result.
        /// </summary>
        public ILanguageEntry SelectedResult
        {
            get
            {
                return this.selectedResult;
            }

            set
            {
                this.SetProperty(ref this.selectedResult, value);
            }
        }

        /// <summary>
        /// The selected translation.
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
        /// Executes the requested search.
        /// </summary>
        private async void ExecuteSearch()
        {
            var searchResults = await this.searchService.Search(this.SearchTerm);

            this.Results = searchResults.Select(result => result.Result.Value).ToList();
        }

        /// <summary>
        /// Executes the selection of the result.
        /// </summary>
        private void SelectResult()
        {
            this.resultChosen(this.SelectedResult.Id, this.selectedTranslation);
        }
    }
}
