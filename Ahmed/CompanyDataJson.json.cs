using Starcounter;

namespace Ahmed
{
    partial class CompanyDataJson : Json, IBound<Company>
    {
        static CompanyDataJson()
        {
            DefaultTemplate.Company.Address.InstanceType = typeof(Addressjson);
            DefaultTemplate.Sale.Address.InstanceType = typeof(Addressjson);
            DefaultTemplate.Sale.Transaction.InstanceType = typeof(TransactionJson);
            DefaultTemplate.Sales.ElementType.InstanceType = typeof(SaleJson);
        }
        void Handle(Input.SaveTrigger action)
        {
           // this.Company.Address.Data = this.Company.Address;
            Transaction.Commit();
        }
        void Handle(Input.NewSaleTrigger action)
        {
            new Sale()
            {
                Company = this.Company.Data as Company,
                Address = new Address
                {
                    Street = this.Sale.Address.Street,
                    Number = this.Sale.Address.Number,
                    Zip = this.Sale.Address.Zip,
                    City = this.Sale.Address.City,
                    Country = this.Sale.Address.Country
                },
                Transaction = new Transaction
                {
                    Date = this.Sale.Transaction.Date,
                    Commission = (double)this.Sale.Transaction.Commission,
                    SalesPrice = (double)this.Sale.Transaction.SalesPrice
                }
            };
            Transaction.Commit();
        }
    }
}
