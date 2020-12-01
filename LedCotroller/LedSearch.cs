using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmptyBox.IO.Devices;
using EmptyBox.IO.Devices.Bluetooth;
using EmptyBox.IO.Devices.Enumeration;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace LedCotroller
{
    class LedSearch
    {
        public event EventHandler<Led> SearchLed;
        private IBluetoothAdapter adapter;

        public async void SearchDevice()
        {
            IDeviceEnumerator a = DeviceEnumerator.Enumerator;
            adapter = await a.GetDefault<IBluetoothAdapter>();
            adapter.DeviceFound += Adapter_DeviceFound;
            adapter.DeviceLost += Adapter_DeviceLost;
            await adapter.ActivateSearcher();
        }

        private void Adapter_DeviceLost(IDeviceSearcher<IBluetoothDevice> provider, IBluetoothDevice device)
        {
            if (device.Name.StartsWith("Triones"))
            {
                
            }
        }

        private async void Adapter_DeviceFound(IDeviceSearcher<IBluetoothDevice> provider, IBluetoothDevice device)
        {
            if (device.Name.StartsWith("Triones"))
            {
                try
                {
                    Windows.Devices.Bluetooth.BluetoothLEDevice k = ((BluetoothDevice)device).LEDevice;
                    GattDeviceServicesResult serviceResult = await k.GetGattServicesForUuidAsync(Guid.Parse("0000ffd5-0000-1000-8000-00805f9b34fb"));
                    GattDeviceService service = serviceResult.Services.FirstOrDefault();
                    GattCharacteristicsResult characteristicResults = service != null ? await service.GetCharacteristicsForUuidAsync(Guid.Parse("0000ffd9-0000-1000-8000-00805f9b34fb")) : null;
                    GattCharacteristic characteristic = characteristicResults?.Characteristics.FirstOrDefault();
                    service?.Dispose();
                    if (characteristic != null) SearchLed.Invoke(this, new Led(device, characteristic, k.Name));
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}
