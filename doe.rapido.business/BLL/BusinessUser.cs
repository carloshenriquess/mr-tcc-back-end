using doe.rapido.business.DAL.User;
using Newtonsoft.Json.Linq;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace doe.rapido.business.BLL
{
    public class BusinessUser
    {
        public async Task<bool?> ConfirmUser(string emailUser, int codeConfirm)
        {
            DispatcherUser dspUser = new DispatcherUser();
            return await dspUser.ConfirmUser(emailUser, codeConfirm);
        }

        public async Task DeleteUserAndCompany(int idUser)
        {
            DispatcherUser dspUser = new DispatcherUser();
            await dspUser.DeleteUserAndCompany(idUser);
        }

        public async Task<DML.User> GetUserByEmail(string emailUser)
        {
            DispatcherUser dspUser = new DispatcherUser();
            return await dspUser.GetUserByEmail(emailUser);
        }

        public async Task<int> InsertUser(DML.User user)
        {
            DispatcherUser dspUser = new DispatcherUser();
            return await dspUser.InsertUser(user);
        }

        public async Task<DML.User> GetUserById(int idUser)
        {
            DispatcherUser dspUser = new DispatcherUser();
            return await dspUser.GetUserById(idUser);
        }

        public async Task UpdateUser(DML.User user)
        {
            DispatcherUser dspUser = new DispatcherUser();
            await dspUser.UpdateUser(user);
        }

        public async Task<DML.User> GetUserByLogin(string emailUser, string passwordUser)
        {
            DispatcherUser dspUser = new DispatcherUser();
            return await dspUser.GetUserByLogin(emailUser, passwordUser);
        }

        public int RandomNumber()
        {
            Random random = new Random();
            int codigo = Convert.ToInt32(random.Next(10000, 99999).ToString());
            return codigo;
        }
        public DML.User MappingUser(DML.User user, DML.User value)
        {
            if (value.Name != null)
                user.Name = value.Name;
            if (value.Email != null)
                user.Email = value.Email;
            if (value.Password != null)
                user.Password = value.Password;
            if (value.Confirmed != null)
                user.Confirmed = value.Confirmed;
            if (value.DtInclude != null)
                user.DtInclude = value.DtInclude;
            if (value.CodeConfirm != null)
                user.CodeConfirm = value.CodeConfirm;
            if (value.StepOnboarding != null)
                user.StepOnboarding = value.StepOnboarding;
            user.DtUpdate = DateTime.Now;
            return user;
        }
    }
}
