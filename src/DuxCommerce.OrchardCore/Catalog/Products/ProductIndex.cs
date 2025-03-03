using DuxCommerce.OrchardCore.Shared;
using DuxCommerce.StoreBuilder.Catalog.Core;
using OrchardCore.ContentManagement;
using YesSql.Indexes;

namespace DuxCommerce.OrchardCore.Catalog.Products;

public class ProductIndex(
    string rowId,
    string name,
    string choiceNames,
    decimal price,
    string sku,
    bool stockEnabled,
    bool hasOptions,
    bool hasCustomerFields,
    bool isVisible,
    string parentId)
    : DuxIndex, IParent
{
    public sealed override string RowId { get; set; } = rowId;
    public string Name { get; set; } = name;
    public string ChoiceNames { get; set; } = choiceNames;
    public decimal Price { get; set; } = price;
    public string Sku { get; set; } = sku;
    public bool StockEnabled { get; set; } = stockEnabled;
    public bool HasOptions { get; set; } = hasOptions;
    public bool HasCustomerFields { get; set; } = hasCustomerFields;
    public bool IsVisible { get; set; } = isVisible;
    public string ParentId { get; set; } = parentId;
}

public class ProductIndexProvider : IndexProvider<ContentItem>
{
    public override void Describe(DescribeContext<ContentItem> context)
    {
        context.For<ProductIndex>()
            .Map(contentItem =>
            {
                if (!contentItem.Published) 
                    return null;
                
                var row = contentItem.As<ProductPart>()?.Row;

                if (row == null)
                    return null;
    
                return new ProductIndex(
                    row.Id,
                    row.Name,
                    row.ChoiceNames,
                    row.Price,
                    row.Sku,
                    row.StockEnabled,
                    row.HasOptions,
                    row.HasCustomerFields,
                    ProductCore.isVisible(row),
                    row.ParentId);
            });
    }
}