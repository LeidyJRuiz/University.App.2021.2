using System;
using University.App.Helpers;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class EditCourseViewModel : BaseViewModel
    {

        #region Fields
        private ApiService _apiService;
        private CourseDTO _course;
        private bool _isEnabled;
        private bool _isRunning;

        #endregion

        #region Properties

        public bool IsEnabled
        {
            get { return this._isEnabled; }
            set { this.SetValue(ref this._isEnabled, value); }
        }

        public bool IsRunning
        {
            get { return this._isRunning; }
            set { this.SetValue(ref this._isRunning, value); }
        }
        public CourseDTO Course
        {
            get { return this._course; }
            set { this.SetValue(ref this._course, value); }
        }



        #endregion

        #region Constructor
        public EditCourseViewModel(CourseDTO course)
        {
            this._apiService = new ApiService();
            this.EditCourseCommand = new Command(EditCourse);
            this.IsEnabled = true;
            this.Course = course;
        }

        #endregion

        #region Methods
        async void EditCourse()
        {
            try
            {
                if (String.IsNullOrEmpty(this.Course.Title) ||
                    this.Course.Credits == 0 ||
                        this.Course.CourseID == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Notificación", "The Fields are required", "Cancel");
                    return;
                }

                this.IsEnabled = false;
                this.IsRunning = true;

                var connection = await _apiService.CheckConnection();
                if (!connection)
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;

                    await Application.Current.MainPage.DisplayAlert("Notificación", "No internet conecction", "Cancel");
                    return;
                }
                var CourseDTO = new CourseDTO
                {
                    CourseID = this.Course.CourseID,
                    Title = this.Course.Title,
                    Credits = this.Course.Credits
                };

                var message = "The process is successful";

                var responseDTO = await _apiService.RequestAPI<CourseDTO>(Endpoint.URL_BASE_UNIVERSITY_API,
                    Endpoint.PUT_COURSES + this.Course.CourseID, this.Course, ApiService.Method.Put);

                if (responseDTO.Code < 200 || responseDTO.Code > 299)
                    message = responseDTO.Message;

                this.IsEnabled = true;
                this.IsRunning = false;

                this.Course.CourseID = this.Course.Credits = 0;
                this.Course.Title = String.Empty;

                await Application.Current.MainPage.DisplayAlert("Notificación", message, "Cancel");


            }
            catch (Exception ex)
            {
                this.IsEnabled = true;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert("Notificación", ex.Message, "Cancel");

            }
        }

        #endregion

        #region  Commands

        public Command EditCourseCommand { get; set; }

        #endregion

    }
}
