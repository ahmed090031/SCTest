using System;
using Starcounter;

namespace Ahmed
{
    class Program
    {
        static void Main()
        {
            Application.Current.Use(new HtmlFromJsonProvider());
            Application.Current.Use(new PartialToStandaloneHtmlProvider());

            Handle.GET("/Ahmed", () =>
            {
                var agency = (AgencyJson)Self.GET("/Ahmed/agency");
                agency.Html = "/Ahmed/AgencyJson.html";
                return agency;
            });

            Handle.GET("/Ahmed/company/{?}", (string id) =>
            {
                return Db.Scope(() =>
                {
                    Company Company = Db.SQL<Company>("SELECT c FROM Ahmed.Company c WHERE ObjectID=?", id).First;
                    QueryResultRows<Sale> Sales = Db.SQL<Sale>("SELECT s FROM Ahmed.Sale s WHERE s.Company.ObjectID=?", id);
                    CompanyDataJson CompanyData = (CompanyDataJson)Self.GET("/Ahmed/company/data");
                    if (Company.Address == null)
                    {
                        Company.Address = new Address
                        {
                            Street = "",
                            Number = "",
                            Zip = "",
                            City = "",
                            Country = ""
                        };
                    }
                    CompanyData.Company.Data = Company;
                    CompanyData.Sales.Data = Sales;
                    return CompanyData;
                });
            });

            Handle.GET("/Ahmed/company/data", () =>
            {
                CompanyDataJson CompanyData = new CompanyDataJson();
                CompanyData.Html = "/Ahmed/CompanyDataJson.html";
                if (Session.Current == null)
                {
                    Session.Current = new Session(SessionOptions.PatchVersioning);
                }
                CompanyData.Session = Session.Current;
                return CompanyData;

            });

            Handle.GET("/Ahmed/agency", () =>
            {
                return Db.Scope(() =>
                {
                    var agency = Db.SQL<Agency>("SELECT a FROM Agency a").First;
                    if (agency == null)
                    {
                        agency = new Agency
                        {
                            Name = ""
                        };
                    }
                    
                    var json = new AgencyJson
                    {
                        Data = agency
                    };

                    if (Session.Current == null)
                    {
                        Session.Current = new Session(SessionOptions.PatchVersioning);
                    }
                    json.Session = Session.Current;
                    return json;
                });
            });
        }
    }
    [Database]
    public class Agency
    {
        public string Name;
        public QueryResultRows<Company> Companies => Db.SQL<Company>("SELECT c FROM Ahmed.Company c WHERE c.Agency = ?", this);
    }
    [Database]
    public class Address
    {
        public string Street;
        public string Number;
        public string Zip;
        public string City;
        public string Country;
    }
    [Database]
    public class Transaction
    {
        public string Date;
        public double SalesPrice;
        public double Commission;
    }
    [Database]
    public class Sale
    {
        public Company Company;
        public Address Address;
        public Transaction Transaction;
    }
    [Database]
    public class Company
    {
        public Agency Agency;
        public string Name;
        public Address Address;
        public Int64 TotalSales => Db.SQL<Int64>("SELECT COUNT(s) FROM Ahmed.Sale s WHERE s.Company = ? ", this).First;
        public QueryResultRows<Sale> Sales => Db.SQL<Sale>("SELECT s FROM Ahmed.Sale s WHERE s.Company = ?", this);
        public double TotalCommission => Db.SQL<double>("SELECT SUM(s.Transaction.Commission) FROM Ahmed.Sale s WHERE s.Company = ?", this).First;
        public double AverageCommission => Db.SQL<double>("SELECT AVG(s.Transaction.Commission) FROM Ahmed.Sale s WHERE s.Company = ?", this).First;
        public int Trend;


    }
}