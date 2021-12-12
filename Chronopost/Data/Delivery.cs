using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Chronopost.Delivery
{
    public class Delivery
    {
        internal static void Delivery_date(string url)
        {
            try
            {
                
                WebRequest request = WebRequest.Create("");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                Console.WriteLine(responseFromServer);
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch
            {

            }
        }
    }
}

