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
        }
        public Command GetCoursesCommand { get; set; }
        public Command GetStudentsCommand { get; set; }
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
    }
}
