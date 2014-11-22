using EmergencyBraceletHost.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyBraceletHost
{
    class Program
    {
        //For test
        //private const string DeviceID = "54ff6a066667515129451467";
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
                //Get all devices
                var devices = await _restClient.Get<List<DevicePatient>>("DevicePatient");
                foreach (var item in devices)
                {
                    var device = await _restClient.Get<Device>("Device/" + item.DeviceID);
                    var patient = await _restClient.Get<Patient>("Patient/" + item.PatientID);

                    var temp = double.Parse(await _deviceClient.Get(device.DeviceID, AccessToken, "temp"));
                    Console.WriteLine("Temp: " + temp);

                    var gps = await _deviceClient.Get(device.DeviceID, AccessToken, "gps");
                    Console.WriteLine("GPS: " + gps);

                    if (temp < patient.LowTemp || temp > patient.HighTemp)
                    {
                        Console.WriteLine("Temp issue.");
                        await GenerateAlert(device.DeviceID, patient.PatientID, gps);
                    }
                    else
                    {
                        //Checking Push Button state
                        var bstate = int.Parse(await _deviceClient.Get(device.DeviceID, AccessToken, "bstate"));
                        Console.WriteLine("BState: " + bstate);

                        //Reset button state
                        await _deviceClient.Post(device.DeviceID, AccessToken, "processed");

                        if (bstate == 1)
                        {
                            Console.WriteLine("Button pushed.");

                            //Button pushed
                            await GenerateAlert(device.DeviceID, patient.PatientID, gps);
                        }
                    }
                }
            }
        }

        private static async Task CheckAccepted()
        {
            while (true)
            {
                var list = await _restClient.Get<List<Alert>>("AcceptedAlert");
                foreach (var alert in list)
                {
                    //Send confirmation to the device
                    await _deviceClient.Post(alert.DeviceID, AccessToken, "accepted");

                    //Update Alert status
                    alert.Status = "Accepted";
                    await _restClient.Post("Alert", alert);
                }
            }
        }

        private static async Task GenerateAlert(string deviceID, string patientID, string gps)
        {
            Console.WriteLine("Generating alert.");

            //Generate alert
            var alert = new Alert()
            {
                PatientID = patientID,
                Location = gps,
                Status = "New"
            };
            await _restClient.Post("Alert", alert);

            //Send alert to the device
            await _deviceClient.Post(deviceID, AccessToken, "alert");
        }
    }
}
