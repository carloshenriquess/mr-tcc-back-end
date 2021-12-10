using doe.rapido.data.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace doe.rapido.business.DAL.User
{
    public class DispatcherUser
    {
        #region Constants
        private const string CONFIRM_USER = "pr_confirm_user";
        private const string DELETE_USER_COMPANY = "pr_delete_user_company";
        private const string SELECT_USER_BY_EMAIL = "pr_select_user_by_email";
        private const string INSERT_USER = "pr_insert_user";
        private const string SELECT_USER_BY_ID = "pr_select_user_by_id";
        private const string SELECT_USER_BY_LOGIN = "pr_select_user_by_login";
        private const string UPDATE_USER = "pr_update_user";
        #endregion

        #region Methods
        internal async Task<bool?> ConfirmUser(string emailUser, int codeConfirm)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@ds_email", emailUser));
            param.Add(new SqlParameter("@nr_code_confirm", codeConfirm));

            DataRow dr = new DataBase().GetRecords(CONFIRM_USER, param.ToArray()).Result.Tables[0].Rows[0];

            if (dr["fg_confirmed"] != DBNull.Value)
                return Convert.ToBoolean(dr["fg_confirmed"]);
            else
                return null;
        }

        internal async Task DeleteUserAndCompany(int idUser)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@id_user", idUser));

            await new DataBase().ExecuteProcedure(DELETE_USER_COMPANY, param.ToArray());
        }

        internal async Task<DML.User> GetUserByEmail(string emailUser)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ds_email", emailUser));

            DataTable dt = new DataBase().GetRecords(SELECT_USER_BY_EMAIL, param.ToArray()).Result.Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                DML.User user = new DML.User();
                FillModel(ref user, dt.Rows[0]);
                return user;
            }

            return null;
        }

        internal async Task<int> InsertUser(DML.User user)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@ds_name", user.Name));
            param.Add(new SqlParameter("@ds_email", user.Email));
            param.Add(new SqlParameter("@ds_password", user.Password));
            param.Add(new SqlParameter("@nr_code_confirm", user.CodeConfirm));
            param.Add(new SqlParameter("@dt_expire_code_confirm", user.DtExpireCodeConfirm));
            param.Add(new SqlParameter("@ds_step_onboarding", user.StepOnboarding));

            DataRow dr = new DataBase().GetRecords(INSERT_USER, param.ToArray()).Result.Tables[0].Rows[0];

            if (dr["id_user"] != DBNull.Value)
                return Convert.ToInt32(dr["id_user"]);
            else
                return 0;
        }

        internal async Task<DML.User> GetUserById(int idUser)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@id_user", idUser));

            DataTable dt = new DataBase().GetRecords(SELECT_USER_BY_ID, param.ToArray()).Result.Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                DML.User user = new DML.User();
                FillModel(ref user, dt.Rows[0]);
                return user;
            }

            return null;
        }

        internal async Task UpdateUser(DML.User user)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@id_user", user.Id));
            param.Add(new SqlParameter("@ds_name", user.Name));
            param.Add(new SqlParameter("@ds_email", user.Email));
            param.Add(new SqlParameter("@ds_password", user.Password));
            param.Add(new SqlParameter("@nr_code_confirm", user.CodeConfirm));
            param.Add(new SqlParameter("@dt_expire_code_confirm", user.DtExpireCodeConfirm));
            param.Add(new SqlParameter("@fg_confirmed", user.Confirmed));
            param.Add(new SqlParameter("@ds_step_onboarding", user.StepOnboarding));

            await new DataBase().ExecuteProcedure(UPDATE_USER, param.ToArray());
        }

        internal async Task<DML.User> GetUserByLogin(string emailUser, string passwordUser)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@ds_email", emailUser));
            param.Add(new SqlParameter("@ds_password", passwordUser));

            DataTable dt = new DataBase().GetRecords(SELECT_USER_BY_LOGIN, param.ToArray()).Result.Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                DML.User user = new DML.User();
                FillModel(ref user, dt.Rows[0]);
                return user;
            }

            return null;
        }

        private void FillModel(ref DML.User user, DataRow row)
        {
            if ((row.Table.Columns.Contains("id_user")) && (row["id_user"] != DBNull.Value))
                user.Id = Convert.ToInt32(row["id_user"]);

            if ((row.Table.Columns.Contains("ds_name")) && (row["ds_name"] != DBNull.Value))
                user.Name = Convert.ToString(row["ds_name"]);

            if ((row.Table.Columns.Contains("ds_email")) && (row["ds_email"] != DBNull.Value))
                user.Email = Convert.ToString(row["ds_email"]);

            if ((row.Table.Columns.Contains("ds_password")) && (row["ds_password"] != DBNull.Value))
                user.Password = Convert.ToString(row["ds_password"]);

            if ((row.Table.Columns.Contains("fg_confirmed")) && (row["fg_confirmed"] != DBNull.Value))
                user.Confirmed = Convert.ToBoolean(row["fg_confirmed"]);

            if ((row.Table.Columns.Contains("nr_code_confirm")) && (row["nr_code_confirm"] != DBNull.Value))
                user.CodeConfirm = Convert.ToInt32(row["nr_code_confirm"]);

            if ((row.Table.Columns.Contains("dt_expire_code_confirm")) && (row["dt_expire_code_confirm"] != DBNull.Value))
                user.DtExpireCodeConfirm = Convert.ToDateTime(row["dt_expire_code_confirm"]);

            if ((row.Table.Columns.Contains("dt_include")) && (row["dt_include"] != DBNull.Value))
                user.DtInclude = Convert.ToDateTime(row["dt_include"]);

            if ((row.Table.Columns.Contains("dt_update")) && (row["dt_update"] != DBNull.Value))
                user.DtUpdate = Convert.ToDateTime(row["dt_update"]);

            if ((row.Table.Columns.Contains("ds_step_onboarding")) && (row["ds_step_onboarding"] != DBNull.Value))
                user.StepOnboarding = Convert.ToString(row["ds_step_onboarding"]);
        }
        #endregion
    }
}