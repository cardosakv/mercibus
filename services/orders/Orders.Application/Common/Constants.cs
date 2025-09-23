namespace Orders.Application.Common;

public class Constants
{
    public static class ErrorCode
    {
        public const string ItemsEmpty = "items_empty";
        public const string ProductIdRequired = "product_id_required";
        public const string ProductNameRequired = "product_name_required";
        public const string ProductNotFound = "product_not_found";
        public const string QuantityRequired = "quantity_required";
        public const string QuantityInvalid = "quantity_invalid";
        public const string OrderNotFound = "order_not_found";
        public const string InvalidOrderStatus = "invalid_order_status";
    }
}