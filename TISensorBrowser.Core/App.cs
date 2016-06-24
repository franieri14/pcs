using System;
using Xamarin.Forms;
using Robotics.Mobile.Core.Bluetooth.LE;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TISensorBrowser
{
	public class App
	{
		public static Guid read = new Guid("713D0002-503E-4C75-BA94-3148F18D941E");
		public static Guid write = new Guid("713D0003-503E-4C75-BA94-3148F18D941E");
		public static Guid hbxService = new Guid("713D0000-503E-4C75-BA94-3148F18D941E");

		public static IService service = null;
		public static ICharacteristic readCharacteristic = null;
		public static ICharacteristic writeCharacteristic = null;

		public static EventHandler<DeviceConnectionEventArgs> deviceConnected ;
		public static EventHandler servicesDiscovered ;
		public static EventHandler characteristicsDiscovered ;
		public static EventHandler<CharacteristicReadEventArgs> readUpdate ;
		public static EventHandler<CharacteristicReadEventArgs> writeUpdate ;

		public static Boolean bleFree {get;set;}
		public static IDevice lastDevice;

		static IAdapter adapter;
		static IDevice	device;

		public static byte[] requestsFlag;

		public static String readReport="";

		public static PressureController pcs = new PressureController();


		public static Page GetMainPage ()
		{	
			bleFree = true;
			var np = new NavigationPage (new DeviceList(adapter));
			if (Device.OS != TargetPlatform.iOS) {
				// we manage iOS themeing via the native app Appearance API
				np.BarBackgroundColor = Color.Red;
			}
			return np;
		}

		public static void SetAdapter (IAdapter _adapter) {
			adapter = _adapter;
		}

		public static IAdapter getAdapter () {
			return adapter;
		}

		public static void SetDevice (IDevice _device) {
			device = _device;
		}

		public static IDevice getDevice () {
			return device;
		}

		public static void removeCharacteristics(){


			readCharacteristic.StopUpdates ();
			writeCharacteristic.StopUpdates ();


			//Unsubscribe from events
			readCharacteristic.ValueUpdated -= readUpdate;
			writeCharacteristic.ValueUpdated -= writeUpdate;

		
			readCharacteristic = null;
			writeCharacteristic = null;



			try{
				//adapter.DisconnectDevice (device);
			}
			catch(Exception exc){}



			try{
				Debug.WriteLine("in rc last device : " + lastDevice.Name);
				Debug.WriteLine("in rc last device : " + device.Name);
			}
			catch(Exception exc){

			}
			lastDevice = device;
			device = null;

			bleFree = true;


		}

		public static async Task connectCharacteristics(){

			updateCharacteristics ().Wait ();

			//confirmCom().Wait();

			byte[] testWrite = { 100 };
			appWrite (testWrite).Wait();
		}

		public static async Task updateCharacteristics(){
			updateReadCharacteristic().Wait();
			updateWriteCharacteristic().Wait();
		}


		public static async Task appWrite(byte[] write){
			//These two lines needed to send a new write
			writeCharacteristic.Write(write);

		}

		public static async Task updateWriteCharacteristic(){

			//connect charactersitics from device here
			setWriteCharacteristic();

			if (writeCharacteristic.CanUpdate) {
				writeCharacteristic.StartUpdates();
				Debug.WriteLine("Write Updates started!");

				writeUpdate = (s, ex) => {
					//Debug.WriteLine("writeCharacteristic.ValueUpdated");
					Device.BeginInvokeOnMainThread( () => {


						Debug.WriteLine(writeCharacteristic.StringValue);  //debug 5
					}); 
				};

				writeCharacteristic.ValueUpdated += writeUpdate;



			}


		}

		public static void setWriteCharacteristic(){


			foreach (IService serv in device.Services) {
				if (serv.ID.Equals (hbxService)) {
					foreach (ICharacteristic c in serv.Characteristics) {
						if(c.ID.Equals(write)){
							writeCharacteristic = c;
							return;
						}
					}
				}
			}

		}
		public static void setReadCharacteristic(){

			foreach (IService serv in device.Services) {
				if (serv.ID.Equals (hbxService)) {
					foreach (ICharacteristic c in serv.Characteristics) {
						if(c.ID.Equals(read)){
							readCharacteristic = c;
							return;
						}
					}
				}
			}

		}

		public static async Task updateReadCharacteristic(){

			setReadCharacteristic ();

			if (readCharacteristic.CanUpdate) {
				readCharacteristic.StartUpdates ();

				readUpdate = (s, ex) => {
					//Debug.WriteLine ("readCharacteristic.ValueUpdated");
					Device.BeginInvokeOnMainThread (() => {

						//What to read and update

						try{
							

							Debug.WriteLine(readCharacteristic.StringValue);

							if(readCharacteristic.StringValue.Equals("A")){
								readReport += "0"; 
							}
							else if(readCharacteristic.StringValue.Equals("B")){
								readReport += "1"; 
							}
							else if(readCharacteristic.StringValue.Equals("C")){
								readReport += "2"; 
							}
							else if(readCharacteristic.StringValue.Equals("D")){
								readReport += "3"; 
							}
							else if(readCharacteristic.StringValue.Equals("E")){
								readReport += "4"; 
							}
							else if(readCharacteristic.StringValue.Equals("F")){
								readReport += "5"; 
							}
							else if(readCharacteristic.StringValue.Equals("G")){
								readReport += "6"; 
							}
							else if(readCharacteristic.StringValue.Equals("H")){
								readReport += "7"; 
							}
							else if(readCharacteristic.StringValue.Equals("I")){
								readReport += "8"; 
							}
							else if(readCharacteristic.StringValue.Equals("J")){
								readReport += "9"; 
							}
							else if(readCharacteristic.StringValue.Equals("K")){
								readReport += ","; 
							}
							else if(readCharacteristic.StringValue.Equals("Q")){
								processReport();
							}

						}
						catch(Exception e){
							Debug.WriteLine("error in read - big catch");
						}

					});
				};

				readCharacteristic.ValueUpdated += readUpdate;




			}
		}

		public static async Task<Boolean> makeDeviceConnectable(IDevice found){

			//adapter.ConnectToDevice (found);

			//Take five seconds MAX to find services
			for (int i = 0; i < 3; ++i) {
				found.DiscoverServices ();
				Debug.WriteLine ("Discovering Services...");
				await Task.Delay (2000);

				List<IService> foundServices = new List<IService> (found.Services);

				if (foundServices.Exists (ser => ser.ID.Equals (hbxService))) {
					var serv = foundServices.Find (ser => ser.ID.Equals (hbxService)) as IService;
					Debug.WriteLine ("Found Habix Service!");

					//Take five seconds MAX to find characterstics.
					for (int j = 0; j < 3; ++j) {
						serv.DiscoverCharacteristics ();
						Debug.WriteLine ("Discovering Characteristics...");
						await Task.Delay (2000);

						List<ICharacteristic> foundCharacteristics = new List<ICharacteristic> (serv.Characteristics);

						//Read and write have been found - device is now connectable
						if (foundCharacteristics.Exists (ch => ch.ID.Equals (write)) && foundCharacteristics.Exists (ch => ch.ID.Equals (read))) {
							Debug.WriteLine ("Ready for connection!");
							return true;
						} else {
							//Can't find charcteristics.
							if (j == 2) {

								//pairDevicePage.devices.Remove(found);
								return false;
							}
						}
					}

				} else {

					//Can't find services.
					if (i == 2) {


						//pairDevicePage.devices.Remove(found);
						return false;
					}
				}
			}

			return false;
		}

		public static void processReport(){

			String[] numbers = new String[3]; //0 = sensorValue1, 1 = sensorValue2, 2 = sensorValue3
			numbers = readReport.Split(',');

			pcs.sensorValue1 = Convert.ToInt32(numbers[0]);
			pcs.sensorValue2 = Convert.ToInt32(numbers[1]);
			pcs.sensorValue3 = Convert.ToInt32(numbers[2]);

			pcs.updateActualPressureUI ();

			Debug.WriteLine ("==========");
			Debug.WriteLine ("Report: ");
			Debug.WriteLine (pcs.sensorValue1);
			Debug.WriteLine (pcs.sensorValue2);
			Debug.WriteLine (pcs.sensorValue3);
			Debug.WriteLine ("==========");

			//Clear the read report.
			readReport += ","; 


		}




	}
}

