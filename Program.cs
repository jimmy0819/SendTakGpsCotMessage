using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace TakCotSender
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CoTGpsPayloadSender gpsSender = new CoTGpsPayloadSender();
            while (true)
            {
                //// Construct the CoT message with dynamic times
                //string cotMessage = CoTGenerator.GenerateCoTMessage();

                //CoTSender sender = new CoTSender();
                //sender.SendCoTMessage(cotMessage);
                Thread.Sleep(100);
                gpsSender.SendCoTMessage();
            }
        }
    }

    public class CoTGpsPayloadSender
    {
        private const string udpAddress = "127.0.0.1"; // Localhost
        private const int udpPort = 4349; // The port for WinTAK

        public void SendCoTMessage()
        {
            // Define dynamic values
            string timeNow = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            string staleTime = DateTime.UtcNow.AddMinutes(5).ToString("yyyy-MM-ddTHH:mm:ssZ");

            // Example dynamic GPS data
            double latitude = 40.712776;
            double longitude = -74.005974;
            double altitude = 10.0;
            double circularError = 5.0;
            double linearError = 3.0;
            double speed = 50.0; // in km/h or m/s
            double course = 180.0; // heading in degrees

            // Additional dynamic data
            string callsign = "Dynamic GPS Device";
            string batteryLevel = "95"; // percentage
            string device = "WinTAK";
            string os = "windows";
            string version = "4.4.1.0";
            string platform = "WinTAK";

            string cotMessage = $@"<event version=""2.0"" uid=""External-GPS"" type=""a-f-G-E-S"" 
                time=""{timeNow}"" start=""{timeNow}"" 
                stale=""{staleTime}"" how=""m-g"">
                <point lat=""{latitude}"" lon=""{longitude}"" hae=""{altitude}"" ce=""{circularError}"" le=""{linearError}"" /> 
                <detail>
                    <precisionlocation geopointsrc=""GPS"" altitudesrc=""GPS""/>
                    <remarks>External GPS</remarks>
                    <__group name=""Network GPS"" role=""Team Member""/>
                    <contact callsign=""{callsign}""/>
                    <track speed=""{speed}"" course=""{course}""/>
                    <takv device=""{device}"" os=""{os}"" version=""{version}"" platform=""{platform}""/>
                    <status battery=""{batteryLevel}""/>
                </detail>
            </event>";
            // Send the CoT message via UDP
            SendUdpMessage(cotMessage);
        }

        private void SendUdpMessage(string message)
        {
            using (UdpClient udpClient = new UdpClient())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(message);
                udpClient.Send(bytes, bytes.Length, udpAddress, udpPort);
                MessageBox.Show("CoT message sent successfully.");
            }
        }
    }

    internal class MessageBox
    {
        public static void Show(string message) { Console.WriteLine(message); }
    }
}
