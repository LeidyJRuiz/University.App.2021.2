using System;
using System.Collections.Generic;
using System.Text;
using University.App.Helpers;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class EditDepartmentsViewModel : BaseViewModel
    {

        #region Fields
        private ApiService _apiService;
        private DepartmentDTO _departments;
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
        public DepartmentDTO Departments
        {
            get { return this._departments; }
            set { this.SetValue(ref this._departments, value); }
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
        public EditDepartmentsViewModel(DepartmentDTO departments)
        {
            this._apiService = new ApiService();
            this.EditDepartmentsCommand = new Command(EditDepartments);
            this.GetInstructorsCommand = new Command(GetInstructors);
            this.GetInstructorsCommand.Execute(null);
            this.IsEnabled = true;
            this.Departments = departments;
            this.InstructorSelected = this.Departments.Instructor;

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

        async void EditDepartments()
        {
            try
            {
                if (String.IsNullOrEmpty(this.Departments.Name) ||
                    this.Departments.Budget == 0 ||
                     string.IsNullOrEmpty(this.Departments.StartDate.ToString()) ||
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


                var message = "The process is successful";

                var responseDTO = await _apiService.RequestAPI<DepartmentDTO>(Endpoint.URL_BASE_UNIVERSITY_API,
                    Endpoint.PUT_DEPARTMENTS + this.Departments.DepartmentID, this.Departments, ApiService.Method.Put);

                if (responseDTO.Code < 200 || responseDTO.Code > 299)
                    message = responseDTO.Message;

                this.IsEnabled = true;
                this.IsRunning = false;


                this.Departments.Name = String.Empty;
                this.Departments.Budget = 0;
                this.Departments.StartDate = DateTime.Now;
                this.Departments.InstructorID = this.InstructorSelected.ID;


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

        public Command EditDepartmentsCommand { get; set; }
        public Command GetInstructorsCommand { get; set; }
        #endregion
    }


}
