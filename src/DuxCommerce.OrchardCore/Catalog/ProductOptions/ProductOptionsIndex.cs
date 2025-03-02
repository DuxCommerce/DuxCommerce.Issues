using DuxCommerce.OrchardCore.Shared;
using YesSql.Indexes;

namespace DuxCommerce.OrchardCore.Catalog.ProductOptions;

public class ProductOptionsIndex(string rowId, string productId) : DuxIndex
{
    // Note: required by QueryIndex
    public ProductOptionsIndex() : this(string.Empty, string.Empty)
    {
    }

    public sealed override string RowId { get; set; } = rowId;
    public string ProductId { get; set; } = productId;
}

public class ProductOptionsIndexProvider : IndexProvider<ProductOptionsPart>
{
    public override void Describe(DescribeContext<ProductOptionsPart> context)
    {
        context.For<ProductOptionsIndex>()
            .Map(x =>
            {
                var optionRow = x.Row;

                return new ProductOptionsIndex(optionRow.Id, optionRow.ProductId);
            });
    }
}
