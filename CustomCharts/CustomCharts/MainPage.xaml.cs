using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CustomCharts
{
#if Release
    [XamlCompilation(XamlCompilationOptions.Compile)]
#endif
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await simpleChart.AnimateAsync();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await simpleChart.AnimateAsync();
        }
    }
}
