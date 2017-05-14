using Starcounter;

namespace Ahmed
{
    partial class SaleJson : Json
    {
        static SaleJson()
        {
            DefaultTemplate.Address.InstanceType = typeof(Addressjson);
            DefaultTemplate.Transaction.InstanceType = typeof(TransactionJson);

        }
    }
}
