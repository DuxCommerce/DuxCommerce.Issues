using DuxCommerce.Storefront.Views.CustomerField.ViewModels;
using DuxCommerce.Storefront.Views.Shared.ViewModels;
using DuxCommerce.Storefront.Views.SharedOption.ViewModels;

namespace DuxCommerce.Storefront.Views.ProductOption.ViewModels;

public class PrivateOptionVm : SharedOptionVm
{
    public string ProductId { get; set; }
}