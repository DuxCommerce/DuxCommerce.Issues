using System.Threading.Tasks;
using DuxCommerce.OrchardCore.Catalog.Products;
using DuxCommerce.StoreBuilder.Catalog.DataStores;
using DuxCommerce.StoreBuilder.Catalog.Requests;
using DuxCommerce.Storefront.Views.AdminProduct.ViewModels;
using DuxCommerce.Storefront.Views.CustomerField.ViewModels;
using OrchardCore.ContentManagement;

namespace DuxCommerce.Storefront.Views.CustomerField.VmBuilders;

public class CustomerFieldsBuilder(IProductStore productStore)
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
            FieldTypes = FieldType.GetAll()
        };
    }

    public PrivateFieldVm BuildCreateModel(PrivateFieldVm model)
    {
        model.FieldTypes = FieldType.GetAll();

        return model;
    }

    public async Task<string> BuildEditModel(string productId, string fieldId)
    {
        throw new System.NotImplementedException();
    }
}