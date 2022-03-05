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
    public class TransactionDA
    {
        internal async Task<List<VMIncomeHistory>> GetIncomeHistoryByUser(Guid userId, DateTime? FromDate,DateTime? ToDate)
        {
            var conn = Utility.Utility.GetConnection();
            List<VMIncomeHistory> result = new List<VMIncomeHistory>();
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("UserId", userId,DbType.Guid);
                parameters.Add("FromDate", FromDate,DbType.DateTime);
                parameters.Add("ToDate", ToDate, DbType.DateTime);
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "IncomeHist_Get";
                result = (await conn.QueryAsync<VMIncomeHistory>(query, parameters, commandType: CommandType.StoredProcedure)).ToList();
                string errorCode = parameters.Get<string>("ErrCode");
                string userMsg = parameters.Get<string>("UserMsg");
                if (errorCode != null && errorCode != "00")
                {
                    throw new CustomException(userMsg);
                }
                return result ?? new List<VMIncomeHistory>();
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
