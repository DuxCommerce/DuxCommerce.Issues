using DuxCommerce.OrchardCore.Shared;
using DuxCommerce.StoreBuilder.Catalog.DataTypes;

namespace DuxCommerce.OrchardCore.Catalog.ProductOptions;

public class ProductOptionsPart : DuxPart<ProductOptionsRow>
{
    public sealed override ProductOptionsRow Row { get; set; } = new();
}