namespace TrenzoStore.Models.Entities
{
    public enum OrderStatus
    {
        Pending = 1,
        Processing = 2,
        Shipped = 3,
        Delivered = 4,
        Cancelled = 5,
        Refunded = 6
    }

    public enum AddressType
    {
        Shipping = 1,
        Billing = 2
    }
}
