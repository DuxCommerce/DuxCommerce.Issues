using DuxCommerce.OrchardCore.Carts;
using DuxCommerce.OrchardCore.Catalog.Categories;
using DuxCommerce.OrchardCore.Catalog.CustomerFields;
using DuxCommerce.OrchardCore.Catalog.Inventory;
using DuxCommerce.OrchardCore.Catalog.MerchantFields;
using DuxCommerce.OrchardCore.Catalog.ProductOptions;
using DuxCommerce.OrchardCore.Catalog.Products;
using DuxCommerce.OrchardCore.Catalog.SharedOptions;
using DuxCommerce.OrchardCore.Checkout;
using DuxCommerce.OrchardCore.Customers;
using DuxCommerce.OrchardCore.Marketing.BulkDiscounts;
using DuxCommerce.OrchardCore.Marketing.Coupons;
using DuxCommerce.OrchardCore.Marketing.GiftCards;
using DuxCommerce.OrchardCore.Marketing.Promotions;
using DuxCommerce.OrchardCore.Orders;
using DuxCommerce.OrchardCore.Payments;
using DuxCommerce.OrchardCore.Settings.Countries;
using DuxCommerce.OrchardCore.Settings.Currencies;
using DuxCommerce.OrchardCore.Settings.States;
using DuxCommerce.OrchardCore.Settings.StoreProfile;
using DuxCommerce.OrchardCore.Settings.TaxProfile;
using DuxCommerce.OrchardCore.Shipping.ShippingOrigins;
using DuxCommerce.OrchardCore.Shipping.ShippingProfiles;
using DuxCommerce.OrchardCore.Taxes.TaxCodes;
using DuxCommerce.OrchardCore.Taxes.TaxZones;
using DuxCommerce.StoreBuilder.Carts.DataStores;
using DuxCommerce.StoreBuilder.Carts.UseCases;
using DuxCommerce.StoreBuilder.Catalog.DataStores;
using DuxCommerce.StoreBuilder.Catalog.UseCases;
using DuxCommerce.StoreBuilder.Checkout.UseCases;
using DuxCommerce.StoreBuilder.Customers.DataStores;
using DuxCommerce.StoreBuilder.Customers.UseCases;
using DuxCommerce.StoreBuilder.Marketing.DataStores;
using DuxCommerce.StoreBuilder.Marketing.UseCases;
using DuxCommerce.StoreBuilder.Orders.DataStores;
using DuxCommerce.StoreBuilder.Orders.Plugins;
using DuxCommerce.StoreBuilder.Orders.UseCases;
using DuxCommerce.StoreBuilder.Payments.DataStores;
using DuxCommerce.StoreBuilder.Payments.UseCases;
using DuxCommerce.StoreBuilder.Settings.DataStores;
using DuxCommerce.StoreBuilder.Settings.UseCases;
using DuxCommerce.StoreBuilder.Shipping.DataStores;
using DuxCommerce.StoreBuilder.Shipping.UseCases;
using DuxCommerce.StoreBuilder.Taxes.DataStores;
using DuxCommerce.StoreBuilder.Taxes.UseCases;
using DuxCommerce.StoreBuilder.Workflows;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data.Migration;
using OrchardCore.Security.Permissions;
using YesSql.Indexes;

namespace DuxCommerce.OrchardCore;

public static class DependencyRegister
{
    public static void Register(IServiceCollection services)
    {
        // Settings
        services.AddContentPart<CurrencyPart>();
        services.AddContentPart<StoreProfilePart>();
        services.AddContentPart<CountryPart>();
        services.AddContentPart<StatePart>();

        services.AddScoped<IDataMigration, CurrencyMigrations>();
        services.AddScoped<IDataMigration, StoreProfileMigrations>();
        services.AddScoped<IDataMigration, CountryMigrations>();
        services.AddScoped<IDataMigration, StateMigrations>();

        services.AddSingleton<IIndexProvider, CurrencyIndexProvider>();
        services.AddSingleton<IIndexProvider, StoreProfileIndexProvider>();
        services.AddSingleton<IIndexProvider, CountryIndexProvider>();
        services.AddSingleton<IIndexProvider, StateIndexProvider>();
        
        services.AddScoped<ICurrencySeeder, CurrencySeeder>();
        services.AddScoped<ICountrySeeder, CountrySeeder>();
        services.AddScoped<IStateSeeder, StateSeeder>();
        
        services.AddScoped<ICurrencyStore, CurrencyStore>();
        services.AddScoped<ICountryStore, CountryStore>();
        services.AddScoped<IStateStore, StateStore>();

        services.AddScoped<CurrencyUseCases>();
        services.AddScoped<CountryUseCases>();
        services.AddScoped<StateUseCases>();

        // Taxes
        services.AddContentPart<TaxProfilePart>();
        services.AddContentPart<TaxCodePart>();
        services.AddContentPart<TaxZonePart>();

        services.AddScoped<IDataMigration, TaxProfileMigrations>();
        services.AddScoped<IDataMigration, TaxCodeMigrations>();
        services.AddScoped<IDataMigration, TaxZoneMigrations>();

        services.AddSingleton<IIndexProvider, TaxProfileIndexProvider>();
        services.AddSingleton<IIndexProvider, TaxCodeIndexProvider>();
        services.AddSingleton<IIndexProvider, TaxZoneIndexProvider>();
        services.AddScoped<ITaxCodeSeeder, TaxCodeSeeder>();
        
        services.AddScoped<ITaxProfileStore, TaxProfileStore>();
        services.AddScoped<ITaxCodeStore, TaxCodeStore>();
        services.AddScoped<ITaxZoneStore, TaxZoneStore>();

        services.AddScoped<TaxProfileUseCases>();
        services.AddScoped<TaxCodeUseCases>();
        services.AddScoped<TaxZoneUseCases>();

        // Shipping
        services.AddContentPart<ShippingOriginPart>();
        services.AddContentPart<ShippingProfilePart>();

        services.AddScoped<IDataMigration, ShippingOriginMigrations>();
        services.AddScoped<IDataMigration, ShippingProfileMigrations>();

        services.AddSingleton<IIndexProvider, ShippingOriginIndexProvider>();
        services.AddSingleton<IIndexProvider, ShippingProfileIndexProvider>();

        services.AddScoped<IShippingOriginStore, ShippingOriginStore>();
        services.AddScoped<IStoreProfileStore, StoreProfileStore>();
        services.AddScoped<IShippingProfileStore, ShippingProfileStore>();
        
        services.AddScoped<ShippingOriginUseCases>();
        services.AddScoped<ShippingProfileUseCases>();
        services.AddScoped<StoreProfileUseCases>();

        // Payments
        services.AddContentPart<PaymentMethodPart>();

        services.AddScoped<IDataMigration, PaymentMethodMigrations>();

        services.AddSingleton<IIndexProvider, PaymentMethodIndexProvider>();

        services.AddScoped<IPaymentMethodSeeder, PaymentMethodSeeder>();

        services.AddScoped<IPaymentMethodStore, PaymentMethodStore>();
        
        services.AddScoped<PaymentMethodUseCases>();
        services.AddScoped<SettingsUseCases>();

        // Catalog
        services.AddContentPart<CategoryPart>();
        services.AddContentPart<CategoryImagePart>();

        services.AddContentPart<ProductPart>();
        services.AddContentPart<ProductImagePart>();
        
        services.AddContentPart<ProductOptionsPart>();
        services.AddContentPart<InventoryEventPart>();
        services.AddContentPart<BulkDiscountPart>();
        services.AddContentPart<CustomerFieldsPart>();
        services.AddContentPart<MerchantFieldsPart>();

        services.AddSingleton<IIndexProvider, CategoryIndexProvider>();
        services.AddSingleton<IIndexProvider, ProductIndexProvider>();
        services.AddSingleton<IIndexProvider, FeaturedProductIndexProvider>();
        services.AddSingleton<IIndexProvider, OptionIndexProvider>();
        services.AddSingleton<IIndexProvider, ProductOptionsIndexProvider>();
        services.AddSingleton<IIndexProvider, LinkedOptionsIndexProvider>();
        services.AddSingleton<IIndexProvider, ProductCategoryIndexProvider>();
        services.AddSingleton<IIndexProvider, ProductChoiceIndexProvider>();
        services.AddSingleton<IIndexProvider, InventoryEventIndexProvider>();
        services.AddSingleton<IIndexProvider, BulkDiscountIndexProvider>();
        services.AddSingleton<IIndexProvider, CustomerFieldListIndexProvider>();
        services.AddSingleton<IIndexProvider, MerchantFieldListIndexProvider>();

        services.AddScoped<IDataMigration, CategoryMigrations>();
        services.AddScoped<IDataMigration, ProductMigrations>();
        services.AddScoped<IDataMigration, OptionMigrations>();
        services.AddScoped<IDataMigration, ProductOptionsMigrations>();
        services.AddScoped<IDataMigration, LinkedOptionsMigrations>();
        services.AddScoped<IDataMigration, InventoryEventMigrations>();
        services.AddScoped<IDataMigration, BulkDiscountMigrations>();
        services.AddScoped<IDataMigration, CustomerFieldsMigrations>();
        services.AddScoped<IDataMigration, MerchantFieldsMigrations>();

        services.AddScoped<ICategoryStore, CategoryStore>();
        services.AddScoped<IProductStore, ProductStore>();
        services.AddScoped<IOptionStore, OptionStore>();
        services.AddScoped<IProductOptionsStore, ProductOptionsStore>();
        services.AddScoped<IInventoryEventStore, InventoryEventStore>();
        services.AddScoped<IBulkDiscountStore, BulkDiscountStore>();
        services.AddScoped<ICustomerFieldsStore, CustomerFieldsStore>();
        services.AddScoped<IMerchantFieldsStore, MerchantFieldsStore>();
        
        services.AddScoped<CategoryUseCases>();
        services.AddScoped<ProductUseCases>();
        services.AddScoped<SharedOptionUseCases>();
        services.AddScoped<ProductOptionsUseCases>();
        services.AddScoped<BulkDiscountUseCases>();
        services.AddScoped<CustomerFieldUseCases>();
        services.AddScoped<MerchantFieldUseCases>();
        services.AddScoped<CategoryHomeUseCases>();
        services.AddScoped<ProductHomeUseCases>();

        // Marketing
        services.AddContentPart<CouponPart>();
        services.AddContentPart<PromotionPart>();
        services.AddContentPart<GiftCardPart>();

        services.AddScoped<IDataMigration, CouponMigrations>();
        services.AddScoped<IDataMigration, PromotionMigrations>();
        services.AddScoped<IDataMigration, GiftCardMigrations>();

        services.AddSingleton<IIndexProvider, CouponIndexProvider>();
        services.AddSingleton<IIndexProvider, PromotionIndexProvider>();
        services.AddSingleton<IIndexProvider, GiftCardIndexProvider>();
        
        services.AddScoped<ICouponStore, CouponStore>();
        services.AddScoped<IPromotionStore, PromotionStore>();
        services.AddScoped<IGiftCardStore, GiftCardStore>();

        services.AddScoped<CouponUseCases>();
        services.AddScoped<CouponUsageUseCases>();
        services.AddScoped<CouponCatalogUseCases>();
        
        services.AddScoped<PromotionUseCases>();
        services.AddScoped<PromotionUsageUseCases>();
        services.AddScoped<PromotionCatalogUseCases>();
        services.AddScoped<GiftCardUseCases>();

        // Carts
        services.AddContentPart<CartPart>();
        services.AddContentPart<CartWidgetPart>()
            .UseDisplayDriver<CartWidgetPartDisplayDriver>();

        services.AddScoped<IDataMigration, CartMigrations>();
        services.AddScoped<IDataMigration, CartWidgetMigrations>();

        services.AddSingleton<IIndexProvider, CartIndexProvider>();
        services.AddSingleton<IIndexProvider, CartItemIndexProvider>();
        
        services.AddScoped<ICartStore, CartStore>();

        services.AddScoped<CartUseCases>();

        // Orders
        services.AddContentPart<OrderPart>();

        services.AddScoped<IDataMigration, OrderMigrations>();

        services.AddSingleton<IIndexProvider, OrderIndexProvider>();
        services.AddSingleton<IIndexProvider, OrderPaymentIndexProvider>();
        services.AddSingleton<IIndexProvider, OrderItemIndexProvider>();
        services.AddSingleton<IIndexProvider, PromotionUsageIndexProvider>();
        services.AddSingleton<IIndexProvider, CustomerPromotionIndexProvider>();
        services.AddSingleton<IIndexProvider, CouponUsageIndexProvider>();
        services.AddSingleton<IIndexProvider, CustomerCouponIndexProvider>();
        
        services.AddScoped<IOrderStore, OrderStore>();

        services.AddScoped<OrderUseCases>();

        services.AddScoped<IOrderNumberGenerator, OrderNumberGenerator>();

        // Checkout
        services.AddScoped<CheckoutUseCases>();
        services.AddScoped<OrderVmBuilder>();
        services.AddScoped<IOrderEmailSender, OrderEmailSender>();

        // Customers
        services.AddContentPart<CustomerPart>();

        services.AddScoped<IDataMigration, CustomerMigrations>();

        services.AddSingleton<IIndexProvider, CustomerIndexProvider>();

        services.AddScoped<ICustomerStore, CustomerStore>();
        
        services.AddScoped<CustomerUseCases>();

        // Workflows
        services.AddScoped<ProductWorkflow>();
        services.AddScoped<ProductOptionWorkflow>();
        services.AddScoped<SharedOptionWorkflow>();

        services.AddScoped<IOrderStatusWatcher, OrderStatusWatcher>();
        
        // Other
        services.AddScoped<IPermissionProvider, PermissionProvider>();
        services.AddScoped<OrderProductService>();
        services.AddScoped<OrderVmBuilder>();
    }
}