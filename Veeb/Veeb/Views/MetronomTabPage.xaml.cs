using System;
using Xamarin.Forms;
using Prism.Commands;
using Prism.Mvvm;
using Veeb.Models;

namespace Veeb.Views
{
    public partial class MetronomTabPage : ContentPage
    {
        //private bool start_on = false;
        //private double fromSecender;
        //private Settings settings;
        //public double FromeSecender
        //{
        //    get
        //    {
        //        FromeSecender = settings.FromeSecender;
        //        fromSecender = FromeSecender;
        //        return FromeSecender;
        //    }
        //    set
        //    {
        //        FromeSecender = fromSecender;
        //    }
        //}
        public MetronomTabPage()
        {
            InitializeComponent();
        }

        //private void StartOn(object sender, EventArgs e)
        //{
        //    if(!start_on)
        //    {
        //        start_on = true;
        //    }
        //    else
        //    {
        //        start_on = false;
        //        image.AnchorX = 70;
        //        image.Source = "gray_round_icon.png";
        //    }
        //}

        //private async void Animate()
        //{
        //    while (start_on)
        //    {
        //        image.Source = "blue_round_icon.png";
        //        await image.TranslateTo(140, 0, 250);
        //        image.Source = "green_round_icon.png";
        //        await image.TranslateTo(-140, 0, 250);
        //    }
        //}
    }
}
