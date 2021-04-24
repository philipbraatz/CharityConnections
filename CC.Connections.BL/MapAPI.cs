using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace Doorfail.Connections.BL
{
    class MapAPI
    {
        //Maybe move key
        private const string KEY = "T2GP5RBJr1MUiQ8lyn8GLpwymCITinAW";

        //static void Main(string[] args)
        //{
        //
        //          string[] locations = {
        //            "825 Pilgrim Way, Green Bay, WI",
        //          "1507 Western Ave, Green Bay, WI"};
        //
        //    Console.WriteLine(GetDistanceFromLocation(locations[0], locations[1]));
        //  Console.ReadKey();
        //
        //      }


        private static HttpClient InitializeClient()
        {
            HttpClient client = new HttpClient { BaseAddress = new Uri("http://www.mapquestapi.com/directions/v2/") };
            //client.DefaultRequestHeaders.Add("x-apikey", "12345");
            return client;
        }

        //Example Address
        //825 Pilgrim Way, Green Bay, WI
        public static double GetDistanceFromLocations(string startAddress, string endAddress)
        {
            HttpClient client = InitializeClient();

            string jsonLocations = "locations:[";
            jsonLocations += startAddress + ",";
            jsonLocations += endAddress + "]";
            string jsonOption = "options:{routeType:shortest}";

            string jsonRequestBody = "{" + jsonLocations + "," + jsonOption + "}";
            //{locations:
            //[Clarendon Blvd, Arlington, VA],[2400 S Glebe Rd, Arlington, VA],options:{routeType:shortest} }
            dynamic responseObject = JsonConvert.DeserializeObject<dynamic>(client.PostAsync(new Uri("route?key=" + KEY),//to dynamic object
                        new StringContent(jsonRequestBody, Encoding.UTF8, "application/json")).Result//HttpResponseMessage
                    .Content.ReadAsStringAsync().Result//to string result
            );
            client.Dispose();
            return (double)responseObject.route.distance;
        }

        //DONT USE, use API instead
        //lat long points to distance in metres
        //public static double GetDistanceBetweenPoints(double lat1, double long1, double lat2, double long2)
        //{
        //    double distance = 0;
        //
        //    double dLat = (lat2 - lat1) / 180 * Math.PI;
        //    double dLong = (long2 - long1) / 180 * Math.PI;
        //
        //    double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
        //               + Math.Cos(lat2) * Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
        //    double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        //
        //    //Calculate radius of earth
        //    // For this you can assume any of the two points.
        //    double radiusE = 6378135; // Equatorial radius, in metres
        //    double radiusP = 6356750; // Polar Radius
        //
        //    //Numerator part of function
        //    double nr = Math.Pow(radiusE * radiusP * Math.Cos(lat1 / 180 * Math.PI), 2);
        //    //Denominator part of the function
        //    double dr = Math.Pow(radiusE * Math.Cos(lat1 / 180 * Math.PI), 2)
        //                + Math.Pow(radiusP * Math.Sin(lat1 / 180 * Math.PI), 2);
        //    double radius = Math.Sqrt(nr / dr);
        //
        //    //Calaculate distance in metres.
        //    distance = radius * c;
        //    return distance;
        //}
    }
}
