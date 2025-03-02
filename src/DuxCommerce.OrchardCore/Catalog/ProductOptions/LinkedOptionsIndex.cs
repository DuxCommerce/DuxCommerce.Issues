using DuxCommerce.OrchardCore.Shared;
using YesSql.Indexes;

namespace DuxCommerce.OrchardCore.Catalog.ProductOptions;

public class LinkedOptionsIndex(string rowId, string productId, string optionId) : DuxIndex
{
    // Note: required by QueryIndex
    public LinkedOptionsIndex() : this(string.Empty, string.Empty, string.Empty)
    {
    }

    public sealed override string RowId { get; set; } = rowId;
    public string ProductId { get; set; } = productId;
    public string OptionId { get; set; } = optionId;
}

public class LinkedOptionsIndexProvider : IndexProvider<ProductOptionsPart>
{
    public override void Describe(DescribeContext<ProductOptionsPart> context)
    {
        context.For<LinkedOptionsIndex>()
            .Map(x =>
            {
                var row = x.Row;
                var options = row.SharedOptions ?? [];

                return options
                    .Select(option => option.OptionId)
                    .Select(optionId => new LinkedOptionsIndex(row.Id, row.ProductId, optionId));
            });
    }
}