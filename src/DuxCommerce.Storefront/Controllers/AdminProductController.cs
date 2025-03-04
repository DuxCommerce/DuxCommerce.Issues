using System.Threading.Tasks;
using DuxCommerce.OrchardCore;
using DuxCommerce.StoreBuilder.Catalog.Requests;
using DuxCommerce.StoreBuilder.Catalog.UseCases;
using DuxCommerce.StoreBuilder.Workflows;
using DuxCommerce.Storefront.Views.AdminProduct.VmBuilders;
using DuxCommerce.Storefront.Views.Shared.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.Admin;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.Navigation;

namespace DuxCommerce.Storefront.Controllers;

[Admin]
[Route("Admin/Product")]
public class AdminProductController(
    IAuthorizationService authorizationService,
    ProductOptionsUseCases productOptionsUseCases,
    ProductPartVmBuilder productPartVmBuilder,
    ProductWorkflow productWorkflow,
    INotifier notifier,
    IHtmlLocalizer<AdminProductController> h)
    : Controller
{
    private readonly IHtmlLocalizer _h = h;

    [Route(nameof(Index))]
    public async Task<ActionResult> Index(ProductSearchVm searchVm, PagerParameters pagerParameters)
    {
        if (!await authorizationService.AuthorizeAsync(User, PermissionProvider.ManageProducts))
            return Forbid();

        var model = await productPartVmBuilder.BuildIndexModel(searchVm, pagerParameters);

        return View(model);
    }

    [HttpPost]
    [Route(nameof(Delete))]
    public async Task<IActionResult> Delete(string productId)
    {
        if (!await authorizationService.AuthorizeAsync(User, PermissionProvider.ManageProducts))
            return Forbid();

        await productWorkflow.RemoveProduct(productId);

        await notifier.SuccessAsync(_h["Product deleted successfully"]);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Route(nameof(LinkOptions))]
    public async Task<IActionResult> LinkOptions(string productId)
    {
        if (!await authorizationService.AuthorizeAsync(User, PermissionProvider.ManageProducts))
            return Forbid();

        var model = await productPartVmBuilder.BuildLinkOptionsVm(productId);

        return View(model);
    }

    [HttpPost]
    [Route(nameof(LinkOptions))]
    public async Task<IActionResult> LinkOptions(LinkOptionsModel model)
    {
        if (!await authorizationService.AuthorizeAsync(User, PermissionProvider.ManageProducts))
            return Forbid();

        if (ModelState.IsValid)
        {
            await productOptionsUseCases.LinkOptions(model);

            await notifier.SuccessAsync(_h["Shared option added successfully"]);
        }

        return RedirectToAction(nameof(LinkOptions), new { model.ProductId });
    }

    public IActionResult LinkCustomerField()
    {
        throw new System.NotImplementedException();
    }
}