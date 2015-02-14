﻿using CourseDB.Model;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Composition;
using System.Linq;

namespace LevelEditor.ViewModels
{
    /// <summary>
    /// View model for course databases.
    /// </summary>
    [Export(typeof(CourseDatabaseViewModel))]
    public class CourseDatabaseViewModel : ViewModel
    {
        /// <summary>
        /// The course ID.
        /// </summary>
        private CourseId id;

        /// <summary>
        /// The name of the course.
        /// </summary>
        private string name;

        /// <summary>
        /// The description of the course.
        /// </summary>
        private string description;

        /// <summary>
        /// The time the course was last updated.
        /// </summary>
        private DateTime updated;

        /// <summary>
        /// The path to the database file.
        /// </summary>
        private string path;

        /// <summary>
        /// The ordered list of levels in the course.
        /// </summary>
        private ObservableCollection<CourseLevelViewModel> levels;

        /// <summary>
        /// The currently selected level.
        /// </summary>
        private CourseLevelViewModel selectedLevel;

        /// <summary>
        /// Construct a new view model from a source.
        /// </summary>
        /// <param name="source">The source database.</param>
        public CourseDatabaseViewModel(ICourseDatabase source)
        {
            this.Id = source.Id;
            this.Name = source.Name;
            this.Description = source.Description;
            this.Updated = source.Updated;
            this.Path = source.Path;
            this.Levels = new ObservableCollection<CourseLevelViewModel>(source.Levels.Select(level => new CourseLevelViewModel(level)));
        }

        /// <summary>
        /// The course ID.
        /// </summary>
        public CourseId Id
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
        /// The name of the course.
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
        /// The description of the course.
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
        /// The time the course was last updated.
        /// </summary>
        public DateTime Updated
        {
            get
            {
                return this.updated;
            }

            set
            {
                this.SetProperty(ref this.updated, value);
            }
        }

        /// <summary>
        /// The path to the database file.
        /// </summary>
        public string Path
        {
            get
            {
                return this.path;
            }

            set
            {
                this.SetProperty(ref this.path, value);
            }
        }

        /// <summary>
        /// The ordered list of levels in the course.
        /// </summary>
        public ObservableCollection<CourseLevelViewModel> Levels
        {
            get
            {
                return this.levels;
            }

            set
            {
                this.SetProperty(ref this.levels, value);
            }
        }

        /// <summary>
        /// The selected level.
        /// </summary>
        public CourseLevelViewModel SelectedLevel
        {
            get
            {
                return this.selectedLevel;
            }

            set
            {
                this.SetProperty(ref this.selectedLevel, value);
            }
        }

        /// <summary>
        /// Convert the view model to the model class.
        /// </summary>
        /// <returns>The view model data in model format.</returns>
        public CourseDatabase ToSource()
        {
            var source = new CourseDatabase();
            source.Id = this.Id;
            source.Name = this.Name;
            source.Description = this.Description;
            source.Updated = this.Updated;
            source.Path = this.Path;

            var levels = new List<CourseLevel>();
            levels.AddRange(this.Levels.Select(entry => entry.ToSource()));
            source.Levels = (IList<ICourseLevel>)levels;

            return source;
        }
    }
}
