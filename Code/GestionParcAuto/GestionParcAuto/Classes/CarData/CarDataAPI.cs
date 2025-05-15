using Newtonsoft.Json;

namespace GestionParcAuto.Classes.CarData
{
    public class CarDataAPI : API
    {

        private static readonly HttpClient CLIENT = new HttpClient();
        private static string URL = "https://car-data.p.rapidapi.com/";
        private static Dictionary<string, string> Headers = new Dictionary<string, string>
        {
            {"x-rapidapi-key", "a876f54f94mshc3bea5956a73fcfp14812ajsn9d232eb74824"},
            {"x-rapidapi-host", "car-data.p.rapidapi.com"}
        };

        public CarDataAPI()
        {
        }

        public static async Task<List<string>> GetCarMakes()
        {
            HttpRequestMessage request = API.BuildRequest(null, null, Headers, URL, "cars/makes", "", HttpMethod.Get);

            HttpResponseMessage? response = null;

            try
            {
                response = await CLIENT.SendAsync(request);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossible de joindre le serveur.");
            }

            return JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
        }

        public static async Task<List<CarModel>> GetModels(string make, string model)
        {
            Dictionary<string, string> param = new Dictionary<string, string> 
            { 
                { 
                    "make", make 
                },
                { 
                    "model", model 
                } ,
                { 
                    "limit", "50" 
                } ,
                { 
                    "page", "0" 
                } 
            };

            HttpRequestMessage request = API.BuildRequest(param, null, Headers, URL, "cars", "", HttpMethod.Get);

            HttpResponseMessage? response = null;

            try
            {
                response = await CLIENT.SendAsync(request);
            }
            catch (Exception ex)
            {
                throw new Exception("Impossible de joindre le serveur.");
            }

            return JsonConvert.DeserializeObject<List<CarModel>>(await response.Content.ReadAsStringAsync());
        }
    }
}
