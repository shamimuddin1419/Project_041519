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
    public class PackageDA
    {
        internal async Task<List<VmPackageCategory>> GetPackageCategoryList()
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
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "PackageCategory_Get";
                List<VmPackageCategory> packageCategoryList = (await conn.QueryAsync<VmPackageCategory>(query, parameters, commandType: CommandType.StoredProcedure)).ToList();
                string errorCode = parameters.Get<string>("ErrCode");
                string userMsg = parameters.Get<string>("UserMsg");
                if (errorCode != null && errorCode != "00")
                {
                    throw new CustomException(userMsg);
                }
                return packageCategoryList;
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

        internal async Task<VmUserPackageRequest> GetUserPackageRequestById(int id)
        {
            var conn = Utility.Utility.GetConnection();
            try
            {
                VmUserPackageRequest result = new VmUserPackageRequest();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("UserPackageRequestId", id);               
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "UserPackageRequestById_Get";
                result = await conn.QueryFirstOrDefaultAsync<VmUserPackageRequest>(query, parameters, commandType: CommandType.StoredProcedure);
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

        internal async Task<VmReturnType> RequestForPaymentAccept(VmUserPackageRequest objVmPackageReq)
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
                parameters.Add("UserId", objVmPackageReq.UserId);
                parameters.Add("PackageId", objVmPackageReq.PackageId);             
                parameters.Add("PaymentMethodId", objVmPackageReq.PaymentMethodId);
                parameters.Add("Reference", objVmPackageReq.Reference);
                parameters.Add("Remarks", objVmPackageReq.Remarks);
                parameters.Add("Amount", objVmPackageReq.Amount);
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "UserPackageRequest_Add";
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

        internal async Task<List<VmPackage>> GetPackageList()
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
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "Package_Get";
                List<VmPackage> packageList = (await conn.QueryAsync<VmPackage>(query, parameters, commandType: CommandType.StoredProcedure)).ToList();
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
        public async Task<VmReturnType> InsertPackage(VmPackage _objPackage)
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
                parameters.Add("PackageCategoryId", _objPackage.PackageCategoryId);
                parameters.Add("PackageName", _objPackage.PackageName);
                parameters.Add("PackageValue", _objPackage.PackageValue);
                parameters.Add("PackageDurationDays", _objPackage.PackageDurationDays);
                parameters.Add("PerClickValue", _objPackage.PerClickValue);
                parameters.Add("DailyTaskCount", _objPackage.DailyTaskCount);
                parameters.Add("DailyValue", _objPackage.DailyValue);
                parameters.Add("WeeklyValue", _objPackage.WeeklyValue);
                parameters.Add("MonthlyValue", _objPackage.MonthlyValue);
                parameters.Add("YearlyValue", _objPackage.YearlyValue);
                parameters.Add("ReferralEarn", _objPackage.ReferralEarn);
                parameters.Add("WorkCommission", _objPackage.WorkCommission);
                parameters.Add("PotentialReferralEarn", _objPackage.PotentialReferralEarn);
                parameters.Add("TargetPotentialYearlyIncome", _objPackage.TargetPotentialYearlyIncome);
                parameters.Add("PotentialYearlyIncome", _objPackage.PotentialYearlyIncome);
                parameters.Add("TCBOnMainInvestPer", _objPackage.TCBOnMainInvestPer);
                parameters.Add("Remarks", _objPackage.Remarks);
                parameters.Add("IsActive", _objPackage.IsActive);
                parameters.Add("IsPublished", _objPackage.IsPublished);
                parameters.Add("CreatedBy", _objPackage.CreatedBy);
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "Package_Add";
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
        public async Task<VmReturnType> UpdatePackage(VmPackage _objPackage)
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
                parameters.Add("PackageId", _objPackage.PackageId);
                parameters.Add("IsActive", _objPackage.IsActive);
                parameters.Add("IsPublished", 1);
                parameters.Add("CreatedBy", _objPackage.CreatedBy);                
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "Package_Edit";
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
        public async Task<VmReturnType> DeletePackage(VmPackage _objPackage)
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
                parameters.Add("PackageId", _objPackage.PackageId); 
                parameters.Add("CreatedBy", _objPackage.CreatedBy);
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "Package_delete";
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
        
            internal async Task<List<VmPaymentMethod>> GetPaymentMethodList()
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
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "PaymentMethodByTypeId_Get";
                List<VmPaymentMethod> paymentMethodTypes = (await conn.QueryAsync<VmPaymentMethod>(query, parameters, commandType: CommandType.StoredProcedure)).ToList();
                string errorCode = parameters.Get<string>("ErrCode");
                string userMsg = parameters.Get<string>("UserMsg");
                if (errorCode != null && errorCode != "00")
                {
                    throw new CustomException(userMsg);
                }
                return paymentMethodTypes;
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
        internal async Task<List<VmPaymentMethodType>> GetPaymentMethodTypeList()
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
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "PaymentMethodType_Get";
                List<VmPaymentMethodType> paymentMethodTypes = (await conn.QueryAsync<VmPaymentMethodType>(query, parameters, commandType: CommandType.StoredProcedure)).ToList();
                string errorCode = parameters.Get<string>("ErrCode");
                string userMsg = parameters.Get<string>("UserMsg");
                if (errorCode != null && errorCode != "00")
                {
                    throw new CustomException(userMsg);
                }
                return paymentMethodTypes;
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

        internal async Task<List<VmUserPackageRequest>> GetUserPackageRequestPendingList()
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
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "UserPackageRequestPending_Get";
                List<VmUserPackageRequest> packageList = (await conn.QueryAsync<VmUserPackageRequest>(query, parameters, commandType: CommandType.StoredProcedure)).ToList();
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

        public async Task<VmReturnType> UserPackageRequestApprove(VmUserPackageRequest _objPackageRequest)
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
                parameters.Add("UserPackageRequestId", _objPackageRequest.UserPackageRequestId);
                parameters.Add("CreatedBy", _objPackageRequest.ApprovedBy);
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "UserPackageRequest_Approve";
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
        public async Task<VmReturnType> UserPackageRequestReject(VmUserPackageRequest _objPackageRequest)
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
                parameters.Add("UserPackageRequestId", _objPackageRequest.UserPackageRequestId);
                parameters.Add("CreatedBy", _objPackageRequest.RejectBy);
                parameters.Add("ErrCode", null, DbType.String, ParameterDirection.Output, 2);
                parameters.Add("UserMsg", null, DbType.String, ParameterDirection.Output, 200);
                string query = "UserPackageRequest_Reject";
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
    }
}
