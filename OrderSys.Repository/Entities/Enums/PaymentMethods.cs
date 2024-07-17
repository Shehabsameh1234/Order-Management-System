using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
