using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using University.App.Helpers;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class StudentsViewModel:BaseViewModel
    {
        #region Fields
        private ApiService _apiService;
        private bool _isRefreshing;
        private ObservableCollection<StudentItemViewModel> _students;
        private List<StudentItemViewModel> _allStudents;
        private string _filter;
        #endregion

        #region Properties

        public bool IsRefreshing
        {
            get { return this._isRefreshing; }
            set { this.SetValue(ref this._isRefreshing, value); }
        }

        public ObservableCollection<StudentItemViewModel> Students
        {
            get { return this._students; }
            set { this.SetValue(ref this._students, value); }
        }
        public string Filter
        {
            get { return this._filter; }
            set
            {
                this.SetValue(ref this._filter, value);
                this.GetStudentsByFilter();
            }
        }


        #endregion
            #region Constructor
        public StudentsViewModel()
        {
            this._apiService = new ApiService();
            this.RefreshCommand = new Command(GetStudents);
            this.RefreshCommand.Execute(null);
        }


        #endregion
        #region Methods
        async void GetStudents()
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
                var responseDTO = await _apiService.RequestAPI<List<StudentItemViewModel>>(Endpoint.URL_BASE_UNIVERSITY_API, Endpoint.GET_STUDENTS, null, ApiService.Method.Get);

                this._allStudents = (List<StudentItemViewModel>) responseDTO.Data;
                this.Students = new ObservableCollection<StudentItemViewModel>(this._allStudents);
                this.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Notificación", ex.Message, "Cancel");

            }
        }
        void GetStudentsByFilter()
        {
            var students = this._allStudents;
            if (!string.IsNullOrEmpty(this.Filter))

                students = students.Where(x => x.FullName.ToLower().Contains(this.Filter)).ToList();
            this.Students = new ObservableCollection<StudentItemViewModel>(students);
        }

        #endregion
            #region  Commands

        public Command RefreshCommand { get; set; }

        #endregion
    }
}
