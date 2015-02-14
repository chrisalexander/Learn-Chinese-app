using CourseDB.Model;
using Microsoft.Practices.Prism.Mvvm;
using System.Collections;
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
            
            var entries = new List<LevelEntry>();
            entries.AddRange(this.Entries.Select(entry => entry.ToSource()));
            source.Entries = (IList<ILevelEntry>)entries;

            source.Prerequisites = this.Prerequisites.ToList();
            return source;
        }
    }
}
