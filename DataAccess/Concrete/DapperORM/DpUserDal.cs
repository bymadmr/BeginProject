using Core.DataAccess.DapperORM;
using Core.Entities.Concrete;
using Dapper;
using DataAccess.Abstract;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.DapperORM
{
    public class DpUserDal : DpEntityRepositoryBase<EUser>, IUserDal
    {
        private static IConfiguration _configuration;
        public DpUserDal()
        {
            _configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", true, true)
        .Build();
        }
        private SqlConnection SqlConnection()
        {
            return new SqlConnection(_configuration["appSettings:SqlCon"]);
        }
        private IDbConnection CreateConnection()
        {
            var conn = SqlConnection();
            conn.Open();
            return conn;
        }
        public List<OperationClaim> GetClaims(EUser user)
        {
            using (var connection = CreateConnection())
            {
                return connection.Query<OperationClaim>("SELECT OC.Id,OC.Name FROM OperationClaim OC WITH(NOLOCK) INNER JOIN UserOperationClaim UOC WITH(NOLOCK) ON OC.Id=UOC.OperationClaimId WHERE UOC.UserId="+user.Id+"").ToList();
            }
        }
    }
}
