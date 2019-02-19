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

        public Merchant CreateMerchant(Merchant merch)
        {
            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var merchantSql = "INSERT INTO Merchants (Id, Name, Ein, YearlyVolume, AverageTicket) VALUES (@Id, @Name, @Ein, @YearlyVolume, @AverageTicket)";

                conn.Execute(merchantSql, merch);

                var addressSql = "INSERT INTO Addresses (Id, MerchantId, Address1, Address2, City, State, PostalCode) VALUES (@Merchant.BusinessAddress.Id, @Merchant.Id, @Merchant.BusinessAddress.Address1, @Merchant.BusinessAddress.Address2, @Merchant.BusinessAddress.City, @Merchant.BusinessAddress.State, @Merchant.BusinessAddress.PostalCode)";
                
                merch.BusinessAddress.Id = Guid.NewGuid().ToString();
                conn.Execute(addressSql, merch);

                merch.MailingAddress.Id = Guid.NewGuid().ToString();
                conn.Execute(addressSql, merch);

                var principalSql = "INSERT INTO Principals (Id, MerchantId, FirstName, LastName, SSN, OwnershipPerentage) VALUES (@Id, @Merchant.Id, @FirstName, @LastName, @SSN, @OwnershipPercentage)";
                foreach(var principal in merch.Principals)
                {
                    principal.Id = Guid.NewGuid().ToString();
                    conn.Execute(principalSql, principal);
                }

                var pricingSql = "INSERT INTO PricingElement (Id, MerchantId, Name, Rate, Fee) VALUES (@Id, @Merchant.Id, @Name, @Rate, @Fee)";
                foreach(var pricing in merch.PricingPlan)
                {
                    pricing.Id = Guid.NewGuid().ToString();
                    conn.Execute(pricingSql, pricing);
                }
                
                conn.Close();
            }
            return merch;
        }

        
    }
}
