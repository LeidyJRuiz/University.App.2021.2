using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using University.App.Helpers;
using University.BL.Services.Implements;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class InstructorsViewModel :BaseViewModel
    {
        #region Fields
        private ApiService _apiService;
        private bool _isRefreshing;
        private ObservableCollection<InstructorsItemViewModel> _instructors;
        private List<InstructorsItemViewModel> _allInstructors;
        private string _filter;
        #endregion

        #region Properties

        public bool IsRefreshing
        {
            get { return this._isRefreshing; }
            set { this.SetValue(ref this._isRefreshing, value); }
        }

        public ObservableCollection<InstructorsItemViewModel> Instructors
        {
            get { return this._instructors; }
            set { this.SetValue(ref this._instructors, value); }
        }
        public string Filter
        {
            get { return this._filter; }
            set
            {
                this.SetValue(ref this._filter, value);
                this.GetInstructorsByFilter();
            }
        }


        #endregion

        #region Constructor
        public InstructorsViewModel()
        {
            this._apiService = new ApiService();
            this.RefreshCommand = new Command(GetInstructors);
            this.RefreshCommand.Execute(null);
        }


        #endregion

        #region Methods
        async void GetInstructors()
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
                var responseDTO = await _apiService.RequestAPI<List<InstructorsItemViewModel>>(Endpoint.URL_BASE_UNIVERSITY_API, Endpoint.GET_INSTRUCTORS, null, ApiService.Method.Get);

                this._allInstructors = (List<InstructorsItemViewModel>)responseDTO.Data;
                this.Instructors = new ObservableCollection<InstructorsItemViewModel>(this._allInstructors);
                this.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Notificación", ex.Message, "Cancel");

            }
        }
        void GetInstructorsByFilter()
        {
            var instructors = this._allInstructors;
            if (!string.IsNullOrEmpty(this.Filter))

                instructors = instructors.Where(x => x.FullName.ToLower().Contains(this.Filter)).ToList();
            this.Instructors = new ObservableCollection<InstructorsItemViewModel>(instructors);
        }

        #endregion

        #region  Commands

        public Command RefreshCommand { get; set; }

        #endregion

    }
}
