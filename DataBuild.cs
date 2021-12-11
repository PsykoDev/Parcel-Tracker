using System;
using System.Net;
using Chronopost.Data;
using Newtonsoft.Json;

namespace Chronopost.Build
{
    public class DataBuild
    {
        internal static DataStruct.Root Data(WebResponse response)
        {
            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                if (Chronopost.Debug)
                    Console.WriteLine(responseFromServer);

                DataStruct.Root myDeserializedClass = JsonConvert.DeserializeObject<DataStruct.Root>(responseFromServer);
                return myDeserializedClass;

            }
        }
    }
}

