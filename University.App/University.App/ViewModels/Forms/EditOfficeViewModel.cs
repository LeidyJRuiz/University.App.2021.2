using System;
using System.Collections.Generic;
using System.Text;
using University.App.Helpers;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class EditOfficeViewModel : BaseViewModel
    {

        #region Fields
        private ApiService _apiService;
        private OfficeDTO _office;
        private bool _isEnabled;
        private bool _isRunning;
        private InstructorDTO _instructorSelected;
        private List<InstructorDTO> _instructors;


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
        public OfficeDTO Office
        {
            get { return this._office; }
            set { this.SetValue(ref this._office, value); }
        }

        public InstructorDTO InstructorSelected
        {
            get { return this._instructorSelected; }
            set { this.SetValue(ref this._instructorSelected, value); }
        }


        public List<InstructorDTO> Instructors
        {
            get { return this._instructors; }
            set { this.SetValue(ref this._instructors, value); }
        }

        #endregion

        #region Constructor
        public EditOfficeViewModel(OfficeDTO office)
        {
            this._apiService = new ApiService();
            this.EditOfficesCommand = new Command(EditOffice);
            
            this.GetInstructorsCommand.Execute(null);
            this.IsEnabled = true;
            this.Office = office;
            this.InstructorSelected = this.Office.Instructor;

        }

        #endregion

        #region Methods

        async void EditOffice()
        {
            try
            {
                if (String.IsNullOrEmpty(this.Office.Location) )
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
                var OfficeDTO = new OfficeDTO
                {
                    InstructorID = this.Office.InstructorID,
                    Location = this.Office.Location,
                    Instructor=this.Office.Instructor
                    
                };

                var message = "The process is successful";

                var responseDTO = await _apiService.RequestAPI<OfficeDTO>(Endpoint.URL_BASE_UNIVERSITY_API,
                    Endpoint.PUT_OFFICES + this.Office.InstructorID, this.Office, ApiService.Method.Put);

                if (responseDTO.Code < 200 || responseDTO.Code > 299)
                    message = responseDTO.Message;

                this.IsEnabled = true;
                this.IsRunning = false;

                this.Office.InstructorID  = 0;
                this.Office.Location= String.Empty;

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

        public Command EditOfficesCommand { get; set; }
        public Command GetInstructorsCommand { get; set; }
        #endregion
    }
}
