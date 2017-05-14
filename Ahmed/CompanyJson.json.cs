using Starcounter;

namespace Ahmed
{
    partial class CompanyJson : Json
    {
        static CompanyJson()
        {
            //DefaultTemplate.TotalSales.Bind = null;   
        }
        public string GetUrl
        {
            get { return "/Ahmed/company/" + this.Data.GetObjectID(); }
        }

    }
}
