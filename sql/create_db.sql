create database Merchant;

use Merchant;

create table Addresses (
   Id varchar(36),
   MerchantId varchar(36),
   Address1 varchar(50),
   Address2 varchar(50),
   City varchar(50),
   State varchar(2),
   PostalCode varchar(10)
);

create table Merchants (
   Id varchar(36),
   Name varchar(50),
   Ein varchar(11),
   YearlyVolume money,
   AverageTicket money
);

create table Principals (
   Id varchar(36),
   MerchantId varchar(36),
   FirstName varchar(50),
   LastName varchar(50),
   SSN varchar(11),
   OwnershipPercentage decimal
);

create table PricingElement (
   Id varchar(36),
   MerchantId varchar(36),
   Name varchar(25),
   Rate decimal,
   Fee decimal
);
