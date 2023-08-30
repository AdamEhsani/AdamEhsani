using CrmWeb.api;
using CrmWeb.Pages.Clients;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Data.SqlClient;

[ApiController]
[Route("api/[controller]/[action]")]
public class NominatimService : Controller
{
    private UserLocationController _userLocation = new UserLocationController();
    public JToken latitude { get; set; }
    public JToken longitude { get; set; }

    public IActionResult Index()
    {
        return View();
        //  _userLocation.GetUserLocationAsync();
    }

    [HttpPost]
    public async Task<object> GetLocationInfoAsync([FromBody] SearchAddress address)
    {
        //var client = new RestClient("https://nominatim.openstreetmap.org");
        //var request = new RestRequest($"/search?q={address.Address}&countrycodes=de-DE", Method.Get);
        //var response = client.Execute(request);


        //if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //{
        //    return Json(response.Content.Trim());
        //}

        //return Json(address.Address.Trim());

        HttpClient client = new HttpClient();
        string url = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(address.Address)}";
        var response = await client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();

        return Json(content);
    }



    public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        var R = 6371; // شعاع زمین در کیلومترها

        var dLat = ToRadians(lat2 - lat1);
        var dLon = ToRadians(lon2 - lon1);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var distance = R * c;

        return distance;
    }

    private double ToRadians(double degree)
    {
        return degree * Math.PI / 180;
    }
}
