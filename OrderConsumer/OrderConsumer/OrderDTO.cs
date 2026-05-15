using System;
using System.Collections.Generic;

namespace OrderConsumer
{
    public record OrderDTO(
        int Id,
        int UserId,
        DateOnly OrderDate,
        double OrderSum,
        ICollection<OrderItemDTO> OrderItems
    );
}
