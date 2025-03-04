using DuxCommerce.OrchardCore;
using DuxCommerce.OrchardCore.Catalog.Categories;
using DuxCommerce.OrchardCore.Catalog.Products;
using DuxCommerce.OrchardCore.Customers;
using DuxCommerce.Storefront.Builders;
using DuxCommerce.Storefront.Drivers;
using DuxCommerce.Storefront.Handlers;
using DuxCommerce.Storefront.Services;
using DuxCommerce.Storefront.Views.AdminCategory.VmBuilders;
using DuxCommerce.Storefront.Views.AdminCountry.VmBuilders;
using DuxCommerce.Storefront.Views.AdminCustomer.VmBuilders;
using DuxCommerce.Storefront.Views.AdminOrder.VmBuilders;
using DuxCommerce.Storefront.Views.AdminProduct.VmBuilders;
using DuxCommerce.Storefront.Views.Category.ViewModels;
using DuxCommerce.Storefront.Views.Category.VmBuilders;
using DuxCommerce.Storefront.Views.Checkout.VmBuilders;
using DuxCommerce.Storefront.Views.Coupon.VmBuilders;
using DuxCommerce.Storefront.Views.Currency.VmBuilders;
using DuxCommerce.Storefront.Views.CustomerField.VmBuilders;
using DuxCommerce.Storefront.Views.Inventory.VmBuilders;
using DuxCommerce.Storefront.Views.LinkedOption.VmBuilders;
using DuxCommerce.Storefront.Views.PaymentMethod.VmBuilders;
using DuxCommerce.Storefront.Views.Product.ViewModels;
using DuxCommerce.Storefront.Views.Product.VmBuilders;
using DuxCommerce.Storefront.Views.ProductOption.VmBuilders;
using DuxCommerce.Storefront.Views.ProductVariant.VmBuilder;
using DuxCommerce.Storefront.Views.Promotion.VmBuilders;
using DuxCommerce.Storefront.Views.Shared.VmBuilders;
using DuxCommerce.Storefront.Views.SharedOption.VmBuilders;
using DuxCommerce.Storefront.Views.ShippingProfile.VmBuilders;
using DuxCommerce.Storefront.Views.ShoppingCart.ViewModels;
using DuxCommerce.Storefront.Views.ShoppingCart.VmBuilders;
using DuxCommerce.Storefront.Views.StoreHome.ViewModels;
using DuxCommerce.Storefront.Views.StoreHome.VmBuilders;
using DuxCommerce.Storefront.Views.StoreProfile.VmBuilders;
using DuxCommerce.Storefront.Views.TaxCode.VmBuilders;
using DuxCommerce.Storefront.Views.TaxProfile.VmBuilders;
using DuxCommerce.Storefront.Views.TaxZone.VmBuilders;
using DuxCommerce.Storefront.Views.YourAddresses.VmBuilders;
using DuxCommerce.Storefront.Views.YourOrders.VmBuilders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.ResourceManagement;
using OrchardCore.Users.Models;

namespace DuxCommerce.Storefront;

public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        DependencyRegister.Register(services);

        services.AddTransient<IConfigureOptions<ResourceManagementOptions>, ResourceManagement>();
        services.AddScoped<INavigationProvider, AdminMenu>();

        // ViewModel services
        services.AddScoped<StoreHomeBuilder>();
        services.AddScoped<ProductsVmBuilder>();
        services.AddScoped<CategoryHomeBuilder>();
        services.AddScoped<ProductHomeBuilder>();
        services.AddScoped<ProductVariantBuilder>();

        services.AddScoped<CartHomeBuilder>();
        services.AddScoped<AddressVmBuilder>();
        services.AddScoped<MiniCartVmBuilder>();
        services.AddScoped<CheckoutAddressesVmBuilder>();
        services.AddScoped<CheckoutOptionsVmBuilder>();
        services.AddScoped<ConfirmPaymentVmBuilder>();

        services.AddScoped<YourOrdersVmBuilder>();
        services.AddScoped<CustomerVmBuilder>();

        services.AddScoped<AdminOrdersVmBuilder>();
        services.AddScoped<OrderDetailsVmBuilder>();
        services.AddScoped<OrderPaymentsVmBuilder>();
        services.AddScoped<OrderShipmentsVmBuilder>();
        services.AddScoped<ProductPartVmBuilder>();
        services.AddScoped<SharedOptionVmBuilder>();
        services.AddScoped<ProductOptionsVmBuilder>();
        services.AddScoped<CustomerFieldsBuilder>();
        services.AddScoped<LinkedOptionVmBuilder>();
        services.AddScoped<ProductVariantsVmBuilder>();
        services.AddScoped<InventoryVmBuilder>();
        services.AddScoped<CategoryPartVmBuilder>();
        services.AddScoped<CategoryIndexVmBuilder>();
        services.AddScoped<CategoryPickerVmBuilder>();
        services.AddScoped<AdminCustomerVmBuilder>();
        services.AddScoped<CustomerAddressVmBuilder>();
        services.AddScoped<CouponVmBuilder>();
        services.AddScoped<PromotionVmBuilder>();
        services.AddScoped<StoreProfileVmBuilder>();
        services.AddScoped<TaxProfileVmBuilder>();
        services.AddScoped<ShippingProfileVmBuilder>();
        services.AddScoped<TaxCodeVmBuilder>();
        services.AddScoped<TaxZoneVmBuilder>();
        services.AddScoped<PaymentMethodVmBuilder>();
        services.AddScoped<CurrencyVmBuilder>();
        services.AddScoped<CountryVmBuilder>();

        services.AddScoped<IShopperInfoProvider, ShopperInfoProvider>();

        services.AddContentPart<CategoryPart>()
            .UseDisplayDriver<CategoryPartDisplayDriver>();

        services.AddContentPart<ProductPart>()
            .UseDisplayDriver<ProductPartDisplayDriver>()
            .AddHandler<ProductPartHandler>();

        services.AddScoped<IDisplayDriver<UserMenu>, YourAccountDisplayDriver>();

        services.AddScoped<IDisplayDriver<StoreHome>, StoreHomeDisplayDriver>();
        services.AddScoped<IDisplayDriver<CategoryHome>, CategoryHomeDisplayDriver>();
        services.AddScoped<IDisplayDriver<ProductHome>, ProductHomeDisplayDriver>();
        services.AddScoped<IDisplayDriver<ShoppingCartHome>, ShoppingCartDisplayDriver>();

        services.AddScoped<CategoryMenuBuilder>();
        services.AddScoped<OrdersVmBuilder>();

        services.AddScoped<ProductService>();
        services.AddScoped<QueryStringParser>();
        services.AddScoped<ProductSearchOptionsFactory>();
        services.AddScoped<CartProductService>();
        services.AddScoped<CategoryProductService>();
        services.AddScoped<PromotionProductService>();
        services.AddScoped<CouponProductService>();
        services.AddScoped<ProductOptionsService>();
    }
}