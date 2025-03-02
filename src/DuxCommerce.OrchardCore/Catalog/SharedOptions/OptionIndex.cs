using DuxCommerce.OrchardCore.Shared;
using DuxCommerce.StoreBuilder.Catalog.DataTypes;
using YesSql.Indexes;

namespace DuxCommerce.OrchardCore.Catalog.SharedOptions;

public class OptionIndex(string rowId) : DuxIndex
{
    // Note: required by QueryIndex
    public OptionIndex() : this(string.Empty)
    {
    }

    public sealed override string RowId { get; set; } = rowId;
}

public class OptionIndexProvider : IndexProvider<OptionPart>
{
    public override void Describe(DescribeContext<OptionPart> context)
    {
        context.For<OptionIndex>()
            .Map(x =>
            {
                var optionRow = x.Row;

                return new OptionIndex(optionRow.Id);
            });
    }
}