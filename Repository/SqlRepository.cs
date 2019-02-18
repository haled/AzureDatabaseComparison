using System;
using System.Data.SqlClient;
using Dapper;

namespace AzureDatabaseComparison
{
    public class SqlRepository
    {
        private string _connectionString;
        
        public SqlRepository(string connString)
        {
            _connectionString = connString;
        }

        public bool CreateMerchant(Merchant merch)
        {
            using(SqlConnnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var merchantSql = "INSERT INTO Merchants (Id, Name, Ein, YearlyVolume, AverageTicket) VALUES (@Id, @Name, @Ein, @YearlyVolume, @AverageTicket)";

                conn.Execute(merchantSql, merch);

                var addressSql = "INSERT INTO Addresses (Id, MerchantId, Address1, Address2, City, State, PostalCode) VALUES (@newGuid, @Merchant.Id, @Address1, @Address2, @City, @State, @PostalCode)";
                
                var newGuid = Guid.NewGuid();
                conn.Execute(addressSql, newGuid, merch, merch.BusinessAddress);

                newGuid = Guid.NewGuid();
                conn.Execute(addressSql, newGuid, merch, merch.MailingAddress);

                var principalSql = "INSERT INTO Principals (Id, MerchantId, FirstName, LastName, SSN, OwnershipPerentage) VALUES (@principalGuid, @Merchant.Id, @FirstName, @LastName, @SSN, @OwnershipPercentage)";
                foreach(var principal in merch.Principals)
                {
                    var principalGuid = Guid.NewGuid();
                    conn.Execute(principalSql, principalGuid, merch, principal);
                }

                
                
                conn.Close();
            }
        }

        
    }
}
