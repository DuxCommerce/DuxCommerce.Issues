using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuxCommerce.OrchardCore.Catalog.Products;
using DuxCommerce.StoreBuilder.Catalog.DataStores;
using DuxCommerce.StoreBuilder.Catalog.DataTypes;
using DuxCommerce.StoreBuilder.Catalog.DomainTypes;
using DuxCommerce.StoreBuilder.Catalog.Requests;
using DuxCommerce.Storefront.Views.AdminProduct.ViewModels;
using DuxCommerce.Storefront.Views.CustomerField.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using OrchardCore.ContentManagement;

namespace DuxCommerce.Storefront.Views.CustomerField.VmBuilders;

public class CustomerFieldsBuilder(IProductStore productStore, ICustomerFieldsStore customerFieldsStore)
{
    public async Task<CustomerFieldsVm> BuildIndexModel(string productId)
    {
        var productItem = await productStore.GetItem<ContentItem>(productId);

        return new CustomerFieldsVm
        {
            Product = productItem.As<ProductPart>().Row,
            Fields = [],
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