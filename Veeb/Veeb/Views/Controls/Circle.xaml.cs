using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veeb.Models;
using Veeb.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Veeb.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Circle : ContentView
    {
        public static BindableProperty CurrentStateProperty =
            BindableProperty.Create(nameof(CurrentState), typeof(State),
                typeof(Circle), default(State), propertyChanged: OnCurrentStatePropertyChanged);
        public static BindableProperty StateProperty =
            BindableProperty.Create(nameof(State), typeof(State),
                typeof(Circle), default(State));

        public Circle()
        {
            InitializeComponent();
        }

        public State CurrentState
        {
            get => (State)GetValue(CurrentStateProperty);
            set => SetValue(CurrentStateProperty, value);
        }

        public State State
        {
            get => (State)GetValue(StateProperty);
            set => SetValue(StateProperty, value);
        }

        private static void OnCurrentStatePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var circle = bindable as Circle;
            circle.Ellipse.Fill = ((State)newvalue) == circle.State ? Colors.OnColor : Colors.OffColor;
        }
    }
}