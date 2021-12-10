using doe.rapido.business.DAL.Category;
using doe.rapido.data.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Linq;

namespace doe.rapido.business.DAL.Company
{
    public class DispatcherCompany
    {
        #region Constants
        private const string INSERT_COMPANY = "pr_insert_company";
        private const string SELECT_COMPANY_BY_ID = "pr_select_company_by_id";
        private const string SELECT_COMPANY_BY_ID_USER = "pr_select_company_by_id_user";
        private const string SELECT_LIST_COMPANY_BY_STATE = "pr_select_list_company_by_state";
        private const string SELECT_LIST_COMPANY_BY_CATEGORIES = "pr_select_list_company_by_categories";
        private const string UPDATE_COMPANY = "pr_update_company";
        #endregion

        #region Methods
        internal async Task<int> InsertCompany(DML.Company company)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@nr_latitude", company.Latitude));
            param.Add(new SqlParameter("@nr_longitude", company.Longitude));
            param.Add(new SqlParameter("@ds_trading_name", company.TradingName));
            param.Add(new SqlParameter("@ds_name", company.Name));
            param.Add(new SqlParameter("@nr_cnpj", company.Cnpj));
            param.Add(new SqlParameter("@nr_cep", company.Cep));
            param.Add(new SqlParameter("@ds_street", company.Street));
            param.Add(new SqlParameter("@nr_number", company.Number));
            param.Add(new SqlParameter("@ds_district", company.District));
            param.Add(new SqlParameter("@ds_city", company.City));
            param.Add(new SqlParameter("@ds_state", company.State));
            param.Add(new SqlParameter("@nr_phone", !string.IsNullOrEmpty(company.Phone) ? company.Phone : (object)DBNull.Value));
            param.Add(new SqlParameter("@nr_phone_whatsapp", !string.IsNullOrEmpty(company.PhoneWhatsapp) ? company.PhoneWhatsapp : (object)DBNull.Value));
            param.Add(new SqlParameter("@ds_email", !string.IsNullOrEmpty(company.Email) ? company.Email : (object)DBNull.Value));
            param.Add(new SqlParameter("@ds_image", !string.IsNullOrEmpty(company.Image) ? company.Image : (object)DBNull.Value));
            param.Add(new SqlParameter("@id_user", company.IdUser));

            DataRow dr = new DataBase().GetRecords(INSERT_COMPANY, param.ToArray()).Result.Tables[0].Rows[0];

            if (dr["id_company"] != DBNull.Value)
                return Convert.ToInt32(dr["id_company"]);
            else
                return 0;
        }

        internal async Task UpdateCompany(DML.Company company)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@id_company", company.Id));
            param.Add(new SqlParameter("@nr_latitude", company.Latitude));
            param.Add(new SqlParameter("@nr_longitude", company.Longitude));
            param.Add(new SqlParameter("@ds_trading_name", company.TradingName));
            param.Add(new SqlParameter("@ds_name", company.Name));
            param.Add(new SqlParameter("@nr_cnpj", company.Cnpj));
            param.Add(new SqlParameter("@nr_cep", company.Cep));
            param.Add(new SqlParameter("@ds_street", company.Street));
            param.Add(new SqlParameter("@nr_number", company.Number));
            param.Add(new SqlParameter("@ds_district", company.District));
            param.Add(new SqlParameter("@ds_city", company.City));
            param.Add(new SqlParameter("@ds_state", company.State));
            param.Add(new SqlParameter("@ds_image", !string.IsNullOrEmpty(company.Image) ? company.Image : (object)DBNull.Value));
            param.Add(new SqlParameter("@nr_phone", !string.IsNullOrEmpty(company.Phone) ? company.Phone : (object)DBNull.Value));
            param.Add(new SqlParameter("@nr_phone_whatsapp", !string.IsNullOrEmpty(company.PhoneWhatsapp) ? company.PhoneWhatsapp : (object)DBNull.Value));
            param.Add(new SqlParameter("@ds_email", !string.IsNullOrEmpty(company.Email) ? company.Email : (object)DBNull.Value));

            await new DataBase().ExecuteProcedure(UPDATE_COMPANY, param.ToArray());
        }

        internal async Task<DML.Company> GetCompanyById(int idCompany)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@id_company", idCompany));

            DataTable dt = new DataBase().GetRecords(SELECT_COMPANY_BY_ID, param.ToArray()).Result.Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                DML.Company company = new DML.Company();
                FillModel(ref company, dt.Rows[0]);
                return company;
            }

            return null;
        }

        internal async Task<DML.Company> GetCompanyByIdUser(int idUser)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@id_user", idUser));

            DataTable dt = new DataBase().GetRecords(SELECT_COMPANY_BY_ID_USER, param.ToArray()).Result.Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                DML.Company company = new DML.Company();
                FillModel(ref company, dt.Rows[0]);
                return company;
            }

            return null;
        }

        internal async Task<List<DML.Company>> GetListCompanyByState(string stateCompany)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ds_state", stateCompany));

            DataSet ds = new DataBase().GetRecords(SELECT_LIST_COMPANY_BY_STATE, param.ToArray()).Result;

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    List<DML.Company> lstCompany = new List<DML.Company>();

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DataTable dtCat = null;
                        DML.Company company = new DML.Company();

                        if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                        {
                            List<DataRow> lstDrCat = ds.Tables[1].AsEnumerable().Where(l => l.Field<int>("id_company") == Convert.ToInt32(row["id_company"])).ToList();

                            if (lstDrCat != null && lstDrCat.Count > 0)
                                dtCat = lstDrCat.CopyToDataTable();
                        }

                        FillModel(ref company, row, dtCat);
                        lstCompany.Add(company);
                    }

                    return lstCompany;
                }
            }

            return null;
        }

        internal async Task<List<DML.Company>> GetListCompanyByCategories(string idsCategories)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@categories", idsCategories));

            DataSet ds = new DataBase().GetRecords(SELECT_LIST_COMPANY_BY_CATEGORIES, param.ToArray()).Result;

            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    List<DML.Company> lstCompany = new List<DML.Company>();

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        DML.Company company = new DML.Company();
                        DataTable dtCat = null;

                        if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                        {
                            List<DataRow> lstDrCat = ds.Tables[1].AsEnumerable().Where(l => l.Field<int>("id_company") == Convert.ToInt32(row["id_company"])).ToList();

                            if (lstDrCat != null && lstDrCat.Count > 0)
                                dtCat = lstDrCat.CopyToDataTable();
                        }

                        FillModel(ref company, row, dtCat);
                        lstCompany.Add(company);
                    }

                    return lstCompany;
                }
            }

            return null;
        }

        private void FillModel(ref DML.Company company, DataRow row, DataTable dtCat = null)
        {
            if ((row.Table.Columns.Contains("id_company")) && (row["id_company"] != DBNull.Value))
                company.Id = Convert.ToInt32(row["id_company"]);

            if ((row.Table.Columns.Contains("id_user")) && (row["id_user"] != DBNull.Value))
                company.IdUser = Convert.ToInt32(row["id_user"]);

            if ((row.Table.Columns.Contains("nr_latitude")) && (row["nr_latitude"] != DBNull.Value))
                company.Latitude = Convert.ToDouble(row["nr_latitude"]);

            if ((row.Table.Columns.Contains("nr_longitude")) && (row["nr_longitude"] != DBNull.Value))
                company.Longitude = Convert.ToDouble(row["nr_longitude"]);

            if ((row.Table.Columns.Contains("ds_trading_name")) && (row["ds_trading_name"] != DBNull.Value))
                company.TradingName = Convert.ToString(row["ds_trading_name"]);

            if ((row.Table.Columns.Contains("ds_name")) && (row["ds_name"] != DBNull.Value))
                company.Name = Convert.ToString(row["ds_name"]);

            if ((row.Table.Columns.Contains("nr_cnpj")) && (row["nr_cnpj"] != DBNull.Value))
                company.Cnpj = Convert.ToString(row["nr_cnpj"]);

            if ((row.Table.Columns.Contains("nr_cep")) && (row["nr_cep"] != DBNull.Value))
                company.Cep = Convert.ToString(row["nr_cep"]);

            if ((row.Table.Columns.Contains("ds_street")) && (row["ds_street"] != DBNull.Value))
                company.Street = Convert.ToString(row["ds_street"]);

            if ((row.Table.Columns.Contains("nr_number")) && (row["nr_number"] != DBNull.Value))
                company.Number = Convert.ToString(row["nr_number"]);

            if ((row.Table.Columns.Contains("ds_district")) && (row["ds_district"] != DBNull.Value))
                company.District = Convert.ToString(row["ds_district"]);

            if ((row.Table.Columns.Contains("ds_city")) && (row["ds_city"] != DBNull.Value))
                company.City = Convert.ToString(row["ds_city"]);

            if ((row.Table.Columns.Contains("ds_state")) && (row["ds_state"] != DBNull.Value))
                company.State = Convert.ToString(row["ds_state"]);

            if ((row.Table.Columns.Contains("nr_phone")) && (row["nr_phone"] != DBNull.Value))
                company.Phone = Convert.ToString(row["nr_phone"]);

            if ((row.Table.Columns.Contains("nr_phone_whatsapp")) && (row["nr_phone_whatsapp"] != DBNull.Value))
                company.PhoneWhatsapp = Convert.ToString(row["nr_phone_whatsapp"]);

            if ((row.Table.Columns.Contains("ds_email")) && (row["ds_email"] != DBNull.Value))
                company.Email = Convert.ToString(row["ds_email"]);

            if ((row.Table.Columns.Contains("ds_image")) && (row["ds_image"] != DBNull.Value))
                company.Image = Convert.ToString(row["ds_image"]);

            if ((row.Table.Columns.Contains("dt_include")) && (row["dt_include"] != DBNull.Value))
                company.DtInclude = Convert.ToDateTime(row["dt_include"]);

            if ((row.Table.Columns.Contains("dt_update")) && (row["dt_update"] != DBNull.Value))
                company.DtUpdate = Convert.ToDateTime(row["dt_update"]);

            company.Needs = new List<int>();

            if (dtCat != null && dtCat.Rows.Count > 0)
            {
                foreach (DataRow category in dtCat.Rows)
                {
                    if (category.Table.Columns.Contains("id_category") && (category["id_category"] != DBNull.Value))
                        company.Needs.Add(Convert.ToInt32(category["id_category"]));
                }
            }
            else
            {
                List<DML.Category> lstCat = new DispatcherCategory().GetCategoriesByIdCompany(company.Id).Result;

                if (lstCat != null && lstCat.Count > 0)
                    company.Needs = lstCat.Select(i => i.Id).ToList();
            }
        }
        #endregion
    }
}
