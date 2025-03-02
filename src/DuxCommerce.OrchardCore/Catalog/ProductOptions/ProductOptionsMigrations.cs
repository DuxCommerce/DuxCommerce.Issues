using DuxCommerce.OrchardCore.Shared;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using YesSql.Sql;

namespace DuxCommerce.OrchardCore.Catalog.ProductOptions;

public class ProductOptionsMigrations(IContentDefinitionManager definitionManager) : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await definitionManager
            .AlterPartDefinitionAsync(nameof(ProductOptionsPart), builder => builder
                .Attachable()
                .WithDescription("Makes a content item into a product option."));

        await definitionManager
            .AlterTypeDefinitionAsync(ContentType.ProductOptions, type => type
                .WithPart(nameof(ProductOptionsPart)));

        await SchemaBuilder
            .CreateMapIndexTableAsync<ProductOptionsIndex>(table => table
                .Column<string>(nameof(ProductOptionsIndex.RowId), column => column.NotNull().WithLength(26))
                .Column<string>(nameof(ProductOptionsIndex.ProductId), column => column.NotNull().WithLength(26))
            );

        await SchemaBuilder
            .AlterIndexTableAsync<ProductOptionsIndex>(table => table
                .CreateIndex(
                    $"IDX_{nameof(ProductOptionsIndex)}_{nameof(ProductOptionsIndex.ProductId)}",
                    nameof(ProductOptionsIndex.ProductId),
                    nameof(DuxDocument.DocumentId))
            );

        return 1;
    }
}