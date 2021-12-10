namespace doe.rapido.api.Models
{
    public class MapCompanyModel
    {


        public int id_company { get; set; }
        public string name { get; set; }
        public string Cep { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? distance { get; set; }
        public double lat { get; set; }
        public double @long { get; set; }
        public string phone { get; set; }
        public List<int> needs { get; set; }
    }
}
