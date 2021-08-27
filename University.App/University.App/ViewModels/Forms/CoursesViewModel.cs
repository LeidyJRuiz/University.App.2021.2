using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using University.App.Helpers;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class CoursesViewModel: BaseViewModel
    {
        #region Fields
        private ApiService _apiService;
        private bool _isRefreshing;
        private ObservableCollection<CourseDTO> _courses;
        #endregion

        #region Properties

        public bool IsRefreshing
        {
            get { return this._isRefreshing; }
            set { this.SetValue(ref this._isRefreshing, value); }
        }

        public ObservableCollection<CourseDTO> Courses
        {
            get { return this._courses; }
            set { this.SetValue(ref this._courses, value); }
        }


        #endregion

        #region Constructor
        public CoursesViewModel()
        {
            this._apiService = new ApiService();
            this.RefreshCommand = new Command(GetCourses);
            this.RefreshCommand.Execute(null);
        }

        #endregion
        #region Methods
        async void GetCourses()
        {
            try
            {
                this.IsRefreshing = true;
                var connection = await _apiService.CheckConnection();
                if (!connection)
                {
                    this.IsRefreshing = false;
                    await Application.Current.MainPage.DisplayAlert("Notificación", "No internet conecction", "Cancel");
                    return;
                }
                var responseDTO = await _apiService.RequestAPI<List<CourseDTO>>(Endpoint.URL_BASE_UNIVERSITY_API,Endpoint.GET_COURSES,null, ApiService.Method.Get);

                this.Courses = new ObservableCollection<CourseDTO>((List<CourseDTO>)responseDTO.Data);
                this.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Notificación", ex.Message, "Cancel");
                
            }
        }

        #endregion
        #region  Commands

        public Command RefreshCommand { get; set;  }

        #endregion

    }
}
