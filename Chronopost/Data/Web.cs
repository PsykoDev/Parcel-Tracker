﻿using System;
using System.Net;

#pragma warning disable CS8600, SYSLIB0014, CS8602

namespace Chronopost.Web
{
    public class Web
    {
        internal static WebResponse? Http(string uri, string iP)
        {
            try
            {
                WebRequest request = WebRequest.Create(uri);
                WebHeaderCollection header = request.Headers;
                request.Method = "GET";
                header.Add("X-Okapi-Key", Chronopost.Okapi_key);
                header.Add("X-Forwarded-For", iP);
                header.Add("accept: application/json");
                request.ContentType = "application/json";
                request.Timeout = 1200;

                WebResponse response = request.GetResponse();
                return response;
            }
            catch (WebException e)
            {
                Console.WriteLine("\nWebException is thrown. \nMessage :" + e.Message);
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    Console.WriteLine("Status Code : {0}", ((HttpWebResponse)e.Response).StatusCode);
                    Console.WriteLine("Status Description : {0}", ((HttpWebResponse)e.Response).StatusDescription);
                    Console.WriteLine("Server : {0}", ((HttpWebResponse)e.Response).Server);
                    return null;
                }
                return null;
            }
        }

        internal static string getIP()
        {
            string IP = new WebClient().DownloadString("https://api.ipify.org");
            return IP;
        }
    }
}

