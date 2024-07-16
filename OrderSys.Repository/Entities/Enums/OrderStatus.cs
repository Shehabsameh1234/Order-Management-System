﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OrderSys.Core.Entities.Enums
{
    public enum OrderStatus
    {
        [EnumMember(Value ="pendig")]
        Pending,

        [EnumMember(Value = "Payment Recieved")]
        PaymentRecieved,

        [EnumMember(Value = "Payment Faild")]
        PaymentFaild
    }
}