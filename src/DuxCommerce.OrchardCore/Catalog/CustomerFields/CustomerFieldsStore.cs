using DuxCommerce.OrchardCore.Shared;
using DuxCommerce.StoreBuilder.Catalog.DataStores;
using DuxCommerce.StoreBuilder.Catalog.DataTypes;
using DuxCommerce.StoreBuilder.Catalog.Dto;
using YesSql;
using YesSql.Services;
using IIdGenerator = OrchardCore.Entities.IIdGenerator;

namespace DuxCommerce.OrchardCore.Catalog.CustomerFields;

public class CustomerFieldsStore(ISession session, IIdGenerator generator)
    : PartStore(session, generator), ICustomerFieldsStore
{
    public async Task<string> CreateOrUpdate(CustomerFieldsRow row)
    {
        PopulateIdsIf(row);
        
        if (string.IsNullOrEmpty(row.Id))
            return await Create<CustomerFieldsPart, CustomerFieldsRow>(row);

        await Update<CustomerFieldsPart, CustomerFieldsRow, CustomerFieldsIndex>(row);

        return row.Id;
    }

    public async Task<CustomerFieldsRow> Get(string id)
    {
        return await base.Get<CustomerFieldsPart, CustomerFieldsRow, CustomerFieldsIndex>(id);    
    }

    public async Task<CustomerFieldsRow> GetByProductId(string productId)
    {
        var part = await Session
            .Query<CustomerFieldsPart, CustomerFieldsIndex>(index => index.ProductId == productId)
            .FirstOrDefaultAsync();

        if (part == null)
            return CustomerFieldsDto.create(productId);

        return part.Row;
    }

    public async Task<IEnumerable<CustomerFieldsRow>> GetByProductIds(IEnumerable<string> productIds)
    {
        var parts = await Session
            .Query<CustomerFieldsPart, CustomerFieldsIndex>(index => index.ProductId.IsIn(productIds))
            .ListAsync();

        return parts.Select(x => x.Row);
    }

    private void PopulateIdsIf(CustomerFieldsRow row)
    {
        var fields = row.PrivateFields.Where(x => string.IsNullOrEmpty(x.Id));
        fields.UpdateId(IdGenerator);

        var dropdownChoices = row.PrivateFields.Select(x => x.DropDownList).Where(x => x != null);
        dropdownChoices.UpdateId(IdGenerator);

        var radioChoices = row.PrivateFields.Select(x => x.RadioGroup).Where(x => x != null);
        radioChoices.UpdateId(IdGenerator);
    }
}