using DuxCommerce.OrchardCore.Shared;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace DuxCommerce.OrchardCore.Catalog.Products;

public class ProductChoiceIndex(
    string rowId,
    string parentId,
    string choiceId) : DuxIndex
{
    public ProductChoiceIndex() : this(string.Empty, string.Empty, string.Empty)
    {
    }

    public sealed override string RowId { get; set; } = rowId;
    public string ParentId { get; set; } = parentId;
    public string ChoiceId { get; set; } = choiceId;
}

public class ProductChoiceIndexProvider : IndexProvider<ContentItem>
{
    public override void Describe(DescribeContext<ContentItem> context)
    {
        context.For<ProductChoiceIndex>()
            .Map(contentItem =>
            {
                if (!contentItem.Latest) 
                    return null;

                if (!contentItem.Published) 
                    return null;

                var row = contentItem.As<ProductPart>()?.Row;

                if (row == null)
                    return null;

                var choiceIds = row.ChoiceIds ?? [];

                return choiceIds.Select(choiceId => new ProductChoiceIndex(row.Id, row.ParentId, choiceId));
            });
    }
}