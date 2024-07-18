using System.Runtime.Serialization;


namespace OrderSys.Core.Entities.Enums
{
    public enum PaymentMethods
    {
        [EnumMember(Value = "Credit Card")]
        CreditCard,

        [EnumMember(Value = "Cash")]
        Cash,

        [EnumMember(Value = "PayPal")]
        Paypal
    }
}
