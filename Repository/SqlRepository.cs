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
                
                var addressSql = "INSERT INTO Addresses (Id, MerchantId, Address1, Address2, City, State, PostalCode) VALUES (@Id, @MerchantId, @Address1, @Address2, @City, @State, @PostalCode)";
                
                merch.BusinessAddress.Id = Guid.NewGuid().ToString();
                var businessAddressParams = new
                    {
                        Id = merch.BusinessAddress.Id,
                        MerchantId = merch.Id,
                        merch.BusinessAddress.Address1,
                        merch.BusinessAddress.Address2,
                        merch.BusinessAddress.City,
                        merch.BusinessAddress.State,
                        merch.BusinessAddress.PostalCode
                    };
                conn.Execute(addressSql, businessAddressParams);
                
                merch.MailingAddress.Id = Guid.NewGuid().ToString();
                var mailingAddressParams = new
                    {
                        Id = merch.MailingAddress.Id,
                        MerchantId = merch.Id,
                        merch.MailingAddress.Address1,
                        merch.MailingAddress.Address2,
                        merch.MailingAddress.City,
                        merch.MailingAddress.State,
                        merch.MailingAddress.PostalCode
                    };
                conn.Execute(addressSql, mailingAddressParams);
                
                var principalSql = "INSERT INTO Principals (Id, MerchantId, FirstName, LastName, SSN, OwnershipPercentage) VALUES (@Id, @MerchantId, @FirstName, @LastName, @SSN, @OwnershipPercentage)";
                foreach(var principal in merch.Principals)
                {
                    principal.Id = Guid.NewGuid().ToString();
                    var principalParams = new
                        {
                            Id = principal.Id,
                            MerchantId = merch.Id,
                            principal.FirstName,
                            principal.LastName,
                            principal.SSN,
                            principal.OwnershipPercentage
                        };
                    conn.Execute(principalSql, principalParams);
                }

                var pricingSql = "INSERT INTO PricingElement (Id, MerchantId, Name, Rate, Fee) VALUES (@Id, @MerchantId, @Name, @Rate, @Fee)";
                foreach(var pricing in merch.PricingPlan)
                {
                    pricing.Id = Guid.NewGuid().ToString();
                    var pricingParams = new
                        {
                            Id = pricing.Id,
                            MerchantId = merch.Id,
                            pricing.Name,
                            pricing.Rate,
                            pricing.Fee
                        };
                    conn.Execute(pricingSql, pricingParams);
                }
                
                conn.Close();
            }
            return merch;
        }

        
    }
}
