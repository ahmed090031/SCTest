using Starcounter;

namespace Ahmed
{
    partial class AgencyJson : Json
    {
        static AgencyJson()
        {
            DefaultTemplate.Companies.ElementType.InstanceType = typeof(CompanyJson);
        }
       
        void Handle(Input.SaveTrigger action)
        {
            Transaction.Commit();
        }
        void Handle(Input.NewCompanyTrigger action)
        {
            new Company()
            {
                Agency = this.Data as Agency,
                Name = this.__bf__CompanyName__,
                Address = new Address()
                {
                    Street = "",
                    Number = "",
                    Zip="",
                    City = "",
                    Country = ""
                },
                Trend = 0
            };
            Transaction.Commit();
        }
    }
}
