using System;
using Chronopost.Data;

#pragma warning disable CS8602

namespace Chronopost.Print
{
    public class Print
    {
        public static string arrived = "";
        private static string timeline = "";

        internal static void Base(DataStruct.Root Last_data)
        {
                arrived = Last_data.shipment.isFinal == true ? "YES" : "NO";

                Console.WriteLine(
                    $"Langue: {Last_data.lang},\n" +
                    $"Product ID: {Last_data.shipment.idShip},\n" +
                    $"Delivery man: {Last_data.shipment.product},\n" +
                    $"Entry Date: {Last_data.shipment.entryDate.ToString("d MMMM yyyy H:mm:ss")},\n" +
                    $"Delivered ? {arrived}\n");
        }

        internal static void TimeLine(DataStruct.Root Last_data)
        {
            foreach (DataStruct.Timeline v in Last_data.shipment.timeline)
            {
                if (v.status == true)
                {
                    timeline = $"{v.shortLabel}\n\t" +
                        $"{v.longLabel}";
                }
            }

            Console.WriteLine($"TimeLine: \n\t{timeline}");
        }

        internal static void Step(DataStruct.Root Last_data)
        {
            for (int i = 0; i < Last_data.shipment.@event.Count; i++)
            {
                Console.WriteLine($"delivery steps: \n\t" +
                    $"{Last_data.shipment.@event[i].date.ToString("d MMMM yyyy H:mm:ss")}\n\t" +
                    $"Progress: {Last_data.shipment.@event[i].label}");
            }
        }

        internal static void URL(DataStruct.Root Last_data)
        {
            Console.WriteLine($"\nURL: {Last_data.shipment.urlDetail}");
        }

        internal static void Refresh()
        {
            Console.Write($"Last Update: {DateTime.Now.ToString("d MMMM yyyy H:mm:ss")}");
        }
    }
}

