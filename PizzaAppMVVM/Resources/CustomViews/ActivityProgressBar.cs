using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

//namespace PizzaAppMVVM.Resources.CustomViews
namespace customviews
{
    [Register("customviews.ActivityProgressBar")]
    public class ActivityProgressBar : ProgressBar
    {
        public ActivityProgressBar(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public ActivityProgressBar(Context context) : base(context)
        {
        }

        public ActivityProgressBar(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public ActivityProgressBar(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public ActivityProgressBar(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        public bool IsVisible
        {
            get { return base.Visibility == ViewStates.Visible; }
            set
            {
                base.Visibility = value ? ViewStates.Visible : ViewStates.Invisible;
                base.Enabled = value;
            }
        }
    }
}