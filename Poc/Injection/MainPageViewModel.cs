using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Injection
{
    internal class MainPageViewModel 
    {
        public string Hello { get; set; } = "Hello everybody";
        ICommand GoCommand { get; }
        public MainPageViewModel()
        {
            GoCommand = new Command(GoCommandExecute);
        }

        private async void GoCommandExecute()
        {
            await Shell.Current.GoToAsync(nameof(SecondPage)); 
        }
    }
}
