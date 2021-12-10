using doe.rapido.api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace doe.rapido.api.Controllers
{
    public class GoogleAPIController
    {
        #region[Métodos publicos]
        public async Task<GooglePlacesViewModel?> GetByAddress(string address)
        {
            string conexao = Settings.MapsUrl + "/place/findplacefromtext/json?inputtype=textquery&&input=" + address + "&fields=formatted_address,name,geometry&key=" + Settings.PUBLIC_MAPS_TOKEN;
            string result = await ConexaoGet(conexao);
            return  result != null ? JsonConvert.DeserializeObject<GooglePlacesViewModel>(result) : null;
        }

        public async Task<GoogleDistanceMatrixModel?> GetDistanceMatrix(CoordenatesModel origins, CoordenatesModel destinations)
        {
            string AllOrigins = origins.Latitude + " " + origins.Longitude;
            string AllDestinations = destinations.Latitude + " " + destinations.Longitude;
            string conexao = Settings.MapsUrl + "/distancematrix/json?origins=" + AllOrigins.Replace(",", ".") + "&destinations=" + AllDestinations.Replace(",", ".") + "&key=" + Settings.PUBLIC_MAPS_TOKEN;
            string result = await ConexaoGet(conexao);
            return  result != null ? JsonConvert.DeserializeObject<GoogleDistanceMatrixModel>(result) : null;
        }

        public async Task<string> GetStateByLatLong(CoordenatesModel coordenates)
        {
            string _coordenates = coordenates.Latitude + " " + coordenates.Longitude;
            string conexao = Settings.MapsUrl + "/geocode/json?latlng=" + _coordenates.Replace(",", ".") + "& language = pt&key=" + Settings.PUBLIC_MAPS_TOKEN;
            string result = await ConexaoGet(conexao);
            return !String.IsNullOrEmpty(result) ? JsonConvert.DeserializeObject<GoogleGeoCode>(result)!.results.FirstOrDefault()!.address_components[4].short_name : "";
        }
        #endregion
        #region[Métodos privados]
        private async Task<string> ConexaoGet(String conexao)
        {
            HttpClient httpClient = new HttpClient();
            var httpResponse = await httpClient.GetAsync(conexao);
            if (!httpResponse.IsSuccessStatusCode)
            {
                return "Erro de comunicação com o servidor";
            }
            // We assume that if the server responds at all, it responds with valid JSON.
            return await httpResponse.Content.ReadAsStringAsync();
        }
        #endregion
    }
}
