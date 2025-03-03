using DuxCommerce.OrchardCore.Shared;
using DuxCommerce.StoreBuilder.Catalog.DataTypes;
using YesSql.Indexes;

namespace DuxCommerce.OrchardCore.Catalog.CustomerFields;

public class CustomerFieldsIndex(string rowId, string productId) : DuxIndex
{
    public sealed override string RowId { get; set; } = rowId;
    public string ProductId { get; set; } = productId;
}

public class CustomerFieldListIndexProvider : IndexProvider<CustomerFieldsPart>
{
    public override void Describe(DescribeContext<CustomerFieldsPart> context)
    {
        context.For<CustomerFieldsIndex>()
            .Map(x =>
            {
                var row = x.Row;

                return new CustomerFieldsIndex(row.Id, row.ProductId);
            });
    }
}