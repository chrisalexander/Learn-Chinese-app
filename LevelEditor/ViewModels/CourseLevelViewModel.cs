using System;
using System.Windows.Input;
using CourseDB.Model;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Composition;
using System.Linq;

namespace LevelEditor.ViewModels
{
    /// <summary>
    /// View model for course levels.
    /// </summary>
    [Export(typeof(CourseLevelViewModel))]
    public class CourseLevelViewModel : ViewModel
    {
        /// <summary>
        /// The level ID.
        /// </summary>
        private LevelId id;

        /// <summary>
        /// The name of the level.
        /// </summary>
        private string name;

        /// <summary>
        /// Description of the level.
        /// </summary>
        private string description;

        /// <summary>
        /// The difficulty of the level.
        /// </summary>
        private LevelDifficulty difficulty;

        /// <summary>
        /// The ordered list of entries that form this level.
        /// </summary>
        private ObservableCollection<LevelEntryViewModel> entries;

        /// <summary>
        /// List of IDs prerequisite levels for this level.
        /// </summary>
        private ObservableCollection<LevelId> prerequisites;

        /// <summary>
        /// The currently selected entry.
        /// </summary>
        private LevelEntryViewModel selectedEntry;

        /// <summary>
        /// Construct a new view model from a source.
        /// </summary>
        /// <param name="source">The source level.</param>
        public CourseLevelViewModel(ICourseLevel source)
        {
            this.Id = source.Id;
            this.Name = source.Name;
            this.Description = source.Description;
            this.Difficulty = source.Difficulty;
            this.Entries = new ObservableCollection<LevelEntryViewModel>(source.Entries.Select(entry => new LevelEntryViewModel(entry)));
            this.Prerequisites = new ObservableCollection<LevelId>(source.Prerequisites);

            this.NewEntryCommand = new DelegateCommand(this.NewEntry);
            this.RemoveEntryCommand = new DelegateCommand(this.RemoveEntry);
        }

        /// <summary>
        /// The level ID.
        /// </summary>
        public LevelId Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.SetProperty(ref this.id, value);
            }
        }

        /// <summary>
        /// The name of the level.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.SetProperty(ref this.name, value);
            }
        }

        /// <summary>
        /// Description of the level.
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                this.SetProperty(ref this.description, value);
            }
        }

        /// <summary>
        /// The difficulty of the level.
        /// </summary>
        public LevelDifficulty Difficulty
        {
            get
            {
                return this.difficulty;
            }

            set
            {
                this.SetProperty(ref this.difficulty, value);
            }
        }

        /// <summary>
        /// The ordered list of entries that form this level.
        /// </summary>
        public ObservableCollection<LevelEntryViewModel> Entries
        {
            get
            {
                return this.entries;
            }

            set
            {
                this.SetProperty(ref this.entries, value);
            }
        }

        /// <summary>
        /// List of IDs prerequisite levels for this level.
        /// </summary>
        public ObservableCollection<LevelId> Prerequisites
        {
            get
            {
                return this.prerequisites;
            }

            set
            {
                this.SetProperty(ref this.prerequisites, value);
            }
        }

        /// <summary>
        /// The selected entry.
        /// </summary>
        public LevelEntryViewModel SelectedEntry
        {
            get
            {
                return this.selectedEntry;
            }

            set
            {
                this.SetProperty(ref this.selectedEntry, value);
            }
        }

        /// <summary>
        /// Command for adding a new entry.
        /// </summary>
        public ICommand NewEntryCommand { get; private set; }

        /// <summary>
        /// Command for removing an entry.
        /// </summary>
        public ICommand RemoveEntryCommand { get; private set; }

        /// <summary>
        /// Convert the view model to the model class.
        /// </summary>
        /// <returns>The view model data in model format.</returns>
        public CourseLevel ToSource()
        {
            var source = new CourseLevel();
            source.Id = this.Id;
            source.Id = this.Id;
            source.Name = this.Name;
            source.Description = this.Description;
            source.Difficulty = this.Difficulty;
            
            var levelEntries = new List<LevelEntry>();
            levelEntries.AddRange(this.Entries.Select(entry => entry.ToSource()));
            source.Entries = levelEntries;

            source.Prerequisites = this.Prerequisites.ToList();
            return source;
        }

        /// <summary>
        /// Create a new entry, and select it.
        /// </summary>
        /// <returns>When complete.</returns>
        private void NewEntry()
        {
            var entry = new LevelEntry();
            entry.EntryId = Guid.NewGuid().ToString();

            var entryViewModel = new LevelEntryViewModel(entry);

            this.Entries.Add(entryViewModel);
            this.SelectedEntry = entryViewModel;
        }

        /// <summary>
        /// Deletes the currently selected entry.
        /// </summary>
        /// <returns>When complete.</returns>
        private void RemoveEntry()
        {
            if (this.SelectedEntry == null)
            {
                return;
            }

            this.Entries.Remove(this.SelectedEntry);

            this.SelectedEntry = this.Entries.Count > 0 ? this.Entries[0] : null;
        }
    }
}
