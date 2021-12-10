using doe.rapido.data.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace doe.rapido.business.DAL.Category
{
    public class DispatcherCategory
    {
        #region Constants
        private const string INSERT_UPDATE_COMPANY_CATEGORY = "pr_insert_update_company_category";
        private const string SELECT_CATEGORY_BY_ID_COMPANY = "pr_select_category_by_id_company";
        #endregion

        #region Methods
        internal async Task InsertUpdateCompanyCategory(string idsCategories, int idCompany)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@categories", idsCategories));
            param.Add(new SqlParameter("@id_company", idCompany));

            await new DataBase().ExecuteProcedure(INSERT_UPDATE_COMPANY_CATEGORY, param.ToArray());
        }

        internal async Task<List<DML.Category>> GetCategoriesByIdCompany(int idCompany)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@id_company", idCompany));

            DataTable dt = new DataBase().GetRecords(SELECT_CATEGORY_BY_ID_COMPANY, param.ToArray()).Result.Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                List<DML.Category> lstCategory = new List<DML.Category>();
                
                foreach (DataRow row in dt.Rows)
                {
                    DML.Category category = new DML.Category();
                    FillModel(ref category, row);
                    lstCategory.Add(category);
                }

                return lstCategory;
            }

            return null;
        }

        private void FillModel(ref DML.Category category, DataRow row)
        {
            if ((row.Table.Columns.Contains("id_category")) && (row["id_category"] != DBNull.Value))
                category.Id = Convert.ToInt32(row["id_category"]);

            if ((row.Table.Columns.Contains("ds_name")) && (row["ds_name"] != DBNull.Value))
                category.Name = Convert.ToString(row["ds_name"]);
        }
        #endregion
    }
}
