using System;
using System.Collections.Generic;
using System.Text;
using University.App.Helpers;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class CreateDepartmentsViewModel :BaseViewModel
    {
        #region Fields
        private ApiService _apiService;
        private string _name;
        private double _budget;
        private DateTime _startdate = DateTime.Now;
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
        public string Name
        {
            get { return this._name; }
            set { this.SetValue(ref this._name, value); }
        }

        public double Budget
        {
            get { return this._budget; }
            set { this.SetValue(ref this._budget, value); }
        }

        public DateTime StartDate
        {
            get { return this._startdate; }
            set { this.SetValue(ref this._startdate, value); }
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
        public CreateDepartmentsViewModel()
        {
            this._apiService = new ApiService();
            this.CreateDepartmentsCommand = new Command(CreateDepartments);
            this.GetInstructorsCommand = new Command(GetInstructors);
            this.GetInstructorsCommand.Execute(null);
            this.IsEnabled = true;

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
        async void CreateDepartments()
        {
            try
            {
                if (String.IsNullOrEmpty(this.Name) ||
                    this.Budget==0||
                     string.IsNullOrEmpty(this.StartDate.ToString()) ||
                    this.InstructorSelected == null)

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
                var departmentDTO = new DepartmentDTO
                {
                    Name = this.Name,
                    Budget = this.Budget,
                    StartDate=this.StartDate,
                    InstructorID=this.InstructorSelected.ID

                };

                var message = "The process is successful";

                var responseDTO = await _apiService.RequestAPI<DepartmentDTO>(Endpoint.URL_BASE_UNIVERSITY_API,
                    Endpoint.POST_DEPARTMENTS, departmentDTO, ApiService.Method.Post);

                if (responseDTO.Code < 200 || responseDTO.Code > 299)
                    message = responseDTO.Message;

                this.IsEnabled = true;
                this.IsRunning = false;

                this.Name = String.Empty;
                this.Budget = 0;
                this.StartDate = DateTime.Now;


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

        public Command CreateDepartmentsCommand { get; set; }
        public Command GetInstructorsCommand { get; set; }

        #endregion

    }
}
