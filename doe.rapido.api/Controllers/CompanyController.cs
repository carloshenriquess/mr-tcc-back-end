using doe.rapido.api.Models;
using doe.rapido.api.Services;
using doe.rapido.business.BLL;
using doe.rapido.business.DML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Threading.Tasks;

namespace doe.rapido.api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        #region[construtores]
        BusinessCompany businessCompany = new BusinessCompany();
        BusinessCategory businessCategory = new BusinessCategory();
        GoogleAPIController googleAPIController = new GoogleAPIController();
        #endregion
        #region[Métodos Post]
        [HttpPost]
        [Route("queryCompanies")]
        public async Task<ActionResult<string>> QueryCompany(double latitude, double longitude, [FromBody] CompanyModel categories)
        {
            CoordenatesModel coordenatesModel = new CoordenatesModel();
            coordenatesModel.Latitude = latitude;
            coordenatesModel.Longitude = longitude;
            string idCategories = string.Empty;
            foreach (JToken category in categories.needs)
            {
                idCategories += category + ";";
            }
            string state = await googleAPIController.GetStateByLatLong(coordenatesModel);
            List<Company> listCompany = new List<Company>();
            if (idCategories != string.Empty)
            {
                listCompany = await businessCompany.GetListCompanyByCategories(idCategories);
                if (listCompany == null)
                    return NotFound("Não foi encontrada nenhuma instituição com base nas categorias selecionadas");
            }
            else
            {
                listCompany = await businessCompany.GetListCompanyByState(state);
                if (listCompany == null)
                    return NotFound("Não foi encontrada nenhuma instituição com base na localização do usuário");
            }
            List<MapCompanyModel> listMapCompany = await GetListMapCompany(coordenatesModel, listCompany);
            IEnumerable<MapCompanyModel> orderedElements = new List<MapCompanyModel>();
            if (listMapCompany != null)
                orderedElements = listMapCompany.ToArray().OrderBy(x => x.distance);
            return orderedElements.Count() > 0 ? Ok(orderedElements) : Ok();
        }

        [HttpPost]
        [Route("map-company/{companyId}")]
        public async Task<ActionResult<string>> QueryCompany(int companyId, [FromBody] CompanyModel categories)
        {
            CoordenatesModel coordenatesModel = new CoordenatesModel();
            string idCategories = string.Empty;
            foreach (JToken category in categories.needs)
            {
                idCategories += category + ";";
            }
            Company originCompany = await businessCompany.GetCompanyById(companyId);
            if (originCompany == null)
                return BadRequest("Empresa não encontrada");

            coordenatesModel.Latitude = originCompany.Latitude;
            coordenatesModel.Longitude = originCompany.Longitude;
            string state = await googleAPIController.GetStateByLatLong(coordenatesModel);
            List<Company> listCompany = idCategories != string.Empty ? await businessCompany.GetListCompanyByCategories(idCategories) : await businessCompany.GetListCompanyByState(state);
            List<MapCompanyModel> listMapCompany = await GetListMapCompany(coordenatesModel, listCompany);
            IEnumerable<MapCompanyModel> orderedElements = new List<MapCompanyModel>();
            if (listMapCompany != null)
                orderedElements = listMapCompany.ToArray().OrderBy(x => x.distance);

            return orderedElements.Count() > 0 ? Ok(orderedElements.Take(20)) : Ok();
        }
        [HttpPost]
        [Authorize]
        [Route("company")]
        public async Task<ActionResult<Company>> CreateCompany(Company company)
        {
            if (company == null)
            {
                return BadRequest("Company cannot be null");
            }
            string address = company.Street + " " + company.Number + "," + company.District;
            GooglePlacesViewModel? model = await googleAPIController.GetByAddress(address);
            if (model != null && model.candidates.Count > 0)
            {
                company.Latitude = model.candidates.FirstOrDefault()!.geometry.location.lat;
                company.Longitude = model.candidates.FirstOrDefault()!.geometry.location.lng;
            }
            int companyId = await businessCompany.InsertCompany(company);
            Company createdCompany = await businessCompany.GetCompanyById(companyId);
            return Ok(createdCompany);
        }
        #endregion
        #region[Métodos Get]
        [HttpGet]
        [Route("company/{companyId}")]
        public async Task<ActionResult<string>> QueryCompanyById(int companyId)
        {
            Company? company = await businessCompany.GetCompanyById(companyId);
            if (company == null)
            {
                return NotFound("Empresa não encontrada");
            }
            return Ok(company);
        }
        [HttpGet]
        [Route("companyByUserId/{userId}")]
        public async Task<ActionResult<string>> QueryCompanyByuserId(int userId)
        {
            Company? company = await businessCompany.GetCompanyByIdUser(userId);
            if (company == null)
            {
                return NotFound("Empresa não encontrada");
            }
            return Ok(company);
        }
        [HttpGet]
        [Route("companies/{companyId}")]
        public async Task<ActionResult<string>> QueryCompaniesById(int companyId)
        {
            Company? company = await businessCompany.GetCompanyById(companyId);
            if (company == null)
            {
                return NotFound("Empresa não encontrada");
            }
            CoordenatesModel coordenatesModel = new CoordenatesModel();
            coordenatesModel.Latitude = company.Latitude;
            coordenatesModel.Longitude = company.Longitude;
            List<Company> companies = await businessCompany.GetListCompanyByState(company.State);
            List<MapCompanyModel> listMapCompany = await GetListMapCompany(coordenatesModel, companies);
            IEnumerable<MapCompanyModel> orderedElements = new List<MapCompanyModel>();
            if (listMapCompany != null)
                orderedElements = listMapCompany.ToArray().OrderBy(x => x.distance);
            return orderedElements.Count() > 0 ? Ok(orderedElements.Take(20)) : Ok();
        }
        #endregion
        #region[Métodos put]
        [HttpPut]
        [Authorize]
        [Route("company/{companyId}")]
        public async Task<ActionResult<string>> UpdateCompany(int companyId, [FromBody] Company companyJson)
        {
            Company? company = await businessCompany.GetCompanyById(companyId);
            string categories = string.Empty;
            if (company == null)
            {
                return NotFound("Empresa não encontrada");
            }
            company = await MapCompany(company, companyJson);
            await businessCompany.UpdateCompany(company);

            if (companyJson.Needs != null && companyJson.Needs.Count > 0)
            {
                foreach (var category in companyJson.Needs!)
                {
                    categories += category + ";";
                }
                await businessCategory.InsertUpdateCompanyCategory(categories, company.Id);
            }
            return Ok(await businessCompany.GetCompanyById(companyId));
        }
        #endregion
        #region[Métodos privados]
        private async Task<Company> MapCompany(Company company, Company value)
        {
            if (value.IdUser != 0)
                company.IdUser = value.IdUser;
            if (value.Latitude != 0)
                company.Latitude = value.Latitude!;
            if (value.Latitude != 0)
                company.Longitude = value.Longitude!;
            if (value.TradingName != null)
                company.TradingName = value.TradingName!;
            if (value.Name != null)
                company.Name = value.Name!;
            if (value.Cnpj != null)
                company.Cnpj = value.Cnpj!;
            if (value.Cep != null)
                company.Cep = value.Cep!;
            if (value.Street != null)
                company.Street = value.Street!;
            if (value.Number != null)
                company.Number = value.Number!;
            if (value.District != null)
                company.District = value.District!;
            if (value.City != null)
                company.City = value.City!;
            if (value.State != null)
                company.State = value.State!;
            if (value.Phone != null)
                company.Phone = value.Phone!;
            if (value.Email != null)
                company.Email = value.Email!;
            if (value.Image != null && !value.Image.StartsWith("https"))
            {
                ImageService imageService = new ImageService();
                string url = await imageService.UploadBase64Image(value.Image!);
                company.Image = url!;
            }
            if (value.DtInclude != null)
                company.DtInclude = value.DtInclude!;
            if (value.DtUpdate != null)
                company.DtUpdate = value.DtUpdate!;
            if (value.PhoneWhatsapp != null)
                company.PhoneWhatsapp = value.PhoneWhatsapp!;
            return company;

        }
        private async Task<List<MapCompanyModel>> GetListMapCompany(CoordenatesModel origin, List<Company> listCompany)
        {
            List<MapCompanyModel> listMapCompany = new List<MapCompanyModel>();
            CoordenatesModel addressOriginCompany = new CoordenatesModel();
            addressOriginCompany.Latitude = origin.Latitude;
            addressOriginCompany.Longitude = origin.Longitude;
            foreach (Company company in listCompany)
            {
                CoordenatesModel destinations = new CoordenatesModel();
                destinations.Latitude = company.Latitude;
                destinations.Longitude = company.Longitude;
                GoogleDistanceMatrixModel? googlePlacesViewModel = await googleAPIController.GetDistanceMatrix(addressOriginCompany, destinations);

                MapCompanyModel mapCompany = new MapCompanyModel();
                mapCompany.lat = company.Latitude;
                mapCompany.@long = company.Longitude;
                if (googlePlacesViewModel != null)
                    mapCompany.distance = googlePlacesViewModel.rows.FirstOrDefault()!.elements.FirstOrDefault()!.status != "NOT_FOUND" ? googlePlacesViewModel.rows.FirstOrDefault()!.elements.FirstOrDefault()!.distance.value : null;
                mapCompany.id_company = company.Id;
                mapCompany.name = company.Name;
                mapCompany.Street = company.Street;
                mapCompany.Number = company.Number;
                mapCompany.District = company.District;
                mapCompany.City = company.City;
                mapCompany.State = company.State;
                mapCompany.Cep = company.Cep;
                mapCompany.phone = company.Phone;
                mapCompany.needs = new List<int>();
                foreach (int item in company.Needs)
                {
                    mapCompany.needs.Add(item);
                }
                listMapCompany.Add(mapCompany);
            }
            return listMapCompany;
        }
        #endregion
    }
}
