using System;
using System.Collections.Generic;
using System.Text;
using University.App.Views.Forms;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class HomeViewModel :BaseViewModel

        
    {

        public HomeViewModel()
        {
            this.GetCoursesCommand = new Command(GoToCoursesPage);
            this.GetStudentsCommand = new Command(GoToStudentsPage);
            this.GetOfficesCommand = new Command(GoToOfficesPage);
            this.GetInstructorsCommand = new Command(GoToInstructorsPage);
        }
        public Command GetCoursesCommand { get; set; }
        public Command GetStudentsCommand { get; set; }
        public Command GetOfficesCommand { get; set; }
        public Command GetInstructorsCommand { get; set; }
        async void GoToCoursesPage()
        {
            MainViewModel.GetInstance().Courses = new CoursesViewModel();

            await Application.Current.MainPage.Navigation.PushAsync(new CoursesPage());

        }
        async void GoToStudentsPage()
        {
            MainViewModel.GetInstance().Students = new StudentsViewModel();

            await Application.Current.MainPage.Navigation.PushAsync(new StudentsPage());

        }
        async void GoToOfficesPage()
        {
            MainViewModel.GetInstance().Offices = new OfficesViewModel();

            await Application.Current.MainPage.Navigation.PushAsync(new OfficePage());

        }
        async void GoToInstructorsPage()
        {
            MainViewModel.GetInstance().Instructors = new InstructorsViewModel();

            await Application.Current.MainPage.Navigation.PushAsync(new InstructorsPage());

        }
    }
}
