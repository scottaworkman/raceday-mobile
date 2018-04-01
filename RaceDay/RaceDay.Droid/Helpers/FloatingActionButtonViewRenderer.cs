﻿using System;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Xamarin.Forms;
using RaceDay;
using RaceDay.Droid;
using Android.Views;
using System.IO;
using Android.Support.Design.Widget;
using Android.Content.Res;
using Android.Content;

[assembly: ExportRenderer(typeof(FloatingActionButtonView), typeof(FloatingActionButtonViewRenderer))]

namespace RaceDay.Droid
{
    public class FloatingActionButtonViewRenderer : ViewRenderer<FloatingActionButtonView, FrameLayout>
    {
        private const int MARGIN_DIPS = 16;
        private const int FAB_HEIGHT_NORMAL = 56;
        private const int FAB_HEIGHT_MINI = 40;
        private const int FAB_FRAME_HEIGHT_WITH_PADDING = (MARGIN_DIPS * 2) + FAB_HEIGHT_NORMAL;
        private const int FAB_FRAME_WIDTH_WITH_PADDING = (MARGIN_DIPS * 2) + FAB_HEIGHT_NORMAL;
        private const int FAB_MINI_FRAME_HEIGHT_WITH_PADDING = (MARGIN_DIPS * 2) + FAB_HEIGHT_MINI;
        private const int FAB_MINI_FRAME_WIDTH_WITH_PADDING = (MARGIN_DIPS * 2) + FAB_HEIGHT_MINI;
        private readonly Android.Content.Context context;
        private readonly FloatingActionButton fab;

        public FloatingActionButtonViewRenderer(Context _context) : base(_context)
        {
            context = _context;

            float d = context.Resources.DisplayMetrics.Density;
            var margin = (int)(MARGIN_DIPS * d); // margin in pixels

            fab = new FloatingActionButton(context);
            var lp = new FrameLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
            lp.Gravity = GravityFlags.CenterVertical | GravityFlags.CenterHorizontal;
            lp.LeftMargin = margin;
            lp.TopMargin = margin;
            lp.BottomMargin = margin;
            lp.RightMargin = margin;
            fab.LayoutParameters = lp;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<FloatingActionButtonView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || this.Element == null)
                return;

            if (e.OldElement != null)
                e.OldElement.PropertyChanged -= HandlePropertyChanged;

            if (this.Element != null)
            {
                this.Element.PropertyChanged += HandlePropertyChanged;
            }

            Element.Show = Show;
            Element.Hide = Hide;
            Element.TranslateY = TranslateY;

            SetFabImage(Element.ImageName);
            SetFabSize(Element.Size);

            ColorStateList csl = new ColorStateList(new int[][] { new int[0] }, new int[] { Element.ColorNormal.ToAndroid() });
            fab.BackgroundTintList = csl;
            fab.RippleColor = Element.ColorRipple.ToAndroid();

            fab.Click += Fab_Click;

            var frame = new FrameLayout(context);
            frame.RemoveAllViews();
            frame.AddView(fab);

            SetNativeControl(frame);
        }

        public void Show()
        {
            fab.Show();
        }

        public void Hide()
        {
            fab.Hide();
        }

        public void TranslateY(int yOffset)
        {
            fab.Animate().TranslationYBy(yOffset);
        }

        void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Content")
            {
                Tracker.UpdateLayout();
            }
            else if (e.PropertyName == FloatingActionButtonView.ColorNormalProperty.PropertyName)
            {
                ColorStateList csl = new ColorStateList(new int[][] { new int[0] }, new int[] { Element.ColorNormal.ToAndroid() });
                fab.BackgroundTintList = csl;
            }
            else if (e.PropertyName == FloatingActionButtonView.ColorRippleProperty.PropertyName)
            {
                fab.RippleColor = Element.ColorRipple.ToAndroid();
            }
            else if (e.PropertyName == FloatingActionButtonView.ImageNameProperty.PropertyName)
            {
                SetFabImage(Element.ImageName);
            }
            else if (e.PropertyName == FloatingActionButtonView.SizeProperty.PropertyName)
            {
                SetFabSize(Element.Size);
            }
        }

        void SetFabImage(string imageName)
        {
            if (!string.IsNullOrWhiteSpace(imageName))
            {
                try
                {
                    var drawableNameWithoutExtension = Path.GetFileNameWithoutExtension(imageName).ToLower();
                    var resources = context.Resources;
                    var imageResourceName = resources.GetIdentifier(drawableNameWithoutExtension, "drawable", context.PackageName);
                    fab.SetImageBitmap(Android.Graphics.BitmapFactory.DecodeResource(context.Resources, imageResourceName));
                }
                catch (Exception ex)
                {
                    throw new FileNotFoundException("There was no Android Drawable by that name.", ex);
                }
            }
        }

        void SetFabSize(FloatingActionButtonSize size)
        {
            if (size == FloatingActionButtonSize.Mini)
            {
                fab.Size = (int)FloatingActionButtonSize.Mini;
                Element.WidthRequest = FAB_MINI_FRAME_WIDTH_WITH_PADDING;
                Element.HeightRequest = FAB_MINI_FRAME_HEIGHT_WITH_PADDING;
            }
            else
            {
                fab.Size = (int)FloatingActionButtonSize.Normal;
                Element.WidthRequest = FAB_FRAME_WIDTH_WITH_PADDING;
                Element.HeightRequest = FAB_FRAME_HEIGHT_WITH_PADDING;
            }
        }

        void Fab_Click(object sender, EventArgs e)
        {
            var clicked = Element.Clicked;
            if (Element != null && clicked != null)
            {
                clicked(sender, e);
            }
        }
    }
}
