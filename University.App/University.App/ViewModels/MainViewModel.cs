﻿using University.App.ViewModels.Forms;
using University.App.Views.Forms;
using Xamarin.Forms;

namespace University.App.ViewModels
{
    public class MainViewModel
    {

        public CoursesViewModel Courses { get; set; }
        public CreateCourseViewModel CreateCourse { get; set; }
        public CreateStudentViewModel CreateStudent { get; set; }
        public StudentsViewModel Students { get; set; }
        public EditCourseViewModel EditCourse { get; set; }
        public EditStudentViewModel EditStudent { get; set; }
        public CreateOfficeViewModel CreateOffice { get; set; }
        public OfficesViewModel Offices { get; set; }
        public HomeViewModel Home { get; set; }
        public EditOfficeViewModel EditOffice { get; set; }
        public InstructorsViewModel Instructors { get; set; }
        public CreateInstructorsViewModel CreateInstructors { get; set; }
        public EditInstructorsViewModel EditInstructors { get; set; }
        public MainViewModel()
        {
            instance = this;
            this.Home = new HomeViewModel();

            this.CreateCourseCommand = new Command(GoToCreateCourse);
          
            this.CreateStudentCommand = new Command(GoToCreateStudent);
            this.CreateOffice = new CreateOfficeViewModel();
            this.CreateOfficesCommand = new Command(GoToCreateOffices);
            this.CreateInstructorsCommand = new Command(GoToCreateInstructors);



        }

        #region Commands
        public Command CreateCourseCommand { get; set; }

        #endregion

        #region Methods
        async void GoToCreateCourse()
        {
            GetInstance().CreateCourse = new CreateCourseViewModel();

            await Application.Current.MainPage.Navigation.PushAsync(new CreateCoursePage());

        }
        #endregion

        #region Singleton

        private static MainViewModel instance;

        public static MainViewModel GetInstance()

        {

        if(instance == null)
            return new MainViewModel();

        return instance;
            }

        #endregion
        #region CommandsEstudents
        public Command CreateStudentCommand { get; set; }

        #endregion

        #region MethodsEstudents
        async void GoToCreateStudent()
        {
            GetInstance().CreateStudent = new CreateStudentViewModel();

            await Application.Current.MainPage.Navigation.PushAsync(new CreateStudentPage());

        }
        #endregion

        #region CommandsOffices
        public Command CreateOfficesCommand { get; set; }
        #endregion
        #region MethodsOffices
        async void GoToCreateOffices()
        {
            GetInstance().CreateOffice = new CreateOfficeViewModel();

            await Application.Current.MainPage.Navigation.PushAsync(new CreateOfficePage());

        }
        #endregion

        #region CommandsInstructors
        public Command CreateInstructorsCommand { get; set; }

        #endregion

        #region MethodsInstructors
        async void GoToCreateInstructors()
        {
            GetInstance().CreateInstructors = new CreateInstructorsViewModel();

            await Application.Current.MainPage.Navigation.PushAsync(new CreateInstructorsPage());

        }
        #endregion

    }
}

