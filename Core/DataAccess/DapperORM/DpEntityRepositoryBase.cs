using Core.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.ComponentModel;

namespace Core.DataAccess.DapperORM
{
    public class DpEntityRepositoryBase<TEntity> : IEntityRepository<TEntity>
    where TEntity : class, IEntity, new()
    {
        protected readonly string _tableName;
        private static IConfiguration _configuration;
        public DpEntityRepositoryBase()
        {
            _configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", true, true)
        .Build();
            _tableName = typeof(TEntity).Name;
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
        private IEnumerable<PropertyInfo> GetProperties => typeof(TEntity).GetProperties();
        public void Add(TEntity entity)
        {
            var insertQuery = GenerateInsertQuery();
            using (var connection = CreateConnection())
            {
                connection.Execute(insertQuery,entity);
            }
        }

        public void Delete(int id)
        {
            using (var connection = CreateConnection())
            {
                connection.Execute($"DELETE FROM {_tableName} WHERE Id=@Id", new { Id = id });
            }
        }
        public TEntity GetById(int id)
        {
            var selectQuery = GenerateSelectQuery(id);
            using (var connection = CreateConnection())
            {
                return connection.QuerySingleOrDefault<TEntity>(selectQuery);
            }
        }

        public List<TEntity> GetAll()
        {
            var selectQuery = GenerateSelectQuery(null);
            using (var connection = CreateConnection())
            {
                return connection.Query<TEntity>(selectQuery).ToList();
            }
        }
        public void Update(TEntity entity)
        {
            var updateQuery = GenerateUpdateQuery();
            using (var connection = CreateConnection())
            {
                connection.Execute(updateQuery, entity);
            }
        }
        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                    select prop.Name).ToList();
        }
        private string GenerateUpdateQuery()
        {
            var updateQuery = new StringBuilder($"UPDATE {_tableName} SET ");
            var properties = GenerateListOfProperties(GetProperties);

            properties.ForEach(property =>
            {
                if (!property.Equals("Id"))
                {
                    updateQuery.Append($"{property}=@{property},");
                }
            });

            updateQuery.Remove(updateQuery.Length - 1, 1); 
            updateQuery.Append(" WHERE Id=@Id");

            return updateQuery.ToString();
        }
        private string GenerateSelectQuery(int? id)
        {
            var updateQuery = new StringBuilder($"SELECT ");
            var properties = GenerateListOfProperties(GetProperties);

            properties.ForEach(property =>
            {
                updateQuery.Append($"{property},");
            });

            updateQuery.Remove(updateQuery.Length - 1, 1); 
            updateQuery.Append($" FROM {_tableName} WITH(NOLOCK)");
            if (id != null)
            {
                updateQuery.Append($" WHERE ID = {id}");
            }
            return updateQuery.ToString();
        }
        private string GenerateInsertQuery()
        {
            var insertQuery = new StringBuilder($"INSERT INTO {_tableName} ");

            insertQuery.Append("(");

            var properties = GenerateListOfProperties(GetProperties);
            properties.ForEach(prop =>
            {
                if (prop != "Id")
                {
                    insertQuery.Append($"[{prop}],");
                }
            });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(") VALUES (");

            properties.ForEach(prop => {
                if (prop != "Id")
                {
                    insertQuery.Append($"@{prop},");
                }
            });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(")");

            return insertQuery.ToString();
        }
    }
}
