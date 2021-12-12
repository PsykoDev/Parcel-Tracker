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
        public static readonly string Lang = "fr_FR"; // fr_FR, en_GB, de_DE, es_ES, it_IT, nl_NL

        // DATA
        private static WebResponse? response;
        public static DataStruct.Root? Last_data;
        public static int refresh = 20; // minutes

        public static void Main(string[] args)
        {
            while (true)
            {

                response = Web.Web.Http($"https://api.laposte.fr/suivi/v2/idships/{Parcel_Number}?lang={Lang}", Web.Web.getIP());
                Last_data = DataBuild.Data(response);
                Print.Print.Check(Last_data);

                Print.Print.Base(Last_data);
                Print.Print.TimeLine(Last_data);
                Print.Print.Step(Last_data);
                Print.Print.RP(Last_data);

                Print.Print.URL(Last_data);
                Print.Print.Refresh();

                Console.Clear();
            }
        }
    }
}