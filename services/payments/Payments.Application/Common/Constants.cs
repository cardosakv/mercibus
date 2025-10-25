namespace Payments.Application.Common;

public class Constants
{
    public static class ErrorCode
    {
        public const string PaymentNotFound = "payment_not_found";
        public const string OrderNotFound = "order_not_found";
        public const string FirstNameRequired = "first_name_required";
        public const string LastNameRequired = "last_name_required";
        public const string EmailRequired = "email_required";
        public const string EmailInvalid = "email_invalid";
        public const string StreetLine1Required = "street_line1_required";
        public const string CityRequired = "city_required";
        public const string StateRequired = "state_required";
        public const string PostalCodeRequired = "postal_code_required";
        public const string CountryRequired = "country_required";
        public const string PaymentCurrentlyProcessing = "payment_currently_processing";
        public const string PaymentAlreadyCompleted = "payment_already_completed";
        public const string PaymentFailed = "payment_failed";
    }

    public static class PaymentClient
    {
        public const string SessionType = "PAY";
        public const string Mode = "PAYMENT_LINK";
        public const string SuccessStatus = "SUCCEEDED";
    }
}