using System;
using University.App.Helpers;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class CreateCourseViewModel : BaseViewModel
    {
        #region Fields
        private ApiService _apiService;
        private int _courseID;
        private string _title;
        private int _credits;
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
        public int CourseID
        {
            get { return this._courseID; }
            set { this.SetValue(ref this._courseID, value); }
        }
        public string Title
        {
            get { return this._title; }
            set { this.SetValue(ref this._title, value); }
        }
        public int Credits
        {
            get { return this._credits; }
            set { this.SetValue(ref this._credits, value); }
        }



        #endregion

        #region Constructor
        public CreateCourseViewModel()
        {
            this._apiService = new ApiService();
            this.CreateCourseCommand = new Command(CreateCourse);
        }

        #endregion

        #region Methods
        async void CreateCourse()
        {
            try
            {
                if(String.IsNullOrEmpty(this.Title) ||
                    this.Credits ==0 ||
                        this.CourseID ==0)
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
                    CourseID = this.CourseID,
                    Title = this.Title,
                    Credits = this.Credits
                };

                var responseDTO = await _apiService.RequestAPI <CourseDTO>(Endpoint.URL_BASE_UNIVERSITY_API, 
                    Endpoint.POST_COURSES, CourseDTO, ApiService.Method.Post);

                this.IsEnabled = true;
                this.IsRunning = false;

                this.CourseID = this.Credits = 0;
                this.Title = String.Empty;

                await Application.Current.MainPage.DisplayAlert("Notificación", "The process is successful", "Cancel");


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

        public Command CreateCourseCommand { get; set; }

        #endregion

    }
}
