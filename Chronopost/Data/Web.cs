using System;
using System.Net;

#pragma warning disable CS8600, SYSLIB0014, CS8602

namespace Chronopost.Web
{
    public class Web
    {
        internal static WebResponse? Http(string uri)
        {
            try
            {
                WebRequest request = WebRequest.Create(uri);
                WebHeaderCollection header = request.Headers;
                request.Method = "GET";
                header.Add("X-Okapi-Key", Chronopost.Okapi_key);
                header.Add("X-Forwarded-For", getIP());
                header.Add("accept: application/json");
                request.ContentType = "application/json";
                request.Timeout = 5000;

                WebResponse response = request.GetResponse();
                return response;
            }
            catch (WebException e)
            {
                HttpWebResponse response = null;
                HttpStatusCode statusCode;
                response = (HttpWebResponse)e.Response;
                statusCode = response.StatusCode;
                switch ((int)statusCode)
                {
                    case 200:
                        break;
                    case 207:
                        Console.WriteLine("Réponse à statut multiple");
                        break;
                    case 400:
                        Console.WriteLine("Numéro invalide (ne respecte pas la syntaxe définie)");
                        break;
                    case 401:
                        Console.WriteLine("Non-autorisé (absence de la clé Okapi)");
                        break;
                    case 404:
                        Console.WriteLine("Ressource non trouvée");
                        break;
                    case 500:
                        Console.WriteLine("Erreur système (message non généré par l’application)");
                        break;
                    case 504:
                        Console.WriteLine("Service indisponible (erreur technique sur service tiers)");
                        break;
                }
                return null;
            }
        }

        internal static string getIP()
        {
            return new WebClient().DownloadString("https://api.ipify.org");
        }
    }
}

