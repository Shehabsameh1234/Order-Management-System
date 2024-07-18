using System.Runtime.Serialization;


namespace OrderSys.Core.Entities.Enums
{
    public enum UserRole
    {
        [EnumMember(Value = "admin")]
        Admin,

        [EnumMember(Value = "customer")]
        Customer,
    }
}
