using Sig.DeviceAPI;

namespace StepOverModel
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>

        public static IDriver driverInterface = new Driver();

        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            bool connectedDevice = false;

            string[]? deviceNames;

            int guiOption = 0;
            FilterDeviceKind DeviceFilter = FilterDeviceKind.dkStepOver;
            driverInterface.DeviceSearch(out deviceNames, guiOption, DeviceFilter);
            if (deviceNames.Length == 0)
            {
                MessageBox.Show("No device found!" +
                                "\nSignature options is disable.");

                Application.Run(new Form1(""));
            }
            else
            {
                Error r = driverInterface.CheckConnectedDevice(deviceNames[0], out bool connection);
                if (r != Error.SUCCESS)
                {
                    MessageBox.Show("Error: \n" + r.ToString());
                    return;
                }

                string deviceStatus = connection ? "Connected" : "Disconnected";


                MessageBox.Show("Device found " +
                                "\nDevice Info: " + deviceNames[0] +
                                "\nConncetion: " + deviceStatus);

                Application.Run(new Form1(deviceNames[0]));
            }
        }
    }
}