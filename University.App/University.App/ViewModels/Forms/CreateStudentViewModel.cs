using System;
using System.Collections.Generic;
using System.Text;
using University.BL.Services.Implements;
using Xamarin.Forms;
using University.BL.DTOs;
using University.App.Helpers;

namespace University.App.ViewModels.Forms
{
    public class CreateStudentViewModel :BaseViewModel
    {

        #region Fields
        private ApiService _apiService;
        private string _lastName;
        private string _firstMidName;
        private DateTime _enrollmentDate= DateTime.Now;
        private string _fullName;
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

        public string LastName
        {
            get { return this._lastName; }
            set { this.SetValue(ref this._lastName, value); }
        }
        public string FirstMidName
        {
            get { return this._firstMidName; }
            set { this.SetValue(ref this._firstMidName, value); }
        }

        public DateTime EnrollmentDate
        { 
              get { return this._enrollmentDate; }
    set { this.SetValue(ref this._enrollmentDate, value); }
}

        public string FullName
        {
            get { return this._fullName; }
            set { this.SetValue(ref this._fullName, value); }
        }


        #endregion

        #region Constructor
        public CreateStudentViewModel()
        {
            this._apiService = new ApiService();
            this.CreateStudentCommand = new Command(CreateStudent);
            this.IsEnabled = true;
        }

        #endregion
        #region Methods
        async void CreateStudent()
        {
            try
            {
                if (
                    String.IsNullOrEmpty(this.LastName) ||
                    String.IsNullOrEmpty(this.FirstMidName) ||
                    string.IsNullOrEmpty (this.EnrollmentDate.ToString())
                    
                    
                       )
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
                var StudentsDTO = new StudentsDTO
                {
                    
                    LastName = this.LastName,
                    FirstMidName = this.FirstMidName,
                    EnrollmentDate = this.EnrollmentDate,
                    FullName = this.FullName
                    
                };

                var message = "The process is successful";

                var responseDTO = await _apiService.RequestAPI<StudentsDTO>(Endpoint.URL_BASE_UNIVERSITY_API,
                    Endpoint.POST_STUDENTS, StudentsDTO, ApiService.Method.Post);

                if (responseDTO.Code < 200 || responseDTO.Code > 299)
                    message = responseDTO.Message;

                this.IsEnabled = true;
                this.IsRunning = false;

                
                this.LastName = this.FirstMidName=this.FullName= String.Empty;
                this.EnrollmentDate = DateTime.Now;

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

        public Command CreateStudentCommand { get; set; }

        #endregion
    }
}
