using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuxCommerce.StoreBuilder.Catalog.DataStores;
using DuxCommerce.StoreBuilder.Catalog.DataTypes;
using DuxCommerce.StoreBuilder.Catalog.Requests;
using DuxCommerce.StoreBuilder.Settings.UseCases;
using DuxCommerce.Storefront.Services;
using DuxCommerce.Storefront.Views.CustomerField.ViewModels;
using DuxCommerce.Storefront.Views.ProductOption.ViewModels;
using DuxCommerce.Storefront.Views.Shared.ViewModels;
using DuxCommerce.Storefront.Views.SharedOption.ViewModels;
using Microsoft.AspNetCore.Routing;
using OrchardCore.DisplayManagement;
using OrchardCore.Navigation;

namespace DuxCommerce.Storefront.Views.SharedOption.VmBuilders;

public class SharedOptionVmBuilder(
    ProductOptionsService productOptionsService,
    CurrencyUseCases currencyUseCases,
    IOptionStore optionStore,
    IShapeFactory shapeFactory)
{
    private readonly dynamic _new = shapeFactory;
    
    public async Task<OptionIndexVm> BuildIndexModel()
    {
        var optionRows = await optionStore.GetAll();

        return new OptionIndexVm
        {
            Options = optionRows
        };
    }

    public SharedOptionVm BuildCreateModel()
    {
        return new SharedOptionVm
        {
            Option = new OptionModel(),
            Choices = new List<ChoiceRow>(),
            DisplayTypes = OptionDisplayType.GetAll()
        };
    }

    public SharedOptionVm BuildCreateModel(SharedOptionVm model)
    {
        var types = OptionDisplayType.GetAll();

        model.DisplayTypes = types;

        return model;
    }

    public async Task<SharedOptionVm> BuildEditModel(string optionId)
    {
        var optionRow = await optionStore.Get(optionId);

        var choices = (optionRow.Choices ?? [])
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.CreatedAtUtc);

        return new SharedOptionVm
        {
            Links = new OptionLinks { OptionId = optionId, OptionLink = true },
            Option = ToOptionModel(optionRow),
            Choices = choices,
            DisplayTypes = OptionDisplayType.GetAll()
        };
    }

    public async Task<SharedOptionVm> BuildEditModel(SharedOptionVm model)
    {
        var optionRow = await optionStore.Get(model.Option.OptionId);

        model.Links = new OptionLinks { OptionId = model.Option.OptionId, OptionLink = true };
        
        var choices = (optionRow.Choices ?? [])
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.CreatedAtUtc);

        model.Choices = choices;
        model.DisplayTypes = OptionDisplayType.GetAll();

        return model;
    }

    public async Task<SharedOptionChoiceVm> BuildEditChoiceModel(string optionId, string choiceId)
    {
        var optionRow = await optionStore.Get(optionId);

        var choice = optionRow.Choices.Single(x => x.Id == choiceId);

        var choiceModel = new OptionChoiceModel
        {
            ChoiceId = choiceId,
            OptionId = optionId,
            Name = choice.Name,
            IsDefault = choice.IsDefault,
            DisplayOrder = choice.DisplayOrder
        };

        return new SharedOptionChoiceVm { Choice = choiceModel };
    }

    public async Task<LinkedProductsVm> BuildProductsModel(string optionId, PagerParameters pagerParameters)
    {
        var currency = await currencyUseCases.GetCurrency();
        var pager = new Pager(pagerParameters, 10);
        var (count, products) = await productOptionsService.GetProducts(optionId, pager);
        var pagerShape = (await _new.Pager(pager)).TotalItemCount(count).RouteData(new RouteData());

        return new LinkedProductsVm
        {
            Links = new OptionLinks { OptionId = optionId, ProductsLink = true},
            Products = products,
            Currency = currency,
            Pager = pagerShape
        };
    }

    private OptionModel ToOptionModel(OptionRow option)
    {
        return new OptionModel
        {
            OptionId = option.Id,
            OptionName = option.OptionName,
            DisplayName = option.DisplayName,
            DisplayType = option.DisplayType,
            DisplayOrder = option.DisplayOrder
        };
    }
}