using DuxCommerce.OrchardCore.Shared;
using OrchardCore.Autoroute.Models;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Media.Fields;
using OrchardCore.Media.Settings;
using YesSql.Sql;

namespace DuxCommerce.OrchardCore.Catalog.Products;

public class ProductMigrations(IContentDefinitionManager definitionManager) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await definitionManager.AlterPartDefinitionAsync(nameof(ProductPart), partBuilder => partBuilder
            .WithDescription("Contains the most common fields of a product."));

        await definitionManager.AlterPartDefinitionAsync(nameof(ProductImagePart), partBuilder => partBuilder
            .WithDescription("Contains image field of a product.")
            .WithField("Image", field => field
                .OfType(nameof(MediaField))
                .WithDisplayName("Product Image")
                .WithSettings(new MediaFieldSettings { Multiple = true }))
        );

        await definitionManager
            .AlterTypeDefinitionAsync(ContentType.Product, typeBuilder => typeBuilder
                .WithPart(nameof(AutoroutePart),
                    settings => settings.WithSettings(new AutoroutePartSettings { AllowCustomPath = true }))
                .WithPart(nameof(ProductPart))
                .WithPart(nameof(ProductImagePart))
            );

        await SchemaBuilder
            .CreateMapIndexTableAsync<ProductIndex>(table => table
                .Column<string>(nameof(ProductIndex.RowId), column => column.NotNull().WithLength(26))
                .Column<string>(nameof(ProductIndex.Name), column => column.NotNull().WithLength(255))
                .Column<string>(nameof(ProductIndex.ChoiceNames), column => column.Nullable().WithLength(255))
                .Column<decimal>(nameof(ProductIndex.Price), column => column.NotNull())
                .Column<string>(nameof(ProductIndex.Sku), column => column.Nullable().WithLength(100))
                .Column<bool>(nameof(ProductIndex.StockEnabled), column => column.NotNull())
                .Column<bool>(nameof(ProductIndex.HasOptions), column => column.NotNull())
                .Column<bool>(nameof(ProductIndex.IsVisible), column => column.NotNull())
                .Column<string>(nameof(ProductIndex.ParentId), column => column.Nullable().WithLength(26))
            );

        await SchemaBuilder
            .AlterIndexTableAsync<ProductIndex>(table => table
                .CreateIndex(
                    $"IDX_{nameof(ProductIndex)}_{nameof(ProductIndex.RowId)}_{nameof(ProductIndex.IsVisible)}",
                    nameof(ProductIndex.RowId),
                    nameof(ProductIndex.IsVisible),
                    nameof(DuxDocument.DocumentId))
            );

        await SchemaBuilder
            .AlterIndexTableAsync<ProductIndex>(table => table
                .CreateIndex(
                    $"IDX_{nameof(ProductIndex)}_{nameof(ProductIndex.RowId)}_{nameof(ProductIndex.Sku)}",
                    nameof(ProductIndex.RowId),
                    nameof(ProductIndex.Sku),
                    nameof(DuxDocument.DocumentId))
            );

        await SchemaBuilder
            .AlterIndexTableAsync<ProductIndex>(table => table
                .CreateIndex(
                    $"IDX_{nameof(ProductIndex)}_{nameof(ProductIndex.ParentId)}",
                    nameof(ProductIndex.ParentId),
                    nameof(DuxDocument.DocumentId))
            );

        await SchemaBuilder
            .CreateMapIndexTableAsync<ProductCategoryIndex>(table => table
                .Column<string>(nameof(ProductCategoryIndex.RowId), column => column.NotNull().WithLength(26))
                .Column<string>(nameof(ProductCategoryIndex.CategoryId), column => column.NotNull().WithLength(26))
            );

        await SchemaBuilder
            .AlterIndexTableAsync<ProductCategoryIndex>(table => table
                .CreateIndex(
                    $"IDX_{nameof(ProductCategoryIndex)}_{nameof(ProductCategoryIndex.CategoryId)}",
                    nameof(ProductCategoryIndex.CategoryId),
                    nameof(DuxDocument.DocumentId))
            );

        return 1;
    }

    public async Task<int> UpdateFrom1Async()
    {
        await SchemaBuilder
            .CreateMapIndexTableAsync<ProductChoiceIndex>(table => table
                .Column<string>(nameof(ProductChoiceIndex.RowId), column => column.NotNull().WithLength(26))
                .Column<string>(nameof(ProductChoiceIndex.ParentId), column => column.NotNull().WithLength(26))
                .Column<string>(nameof(ProductChoiceIndex.ChoiceId), column => column.NotNull().WithLength(26))
            );

        // Note: based on assumption that options are not shared between products
        await SchemaBuilder
            .AlterIndexTableAsync<ProductChoiceIndex>(table => table
                .CreateIndex(
                    $"IDX_{nameof(ProductChoiceIndex)}_{nameof(ProductChoiceIndex.ChoiceId)}_{nameof(ProductChoiceIndex.ParentId)}",
                    nameof(ProductChoiceIndex.ParentId),
                    nameof(ProductChoiceIndex.ChoiceId),
                    nameof(ProductChoiceIndex.RowId))
            );

        return 2;
    }

    public async Task<int> UpdateFrom2Async()
    {
        await SchemaBuilder
            .CreateMapIndexTableAsync<FeaturedProductIndex>(table => table
                .Column<string>(nameof(FeaturedProductIndex.RowId), column => column.NotNull().WithLength(26))
            );

        await SchemaBuilder
            .AlterIndexTableAsync<FeaturedProductIndex>(table => table
                .CreateIndex(
                    $"IDX_{nameof(FeaturedProductIndex)}_{nameof(FeaturedProductIndex.RowId)}",
                    nameof(FeaturedProductIndex.RowId),
                    nameof(DuxDocument.DocumentId))
            );

        return 3;
    }

    public async Task<int> UpdateFrom3Async()
    {
        await SchemaBuilder.AlterIndexTableAsync<ProductIndex>(table => table
            .AddColumn<int>(nameof(ProductIndex.HasCustomerFields), c => c.NotNull().WithDefault(0)));

        return 4;
    }
}