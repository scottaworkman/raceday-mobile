using System;
using Xamarin.Forms;

// Bindable view for the Android Floating Acction Button
//
namespace RaceDay
{
    public enum FloatingActionButtonSize
    {
        Normal = 0,
        Mini = 1
    }

    public class FloatingActionButtonView : Xamarin.Forms.View
    {
        public static readonly BindableProperty ImageNameProperty = BindableProperty.Create(nameof(ImageName), typeof(String), typeof(FloatingActionButtonView), string.Empty);
        public string ImageName
        {
            get { return (string)GetValue(ImageNameProperty); }
            set { SetValue(ImageNameProperty, value); }
        }

        public static readonly BindableProperty ColorNormalProperty = BindableProperty.Create(nameof(ColorNormal), typeof(Color), typeof(FloatingActionButtonView), Color.White);
        public Color ColorNormal
        {
            get { return (Color)GetValue(ColorNormalProperty); }
            set { SetValue(ColorNormalProperty, value); }
        }

        public static readonly BindableProperty ColorRippleProperty = BindableProperty.Create(nameof(ColorRipple), typeof(Color), typeof(FloatingActionButtonView), Color.White);
        public Color ColorRipple
        {
            get { return (Color)GetValue(ColorRippleProperty); }
            set { SetValue(ColorRippleProperty, value); }
        }

        public static readonly BindableProperty SizeProperty = BindableProperty.Create(nameof(Size), typeof(FloatingActionButtonSize), typeof(FloatingActionButtonView), FloatingActionButtonSize.Normal);
        public FloatingActionButtonSize Size
        {
            get { return (FloatingActionButtonSize)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public delegate void ShowHideDelegate();
        public delegate void TranslateDelegate(int yOffset);
        public delegate void AttachToListViewDelegate(ListView listView);

        public ShowHideDelegate Show { get; set; }
        public ShowHideDelegate Hide { get; set; }
        public TranslateDelegate TranslateY { get; set; }
        public Action<object, EventArgs> Clicked { get; set; }
    }
}
