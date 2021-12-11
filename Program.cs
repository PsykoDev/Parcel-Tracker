using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Chronopost.Web;
using Chronopost.Data;
using Chronopost.Build;

#pragma warning disable CS8600, CS8603, SYSLIB0014, CS8602, CS8604

namespace Chronopost
{
    public class Chronopost
    {
        // CONFIG
        public static readonly string Parcel_Number = "";
        public static readonly string Okapi_key = "";
        public static readonly string Lang = "fr_FR";

        // DATA
        private static WebResponse? response;
        private static DataStruct.Root? Last_data;
        public static bool Debug = false;
        private static string arrived = "";
        private static int refresh = 20; // minutes

        public static void Main(string[] args)
        {
            while (true)
            {

                string IP = new WebClient().DownloadString("https://api.ipify.org");
                response = Web.Web.Http($"https://api.laposte.fr/suivi/v2/idships/{Parcel_Number}?lang={Lang}", IP);
                Last_data = DataBuild.Data(response);
                arrived = Last_data.shipment.isFinal == true ? "YES" : "NO";

                Console.WriteLine(
                    $"Langue: {Last_data.lang},\n" +
                    $"Product ID: {Last_data.shipment.idShip},\n" +
                    $"Delivery man: {Last_data.shipment.product}\n" +
                    $"Entry Date: {Last_data.shipment.entryDate.ToString("d MMMM yyyy H:mm:ss")},\n" +
                    $"Delivered ? {arrived}\n");

                for (int i = 0; i < Last_data.shipment.@event.Count; i++)
                {
                    Console.WriteLine($"Date: {Last_data.shipment.@event[i].date.ToString("d MMMM yyyy H:mm:ss")}\n\t" +
                        $"Progress: {Last_data.shipment.@event[i].label}");
                }
                Console.WriteLine($"\nURL: {Last_data.shipment.urlDetail}");
                Console.Write($"Last Update: {DateTime.Now.ToString("d MMMM yyyy H:mm:ss")}");
                Thread.Sleep(refresh * 60000);
                Console.Clear();
            }
        }
    }
}