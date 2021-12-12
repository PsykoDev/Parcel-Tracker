using Chronopost.Web;
using Chronopost.Data;
using Chronopost.Build;
using Chronopost.Print;
using System.Net;

#pragma warning disable SYSLIB0014, CS8604

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
        public static DataStruct.Root? Last_data;
        private static int refresh = 20; // minutes

        public static void Main(string[] args)
        {
            while (true)
            {

                response = Web.Web.Http($"https://api.laposte.fr/suivi/v2/idships/{Parcel_Number}?lang={Lang}", Web.Web.getIP());
                Last_data = DataBuild.Data(response);

                Print.Print.Base(Last_data);
                Print.Print.TimeLine(Last_data);
                Print.Print.Step(Last_data);
                Print.Print.URL(Last_data);
                Print.Print.Refresh();
                
                Thread.Sleep(refresh * 60000);
                Console.Clear();
            }
        }
    }
}