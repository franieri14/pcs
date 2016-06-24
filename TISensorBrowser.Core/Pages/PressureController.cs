using System;

using Xamarin.Forms;
using Syncfusion.SfChart.XForms;
using System.Diagnostics;

namespace TISensorBrowser
{
	public class PressureController : ContentPage
	{

		public AbsoluteLayout simpleLayout;

		public Balloon b1;
		public Balloon b2;
		public Balloon b3;

		public Label setPoint1Label;
		public Label setPoint2Label;
		public Label setPoint3Label;

		public Label actualPressure1Label;
		public Label actualPressure2Label;
		public Label actualPressure3Label;

		public Button allButton;
		public Button b1Button;
		public Button b2Button;
		public Button b3Button;

		//Actual Pressures
		public int sensorValue1=0;
		public int sensorValue2=0;
		public int sensorValue3=0;

		//Set Points
		public int setPoint1=0;
		public int setPoint2=0;
		public int setPoint3=0;

		public int allOn = 0;
		public int b1On = 0;
		public int b2On = 0;
		public int b3On;

		public PressureController ()
		{
			//===========================================
			//Pickers


			String[] procedureOptions = { "PROCEDURE TYPE", "Normal Colon", "Small Colon", "Upper GI" };
			PickerCell procedure = new PickerCell (procedureOptions, 170);
			AbsoluteLayout.SetLayoutFlags (procedure.picker, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds (procedure.picker, new Rectangle (0.025, .05, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));


			String[] endoscopeOptions = { "ENDOSCOPE TYPE", "Adult Colonoscope", "Pediatric Colonosope", "Upper Endoscope" };
			PickerCell endoscopeType = new PickerCell (endoscopeOptions, 170);
			AbsoluteLayout.SetLayoutFlags (endoscopeType.picker, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds (endoscopeType.picker, new Rectangle (.52, .05, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));


			String[] displayOptions = { "DISPLAY TYPE", "Pressure (mmHg)", "Size (mm)"};
			PickerCell displayType = new PickerCell (displayOptions, 140);
			AbsoluteLayout.SetLayoutFlags (displayType.picker, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds (displayType.picker, new Rectangle (.95, .05, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));




			//===========================================
			//Balloons

			b1 = new Balloon (1, sensorValue1);
			AbsoluteLayout.SetLayoutFlags (b1.circle, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds (b1.circle, new Rectangle (.1, .1, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

			b2 = new Balloon (2, sensorValue2);
			AbsoluteLayout.SetLayoutFlags (b2.circle, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds (b2.circle, new Rectangle (.5, .1, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

			b3 = new Balloon (3, sensorValue3);
			AbsoluteLayout.SetLayoutFlags (b3.circle, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds (b3.circle, new Rectangle (.9, .1, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));


			//===========================================
			// Images

			Image up1 = new Image { Source = "up.png" };
			AbsoluteLayout.SetLayoutFlags (up1, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds (up1, new Rectangle (.2, .6, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
			var tapUp1 = new TapGestureRecognizer();
			tapUp1.Tapped += (s, e) => {
				Debug.WriteLine("Pressed Up 1");
				if (setPoint1 < 200) {
					setPoint1++;
					Byte[] report = {0,Convert.ToByte(setPoint1)};
					App.appWrite (report).Wait();
				}
				updateSetPointUI();

				//update button UI
			};
			up1.GestureRecognizers.Add(tapUp1);

			Image down1 = new Image { Source = "down.png" };
			AbsoluteLayout.SetLayoutFlags (down1, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds (down1, new Rectangle (.2, .81, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
			var tapDown1 = new TapGestureRecognizer();
			tapDown1.Tapped += (s, e) => {
				Debug.WriteLine("Pressed Down 1");
				if (setPoint1 > 0) {
					setPoint1--;
					Byte[] report = {1,Convert.ToByte(setPoint1)};
					App.appWrite (report).Wait();
				} 
				updateSetPointUI();
			};
			down1.GestureRecognizers.Add(tapDown1);

			//--------------------

			Image up2 = new Image { Source = "up.png" };
			AbsoluteLayout.SetLayoutFlags (up2, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds (up2, new Rectangle (.47, .6, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
			var tapUp2 = new TapGestureRecognizer();
			tapUp2.Tapped += (s, e) => {
				Debug.WriteLine("Pressed Up 2");
				if (setPoint2 < 200) {
					setPoint2++;
					Byte[] report = {2,Convert.ToByte(setPoint2)};
					App.appWrite (report).Wait();
				} 
				updateSetPointUI();
			};
			up2.GestureRecognizers.Add(tapUp2);

			//------

			Image down2 = new Image { Source = "down.png" };
			AbsoluteLayout.SetLayoutFlags (down2, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds (down2, new Rectangle (.47, .81, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
			var tapDown2 = new TapGestureRecognizer();
			tapDown2.Tapped += (s, e) => {
				Debug.WriteLine("Pressed Down 2");
				if (setPoint2 > 0) {
					setPoint2--;
					Byte[] report = {3,Convert.ToByte(setPoint2)};
					App.appWrite (report).Wait();
				} 
				updateSetPointUI();
			};
			down2.GestureRecognizers.Add(tapDown2);

			Image up3 = new Image { Source = "up.png" };
			AbsoluteLayout.SetLayoutFlags (up3, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds (up3, new Rectangle (.74, .6, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
			var tapUp3 = new TapGestureRecognizer();
			tapUp3.Tapped += (s, e) => {
				Debug.WriteLine("Pressed Up 3");
				if (setPoint3 < 200) {
					setPoint3++;
					Byte[] report = {4,Convert.ToByte(setPoint3)};
					App.appWrite (report).Wait();
				} 
				updateSetPointUI();
			};
			up3.GestureRecognizers.Add(tapUp3);

			Image down3 = new Image { Source = "down.png" };
			AbsoluteLayout.SetLayoutFlags (down3, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds (down3, new Rectangle (.74, .81, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
			var tapDown3 = new TapGestureRecognizer();
			tapDown3.Tapped += (s, e) => {
				Debug.WriteLine("Pressed Down 3");
				if (setPoint3 > 0) {
					setPoint3--;
					Byte[] report = {5,Convert.ToByte(setPoint3)};
					App.appWrite (report).Wait();
				} 
				updateSetPointUI();
			};
			down3.GestureRecognizers.Add(tapDown3);


			//==========================================

			Label setPointLabel = new Label {
				Text = "SP:",
				FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};
			AbsoluteLayout.SetLayoutFlags (setPointLabel, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds (setPointLabel, new Rectangle (.01, .7, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));


			setPoint1Label = new Label {
				Text = setPoint1 + " mmhg",
				FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};
			AbsoluteLayout.SetLayoutFlags (setPoint1Label, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds (setPoint1Label, new Rectangle (.2, .7, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));


			setPoint2Label = new Label {
				Text = setPoint2 + " mmhg",
				FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};
			AbsoluteLayout.SetLayoutFlags (setPoint2Label, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds (setPoint2Label, new Rectangle (.48, .7, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));


			setPoint3Label = new Label {
				Text = setPoint3 + " mmhg",
				FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};
			AbsoluteLayout.SetLayoutFlags (setPoint3Label, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds (setPoint3Label, new Rectangle (.78, .7, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

			actualPressure1Label = new Label {
				Text = sensorValue1 + " mmhg",
				FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};
			AbsoluteLayout.SetLayoutFlags (actualPressure1Label, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds (actualPressure1Label, new Rectangle (.18, .25, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

			actualPressure2Label = new Label {
				Text = sensorValue2 + " mmhg",
				FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};
			AbsoluteLayout.SetLayoutFlags (actualPressure2Label, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds (actualPressure2Label, new Rectangle (.47, .25, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

			actualPressure3Label = new Label {
				Text = sensorValue3 + " mmhg",
				FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label)),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};
			AbsoluteLayout.SetLayoutFlags (actualPressure3Label, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds (actualPressure3Label, new Rectangle (.77, .25, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));


			//==========================================

			allButton = new Button
			{
				Text = "All Off",
				Font = Font.SystemFontOfSize(NamedSize.Large),
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HeightRequest = 30,
				TextColor = Color.FromRgb(230,230,230),
				BackgroundColor = Color.FromRgb(255,255,255),
				BorderColor = Color.FromRgb(230,230,230),


				
			};
			AbsoluteLayout.SetLayoutFlags (allButton, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds (allButton, new Rectangle (.01, .95, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
			allButton.Clicked += AllButtonClicked;


			b1Button = new Button
			{
				Text = "Off",
				Font = Font.SystemFontOfSize(NamedSize.Medium),
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HeightRequest = 30,
				TextColor = Color.FromRgb(230,230,230),
				BackgroundColor = Color.FromRgb(255,255,255),
				BorderColor = Color.FromRgb(230,230,230),
			};
			AbsoluteLayout.SetLayoutFlags (b1Button, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds (b1Button, new Rectangle (.2, .95, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
			b1Button.Clicked += B1ButtonClicked;

			b2Button = new Button
			{
				Text = "Off",
				Font = Font.SystemFontOfSize(NamedSize.Medium),
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HeightRequest = 30,
				TextColor = Color.FromRgb(230,230,230),
				BackgroundColor = Color.FromRgb(255,255,255),
				BorderColor = Color.FromRgb(230,230,230),
			};
			AbsoluteLayout.SetLayoutFlags (b2Button, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds (b2Button, new Rectangle (.47, .95, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
			b2Button.Clicked += B2ButtonClicked;

			b3Button = new Button
			{
				Text = "Off",
				Font = Font.SystemFontOfSize(NamedSize.Medium),
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HeightRequest = 30,
				TextColor = Color.FromRgb(230,230,230),
				BackgroundColor = Color.FromRgb(255,255,255),
				BorderColor = Color.FromRgb(230,230,230),
			};
			AbsoluteLayout.SetLayoutFlags (b3Button, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds (b3Button, new Rectangle (.74, .95, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
			b3Button.Clicked += B3ButtonClicked;

			//==========================================
			//Screen Layout 

			simpleLayout = new AbsoluteLayout {

				Opacity = 50.0,
				BackgroundColor = Color.Transparent,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};
			simpleLayout.Children.Add (procedure.picker);
			simpleLayout.Children.Add (endoscopeType.picker);
			simpleLayout.Children.Add (displayType.picker);

			simpleLayout.Children.Add (b1.circle);
			simpleLayout.Children.Add (b2.circle);
			simpleLayout.Children.Add (b3.circle);

			simpleLayout.Children.Add (up1);
			simpleLayout.Children.Add (up2);
			simpleLayout.Children.Add (up3);
			simpleLayout.Children.Add (down1);
			simpleLayout.Children.Add (down2);
			simpleLayout.Children.Add (down3);

			simpleLayout.Children.Add (setPointLabel);
			simpleLayout.Children.Add (setPoint1Label);
			simpleLayout.Children.Add (setPoint2Label);
			simpleLayout.Children.Add (setPoint3Label);
			simpleLayout.Children.Add (actualPressure1Label);
			simpleLayout.Children.Add (actualPressure2Label);
			simpleLayout.Children.Add (actualPressure3Label);
	
			simpleLayout.Children.Add (allButton);
			simpleLayout.Children.Add (b1Button);
			simpleLayout.Children.Add (b2Button);
			simpleLayout.Children.Add (b3Button);



			Content = simpleLayout;

		}

		void AllButtonClicked(object sender, EventArgs e)
		{
			if (allOn == 0) {
				allOn = 1;
				Device.BeginInvokeOnMainThread(() =>
				{
					allButton.Text = "All On";
					allButton.BorderColor = Color.FromRgb(0,230,0);
					allButton.TextColor = Color.FromRgb(0,230,0);
					b1Button.Text = "On";
					b1Button.BorderColor = Color.FromRgb(0,230,0);
					b1Button.TextColor = Color.FromRgb(0,230,0);
					b2Button.Text = "On";
					b2Button.BorderColor = Color.FromRgb(0,230,0);
					b2Button.TextColor = Color.FromRgb(0,230,0);
					b3Button.Text = "On";
					b3Button.BorderColor = Color.FromRgb(0,230,0);
					b3Button.TextColor = Color.FromRgb(0,230,0);
					
				});
			} else {
				allOn = 0;
				Device.BeginInvokeOnMainThread(() =>
				{
					allButton.Text = "All Off";
					allButton.BorderColor = Color.FromRgb(230,230,230);
					allButton.TextColor = Color.FromRgb(230,230,230);
					b1Button.Text = "Off";
					b1Button.BorderColor = Color.FromRgb(230,230,230);
					b1Button.TextColor = Color.FromRgb(230,230,230);
					b2Button.Text = "Off";
					b2Button.BorderColor = Color.FromRgb(230,230,230);
					b2Button.TextColor = Color.FromRgb(230,230,230);
					b3Button.Text = "Off";
					b3Button.BorderColor = Color.FromRgb(230,230,230);
					b3Button.TextColor = Color.FromRgb(230,230,230);
				});
			}
			Byte[] report = {6,Convert.ToByte(allOn)};
			App.appWrite (report).Wait();
			//update button UI


		}
		void B1ButtonClicked(object sender, EventArgs e)
		{
			if (b1On == 0) {
				b1On = 1;
				Device.BeginInvokeOnMainThread(() =>
				{
					b1Button.Text = "On";
					b1Button.BorderColor = Color.FromRgb(0,230,0);
					b1Button.TextColor = Color.FromRgb(0,230,0);
				});
			} else {
				b1On = 0;
				Device.BeginInvokeOnMainThread(() =>
				{
					b1Button.Text = "Off";
					b1Button.BorderColor = Color.FromRgb(230,230,230);
					b1Button.TextColor = Color.FromRgb(230,230,230);
				});
			}
			Byte[] report = {7,Convert.ToByte(b1On)};
			App.appWrite (report).Wait();
			//update button UI

		}
		void B2ButtonClicked(object sender, EventArgs e)
		{
			if (b2On == 0) {
				b2On = 1;
				Device.BeginInvokeOnMainThread(() =>
				{
					b2Button.Text = "On";
					b2Button.BorderColor = Color.FromRgb(0,230,0);
					b2Button.TextColor = Color.FromRgb(0,230,0);
				});
			} else {
				b2On = 0;
				Device.BeginInvokeOnMainThread(() =>
				{
					b2Button.Text = "Off";
					b2Button.BorderColor = Color.FromRgb(230,230,230);
					b2Button.TextColor = Color.FromRgb(230,230,230);
				});
			}
			Byte[] report = {8,Convert.ToByte(b2On)};
			App.appWrite (report).Wait();
			//update button UI

		}
		void B3ButtonClicked(object sender, EventArgs e)
		{
			if (b3On == 0) {
				b3On = 1;
				Device.BeginInvokeOnMainThread(() =>
				{
					b3Button.Text = "On";
					b3Button.BorderColor = Color.FromRgb(0,230,0);
					b3Button.TextColor = Color.FromRgb(0,230,0);
				});
			} else {
				b3On = 0;
				Device.BeginInvokeOnMainThread(() =>
				{
					b3Button.Text = "Off";
					b3Button.BorderColor = Color.FromRgb(230,230,230);
					b3Button.TextColor = Color.FromRgb(230,230,230);
				});
			}
			Byte[] report = {9,Convert.ToByte(b3On)};
			App.appWrite (report).Wait();
			//update button UI

		}

		public void updateActualPressureUI(){

			//Update GUI for balloon sizes based on sensor value.
			Device.BeginInvokeOnMainThread(() =>
			{

				
				b1 = new Balloon(1,sensorValue1);
				b2 = new Balloon(2,sensorValue2);
				b3 = new Balloon(3,sensorValue3);

				actualPressure1Label.Text = sensorValue1 + " mmhg";
				actualPressure2Label.Text = sensorValue2 + " mmhg";
				actualPressure3Label.Text = sensorValue3 + " mmhg";

				/*
				 * Auto resizing the circle in the UI is very slow (sometimes freezing). Still needs work.
				 * 
				simpleLayout.Children.Remove(b1.circle);
				simpleLayout.Children.Remove(b2.circle);
				simpleLayout.Children.Remove(b3.circle);

				AbsoluteLayout.SetLayoutFlags (b1.circle, AbsoluteLayoutFlags.PositionProportional);
				AbsoluteLayout.SetLayoutBounds (b1.circle, new Rectangle (.1, .1, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

				AbsoluteLayout.SetLayoutFlags (b2.circle, AbsoluteLayoutFlags.PositionProportional);
				AbsoluteLayout.SetLayoutBounds (b2.circle, new Rectangle (.5, .1, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

				AbsoluteLayout.SetLayoutFlags (b3.circle, AbsoluteLayoutFlags.PositionProportional);
				AbsoluteLayout.SetLayoutBounds (b3.circle, new Rectangle (.9, .1, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

				simpleLayout.Children.Add(b1.circle);
				simpleLayout.Children.Add(b2.circle);
				simpleLayout.Children.Add(b3.circle);
				*/


			});
		}

		public void updateSetPointUI(){

			//Update GUI for balloon sizes based on sensor value.
			Device.BeginInvokeOnMainThread(() =>
			{

				setPoint1Label.Text = setPoint1 + " mmhg";
				setPoint2Label.Text = setPoint2 + " mmhg";
				setPoint3Label.Text = setPoint3 + " mmhg";
				

			});
		}
	}
}


