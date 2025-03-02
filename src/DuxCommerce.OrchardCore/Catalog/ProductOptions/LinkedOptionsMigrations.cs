using DuxCommerce.OrchardCore.Shared;
using OrchardCore.Data.Migration;
using YesSql.Sql;

namespace DuxCommerce.OrchardCore.Catalog.ProductOptions;

public class LinkedOptionsMigrations : DataMigration
{
    public async Task<int> CreateAsync()
    {
        await SchemaBuilder
            .CreateMapIndexTableAsync<LinkedOptionsIndex>(table => table
                .Column<string>(nameof(LinkedOptionsIndex.RowId), column => column.NotNull().WithLength(26))
                .Column<string>(nameof(LinkedOptionsIndex.ProductId), column => column.NotNull().WithLength(26))
                .Column<string>(nameof(LinkedOptionsIndex.OptionId), column => column.NotNull().WithLength(26))
            );

        await SchemaBuilder
            .AlterIndexTableAsync<LinkedOptionsIndex>(table => table
                .CreateIndex(
                    $"IDX_{nameof(LinkedOptionsIndex)}_{nameof(LinkedOptionsIndex.ProductId)}",
                    nameof(LinkedOptionsIndex.ProductId),
                    nameof(DuxDocument.DocumentId))
            );

        await SchemaBuilder
            .AlterIndexTableAsync<LinkedOptionsIndex>(table => table
                .CreateIndex(
                    $"IDX_{nameof(LinkedOptionsIndex)}_{nameof(LinkedOptionsIndex.OptionId)}",
                    nameof(LinkedOptionsIndex.OptionId),
                    nameof(DuxDocument.DocumentId))
            );

        return 1;
    }
}