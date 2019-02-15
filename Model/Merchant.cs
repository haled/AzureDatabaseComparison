using System;
using System.Collections.Generic;

namespace AzureDatabaseComparison
{
  public class Merchant
  {
      public string Id {get; set;}
      public string Name {get; set;}
      public string Ein {get; set;}
      public decimal YearlyVolume {get; set;}
      public decimal AverageTicket {get; set;}
      public List<Principal> Principals {get; set;}
      public Address BusinessAddress {get; set;}
      public Address MailingAddress {get; set;}
      public List<PricingElement> PricingPlan {get; set;}

      public Merchant()
      {
          Principals = new List<Principal>();
          PricingPlan = new List<PricingElement>();
      }
  }
}
