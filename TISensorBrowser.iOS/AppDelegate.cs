using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using Xamarin.Forms;
using Robotics.Mobile.Core.Bluetooth.LE;
using Syncfusion.SfChart.XForms;
using Syncfusion.SfChart.XForms.iOS.Renderers;

namespace TISensorBrowser
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	//public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		UIWindow window;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			Xamarin.Forms.Forms.Init ();

			UIApplication.SharedApplication.IdleTimerDisabled = true;
			new SfChartRenderer ();

			window = new UIWindow (UIScreen.MainScreen.Bounds);

			App.SetAdapter (Adapter.Current);

			window.RootViewController = App.GetMainPage ().CreateViewController ();
			window.MakeKeyAndVisible ();

			UINavigationBar.Appearance.TintColor = UIColor.Red;

			return true;
		}
	}
}

