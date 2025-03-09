using System.Collections.Generic;
using DuxCommerce.StoreBuilder.Catalog.DataTypes;
using DuxCommerce.Storefront.Views.Shared.ViewModels;

namespace DuxCommerce.Storefront.Views.CustomerField.ViewModels;

public class OptionFieldVm : SharedFieldVm
{
    public string ProductId { get; set; }
    public IEnumerable<ChoiceRow> Choices { get; set; }
}