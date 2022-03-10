using Dapper;
using Dos4PeopleApp.Models;
using Dos4PeopleApp.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Dos4PeopleApp.DA
{
    public class ChattingDA
    {
        internal async Task<List<VmChatting>> GetIndividualChatList(VmChatting _objVmChatting)
        {
            var conn = Utility.Utility.GetConnection();
            try
            {
                VmChatting result = new VmChatting();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("ReceiverID", _objVmChatting.ReceiverID);
                parameters.Add("SenderID", _objVmChatting.SenderID);
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "IndividualChatList_Get";
                List<VmChatting> InvChattingList = (await conn.QueryAsync<VmChatting>(query, parameters, commandType: CommandType.StoredProcedure)).ToList();
                string errorCode = parameters.Get<string>("ErrCode");
                string userMsg = parameters.Get<string>("UserMsg");
                if (errorCode != null && errorCode != "00")
                {
                    throw new CustomException(userMsg);
                }
                return InvChattingList;
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
        
        public async Task<VmReturnType> InsertIndividualChat(VmChatting _objVmChatting)
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
                parameters.Add("SenderID", _objVmChatting.SenderID);
                parameters.Add("ReceiverID", _objVmChatting.ReceiverID);
                parameters.Add("MessageBody", _objVmChatting.MessageBody);
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "IndividualChat_Add";
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
        internal async Task<List<VmUser>> GetIndividualChatUserList(Guid UserId)
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
                parameters.Add("UserID", UserId);
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "IndividualChatUserList_Get";
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

        internal async Task<List<VmChatting>> GetIndividualUnseenChatListByReceiverId(Guid ReceiverID)
        {
            var conn = Utility.Utility.GetConnection();
            try
            {
                VmChatting result = new VmChatting();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("ReceiverID", ReceiverID);               
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "IndividualUnseenChatListByReceiverID_Get";
                List<VmChatting> InvChattingList = (await conn.QueryAsync<VmChatting>(query, parameters, commandType: CommandType.StoredProcedure)).ToList();
                string errorCode = parameters.Get<string>("ErrCode");
                string userMsg = parameters.Get<string>("UserMsg");
                if (errorCode != null && errorCode != "00")
                {
                    throw new CustomException(userMsg);
                }
                return InvChattingList;
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

        internal async Task<List<VmChatting>> IndividualUnseenChatListForAdmin(Guid ReceiverID)
        {
            var conn = Utility.Utility.GetConnection();
            try
            {
                VmChatting result = new VmChatting();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("ReceiverID", ReceiverID);
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "IndividualUnseenChatListForAdmin_Get";
                List<VmChatting> InvChattingList = (await conn.QueryAsync<VmChatting>(query, parameters, commandType: CommandType.StoredProcedure)).ToList();
                string errorCode = parameters.Get<string>("ErrCode");
                string userMsg = parameters.Get<string>("UserMsg");
                if (errorCode != null && errorCode != "00")
                {
                    throw new CustomException(userMsg);
                }
                return InvChattingList;
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

        public async Task<VmReturnType> UpdateIndividualUnseenChatStatus(VmChatting objChatting)
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
                parameters.Add("ReceiverID", objChatting.ReceiverID);
                parameters.Add("SenderID", objChatting.SenderID);
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "IndividualUnseenChatStatus_Edit";
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

        internal async Task<List<VmChatting>> GetIndividualChatListForReceiver(VmChatting _objVmChatting)
        {
            var conn = Utility.Utility.GetConnection();
            try
            {
                VmChatting result = new VmChatting();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("ReceiverID", _objVmChatting.ReceiverID);
                parameters.Add("SenderID", _objVmChatting.SenderID);
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "IndividualChatListForReceiver_Get";
                List<VmChatting> InvChattingList = (await conn.QueryAsync<VmChatting>(query, parameters, commandType: CommandType.StoredProcedure)).ToList();
                string errorCode = parameters.Get<string>("ErrCode");
                string userMsg = parameters.Get<string>("UserMsg");
                if (errorCode != null && errorCode != "00")
                {
                    throw new CustomException(userMsg);
                }
                return InvChattingList;
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
