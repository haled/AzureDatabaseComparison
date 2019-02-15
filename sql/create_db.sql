create table Addresses (
  add column Id varchar2(32),
  add column MerchantId varchar2(32),
  add column Address1 varchar2(50),
  add column Address2 varchar2(50),
  add column City varchar2(50),
  add column State varchar2(2),
  add column PostalCode varchar2(10)
)

create table Merchants (
  add column Id varchar2(32),
  add column Name varchar2(50),
  add column Ein varchar2(11),
  add column YearlyVolume money,
  add column AverageTicket money
)

create table Principals (
  add column Id varchar2(32),
  add column MerchantId varchar2(32),
  add column FirstName varchar2(50),
  add column LastName varchar2(50),
  add column SSN varchar2(11),
  add column OwnershipPercentage decimal
)

create table PricingElement (
  add column Id varchar2(32),
  add column MerchantId varchar2(32),
  add column Name varchar2(25),
  add column Rate decimal,
  add column Fee decimal
)
