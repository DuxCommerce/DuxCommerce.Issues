using DuxCommerce.StoreBuilder.ErrorCodes;
using DuxCommerce.StoreBuilder.ErrorTypes;

namespace DuxCommerce.OrchardCore;

public static class DuxErrorExtensions
{
    // Todo: add some unit tests
    public static string ToMessage(this DuxError error)
    {
        var message = ErrorMessages.All[error.ErrorCode];

        foreach (var property in error.Properties)
        {
            // Todo: improve the handling of DateTime type
            var key = "{" + property.Key + "}";
            message = message.Replace(key, property.Value.ToString());
        }

        return message;
    }
}

public static class ErrorMessages
{
    public static readonly IDictionary<ErrorCode, string> All = new Dictionary<ErrorCode, string>
    {
        { ErrorCode.CannotBeNullOrEmpty, "{FieldName} cannot be null or empty" },
        { ErrorCode.MaxLengthExceeded, "{FieldName} cannot be more than {MaxLength} characters" },
        { ErrorCode.PatternNotMatched, "{FieldName} must match the pattern {Pattern}" },
        { ErrorCode.LessThanMinValue, "{FieldName} cannot be less than {MinValue}" },
        { ErrorCode.GreaterThanMaxValue, "{FieldName} cannot be greater than {MaxValue}" },
        { ErrorCode.UnitSystemInvalid, "Unit system must be ImperialSystem or MetricSystem" },
        { ErrorCode.ImperialWeightUnitInvalid, "Imperial weight unit must be Pound or Ounce" },
        { ErrorCode.MetricWeightUnitInvalid, "Metric weight unit must be Kilogram or Gram" },
        { ErrorCode.ImperialLengthUnitInvalid, "Imperial length unit must be Inch or Foot" },
        { ErrorCode.MetricLengthUnitInvalid, "Metric length unit must be Centimeter or Meter" },
        { ErrorCode.ShippingMethodTypeInvalid, "Shipping method type is not valid" },
        { ErrorCode.PaymentMethodTypeInvalid, "Payment method type is not valid" },
        { ErrorCode.TaxZoneTypeInvalid, "Tax zone type is not valid" },
        { ErrorCode.ReceivedAmountMustBePositive, "The amount must be positive For Restocked or Received" },
        { ErrorCode.DamagedAmountMustBeNegative, "The amount must be negative for Damaged, Lost or Used" },
        { ErrorCode.InventoryEventTypeInvalid, "Inventory event type is not valid" },
        { ErrorCode.PaymentStatusInvalid, "Payment status is not valid" },
        { ErrorCode.NoEnoughStock, "There is no enough stock for {ProductName}" },
        { ErrorCode.FieldInputMissing, "{FieldName} is a required field" },
        { ErrorCode.FieldInputTooShort, "{FieldName} cannot be shorter than {MinLength}" },
        { ErrorCode.FieldInputTooLong, "{FieldName} cannot be longer than {MaxLength}" },
        { ErrorCode.FieldInputInvalid, "{FieldName} is not valid" },
        { ErrorCode.CheckboxFieldNotSelected, "{FieldName} must be checked" },
        { ErrorCode.FieldInputNotANumber, "{FieldName} must be a number" },
        { ErrorCode.EarlierThanEarliest, "{FieldName} cannot be before {EarliestTime}" },
        { ErrorCode.LaterThanLatest, "{FieldName} cannot be after {LatestTime}" },
        { ErrorCode.FieldInputNotADate, "{FieldName} must be a date" },
        { ErrorCode.BulkDiscountTypeInvalid, "Bulk discount type is not valid" },
        { ErrorCode.DateRangeInvalid, "Date range is not valid" },
        { ErrorCode.DateLimitTypeInvalid, "Date limit type is not valid" },
        { ErrorCode.NumberRangeInvalid, "Number range is not valid" },
        { ErrorCode.NumberLimitTypeInvalid, "Number limit type is not valid" },
        { ErrorCode.FieldLengthRangeInvalid, "Length range is not valid" },
        { ErrorCode.MaxLineCountInvalid, "Max line count is not valid" },
        { ErrorCode.PercentageOutOfRange, "Percentage must be between 0 and 100" },
        { ErrorCode.PriceAdjustmentTypeInvalid, "Price adjustment type is not valid" },
        { ErrorCode.FieldTypeInvalid, "Field type is not valid" },
        { ErrorCode.PromotionTypeInvalid, "Promotion type is not valid" },
        { ErrorCode.DiscountTypeInvalid, "Discount type is not valid" },
        { ErrorCode.ProductRuleTypeInvalid, "Product rule type is not valid" },
        { ErrorCode.MinRuleTypeInvalid, "Minimum rule type is not valid min" },
        { ErrorCode.CustomerRuleTypeInvalid, "Customer rule type is not valid " },
        { ErrorCode.CountryRuleTypeInvalid, "Country rule type is not valid" },
        { ErrorCode.StartOrEndTimeInvalid, "End time must be after start time" },
        { ErrorCode.CouponTypeInvalid, "Coupon type is not valid" },
        { ErrorCode.GiftCardPriceRequired, "Price is required" },
        { ErrorCode.GiftCardPriceRangeInvalid, "Price range is not valid" },
        { ErrorCode.GiftCardPriceTypeInvalid, "Price type is not valid" },
        { ErrorCode.GiftCardExpiryTypeInvalid, "Expiry type is not valid" },
        { ErrorCode.CouponExpired, "Coupon has expired" },
        { ErrorCode.CouponUsageLimitReached, "Coupon usage limit has been reached" },
        { ErrorCode.CouponDoesNotApply, "Coupon code doesn't apply" },
        { ErrorCode.CouponCodeInvalid, "Coupon code is not valid" },
        { ErrorCode.EmailAddressInvalid, "Email address is not valid" },
        { ErrorCode.ShippingMethodAlreadyEnabled, "Shipping method already enabled" },
        { ErrorCode.TaxZoneCountriesRequired, "Countries are required" },
        { ErrorCode.TaxZoneStatesRequired, "States are required" },
        { ErrorCode.TaxZonePostalCodesRequired, "Country and postal codes are required" },
        { ErrorCode.ShippingStatusInvalid, "Shipping status is not valid" },
        { ErrorCode.OrderStatusInvalid, "Order status is not valid" },
        { ErrorCode.ShipmentQuantityInvalid, "Shipment quantity is not valid" },
        { ErrorCode.OptionDisplayTypeInvalid, "Display type is not valid" },
        { ErrorCode.PaymentReferenceNotFound, "Order with payment reference '{PaymentReference}' cannot be found" },
        { ErrorCode.ErrorOccurredCallingExternalService, "{}" },
        { ErrorCode.PricesDisplayOptionInvalid, "Prices display type is not valid" },
        { ErrorCode.TaxCalculationTypeInvalid, "Tax calculation type is not valid" },
        { ErrorCode.QuantityToShipInvalid, "Quantity to ship is not valid" },
        { ErrorCode.OfferRuleInvalid, "Offer rule is not valid" },
        { ErrorCode.DiscountAmountRequired, "Discount amount is required" },
        { ErrorCode.DuplicateProductSku, "Product SKU is not unique" },
        { ErrorCode.DuplicateCouponCode, "Coupon code is not unique" }
    };
}