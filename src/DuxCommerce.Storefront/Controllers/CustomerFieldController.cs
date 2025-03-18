using System.Threading.Tasks;
using DuxCommerce.OrchardCore;
using DuxCommerce.StoreBuilder.Catalog.UseCases;
using DuxCommerce.Storefront.Views.CustomerField.ViewModels;
using DuxCommerce.Storefront.Views.CustomerField.VmBuilders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.Admin;
using OrchardCore.DisplayManagement.Notify;

namespace DuxCommerce.Storefront.Controllers;

[Admin]
[Route("Admin/CustomerField")]
public class CustomerFieldController(
    CustomerFieldsUseCases customerFieldsUseCases,
    CustomerFieldsBuilder customerFieldsBuilder,
    IAuthorizationService authorizationService,
    INotifier notifier,
    IHtmlLocalizer<ProductOptionController> h)
    : Controller
{
    private readonly IHtmlLocalizer _h = h;
    
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

    [HttpPost]
    [Route(nameof(Create))]
    public async Task<IActionResult> Create(string productId, PrivateFieldVm model)
    {
        if (!await authorizationService.AuthorizeAsync(User, PermissionProvider.ManageProducts))
            return Forbid();

        if (ModelState.IsValid)
        {
            var result = await customerFieldsUseCases.AddPrivateField(productId, model.Field);

            if (result.Succeeded)
            {
                await notifier.SuccessAsync(_h["Customer field created successfully"]);
                return RedirectToAction(nameof(OptionField), new { productId, FieldId = result.Result.Id });
            }

            ModelState.AddModelError(string.Empty, result.Error.ToMessage());
        }

        var vm = customerFieldsBuilder.BuildCreateModel(model);

        return View(vm);
    }

    [Route(nameof(OptionField))]
    public async Task<IActionResult> OptionField(string productId, string fieldId)
    {
        if (!await authorizationService.AuthorizeAsync(User, PermissionProvider.ManageProducts))
            return Forbid();

        var model = await customerFieldsBuilder.BuildOptionModel(productId, fieldId);

        return View(model);
    }

    [Route(nameof(Edit))]
    public async Task<IActionResult> Edit(string productId, string fieldId)
    {
        if (!await authorizationService.AuthorizeAsync(User, PermissionProvider.ManageProducts))
            return Forbid();

        var model = await customerFieldsBuilder.BuildEditModel(productId, fieldId);

        return View(model);
    }

    [HttpPost]
    [Route(nameof(Delete))]
    public async Task<IActionResult> Delete(string productId, string fieldId)
    {
        if (!await authorizationService.AuthorizeAsync(User, PermissionProvider.ManageProducts))
            return Forbid();

        await customerFieldsUseCases.DeletePrivateField(productId, fieldId);

        await notifier.SuccessAsync(_h["Customer field deleted successfully"]);

        return RedirectToAction(nameof(Index), new { productId });
    }
}