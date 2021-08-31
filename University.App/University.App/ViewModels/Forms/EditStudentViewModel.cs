using System;
using University.App.Helpers;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class EditStudentViewModel : BaseViewModel
    {
        #region Fields
        private ApiService _apiService;
        private StudentsDTO _student;
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

        public StudentsDTO Student
        {
            get { return this._student; }
            set { this.SetValue(ref this._student, value); }
        }
  

        #endregion

        #region Constructor
        public EditStudentViewModel(StudentsDTO student)
        {
            this._apiService = new ApiService();
            this.EditStudentCommand = new Command(EditStudent);
            this.IsEnabled = true;
            this.Student = student;
        }

        #endregion
        #region Methods
        async void EditStudent()
        {
            try
            {
                if (String.IsNullOrEmpty(this.Student.FullName) ||
                    String.IsNullOrEmpty(this.Student.LastName) ||
                    String.IsNullOrEmpty(this.Student.FirstMidName) ||
                    string.IsNullOrEmpty(this.Student.EnrollmentDate.ToString()) ||
                    String.IsNullOrEmpty(this.Student.LastName) ||
                    this.Student.ID == 0)

                       
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
                    ID = this.Student.ID,
                    LastName = this.Student.LastName,
                    FirstMidName = this.Student.FirstMidName,
                    EnrollmentDate = this.Student.EnrollmentDate,
                    FullName = this.Student.FullName
                };

                var message = "The process is successful";

                var responseDTO = await _apiService.RequestAPI<StudentsDTO>(Endpoint.URL_BASE_UNIVERSITY_API,
                    Endpoint.PUT_STUDENTS + this.Student.ID, this.Student, ApiService.Method.Put);

                if (responseDTO.Code < 200 || responseDTO.Code > 299)
                    message = responseDTO.Message;

                this.IsEnabled = true;
                this.IsRunning = false;

                this.Student.ID = 0;
                this.Student.LastName = this.Student.FirstMidName = this.Student.FullName = String.Empty;
                this.Student.EnrollmentDate = DateTime.Now;

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

        public Command EditStudentCommand { get; set; }

        #endregion
    }
}
