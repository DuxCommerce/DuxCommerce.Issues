using System.Collections.Generic;
using DuxCommerce.StoreBuilder.Catalog.DataTypes;
using DuxCommerce.Storefront.Views.AdminProduct.ViewModels;

namespace DuxCommerce.Storefront.Views.CustomerField.ViewModels;

public class CustomerFieldsVm
{
    public ProductLinksVm Links { get; set; }
    public ProductRow Product { get; set; }
    public IEnumerable<CustomerFieldVm> Fields { get; set; }
}