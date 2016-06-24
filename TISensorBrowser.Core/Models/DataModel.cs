using System;
using System.Collections.ObjectModel;
using Syncfusion.SfChart.XForms;

namespace TISensorBrowser
{
	public class DataModel
	{
	     
		public ObservableCollection<ChartDataPoint> data 
		{
			get;
			set;
		}

		public DataModel  ()         
		{            

			data  =  new ObservableCollection < ChartDataPoint >   ();

			
		}   


		public void add(String x, double y){
			data.Add  (new ChartDataPoint  (x,  y));
		}

		public void add(int x, double y){
			data.Add  (new ChartDataPoint  (x,  y));
		}
	}
}

