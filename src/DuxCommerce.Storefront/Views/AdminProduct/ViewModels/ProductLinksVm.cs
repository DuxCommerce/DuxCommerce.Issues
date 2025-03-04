using OrchardCore.ContentManagement;

namespace DuxCommerce.Storefront.Views.AdminProduct.ViewModels;

public class ProductLinksVm
{
    public ContentItem ContentItem { get; set; }
    public bool EditLink { get; set; }
    public bool OptionsLink { get; set; }
    public bool FieldsLink { get; set; }
}