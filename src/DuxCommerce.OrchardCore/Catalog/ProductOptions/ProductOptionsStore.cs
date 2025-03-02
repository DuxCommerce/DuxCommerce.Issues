using DuxCommerce.OrchardCore.Shared;
using DuxCommerce.StoreBuilder.Catalog.DataStores;
using DuxCommerce.StoreBuilder.Catalog.DataTypes;
using DuxCommerce.StoreBuilder.Catalog.Dto;
using YesSql;
using IIdGenerator = OrchardCore.Entities.IIdGenerator;

namespace DuxCommerce.OrchardCore.Catalog.ProductOptions;

public class ProductOptionsStore(ISession session, IIdGenerator generator)
    : PartStore(session, generator), IProductOptionsStore
{
    public async Task<string> CreateOrUpdate(ProductOptionsRow row)
    {
        PopulateIdsIf(row);

        if (string.IsNullOrEmpty(row.Id))
            return await Create<ProductOptionsPart, ProductOptionsRow>(row);

        await Update<ProductOptionsPart, ProductOptionsRow, ProductOptionsIndex>(row);

        return row.Id;
    }

    public async Task<ProductOptionsRow> Get(string id)
    {
        return await base.Get<ProductOptionsPart, ProductOptionsRow, ProductOptionsIndex>(id);
    }

    public async Task<ProductOptionsRow> GetByProductId(string productId)
    {
        var part = await Session
            .Query<ProductOptionsPart, ProductOptionsIndex>(index => index.ProductId == productId)
            .FirstOrDefaultAsync();

        if (part == null)
            return ProductOptionsDto.create(productId);

        return part.Row;
    }

    public async Task<int> GetLinkedProductCount(string optionId)
    {
        return await Session
            .QueryIndex<LinkedOptionsIndex>(index => index.OptionId == optionId)
            .CountAsync();
    }

    public async Task<IEnumerable<string>> GetLinkedProductIds(string optionId)
    {
        var indexes = await Session
            .QueryIndex<LinkedOptionsIndex>(index => index.OptionId == optionId)
            .ListAsync();

        return indexes.Select(x => x.ProductId);
    }

    public async Task<bool> Delete(string optionId)
    {
        return await base.Delete<ProductOptionsPart, ProductOptionsRow, ProductOptionsIndex>(optionId);
    }

    private void PopulateIdsIf(ProductOptionsRow row)
    {
        var options = row.PrivateOptions.Where(x => string.IsNullOrEmpty(x.Id));

        foreach (var option in options)
        {
            if (string.IsNullOrEmpty(option.Id) )
                option.Id = IdGenerator.GenerateUniqueId();
        }

        var choices = row.PrivateOptions.SelectMany(x => x.Choices);

        foreach (var choice in choices)
        {
            if (string.IsNullOrEmpty(choice.Id))
                choice.Id = IdGenerator.GenerateUniqueId();
        }
    }
}