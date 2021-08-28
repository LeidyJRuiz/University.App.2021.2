using University.App.ViewModels.Forms;
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
        public MainViewModel()
        {
            instance = this;
            this.Courses = new CoursesViewModel();
            this.CreateCourseCommand = new Command(GoToCreateCourse);
            this.Students = new StudentsViewModel();
            this.CreateStudentCommand = new Command(GoToCreateStudent);

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

    }
}

