using System;
using System.Collections.Generic;

namespace AzureDatabaseComparison
{
    public static class MerchantFactory
    {
        public static Merchant CreateMerchant()
        {
            var m = new Merchant {
                Id = Guid.NewGuid().ToString(),
                Name = "New Merchant",
                Ein = "12-3456789",
                YearlyVolume = 23456.78m,
                AverageTicket = 76.54m,
                BusinessAddress = new Address {
                    Address1 = "123 Main St.",
                    Address2 = "Some Suite",
                    City = "Clayton",
                    State = "MO",
                    PostalCode = "63105"
                },
                MailingAddress = new Address {
                    Address1 = "999 Park Ave.",
                    Address2 = "",
                    City = "St. Louis",
                    State = "MO",
                    PostalCode = "63101"
                }
            };

            m.Principals.Add(new Principal {
                    FirstName = "John",
                    LastName = "Doe",
                    SSN = "987-65-4321",
                    OwnershipPercentage = 0.5m
                });
            m.Principals.Add(new Principal {
                    FirstName = "Jane",
                    LastName = "Doe",
                    SSN = "987-65-4321",
                    OwnershipPercentage = 0.5m
                });

            m.PricingPlan.Add(new PricingElement {
                    Name = "Discount",
                    Rate = 0.02m,
                    Fee = 0.10m
                });
            m.PricingPlan.Add(new PricingElement {
                    Name = "Interchange",
                    Rate = 0.02m,
                    Fee = 0.10m
                });
            m.PricingPlan.Add(new PricingElement {
                    Name = "EMF",
                    Rate = 0.02m,
                    Fee = 0.10m
                });
            m.PricingPlan.Add(new PricingElement {
                    Name = "Amex",
                    Rate = 0.02m,
                    Fee = 0.10m
                });
            m.PricingPlan.Add(new PricingElement {
                    Name = "PIN Debit",
                    Rate = 0.02m,
                    Fee = 0.10m
                });
            m.PricingPlan.Add(new PricingElement {
                    Name = "Monthly Fee",
                    Rate = 0.02m,
                    Fee = 0.10m
                });
            m.PricingPlan.Add(new PricingElement {
                    Name = "Paper Fee",
                    Rate = 0.02m,
                    Fee = 0.10m
                });
            m.PricingPlan.Add(new PricingElement {
                    Name = "Other",
                    Rate = 0.02m,
                    Fee = 0.10m
                });

            return m;
        }
    }
}
