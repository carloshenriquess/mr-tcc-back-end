using doe.rapido.business.DAL.Company;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace doe.rapido.business.BLL
{
    public class BusinessCompany
    {
        public async Task<int> InsertCompany(DML.Company company)
        {
            DispatcherCompany dspCompany = new DispatcherCompany();
            return await dspCompany.InsertCompany(company);
        }

        public async Task UpdateCompany(DML.Company company)
        {
            DispatcherCompany dspCompany = new DispatcherCompany();
            await dspCompany.UpdateCompany(company);
        }

        public async Task<DML.Company> GetCompanyById(int idCompany)
        {
            DispatcherCompany dspCompany = new DispatcherCompany();
            return await dspCompany.GetCompanyById(idCompany);
        }

        public async Task<DML.Company> GetCompanyByIdUser(int idUser)
        {
            DispatcherCompany dspCompany = new DispatcherCompany();
            return await dspCompany.GetCompanyByIdUser(idUser);
        }

        public async Task<List<DML.Company>> GetListCompanyByState(string stateCompany)
        {
            DispatcherCompany dspCompany = new DispatcherCompany();
            return await dspCompany.GetListCompanyByState(stateCompany);
        }

        public async Task<List<DML.Company>> GetListCompanyByCategories(string idsCategories)
        {
            DispatcherCompany dspCompany = new DispatcherCompany();
            return await dspCompany.GetListCompanyByCategories(idsCategories);
        }
    }
}
