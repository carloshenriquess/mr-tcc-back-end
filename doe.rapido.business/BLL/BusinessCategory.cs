using doe.rapido.business.DAL.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace doe.rapido.business.BLL
{
    public class BusinessCategory
    {
        public async Task InsertUpdateCompanyCategory(string idsCategories, int idCompany)
        {
            DispatcherCategory dspCategory = new DispatcherCategory();
            await dspCategory.InsertUpdateCompanyCategory(idsCategories, idCompany);
        }

        public async Task<List<DML.Category>> GetCategoriesByIdCompany(int idCompany)
        {
            DispatcherCategory dspCategory = new DispatcherCategory();
            return await dspCategory.GetCategoriesByIdCompany(idCompany);
        }
    }
}
