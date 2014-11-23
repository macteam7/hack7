using EmergencyBraceletHost.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmergencyBraceletHost
{
    class Program
    {
        private const string AccessToken = "d14ef4469b3321de294543f1a654675c959dd2f6";

        private static readonly RestClient _restClient = new RestClient();
        private static readonly DeviceClient _deviceClient = new DeviceClient();

        static void Main(string[] args)
        {
            Task.Run(async () => await CheckDevices());
            Task.Run(async () => await CheckAccepted());
            Console.ReadLine();
        }

        private static async Task CheckDevices()
        {
            while (true)
            {
                try
                {
                    var events = new List<ManualResetEvent>();

                    //Get all devices
                    var devices = await _restClient.Get<List<DevicePatient>>("DevicePatient");
                    if (devices.Count > 0)
                    {
                        foreach (var device in devices)
                        {
                            var ev = new ManualResetEvent(false);
                            events.Add(ev);

                            Task.Run(async () => await ProcessDevice(device.DeviceId, device.PatientId, ev));
                        }

                        if (events.Count > 0)
                        {
                            WaitHandle.WaitAll(events.ToArray());
                        }
                    }
                    else
                    {
                        Console.WriteLine("No devices found.");
                    }
                }
                catch (Exception ex2)
                {
                    Console.WriteLine("Error: " + ex2.Message);
                }
            }
        }

        private static void Test()
        {
            System.Threading.Thread.Sleep(3000);
        }

        private async static Task ProcessDevice(string deviceId, string patiendId, ManualResetEvent ev)
        {
            try
            {
                Console.WriteLine("Processing Device: " + deviceId);

                var patient = await _restClient.Get<Patient>("Patients/id/" + patiendId);

                var temp = double.Parse(await _deviceClient.Get(deviceId, AccessToken, "temp"));
                Console.WriteLine("Temp: " + temp);

                var gps = await _deviceClient.Get(deviceId, AccessToken, "gps");
                Console.WriteLine("GPS: " + gps);

                //Update Device location
                var location = ParseGpsLocation(gps);
                if (location.Item1 > 0 && location.Item2 > 0)
                {
                    var updatedDevice = new Device();
                    updatedDevice.DeviceId = deviceId;
                    updatedDevice.Latitude = location.Item1;
                    updatedDevice.Longitude = location.Item2;
                    await _restClient.Put("Devices", updatedDevice);
                }

                if (temp < patient.LowTemp || temp > patient.HighTemp)
                {
                    Console.WriteLine("Temp issue.");
                    await GenerateAlert(deviceId, patient.Id, location);
                }
                else
                {
                    //Checking Push Button state
                    var bstate = int.Parse(await _deviceClient.Get(deviceId, AccessToken, "bstate"));
                    Console.WriteLine("BState: " + bstate);

                    //Reset button state
                    await _deviceClient.Post(deviceId, AccessToken, "processed");

                    if (bstate == 1)
                    {
                        Console.WriteLine("Button pushed.");

                        //Button pushed
                        await GenerateAlert(deviceId, patient.Id, location);
                    }
                }
            }
            catch (Exception ex1)
            {
                Console.WriteLine("Error: " + ex1.Message);
            }
            finally
            {
                ev.Set();
            }
        }

        private static async Task CheckAccepted()
        {
            while (true)
            {
                var list = await _restClient.Get<List<Alert>>("Alerts/AcceptedAlert");
                foreach (var alert in list)
                {
                    Console.WriteLine("Sending accept notification.");

                    //Send confirmation to the device
                    await _deviceClient.Post(alert.DeviceId, AccessToken, "accepted");

                    //Update Alert status
                    alert.Status = 3;
                    await _restClient.Post("Alerts/update/" + alert.RowKey, alert);
                }
            }
        }

        private static async Task GenerateAlert(string deviceId, string patientId, Tuple<double, double> location)
        {
            Console.WriteLine("Generating alert.");

            //Generate alert
            var alert = new Alert()
            {
                HospitalId = "00000000-0000-0000-0000-000000000005", //TODO
                DeviceId = deviceId,
                PatientId = patientId,
                Latitude = location.Item1,
                Longitude = location.Item2,
                Status = 1
            };
            await _restClient.Put("Alerts/add/" + deviceId, alert);

            //Send alert to the device
            await _deviceClient.Post(deviceId, AccessToken, "alert");
        }

        private static Tuple<double, double> ParseGpsLocation(string gps)
        {
            var list = gps.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (list.Count == 2)
            {
                return new Tuple<double, double>(double.Parse(list[0]), double.Parse(list[1]));
            }
            return new Tuple<double, double>(0, 0);
        }
    }
}
