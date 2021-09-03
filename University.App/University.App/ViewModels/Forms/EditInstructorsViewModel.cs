using System;
using System.Collections.Generic;
using System.Text;
using University.App.Helpers;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class EditInstructorsViewModel : BaseViewModel
    {
        #region Fields
        private ApiService _apiService;
        private InstructorDTO _instructors;
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

        public InstructorDTO Instructors
        {
            get { return this._instructors; }
            set { this.SetValue(ref this._instructors, value); }
        }


        #endregion

        #region Constructor
        public EditInstructorsViewModel(InstructorDTO instructor)
        {
            this._apiService = new ApiService();
            this.EditInstructorsCommand = new Command(EditInstructors);
            this.IsEnabled = true;
            this.Instructors = instructor;
        }

        #endregion

        #region Methods
        async void EditInstructors()
        {
            try
            {
                if (
                    String.IsNullOrEmpty(this.Instructors.LastName) ||
                    String.IsNullOrEmpty(this.Instructors.FirstMidName) ||
                    string.IsNullOrEmpty(this.Instructors.HireDate.ToString()) ||
                    this.Instructors.ID == 0)


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


                var message = "The process is successful";

                var responseDTO = await _apiService.RequestAPI<InstructorDTO>(Endpoint.URL_BASE_UNIVERSITY_API,
                    Endpoint.PUT_INSTRUCTORS + this.Instructors.ID, this.Instructors, ApiService.Method.Put);

                if (responseDTO.Code < 200 || responseDTO.Code > 299)
                    message = responseDTO.Message;

                this.IsEnabled = true;
                this.IsRunning = false;

                this.Instructors.ID = 0;
                this.Instructors.LastName = this.Instructors.FirstMidName = String.Empty;
                this.Instructors.HireDate = DateTime.Now;

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

        public Command EditInstructorsCommand { get; set; }

        #endregion

    }
}
