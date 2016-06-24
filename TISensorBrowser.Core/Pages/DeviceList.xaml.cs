using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Robotics.Mobile.Core.Bluetooth.LE;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace TISensorBrowser
{	
	public partial class DeviceList : ContentPage
	{	
		IAdapter adapter;
		ObservableCollection<IDevice> devices;

		ToolbarItem scanAll = new ToolbarItem();
		ToolbarItem disconnect = new ToolbarItem();

		Guid hbxService = new Guid ("713D0000-503E-4C75-BA94-3148F18D941E");


		public DeviceList (IAdapter adapter)
		{
			InitializeComponent ();

			this.adapter = adapter;
			this.devices = new ObservableCollection<IDevice> ();
			listView.ItemsSource = devices;

			scanAll = ScanAllButton;
			disconnect = DisconnectButton;

			devices.Clear();
			listView.ItemsSource = null;


			adapter.DeviceDiscovered += (object sender, DeviceDiscoveredEventArgs e) => {
				Device.BeginInvokeOnMainThread(() => {

					if(e.Device.Name.StartsWith("H")){
						devices.Add (e.Device);
					}


					listView.ItemsSource = devices;

				});
			};

			adapter.ScanTimeoutElapsed += (sender, e) => {
				adapter.StopScanningForDevices();
			};


			adapter.DeviceDisconnected += async (object sender, DeviceConnectionEventArgs e) => {
				Debug.WriteLine ("Device disconnected !!!");
				//App.isInWorkout = false;

				//if(!App.autoDisconnecting){
				//	speechTask("Device disconnected.");
				//}

				Device.BeginInvokeOnMainThread(() => {

					//setWorkoutPage ();

					if(App.getDevice() != null){
						try{
							Debug.WriteLine("in de device : " + App.getDevice().Name);
						}
						catch(Exception exc){

						}
						try{

							App.removeCharacteristics();

						}
						catch(Exception exc ){
							//pretty much a bandaid ?
						}

					}
					//on disconnect - probably go back to this device page
					//App.appTabs.profilePage.deviceLabel.Text = "No Device Connected";


				});



			};

			ScanAllButton.Activated += (sender, e) => {
				InfoFrame.IsVisible = false;
				listView.ItemsSource = null;
				StartScanning();
			};

			DisconnectButton.Activated += (sender, e) => {

				disconnection();


			};
		}

		public async void OnItemSelected (object sender, SelectedItemChangedEventArgs e) {

			if (App.bleFree) {
				if (((ListView)sender).SelectedItem == null) {
					return;
				}
				Debug.WriteLine (" xxxxxxxxxxxx  OnItemSelected " + e.SelectedItem.ToString ());
				//StopScanning ();

				var device = e.SelectedItem as IDevice;

				//Set the device.
				App.SetDevice (device);

				App.getAdapter ().ConnectToDevice (App.getDevice ());

				bool found = await App.makeDeviceConnectable (App.getDevice());
				if (!found) {
					//await speechTask("Pairing failed.");
					((ListView)sender).SelectedItem = null; // clear selection
					return;
				}

				//Set workout page content based on whether or not device is connected.
				//setWorkoutPage ();

				App.bleFree = false;

				App.connectCharacteristics ().Wait ();

				//speechTask("Device paired."); //in the future maybe say 'device name' paired.
				//App.userDisconnect = false;

				Debug.WriteLine ("Device paired!");
				Navigation.PushAsync (App.pcs);

			} 
			//else {
			//	Navigation.PopToRootAsync ();
			//}
			((ListView)sender).SelectedItem = null; // clear selection

		}

		void StartScanning () {
			
			StartScanning (hbxService);
		}
		void StartScanning (Guid forService) {

			if (adapter.IsScanning) {
				//adapter.StopScanningForDevices();
				//Debug.WriteLine ("adapter.StopScanningForDevices()");
			} else {
				devices.Clear();
				adapter.StartScanningForDevices(forService);
				Debug.WriteLine ("adapter.StartScanningForDevices("+forService+")");
			}
		}

		void StopScanning () {
			// stop scanning
			new Task( () => {
				if(adapter.IsScanning) {
					Debug.WriteLine ("Still scanning, stopping the scan");
					adapter.StopScanningForDevices();
				}
			}).Start();
		}

		public void disconnection(){

			App.lastDevice = App.getDevice ();

			//App.userDisconnect = true;

			if(App.getDevice() != null){
				//App.disconnectDevice().Wait();
				adapter.DisconnectDevice(App.getDevice());
			}

			listView.ItemsSource = null;


		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			//App.isPairing = true;

		}
	}
}
