using Dapper;
using Dos4PeopleApp.Models;
using Dos4PeopleApp.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Dos4PeopleApp.DA
{
    public class UserDA
    {
        public async Task<VmReturnType> InsertUser(VmUser _objUser)
        {
            var conn = Utility.Utility.GetConnection();
            VmReturnType _objReturnType = new VmReturnType();
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                var parameters = new DynamicParameters();
                parameters.Add("UserName", _objUser.UserName.Trim());
                parameters.Add("FullName", _objUser.FullName.Trim());
                parameters.Add("Email", _objUser.Email.Trim());
                parameters.Add("Mobile", _objUser.Mobile.Trim());
                parameters.Add("Password", _objUser.Password.Trim());
                parameters.Add("ReferrelUserName", _objUser.ReferrelUserName == null || _objUser.ReferrelUserName == "" ? null : _objUser.ReferrelUserName.Trim());
                parameters.Add("UserId", null, DbType.String, ParameterDirection.Output, 37);
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "UserInfo_Add";
                var result = await conn.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
                _objReturnType.ID = parameters.Get<string>("UserId");
                _objReturnType.ErrCode = parameters.Get<string>("ErrCode");
                _objReturnType.UserMsg = parameters.Get<string>("UserMsg");
                if (_objReturnType.ErrCode != null && _objReturnType.ErrCode != "00")
                {
                    _objReturnType.Status = false;
                    throw new CustomException(_objReturnType.UserMsg);
                }
                _objReturnType.Status = true;
            }
            catch (Exception ex)
            {
                _objReturnType.Status = false;
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return _objReturnType;
        }

        internal async Task<VmUser> CheckExistingPassword(string Password,string UserName)
        {
            var conn = Utility.Utility.GetConnection();
            try
            {
                VmUser result = new VmUser();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("UserName", UserName);
                parameters.Add("Password", Password);
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "CheckUserPassword_Get";
                result = await conn.QueryFirstOrDefaultAsync<VmUser>(query, parameters, commandType: CommandType.StoredProcedure);
                string errorCode = parameters.Get<string>("ErrCode");
                string userMsg = parameters.Get<string>("UserMsg");
                if (errorCode != null && errorCode != "00")
                {
                    throw new CustomException(userMsg);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        internal async Task<VmReturnType> ChangeCurrentPassword(VmUser objVmUser)
        {
            var conn = Utility.Utility.GetConnection();
            VmReturnType _objReturnType = new VmReturnType();
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                var parameters = new DynamicParameters();
                parameters.Add("UserName", objVmUser.UserName.Trim());               
                parameters.Add("Password", objVmUser.Password.Trim());   
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "UserInfo_Update";
                var result = await conn.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);               
                _objReturnType.ErrCode = parameters.Get<string>("ErrCode");
                _objReturnType.UserMsg = parameters.Get<string>("UserMsg");
                if (_objReturnType.ErrCode != null && _objReturnType.ErrCode != "00")
                {
                    _objReturnType.Status = false;
                    throw new CustomException(_objReturnType.UserMsg);
                }
                _objReturnType.Status = true;
            }
            catch (Exception ex)
            {
                _objReturnType.Status = false;
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return _objReturnType;
        }

        internal async Task<VmUser> GetUserInfoByEmail(string email)
        {
            var conn = Utility.Utility.GetConnection();
            try
            {
                VmUser result = new VmUser();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Email", email);
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "UserInfoByEmail_Get";
                result = await conn.QueryFirstOrDefaultAsync<VmUser>(query, parameters, commandType: CommandType.StoredProcedure);
                string errorCode = parameters.Get<string>("ErrCode");
                string userMsg = parameters.Get<string>("UserMsg");
                if (errorCode != null && errorCode != "00")
                {
                    throw new CustomException(userMsg);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<VmUser> CheckAutehtication(VmUser _objUser)
        {
            var conn = Utility.Utility.GetConnection();
            try
            {
                VmUser result = new VmUser();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("UserName", _objUser.UserName);
                parameters.Add("Password", _objUser.Password);
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "User_Autehticate";
                result = await conn.QueryFirstOrDefaultAsync<VmUser>(query, parameters, commandType: CommandType.StoredProcedure);
                string errorCode = parameters.Get<string>("ErrCode");
                string userMsg = parameters.Get<string>("UserMsg");
                if (errorCode != null && errorCode != "00")
                {
                    throw new CustomException(userMsg);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public async Task<VmUser> GetUserInfoByUserName(VmUser _objUser)
        {
            var conn = Utility.Utility.GetConnection();
            try
            {
                VmUser result = new VmUser();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("UserId", _objUser.UserId);
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "UserInfo_Get";
                result = await conn.QueryFirstOrDefaultAsync<VmUser>(query, parameters, commandType: CommandType.StoredProcedure);
                string errorCode = parameters.Get<string>("ErrCode");
                string userMsg = parameters.Get<string>("UserMsg");
                if (errorCode != null && errorCode != "00")
                {
                    throw new CustomException(userMsg);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        // By using this procedure, we can do User profile update and user password change 
        public async Task<bool> UpdateUserInfo(VmUser _objUser)
        {
            var conn = Utility.Utility.GetConnection();
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                var parameters = new DynamicParameters();
                parameters.Add("UserId", _objUser.UserId);
                parameters.Add("FullName", _objUser.FullName);
                parameters.Add("Status", null);
                parameters.Add("ImagePath", _objUser.ImagePath);
                parameters.Add("Password", null);
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);

                string query = "UserInfo_Edit";
                var result = await conn.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
                string errorCode = parameters.Get<string>("ErrCode");
                string userMsg = parameters.Get<string>("UserMsg");
                if (errorCode != null && errorCode != "00")
                {
                    throw new CustomException(userMsg);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        internal async Task<List<VmUser>> GetUserList(Guid UserId)
        {
            var conn = Utility.Utility.GetConnection();
            try
            {
                VmUser result = new VmUser();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("UserId", UserId);
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "UserList_Get";
                List<VmUser> packageList = (await conn.QueryAsync<VmUser>(query, parameters, commandType: CommandType.StoredProcedure)).ToList();
                string errorCode = parameters.Get<string>("ErrCode");
                string userMsg = parameters.Get<string>("UserMsg");
                if (errorCode != null && errorCode != "00")
                {
                    throw new CustomException(userMsg);
                }
                return packageList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        internal async Task<List<VMUserTeam>> GetUserTeam(Guid UserId)
        {
            var conn = Utility.Utility.GetConnection();
            try
            {
                VmUser result = new VmUser();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("UserId", UserId);
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "UserTeam_Get";
                List<VMUserTeam> userTeam = (await conn.QueryAsync<VMUserTeam>(query, parameters, commandType: CommandType.StoredProcedure)).ToList();
                string errorCode = parameters.Get<string>("ErrCode");
                string userMsg = parameters.Get<string>("UserMsg");
                if (errorCode != null && errorCode != "00")
                {
                    throw new CustomException(userMsg);
                }
                return userTeam;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

    }
}
