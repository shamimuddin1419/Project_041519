﻿using Dapper;
using Dos4PeopleApp.Models;
using Dos4PeopleApp.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Dos4PeopleApp.DA
{
    public class WithdrawalDA
    {
        internal async Task<VmReturnType> WithdrawalRequest(VmWithdrawal objVmWithdrawal)
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
                parameters.Add("UserId", objVmWithdrawal.UserId);
                parameters.Add("WithdrawBalanceType", objVmWithdrawal.WithdrawBalanceType.Trim());
                parameters.Add("PaymentMethodTypeId", objVmWithdrawal.PaymentMethod.Trim());
                parameters.Add("PaymentDetails", objVmWithdrawal.WalletAddress.Trim());
                parameters.Add("WithdrawRequestBalance", objVmWithdrawal.WithdrawAmount);
                parameters.Add("Remarks", objVmWithdrawal.Remark.Trim());
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "WithdrawRequest_Add";
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

        internal async Task<List<VmWithdrawal>> GetWithdrawalListByUserId(Guid UserId)
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
                string query = "Withdraw_Get";
                List<VmWithdrawal> withdrawalList = (await conn.QueryAsync<VmWithdrawal>(query, parameters, commandType: CommandType.StoredProcedure)).ToList();
                string errorCode = parameters.Get<string>("ErrCode");
                string userMsg = parameters.Get<string>("UserMsg");
                if (errorCode != null && errorCode != "00")
                {
                    throw new CustomException(userMsg);
                }
                return withdrawalList;
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
