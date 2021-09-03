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
    public class OfficesViewModel : BaseViewModel
    {

        #region Fields
        private ApiService _apiService;
        private bool _isRefreshing;
        private ObservableCollection<OfficesItemViewModel> _office;
        private List<OfficesItemViewModel> _allOffices;
        private string _filter;

        #endregion
        #region Properties

        public bool IsRefreshing
        {
            get { return this._isRefreshing; }
            set { this.SetValue(ref this._isRefreshing, value);
            }
        }
        public ObservableCollection<OfficesItemViewModel> Offices
        {
            get { return this._office; }
            set { this.SetValue(ref this._office, value); }
        }
        public string Filter
        {
            get { return this._filter; }
            set
            {
                this.SetValue(ref this._filter, value);
                this.GetOfficesByFilter();
            }
        }


        #endregion
        #region Constructor
        public OfficesViewModel()
        {
            this._apiService = new ApiService();
            this.RefreshCommand = new Command(GetOffices);
            this.RefreshCommand.Execute(null);
        }

        #endregion

        #region Methods
        async void GetOffices()
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
                var responseDTO = await _apiService.RequestAPI<List<OfficesItemViewModel>>(Endpoint.URL_BASE_UNIVERSITY_API, Endpoint.GET_OFFICES, null, ApiService.Method.Get);


                this._allOffices = (List<OfficesItemViewModel>)responseDTO.Data;
                this.Offices = new ObservableCollection<OfficesItemViewModel>(this._allOffices);
                this.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Notificación", ex.Message, "Cancel");

            }
        }
        void GetOfficesByFilter()
        {
            var offices = this._allOffices;
            if (!string.IsNullOrEmpty(this.Filter))

                offices = offices.Where(x => x.Instructor.FullName.ToLower().Contains(this.Filter)).ToList();
            this.Offices = new ObservableCollection<OfficesItemViewModel>(offices);


        }

        #endregion
        #region  Commands

        public Command RefreshCommand { get; set; }

        #endregion



    }
}
