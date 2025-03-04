using System.Threading.Tasks;
using DuxCommerce.OrchardCore;
using DuxCommerce.Storefront.Views.CustomerField.VmBuilders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.Admin;

namespace DuxCommerce.Storefront.Controllers;

[Admin]
[Route("Admin/CustomerField")]
public class CustomerFieldController(
    CustomerFieldsBuilder customerFieldsBuilder,
    IAuthorizationService authorizationService)
    : Controller
{
    [Route(nameof(Index))]
    public async Task<IActionResult> Index(string productId)
    {
        if (!await authorizationService.AuthorizeAsync(User, PermissionProvider.ManageProducts))
            return Forbid();

        var model = await customerFieldsBuilder.BuildIndexModel(productId);

        return View(model);
    }

    [Route(nameof(Create))]
    public async Task<IActionResult> Create(string productId)
    {
        if (!await authorizationService.AuthorizeAsync(User, PermissionProvider.ManageProducts))
            return Forbid();

        var model = customerFieldsBuilder.BuildCreateModel(productId);

        return View(model);
    }
}