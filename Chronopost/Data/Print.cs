using System;
using Chronopost.Data;
using Chronopost.Data.TableauBuild;

#pragma warning disable CS8602

namespace Chronopost.Print
{
    public class Print
    {
        private static string arrived = "Unknown";
        private static string timeline = "Unknown";
        private static string change = "Unknown";
        private static string deliv = "Unknown";
        private static string entry = "Unknown";
        private static string RPs = "Unknown";

        private static Dictionary<string, string> event_code = new Dictionary<string, string>(){
            {"DR1", "Déclaratif réceptionné"},
            {"PC1", "Pris en charge"},
            {"PC2", "Pris en charge dans le pays d’expédition"},
            {"ET1", "En cours de traitement"},
            {"ET2", "En cours de traitement dans le pays d’expédition"},
            {"ET3", "En cours de traitement dans le pays de destination	"},
            {"ET4", "En cours de traitement dans un pays de transit"},
            {"EP1", "En attente de présentation"},
            {"DO1", "Entrée en Douane"},
            {"DO2", "Sortie  de Douane"},
            {"DO3", "Retenu en Douane"},
            {"PB1", "Problème en cours"},
            {"PB2", "Problème résolu"},
            {"MD2", "Mis en distribution"},
            {"ND1", "Non distribuable"},
            {"AG1", "En attente d'être retiré au guichet"},
            {"RE1", "Retourné à l'expéditeur"},
            {"DI1", "Distribué"},
            {"DI2", "Distribué à l'expéditeur"}
        };

        private static Dictionary<string, string> event_codeEN = new Dictionary<string, string>(){
            {"DR1", "Declaration received"},
            {"PC1", "Supported"},
            {"PC2", "Picked up in the country of shipment"},
            {"ET1", "In process"},
            {"ET2", "Being processed in the country of dispatch"},
            {"ET3", "Being processed in the country of destination"},
            {"ET4", "Being processed in a transit country"},
            {"EP1", "Awaiting presentation"},
            {"DO1", "Customs Entry"},
            {"DO2", "Customs Exit"},
            {"DO3", "Detained at Customs"},
            {"PB1", "Problem in progress"},
            {"PB2", "Problem solved"},
            {"MD2", "Distributed"},
            {"ND1", "Not distributable"},
            {"AG1", "Waiting to be collected at the counter"},
            {"RE1", "Returned to sender"},
            {"DI1", "Distributed"},
            {"DI2", "Distributed to sender"}
        };

        private static Dictionary<int, string> Hoder = new Dictionary<int, string>(){
            {1, "courrier nat"},
            {2, "courrier inter"},
            {3, "chronopost"},
            {4, "colissimo"}

        };

        private static Dictionary<int, string> deliveryChoice = new Dictionary<int, string>(){
            {0, "No"},
            {1, "Possible"},
            {2, "Chosen"}
        };

        internal static async void Base(DataStruct.Root Last_data)
        {
            arrived = Last_data.shipment.isFinal == true ? "YES" : "NO";
            if(Check(Last_data))
                change = Last_data.shipment.contextData.deliveryChoice != null ? deliveryChoice[Last_data.shipment.contextData.deliveryChoice.deliveryChoice] : "Unknown";
            deliv = Last_data.shipment.deliveryDate.ToString("d MMMM yyyy H:mm") != DateTime.MinValue.ToString("d MMMM yyyy H:mm") ? Last_data.shipment.deliveryDate.ToString("d MMMM yyyy H:mm") : await Delivery.Delivery.Delivery_dateAsync(Last_data.shipment.urlDetail);
            entry = Last_data.shipment.entryDate.ToString("d MMMM yyyy H:mm") != DateTime.MinValue.ToString("d MMMM yyyy H:mm") ? Last_data.shipment.entryDate.ToString("d MMMM yyyy H:mm") : "Unknown";
            Console.Clear();
            Console.WriteLine($"Press ENTER to manually refresh or wait {Chronopost.refresh} minutes!\n");

            Console.WriteLine(
                $"Langue: {Last_data.lang},\n" +
                $"Product ID: {Last_data.shipment.idShip},\n" +
                $"Type: {Hoder[Last_data.shipment.holder]},\n" +
                $"Delivery man: {Last_data.shipment.product},\n" +
              /*$"Entry Date: {entry},\n" +
                $"Delivery Date: {deliv},\n" +*/
                $"Can change delivery type: {change},\n" +
                $"Delivered ? {arrived}\n");

            TableauBuild.BuildFind1("Entry Date", "Delivery Date", entry, deliv);


            if (!deliv.Contains("Unknown"))
            {
                Console.Title = $"Delivery Date: {deliv}";
            }
            else
                Console.Title = "Parcel Tracker";
        }

        internal static void TimeLine(DataStruct.Root Last_data)
        {
            foreach (DataStruct.Timeline v in Last_data.shipment.timeline)
            {
                if (v.status == true)
                {
                    /*timeline = $"{v.shortLabel}\n\t" +
                        $"{v.longLabel}";*/
                    timeline = $"{v.shortLabel}";
                }
            }

            //Console.WriteLine($"TimeLine: \n\t{timeline}");
        }

        internal static void Step(DataStruct.Root Last_data)
        {
            string result = "";
            for (int i = 0; i < Last_data.shipment.@event.Count; i++)
            {
                switch (Chronopost.Lang)
                {
                    case "fr_FR":
                        result = event_code[Last_data.shipment.@event[i].code];
                        break;
                    case "en_GB":
                        result = event_codeEN[Last_data.shipment.@event[i].code];
                        break;
                    case "de_DE":
                        result = event_codeEN[Last_data.shipment.@event[i].code];
                        break;
                    case "es_ES":
                        result = event_codeEN[Last_data.shipment.@event[i].code];
                        break;
                    case "it_IT":
                        result = event_codeEN[Last_data.shipment.@event[i].code];
                        break;
                    case "nl_NL":
                        result = event_codeEN[Last_data.shipment.@event[i].code];
                        break;
                }
                /*Console.WriteLine($"delivery steps: \n\t" +
                    $"{Last_data.shipment.@event[i].date.ToString("d MMMM yyyy H:mm")}\n\t" +
                    $"Progress: {Last_data.shipment.@event[i].label}\n\t" +
                    $"Code: {Last_data.shipment.@event[i].code} : {result}\n");*/

                TableauBuild.BuildFind("TimeLine", "delivery steps",timeline, $"{Last_data.shipment.@event[i].label}", $"{Last_data.shipment.@event[i].date.ToString("d/MM/yyyy H:mm")}");
            }
        }

        internal static void RP(DataStruct.Root Last_data)
        {
            if(Check(Last_data))
                RPs = Last_data.shipment.contextData.removalPoint != null ? Last_data.shipment.contextData.removalPoint.name : "Unknown";
            Console.WriteLine($"Removal Point: \n\t" +
                $"{RPs}\n");
        }

        internal static void URL(DataStruct.Root Last_data)
        {
            Console.WriteLine($"URL: {Last_data.shipment.urlDetail}");
            Refresh();
        }

        internal static void Refresh()
        {
            Console.Write($"Last Update: {DateTime.Now.ToString("d MMMM yyyy H:mm:ss")}\n");
            TimeSpan t;

            if (Chronopost.refresh < 1)
                Console.Write("ERROR refresh can't below 1");
            else
            {
                for (double a = Chronopost.refresh * 60; a >= 0; a--)
                {
                    if (WaitOrBreak()) break;
                    t = TimeSpan.FromSeconds(a);
                    string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s", t.Hours, t.Minutes, t.Seconds);

                    Console.Write($"\rNext Update: {answer}");
                    Thread.Sleep(1000);
                }
            }

            Console.Clear();
        }

        static ConsoleKeyInfo cki = new();
        private static bool WaitOrBreak()
        {
            if (Console.KeyAvailable) cki = Console.ReadKey(true);
            if (cki.Key == ConsoleKey.Enter)
            {
                cki = new ConsoleKeyInfo();
                return true;
            }
            return false;
        }

        static bool Check(DataStruct.Root Last_data)
        {
            if(Last_data.shipment.contextData != null) return true;
            else return false;
        }
    }
}