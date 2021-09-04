using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using University.App.Helpers;
using University.BL.DTOs;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class DepartmentsViewModel : BaseViewModel
    {
        #region Fields
        private ApiService _apiService;
        private bool _isRefreshing;
        private ObservableCollection<DepartmentsItemViewModel> _departments;
        private List<DepartmentsItemViewModel> _allDepartments;
        private string _filter;

        #endregion

        #region Properties

        public bool IsRefreshing
        {
            get { return this._isRefreshing; }
            set
            {
                this.SetValue(ref this._isRefreshing, value);
            }
        }
        public ObservableCollection<DepartmentsItemViewModel> Departments
        {
            get { return this._departments; }
            set { this.SetValue(ref this._departments, value); }
        }
        public string Filter
        {
            get { return this._filter; }
            set
            {
                this.SetValue(ref this._filter, value);
                this.GetDepartmentsByFilter();
            }
        }


        #endregion

        #region Constructor
        public DepartmentsViewModel()
        {
            this._apiService = new ApiService();
            this.RefreshCommand = new Command(GetDepartments);
            this.RefreshCommand.Execute(null);
        }

        #endregion

        #region Methods
        async void GetDepartments()
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
                var responseDTO = await _apiService.RequestAPI<List<DepartmentsItemViewModel>>(Endpoint.URL_BASE_UNIVERSITY_API, Endpoint.GET_DEPARTMENTS, null, ApiService.Method.Get);


                this._allDepartments = (List<DepartmentsItemViewModel>)responseDTO.Data;
                this.Departments = new ObservableCollection<DepartmentsItemViewModel>(this._allDepartments);
                this.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Notificación", ex.Message, "Cancel");

            }
        }
        void GetDepartmentsByFilter()
        {
            var departments = this._allDepartments;
            if (!string.IsNullOrEmpty(this.Filter))

                departments = departments.Where(x => x.Instructor.FullName.ToLower().Contains(this.Filter)).ToList();
            this.Departments = new ObservableCollection<DepartmentsItemViewModel>(departments);


        }

        #endregion


        #region  Commands

        public Command RefreshCommand { get; set; }

        #endregion

    }
}
