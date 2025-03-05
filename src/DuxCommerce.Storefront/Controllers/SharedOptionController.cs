using System.Threading.Tasks;
using DuxCommerce.OrchardCore;
using DuxCommerce.StoreBuilder.Catalog.Requests;
using DuxCommerce.StoreBuilder.Catalog.UseCases;
using DuxCommerce.StoreBuilder.Workflows;
using DuxCommerce.Storefront.Views.CustomerField.ViewModels;
using DuxCommerce.Storefront.Views.Shared.ViewModels;
using DuxCommerce.Storefront.Views.SharedOption.ViewModels;
using DuxCommerce.Storefront.Views.SharedOption.VmBuilders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using OrchardCore.Admin;
using OrchardCore.DisplayManagement.Notify;
using OrchardCore.Navigation;

namespace DuxCommerce.Storefront.Controllers;

[Admin]
[Route("Admin/SharedOption")]
public class SharedOptionController(
    SharedOptionWorkflow sharedOptionWorkflow,
    SharedOptionUseCases sharedOptionUseCases,
    SharedOptionVmBuilder sharedOptionVmBuilder,
    IAuthorizationService authorizationService,
    INotifier notifier,
    IHtmlLocalizer<SharedOptionController> h)
    : Controller
{
    private readonly IHtmlLocalizer _h = h;

    [Route(nameof(Index))]
    public async Task<IActionResult> Index(string productId)
    {
        if (!await authorizationService.AuthorizeAsync(User, PermissionProvider.ManageSharedOptions))
            return Forbid();

        var model = await sharedOptionVmBuilder.BuildIndexModel();

        return View(model);
    }

    [Route(nameof(Create))]
    public async Task<IActionResult> Create()
    {
        if (!await authorizationService.AuthorizeAsync(User, PermissionProvider.ManageSharedOptions))
            return Forbid();

        var model = sharedOptionVmBuilder.BuildCreateModel();

        return View(model);
    }

    [HttpPost]
    [Route(nameof(Create))]
    public async Task<IActionResult> Create(SharedOptionVm model)
    {
        if (!await authorizationService.AuthorizeAsync(User, PermissionProvider.ManageSharedOptions))
            return Forbid();

        if (ModelState.IsValid)
        {
            var result = await sharedOptionUseCases.CreateOption(model.Option);

            if (result.Succeeded)
            {
                await notifier.SuccessAsync(_h["Shared option created successfully"]);
                return RedirectToAction(nameof(Edit), new { OptionId = result.Result.Id });
            }

            ModelState.AddModelError(string.Empty, result.Error.ToMessage());
        }

        var vm = sharedOptionVmBuilder.BuildCreateModel(model);

        return View(vm);
    }

    [Route(nameof(Edit))]
    public async Task<IActionResult> Edit(string optionId)
    {
        if (!await authorizationService.AuthorizeAsync(User, PermissionProvider.ManageSharedOptions))
            return Forbid();

        var model = await sharedOptionVmBuilder.BuildEditModel(optionId);

        return View(model);
    }

    [HttpPost]
    [Route(nameof(Edit))]
    public async Task<IActionResult> Edit(SharedOptionVm model)
    {
        if (!await authorizationService.AuthorizeAsync(User, PermissionProvider.ManageSharedOptions))
            return Forbid();

        if (ModelState.IsValid)
        {
            var result = await sharedOptionUseCases.UpdateOption(model.Option);

            if (result.Succeeded)
            {
                await notifier.SuccessAsync(_h["Shared option updated successfully"]);
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result.Error.ToMessage());
        }

        var vm = await sharedOptionVmBuilder.BuildEditModel(model);

        return View(vm);
    }

    [HttpPost]
    [Route(nameof(Delete))]
    public async Task<IActionResult> Delete(string optionId)
    {
        if (!await authorizationService.AuthorizeAsync(User, PermissionProvider.ManageSharedOptions))
            return Forbid();

        await sharedOptionWorkflow.DeleteOption(optionId);

        await notifier.SuccessAsync(_h["Shared option deleted successfully"]);

        return RedirectToAction(nameof(Index));
    }

    [Route(nameof(CreateChoice))]
    public async Task<IActionResult> CreateChoice(string optionId)
    {
        if (!await authorizationService.AuthorizeAsync(User, PermissionProvider.ManageSharedOptions))
            return Forbid();
        
        var model = new SharedOptionChoiceVm
        {
            Choice = new OptionChoiceModel { OptionId = optionId }
        };

        return View(model);
    }

    [HttpPost]
    [Route(nameof(CreateChoice))]
    public async Task<IActionResult> CreateChoice(string optionId, SharedOptionChoiceVm model)
    {
        if (!await authorizationService.AuthorizeAsync(User, PermissionProvider.ManageSharedOptions))
            return Forbid();

        if (ModelState.IsValid)
        {
            await sharedOptionUseCases.CreateChoice(optionId, model.Choice);

            await notifier.SuccessAsync(_h["Option choice created successfully"]);

            return RedirectToAction(nameof(Edit), new { model.Choice.OptionId });
        }

        return View(model);
    }

    [Route(nameof(EditChoice))]
    public async Task<IActionResult> EditChoice(string optionId, string choiceId)
    {
        if (!await authorizationService.AuthorizeAsync(User, PermissionProvider.ManageSharedOptions))
            return Forbid();

        var model = await sharedOptionVmBuilder.BuildEditChoiceModel(optionId, choiceId);

        return View(model);
    }

    [HttpPost]
    [Route(nameof(EditChoice))]
    public async Task<IActionResult> EditChoice(SharedOptionChoiceVm model)
    {
        if (!await authorizationService.AuthorizeAsync(User, PermissionProvider.ManageSharedOptions))
            return Forbid();

        if (ModelState.IsValid)
        {
            await sharedOptionUseCases.UpdateChoice(model.Choice);

            await notifier.SuccessAsync(_h["Option choice updated successfully"]);

            return RedirectToAction(nameof(Edit), new { model.Choice.OptionId });
        }

        return View(model);
    }

    [HttpPost]
    [Route(nameof(DeleteChoice))]
    public async Task<IActionResult> DeleteChoice(string optionId, string choiceId)
    {
        if (!await authorizationService.AuthorizeAsync(User, PermissionProvider.ManageSharedOptions))
            return Forbid();

        await sharedOptionWorkflow.DeleteChoice(optionId, choiceId);

        await notifier.SuccessAsync(_h["Option choice deleted successfully"]);

        return RedirectToAction(nameof(Edit), new { optionId });
    }

    public async Task<IActionResult> Products(string optionId, PagerParameters pagerParameter)
    {
        if (!await authorizationService.AuthorizeAsync(User, PermissionProvider.ManageCategories))
            return Forbid();

        var model = await sharedOptionVmBuilder.BuildProductsModel(optionId, pagerParameter);

        return View(model);
    }
}