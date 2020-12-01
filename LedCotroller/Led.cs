using EmptyBox.IO.Devices.Bluetooth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.UI;
using BluetoothDevice = EmptyBox.IO.Devices.Bluetooth.BluetoothDevice;

namespace LedCotroller
{
    public class RSSIComparer : IComparer<Led>

    {

        public int Compare(Led x, Led y)
        {
            if (x == null || y == null)
            {
                throw new InvalidOperationException("Compared objects are not ObservableBluetoothLEDevice");
            }
            // If they're equal
            if (x.Signal == y.Signal)
            {
                return 0;
            }



            // RSSI == 0 means we don't know it. Always make that the end.

            if (y.Signal == 0)

            {

                return -1;

            }



            if (x.Signal < y.Signal || x.Signal == 0)

            {

                return 1;

            }

            else

            {

                return -1;

            }

        }

    }

    public class Led : INotifyPropertyChanged
    {

        public string Name { get; private set; }
        private IBluetoothDevice device;
        private GattCharacteristic characteristic;
        private GattDeviceService service;

        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsVideoMode { get; set; }
        public int Signal => (int)((BluetoothDevice)device).LEDevice.DeviceInformation.Properties["System.Devices.Aep.SignalStrength"];

        public async void Start()
        {
            BluetoothLEDevice k = ((BluetoothDevice)device).LEDevice;
            GattDeviceServicesResult serviceResult = await k.GetGattServicesForUuidAsync(Guid.Parse("0000ffd5-0000-1000-8000-00805f9b34fb"));
            service = serviceResult.Services.FirstOrDefault();
            GattCharacteristicsResult characteristicResults = service != null ? await service.GetCharacteristicsForUuidAsync(Guid.Parse("0000ffd9-0000-1000-8000-00805f9b34fb")) : null;
            characteristic = characteristicResults?.Characteristics.FirstOrDefault();
        }

        public void Stop()
        {
            service.Dispose();
        }

        public Color? Color 
        {
            get 
            {
                Task<GattReadResult> task = characteristic.ReadValueAsync(BluetoothCacheMode.Uncached).AsTask();
                try
                {
                    task.Wait();
                    var result = task.Result;
                    byte[] array = result.Value.ToArray();
                    if (array[0] != 86)
                    {
                        return null;
                    }
                    else
                    {
                        return new Color() { A = 0xFF, R = array[1], G = array[2], B = array[3] };
                    }
                }
                catch
                {
                    return new Color() { A = 0xFF, R = 0, G = 0, B = 0 };

                }

            }
            set 
            {
                byte mode = !IsVideoMode && value.Value.B == value.Value.G && value.Value.G == value.Value.R ? (byte)0x0F : (byte)0xF0;
                byte white = value.Value.B;
                byte[] result = { 86, value.Value.R, value.Value.G, value.Value.B, white, mode, 0xAA };
                _ = characteristic.WriteValueAsync(result.AsBuffer());
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Color)));
            }
        }

        public Led(IBluetoothDevice device, GattCharacteristic characteristic, string name)
        {
            this.characteristic = characteristic;
            this.Name = name;
            this.device = device;
        }

    }
}
