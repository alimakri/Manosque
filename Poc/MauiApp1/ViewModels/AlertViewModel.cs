using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MauiApp1.ViewModels
{
    public class AlertViewModel : INotifyPropertyChanged
    {
        private readonly IAlertService _alertService;

        public AlertViewModel(IAlertService alertService)
        {
            _alertService = alertService;
            ShowAlertCommand = new Command(async () => await ShowAlert());
        }

        public ICommand ShowAlertCommand { get; }

        private async Task ShowAlert()
        {
            await _alertService.ShowAlert("Alerte", "Ceci est un message d'alerte", "OK");
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    public interface IAlertService
    {
        Task ShowAlert(string title, string message, string cancel);
    }

    public class AlertService : IAlertService
    {
        public Task ShowAlert(string title, string message, string cancel)
        {
            return Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }
    }
}
