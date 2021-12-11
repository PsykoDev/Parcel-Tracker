using System;
namespace Chronopost.Data
{
    public class DataStruct
    {
        public class RemovalPoint
        {
            public string? name { get; set; }
            public string? type { get; set; }
        }

        public class ContextData
        {
            public RemovalPoint? removalPoint { get; set; }
        }

        public class Timeline
        {
            public int id { get; set; }
            public string? shortLabel { get; set; }
            public string? longLabel { get; set; }
            public bool status { get; set; }
            public int type { get; set; }
            public string? country { get; set; }
        }

        public class Event
        {
            public DateTime date { get; set; }
            public string? label { get; set; }
            public string? code { get; set; }
        }

        public class Shipment
        {
            public string? idShip { get; set; }
            public string? product { get; set; }
            public int holder { get; set; }
            public bool isFinal { get; set; }
            public DateTime entryDate { get; set; }
            public List<Timeline>? timeline { get; set; }
            public List<Event>? @event { get; set; }
            public string? url { get; set; }
            public string? urlDetail { get; set; }
            public ContextData? contextData { get; set; }
        }

        public class Root
        {
            public string? lang { get; set; }
            public string? scope { get; set; }
            public int returnCode { get; set; }
            public Shipment? shipment { get; set; }
        }
    }
}

