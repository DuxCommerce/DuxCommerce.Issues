using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuxCommerce.OrchardCore.Catalog.Products;
using DuxCommerce.StoreBuilder.Catalog.Core;
using DuxCommerce.StoreBuilder.Catalog.DataStores;
using DuxCommerce.StoreBuilder.Catalog.DataTypes;
using DuxCommerce.StoreBuilder.Catalog.DomainTypes;
using DuxCommerce.StoreBuilder.Catalog.Requests;
using DuxCommerce.StoreBuilder.Catalog.UseCases;
using DuxCommerce.Storefront.Views.AdminProduct.ViewModels;
using DuxCommerce.Storefront.Views.CustomerField.ViewModels;
using DuxCommerce.Storefront.Views.ProductOption.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrchardCore.ContentManagement;

namespace DuxCommerce.Storefront.Views.CustomerField.VmBuilders;

public class CustomerFieldsBuilder(
    IProductStore productStore,
    ICustomerFieldsStore customerFieldsStore,
    CustomerFieldsUseCases customerFieldsUseCases)
{
    public async Task<CustomerFieldsVm> BuildIndexModel(string productId)
    {
        var fields = await GetAllFields(productId);

        var productItem = await productStore.GetItem<ContentItem>(productId);

        return new CustomerFieldsVm
        {
            Product = productItem.As<ProductPart>().Row,
            Fields = fields,
            Links = new ProductLinksVm { ContentItem = productItem, FieldsLink = true }
        };
    }

    public PrivateFieldVm BuildCreateModel(string productId)
    {
        return new PrivateFieldVm
        {
            ProductId = productId,
            Field = new FieldModel(),
            FieldTypes = GetFieldTypes()
        };
    }

    public PrivateFieldVm BuildCreateModel(PrivateFieldVm model)
    {
        model.FieldTypes = GetFieldTypes();

        return model;
    }

    public async Task<OptionFieldVm> BuildOptionModel(string productId, string fieldId)
    {
        var fieldsRow = await customerFieldsStore.GetByProductId(productId);
        var fieldRow = fieldsRow.PrivateFields.Single(x => x.Id == fieldId);

        var choices = (fieldRow.Option?.Choices ?? [])
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.CreatedAtUtc);

        return new OptionFieldVm
        {
            ProductId = fieldsRow.ProductId,
            Field = ToFieldModel(fieldRow),
            Choices = choices,
            FieldTypes = GetFieldTypes()
        };
    }

    private async Task<IEnumerable<CustomerFieldVm>> GetAllFields(string productId)
    {
        var customerFields = await customerFieldsStore.GetByProductId(productId);
        // var sharedFields = await customerFieldsUseCases.GetSharedFields(customerFields);
        var sharedFields = new List<CustomFieldRow>();
        
        var sharedFieldsVms = CustomerFieldsCore.getSharedFields(customerFields, sharedFields)
            .Select(x => new CustomerFieldVm { Field = x, Shared = true });

        var privateFieldVms = customerFields.PrivateFields
            .Select(x => new CustomerFieldVm { Field = x });

        return sharedFieldsVms.Concat(privateFieldVms).OrderBy(x => x.Field.DisplayOrder);        
    }

    public async Task<PrivateFieldVm> BuildEditModel(string productId, string fieldId)
    {
        throw new System.NotImplementedException();
    }

    private FieldModel ToFieldModel(CustomFieldRow fieldRow)
    {
        return new FieldModel
        {
            FieldId = fieldRow.Id,
            FieldName = fieldRow.FieldName,
            DisplayName = fieldRow.DisplayName,
            FieldType = fieldRow.FieldType,
            IsRequired = fieldRow.IsRequired,
            DisplayOrder = fieldRow.DisplayOrder
        };
    }

    private IEnumerable<SelectListItem> GetFieldTypes()
    {
        return new List<SelectListItem>
        {
            new("Option Field", nameof(CustomField.OptionField)),
            new("Checkbox Field", nameof(CustomField.CheckboxField)),
        };
    }
}