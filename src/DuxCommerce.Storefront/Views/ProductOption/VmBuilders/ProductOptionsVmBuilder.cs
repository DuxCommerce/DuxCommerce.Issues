using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuxCommerce.OrchardCore.Catalog.Products;
using DuxCommerce.StoreBuilder.Catalog.Core;
using DuxCommerce.StoreBuilder.Catalog.DataStores;
using DuxCommerce.StoreBuilder.Catalog.DataTypes;
using DuxCommerce.StoreBuilder.Catalog.Requests;
using DuxCommerce.StoreBuilder.Catalog.UseCases;
using DuxCommerce.Storefront.Views.AdminProduct.ViewModels;
using DuxCommerce.Storefront.Views.ProductOption.ViewModels;
using OrchardCore.ContentManagement;

namespace DuxCommerce.Storefront.Views.ProductOption.VmBuilders;

public class ProductOptionsVmBuilder(
    ProductOptionsUseCases productOptionsUseCases,
    IProductOptionsStore productOptionsStore,
    IProductStore productStore)
{
    public async Task<ProductOptionsVm> BuildIndexModel(string productId)
    {
        var options = await GetAllOptions(productId);

        var productItem = await productStore.GetItem<ContentItem>(productId);
        var productRow = productItem.As<ProductPart>().Row;

        return new ProductOptionsVm
        {
            Product = productRow,
            Options = options,
            Links = new ProductLinksVm { ContentItem = productItem, OptionsLink = true }
        };
    }
    public PrivateOptionVm BuildCreateModel(string productId)
    {
        return new PrivateOptionVm
        {
            ProductId = productId,
            Option = new OptionModel(),
            Choices = new List<ChoiceRow>(),
            DisplayTypes = OptionDisplayType.GetAll()
        };
    }

    public PrivateOptionVm BuildCreateModel(PrivateOptionVm model)
    {
        var types = OptionDisplayType.GetAll();

        model.DisplayTypes = types;

        return model;
    }

    public async Task<PrivateOptionVm> BuildEditModel(string productId, string optionId)
    {
        var optionsRow = await productOptionsStore.GetByProductId(productId);
        var option = optionsRow.PrivateOptions.Single(x => x.Id == optionId);

        var choices = (option.Choices ?? Array.Empty<ChoiceRow>())
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.CreatedAtUtc);

        return new PrivateOptionVm
        {
            ProductId = optionsRow.ProductId,
            Option = ToOptionModel(option),
            Choices = choices,
            DisplayTypes = OptionDisplayType.GetAll()
        };
    }

    public async Task<PrivateOptionVm> BuildEditModel(string productId, PrivateOptionVm model)
    {
        var optionsRow = await productOptionsStore.GetByProductId(productId);
        var option = optionsRow.PrivateOptions.Single(x => x.Id == model.Option.OptionId);

        var choices = (option.Choices ?? [])
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.CreatedAtUtc);

        model.ProductId = model.ProductId;
        model.Choices = choices;
        model.DisplayTypes = OptionDisplayType.GetAll();

        return model;
    }

    public async Task<PrivateOptionChoiceVm> BuildEditChoiceModel(string productId, string optionId, string choiceId)
    {
        var optionsRow = await productOptionsStore.GetByProductId(productId);
        var option = optionsRow.PrivateOptions.Single(x => x.Id == optionId);

        var choice = option.Choices.Single(x => x.Id == choiceId);

        var choiceModel = new OptionChoiceModel
        {
            ChoiceId = choiceId,
            OptionId = optionId,
            Name = choice.Name,
            IsDefault = choice.IsDefault,
            DisplayOrder = choice.DisplayOrder
        };

        return new PrivateOptionChoiceVm { ProductId = productId, Choice = choiceModel };
    }
    
    private async Task<IEnumerable<OptionVm>> GetAllOptions(string productId)
    {
        var productOptions = await productOptionsStore.GetByProductId(productId);
        var sharedOptions = await productOptionsUseCases.GetSharedOptions(productOptions);

        var sharedOptionVms = ProductOptionsCore.getSharedOptions(productOptions, sharedOptions)
            .Select(x => new OptionVm { Option = x, Shared = true });

        var privateOptionVms = productOptions.PrivateOptions
            .Select(x => new OptionVm { Option = x });

        return sharedOptionVms.Concat(privateOptionVms).OrderBy(x => x.Option.DisplayOrder);
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