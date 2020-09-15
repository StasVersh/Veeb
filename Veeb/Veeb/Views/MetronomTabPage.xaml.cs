using Xamarin.Forms;

namespace Veeb.Views
{
    public partial class MetronomTabPage : ContentPage
    {
        public MetronomTabPage()
        {
            InitializeComponent();
        }

        private void OnUnfocused(object sender, FocusEventArgs e)
        {
            var entry = sender as Entry;
            entry?.ReturnCommand.Execute(entry.ReturnCommandParameter);
        }
    }
}
