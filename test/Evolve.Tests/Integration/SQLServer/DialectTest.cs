﻿using System.Data.SqlClient;
using EvolveDb.Connection;
using EvolveDb.Dialect;
using EvolveDb.Dialect.SQLServer;
using EvolveDb.Tests.Infrastructure;
using Xunit;
using static EvolveDb.Tests.TestContext;

namespace EvolveDb.Tests.Integration.SQLServer
{
    [Collection("SQLServer collection")]
    public class DialectTest
    {
        public const string DbName = "my_database_1";
        private readonly SQLServerFixture _dbContainer;

        public DialectTest(SQLServerFixture dbContainer)
        {
            _dbContainer = dbContainer;

            if (Local)
            {
                dbContainer.Run(fromScratch: true);
            }

            TestUtil.CreateSqlServerDatabase(DbName, _dbContainer.GetCnxStr("master"));
        }

        [Fact]
        [Category(Test.SQLServer)]
        public void Run_all_SQLServer_integration_tests_work()
        {
            // Arrange
            var cnn = new SqlConnection(_dbContainer.GetCnxStr(DbName));
            var wcnn = new WrappedConnection(cnn).AssertDatabaseServerType(DBMS.SQLServer);
            var db = DatabaseHelperFactory.GetDatabaseHelper(DBMS.SQLServer, wcnn);
            string schemaName = "dbo";
            var schema = new SQLServerSchema(schemaName, wcnn);

            // Assert
            db.AssertDefaultSchemaName(schemaName)
              .AssertApplicationLock(new SqlConnection(_dbContainer.GetCnxStr(DbName)))
              .AssertMetadataTableCreation(schemaName, "changelog")
              .AssertMetadataTableLock()
              .AssertSchemaIsErasableWhenEmptySchemaFound(schemaName) // id:1
              .AssertVersionedMigrationSave() // id:2
              .AssertVersionedMigrationChecksumUpdate()
              .AssertRepeatableMigrationSave(); // id:3

            schema.AssertIsNotEmpty();
            schema.Erase();
            schema.AssertIsEmpty();

            db.AssertCloseConnection();
        }
    }
}
