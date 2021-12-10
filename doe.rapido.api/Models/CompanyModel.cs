namespace doe.rapido.api.Models
{
    public class CompanyModel
    {
        public CompanyModel()
        {
            this.needs = new List<int>();
        }
        public List<int> needs { get; set; }
    }
}
