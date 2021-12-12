using System;
using System.Net;
using Chronopost.Data;
using Newtonsoft.Json;

#pragma warning disable CS8600, CS8603

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

                DataStruct.Root myDeserializedClass = JsonConvert.DeserializeObject<DataStruct.Root>(responseFromServer);
                return myDeserializedClass;

            }
        }
    }
}

