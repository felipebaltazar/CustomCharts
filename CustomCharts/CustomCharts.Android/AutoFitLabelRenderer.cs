using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using CustomCharts.Droid;
using CustomCharts.View;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(AutoFitLabel), typeof(AutoFitLabelRenderer))]
namespace CustomCharts.Droid
{
    public class AutoFitLabelRenderer : LabelRenderer
    {
        public AutoFitLabelRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
                TextViewCompat.SetAutoSizeTextTypeWithDefaults(Control, (int)AutoSizeTextType.Uniform);
        }
    }
}