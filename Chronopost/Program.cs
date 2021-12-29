using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using Chronopost.Build;
using Chronopost.Data;
using Chronopost.Print;
using Chronopost.Web;
#pragma warning disable
namespace Chronopost
{
    public class Chronopost
    {
        // CONFIG
        public static string Parcel_Number = "";
        public static readonly string Okapi_key ="";
        public static readonly string Lang = "fr_FR"; // fr_FR, en_GB, de_DE, es_ES, it_IT, nl_NL
        public static int refresh = 20; // minutes

        // DATA
        private static WebResponse? response;
        public static DataStruct.Root? Last_data;

        public static void Main(string[] args)
        {
            if (Parcel_Number == String.Empty)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    Parcel_Number = args[i];
                }
            }

            while (true)
            {
                response =Web.Web.Http($"https://api.laposte.fr/suivi/v2/idships/{Parcel_Number}?lang={Lang}");
                Last_data = DataBuild.Data(response);
                Print.Print.Base (Last_data);
                Print.Print.TimeLine (Last_data);
                Print.Print.Step (Last_data);
                Print.Print.RP (Last_data);
                Print.Print.URL (Last_data);
            }
        }
    }
}
