using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace WorkSoftCase.Database
{
    public class DbInitializer
    {
         public static async Task InitializeTablesAsync(IDbConnection connection)
        {
            //TODO: 
            var createCategoryTableSql = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Categories' AND xtype='U')
                CREATE TABLE Categories (
                    Id UNIQUEIDENTIFIER PRIMARY KEY,
                    CreateDate DATETIME2 NOT NULL,
                    ModifyDate DATETIME2 NULL,
                    ModifyIP NVARCHAR(45) NULL,
                    CategoryName NVARCHAR(100) NOT NULL UNIQUE,
                    CategoryIcon NVARCHAR(255) NOT NULL,
                    CategoryModifyDate DATETIME2 NULL
                );";

            var createProductTableSql = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Products' AND xtype='U')
                CREATE TABLE Products (
                    Id UNIQUEIDENTIFIER PRIMARY KEY,
                    CreateDate DATETIME2 NOT NULL,
                    ModifyDate DATETIME2 NULL,
                    ModifyIP NVARCHAR(45) NULL,
                    ProductName NVARCHAR(100) NOT NULL,
                    ProductIcon NVARCHAR(255) NOT NULL,
                    ProductModifyDate DATETIME2 NULL,
                    CategoryId UNIQUEIDENTIFIER NOT NULL,
                    CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
                );";

            var createUserTableSql = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
                CREATE TABLE Users (
                    Id UNIQUEIDENTIFIER PRIMARY KEY,
                    CreateDate DATETIME2 NOT NULL,
                    ModifyDate DATETIME2 NULL,
                    ModifyIP NVARCHAR(45) NULL,
                    UserName NVARCHAR(100) NOT NULL,
                    FirstName NVARCHAR(100) NOT NULL,
                    LastName NVARCHAR(100) NOT NULL,
                    UserPassword NVARCHAR(255) NOT NULL,
                    PasswordModifyDate DATETIME2 NULL
                );";

            await connection.ExecuteAsync(createCategoryTableSql);
            await connection.ExecuteAsync(createProductTableSql);
            await connection.ExecuteAsync(createUserTableSql);
        }
    }
}