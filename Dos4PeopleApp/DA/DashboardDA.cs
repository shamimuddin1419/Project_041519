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
    public class DashboardDA
    {
        internal async Task<VMDashboardFirstCardData> GetDashboardFirstCardData(string userId)
        {
            var conn = Utility.Utility.GetConnection();
            try
            {
                VMDashboardFirstCardData result = new VMDashboardFirstCardData();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("UserId", userId, DbType.String);
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "DashboardFirstCardData_Get";
                result = (await conn.QueryAsync<VMDashboardFirstCardData>(query, parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
                string errorCode = parameters.Get<string>("ErrCode");
                string userMsg = parameters.Get<string>("UserMsg");
                if (errorCode != null && errorCode != "00")
                {
                    throw new CustomException(userMsg);
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
        internal async Task<List<VMDashboardGraphData>> GetDashboardGraphData(string userId)
        {
            var conn = Utility.Utility.GetConnection();
            try
            {
                List<VMDashboardGraphData> result = new List<VMDashboardGraphData>();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("UserId", userId, DbType.String);
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "DashboardGraphData_Get";
                result = (await conn.QueryAsync<VMDashboardGraphData>(query, parameters, commandType: CommandType.StoredProcedure)).ToList();
                string errorCode = parameters.Get<string>("ErrCode");
                string userMsg = parameters.Get<string>("UserMsg");
                if (errorCode != null && errorCode != "00")
                {
                    throw new CustomException(userMsg);
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
