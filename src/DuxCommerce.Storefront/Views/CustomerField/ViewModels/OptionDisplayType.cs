using System.Collections.Generic;
using System.Dynamic;
using DuxCommerce.StoreBuilder.Catalog.SimpleTypes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DuxCommerce.Storefront.Views.CustomerField.ViewModels;

public static class FieldType
{
    public static IEnumerable<SelectListItem> GetAll()
    {
        dynamic dropDownList = new ExpandoObject();

        dropDownList.Name = "Dropdown List";
        dropDownList.Type = nameof(DisplayType.DropDownList);

        dynamic radioGroup = new ExpandoObject();

        radioGroup.Name = "Radio Group";
        radioGroup.Type = nameof(DisplayType.RadioGroup);

        return new List<SelectListItem>
        {
            new SelectListItem(dropDownList.Name, dropDownList.Type),
            new SelectListItem(radioGroup.Name, radioGroup.Type)
        };
    }
}