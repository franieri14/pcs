using System;
using Syncfusion.SfChart.XForms;
using Xamarin.Forms;
using System.Diagnostics;

namespace TISensorBrowser
{
	public class Balloon
	{
		 
		public SfChart circle;
		public int pressure;

		public Balloon (int id, int pressure) //id is baloon number, pressure is 0-200
		{
			this.pressure = pressure;

			circle = new SfChart ();

			DataModel d = new DataModel ();
			String balloonId = "Balloon " + id;
			d.add (balloonId, 100);


			PieSeries pie = new PieSeries() 
			{ 
				ItemsSource = d.data, 
				XBindingPath = "Expense", 
				YBindingPath = "Value",
				ExplodeAll = true,
				CircularCoefficient = scale(),	
				Color = Color.Blue

			};

			circle.Series.Add (pie);

			//circle.HorizontalOptions = LayoutOptions.FillAndExpand;
			//circle.VerticalOptions = LayoutOptions.FillAndExpand;

			circle.HeightRequest = 200;
			circle.WidthRequest = 200;

		}

		public double scale(){

			//Mapping function that maps the circuular coefficient based on the desired size on scale from 0-200.
			//We will map to .1-.7


			double scale = (double)(.7 - .1) / (200 - 0);
			double map = (.1 + ((pressure - 0) * scale));
			Debug.WriteLine (map);
			return map;

		}


	}
}

