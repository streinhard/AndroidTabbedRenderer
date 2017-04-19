﻿using System;
using System.Collections.Specialized;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Content.Res;
using Android.Support.V4.View;
using Android.Support.V7.View;
using AndroidTabbedRenderer;
using AndroidTabbedRenderer.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;


using Color = Android.Graphics.Color;
using Android.Support.V7.View.Menu;

[assembly: ExportRenderer(typeof(BottomTabbedPage), typeof(BottomTabbedPageRenderer))]
namespace AndroidTabbedRenderer.Droid
{
	public class BottomTabbedPageRenderer : TabbedPageRenderer, BottomNavigationView.IOnNavigationItemSelectedListener
	{
		TabLayout tabLayout;
		ViewPager viewPager;

		BottomNavigationView bottomNavigationView;

		protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
		{
			base.OnElementChanged(e);

			if (bottomNavigationView == null)
			{
				FindViews();

				tabLayout.Visibility = ViewStates.Gone;

				bottomNavigationView = new BottomNavigationView(this.Context);
				bottomNavigationView.SetBackgroundResource(Resource.Color.indigo);
				bottomNavigationView.ItemIconTintList = ColorStateList.ValueOf(Color.White);
				bottomNavigationView.ItemTextColor = ColorStateList.ValueOf(Color.White);
				bottomNavigationView.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
				AddView(bottomNavigationView);

				bottomNavigationView.SetOnNavigationItemSelectedListener(this);
			}

			if (e.NewElement != null)
			{
				var page = e.NewElement as BottomTabbedPage;

				if (page.AndroidMenu == BottomMenu.Photos)
				{
					bottomNavigationView.InflateMenu(Resource.Menu.photos_menu);
				}
			}
		}

		protected override void OnLayout(bool changed, int l, int t, int r, int b)
		{
			base.OnLayout(changed, l, t, r, b);

			int width = r - l;
			int height = b - t;

			bottomNavigationView.Measure(MakeMeasureSpec(width, MeasureSpecMode.Exactly), MakeMeasureSpec(height, MeasureSpecMode.AtMost));

			var viewHeight = Math.Min(height, Math.Max(bottomNavigationView.MeasuredHeight, bottomNavigationView.MinimumHeight));
			var viewY = height - viewHeight;

			bottomNavigationView.Layout(0, viewY, width, height);
		}

		public static int MakeMeasureSpec(int size, MeasureSpecMode mode)
		{
			return size + (int)mode;
		}

		private void FindViews()
		{
			for (int i = 0; i < ViewGroup.ChildCount; i++)
			{
				var child = ViewGroup.GetChildAt(i);

				var asTabLayout = child as TabLayout;
				if (asTabLayout != null)
					this.tabLayout = asTabLayout;

				var asViewPager = child as ViewPager;
				if (asViewPager != null)
					this.viewPager = asViewPager;
			}
		}

		public bool OnNavigationItemSelected(IMenuItem item)
		{
			var index = item.Order;

			if (index < viewPager.Adapter.Count)
			{
				viewPager.CurrentItem = index;
				return true;
			}

			return false;
		}
	}
}