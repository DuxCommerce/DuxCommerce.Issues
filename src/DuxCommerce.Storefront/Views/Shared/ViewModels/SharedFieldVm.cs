using System.Collections.Generic;
using DuxCommerce.StoreBuilder.Catalog.Requests;
using DuxCommerce.Storefront.Views.SharedField.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DuxCommerce.Storefront.Views.Shared.ViewModels;

public class SharedFieldVm
{
    public FieldLinks Links { get; set; }
    public FieldModel Field { get; set; }
    public IEnumerable<SelectListItem> FieldTypes { get; set; }    
}