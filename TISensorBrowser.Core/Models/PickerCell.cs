using System;

namespace TISensorBrowser
{
	using System;
	using Xamarin.Forms;
	using System.Collections.Generic;


		public class PickerCell : ContentView
		{
			public Picker picker;
			public String[] pickerList;

			public PickerCell(String[] pickerList, int size)
			{
				this.pickerList = pickerList;


				//Create the picker
				this.picker = new Picker
				{

					//HorizontalOptions = LayoutOptions.,
					WidthRequest = size,
					HeightRequest = 40,
					SelectedIndex = 0,
					HorizontalOptions = LayoutOptions.EndAndExpand,
					VerticalOptions = LayoutOptions.CenterAndExpand,
					BackgroundColor = Color.White,

				};



				//Add items to picker
				foreach (String s in this.pickerList) {
					picker.Items.Add (s);
				}
				picker.SelectedIndex = 0;

				
			}
			
		}


}

