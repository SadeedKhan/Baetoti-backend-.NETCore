﻿namespace Baetoti.Shared.Enum
{
    public enum AppRole
    {
        Admin = 1,
        User = 2
    }

    public enum EmployementStatus
    {
        Inactive = 0,
        Active = 1,
        Blocked = 2
    }

    public enum UserStatus
    {
        Inactive = 0,
        Active = 1
    }

    public enum UserType
    {
        Buyer = 1,
        Provider = 2,
        Driver = 3
    }

    public enum ProviderStatus
    {
        Inactive = 0,
        Active = 1,
        Pending = 2,
        Approved = 3,
        Rejected = 4
    }

    public enum DriverStatus
    {
        Inactive = 0,
        Active = 1,
        Pending = 2,
        Approved = 3,
        Rejected = 4
    }

    public enum OTPStatus
    {
        Inactive = 0,
        Active = 1,
        Approved = 2,
        Rejected = 3
    }

    public enum ItemStatus
    {
        Inactive = 0,
        Active = 1,
        Pending = 2,
        Approved = 3,
        Rejected = 4
    }

    public enum OrderType
    {
        Delivery = 0,
        PickUP = 1
    }

    public enum OrderStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        InProgress = 3,
        Ready = 4,
        PickedUp = 5,
        Delivered = 6,
        Complaint = 7,
        CancelledByCustomer = 8,
        CancelledByDriver = 9,
        CancelledByProvider = 10,
        Completed = 11,
        Late = 12
    }

    public enum ProviderOrderStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        Cancelled = 3
    }

    public enum DriverOrderStatus
    {
        Pending = 0,
        PickedUp = 1,
        Delivered = 2
    }

    public enum TransactionStatus
    {
        UnPaid = 0,
        Paid = 1,
        Declined = 2
    }

    public enum TransactionType
    {
        Cash = 1,
        Online = 2
    }

    public enum StoreStatus
    {
        Private = 0,
        Public = 1
    }

    public enum DBSchema
    {
        baetoti,
        audit
    }

}
