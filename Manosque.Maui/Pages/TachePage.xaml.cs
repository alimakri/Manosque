
using Manosque.Maui.Models;
using System.Collections.ObjectModel;
using System.Data;

namespace Manosque.Maui.Pages
{
    public partial class TachePage : ContentPage, IQueryAttributable
    {
        public TachePage()
        {
            InitializeComponent();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("tache"))
            {
                string? tache = query["tache"] as string;

                TacheName.Text = tache;
            }
        }

    }

}
