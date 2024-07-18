using System.Runtime.Serialization;

namespace OrderSys.Core.Entities.Enums
{
    public enum OrderStatus
    {
        [EnumMember(Value ="pendig")]
        Pending,

        [EnumMember(Value = "placed")]
        placed,

    }
}
