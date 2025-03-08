using DuxCommerce.StoreBuilder.Settings.DataTypes;
using OrchardCore.Entities;

namespace DuxCommerce.OrchardCore.Shared;

public static class RowExtensions
{
    public static void UpdateId<TRow>(this IEnumerable<TRow> rows, IIdGenerator idGenerator)
        where TRow: IRow
    {
        foreach (var row in rows)
        {
            if (string.IsNullOrEmpty(row.Id))
                row.Id = idGenerator.GenerateUniqueId();
        }
    }
}