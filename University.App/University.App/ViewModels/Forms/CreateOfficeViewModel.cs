using System;
using System.Collections.Generic;
using University.App.Helpers;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class CreateOfficeViewModel :BaseViewModel
    {
        #region Fields
        private ApiService _apiService;
        private string _location;
        private bool _isEnabled;
        private bool _isRunning;
        private List<InstructorDTO> _instructors;
        private InstructorDTO _instructorSelected;

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
        public string Location
        {
            get { return this._location; }
            set { this.SetValue(ref this._location, value); }
        }

        public List<InstructorDTO> Instructors
        {
            get { return this._instructors; }
            set { this.SetValue(ref this._instructors, value); }
        }

        public InstructorDTO InstructorSelected
        {
            get { return this._instructorSelected; }
            set { this.SetValue(ref this._instructorSelected, value); }
        }
        #endregion

        #region Constructor
        public CreateOfficeViewModel()
        {
            this._apiService = new ApiService();
            this.CreateOfficeCommand = new Command(CreateOffice);
            this.GetInstructorsCommand = new Command(GetInstructors);
            this.GetInstructorsCommand.Execute(null);
            this.IsEnabled = true;

            //editpage
            //this.Office = office
        }

        #endregion

        #region Methods

        async void GetInstructors()
        {
            try
            {
                var connection = await _apiService.CheckConnection();
                if (!connection)
                {
                    this.IsEnabled = true;
                    this.IsRunning = false;

                    await Application.Current.MainPage.DisplayAlert("Notificación", "No internet conecction", "Cancel");
                    return;
                }
                    var responseDTO = await _apiService.RequestAPI<List<InstructorDTO>>(Endpoint.URL_BASE_UNIVERSITY_API,
                   Endpoint.GET_INSTRUCTORS, null, ApiService.Method.Get);

                this.Instructors = (List<InstructorDTO>)responseDTO.Data;

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Notificación", ex.Message, "Cancel");
            }

        }
        async void CreateOffice()
        {
            try
            {
                if (String.IsNullOrEmpty(this.Location) ||
                    this.InstructorSelected== null)
                    
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
                var officeDTO = new OfficeDTO
                {
                    InstructorID = this.InstructorSelected.ID,
                    Location = this.Location
                  
                };

                var message = "The process is successful";

                var responseDTO = await _apiService.RequestAPI<OfficeDTO>(Endpoint.URL_BASE_UNIVERSITY_API,
                    Endpoint.POST_OFFICES, officeDTO, ApiService.Method.Post);

                if (responseDTO.Code < 200 || responseDTO.Code > 299)
                    message = responseDTO.Message;

                this.IsEnabled = true;
                this.IsRunning = false;

                this.Location = String.Empty;
               

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

        public Command CreateOfficeCommand { get; set; }
        public Command GetInstructorsCommand { get; set; }

        #endregion


    }
}
