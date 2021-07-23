using Baetoti.Shared.Response.Item;
using System;
using System.Collections.Generic;
using System.Text;

namespace Baetoti.Shared.Response.User
{
    public class UserProfile
    {
        public BuyerResponse buyer { get; set; }
        public ProviderResponse provider { get; set; }
        public DriverResponse driver { get; set; }
        public WalletResponse wallet { get; set; }
        public AnalyticsResponse analytics { get; set; }
    }

    public class BuyerResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string LongitudeLatitude { get; set; }
        public decimal Rating { get; set; }
        public string Status { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string WalletData { get; set; }
        public List<BuyerHistory> buyerHistory { get; set; }
    }

    public class BuyerHistory
    {
        public int SrNo { get; set; }
        public long OrderID { get; set; }
        public int OrderAmount { get; set; }
        public string Provider { get; set; }
        public string Driver { get; set; }
        public string PaymentType { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderStatus { get; set; }
        public string DeliveryPickUp { get; set; }
        public DateTime Date { get; set; }
    }

    public class ProviderResponse
    {
        public string Name { get; set; }
        public string MaroofID { get; set; }
        public string GovernmentID { get; set; }
        public string GovernmentIDPicture { get; set; }
        public string Picture { get; set; }
        public string Status { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Instagram { get; set; }
        public string WalletData { get; set; }
        public string StoreName { get; set; }
        public string Location { get; set; }
        public string Rating { get; set; }
        public int Reviews { get; set; }
        public string Description { get; set; }
        public List<WeekDays> weekDays { get; set; }
        public List<ItemListResponse> Items { get; set; }
        public List<ProviderOrders> Orders { get; set; }
        public List<ProviderOrders2> Orders2 { get; set; }
    }

    public class ProviderOrders
    {
        public long OrderID { get; set; }
        public string Driver { get; set; }
        public DateTime? PickUp { get; set; }
        public DateTime? Delivery { get; set; }
        public string Rating { get; set; }
        public string Review { get; set; }
    }

    public class ProviderOrders2
    {
        public long OrderID { get; set; }
        public string Buyer { get; set; }
        public string Driver { get; set; }
        public int OrderAmount { get; set; }
        public string PaymentType { get; set; }
        public DateTime? Date { get; set; }
        public string DeliverOrPickup { get; set; }
        public string PaymentStatus { get; set; }
    }

    public class WeekDays
    {
        public string Day { get; set; }
        public string OpeningHours { get; set; }
        public string ClosingHours { get; set; }
    }

    public class DriverResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string Rating { get; set; }
        public string Status { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string WalletData { get; set; }
        public string Gender { get; set; }
        public string SmartPhone { get; set; }
        public string DriverID { get; set; }
        public string DrivingLiscence { get; set; }
        public string VehicleAuthorization { get; set; }
        public string MedicalCheckup { get; set; }
        public string VehicleInsurance { get; set; }
        public string GovernmentIDFrontPic { get; set; }
        public string DrivingLiscensePic { get; set; }
        public string VehicleRegistrationPic { get; set; }
        public string VehicleAuthorizationPic { get; set; }
        public string MedicalReportPic { get; set; }
        public string VehicleInsurancePic { get; set; }
        public List<DeliveryDetail> deliveryDetails { get; set; }
    }

    public class DeliveryDetail
    {
        public long OrderID { get; set; }
        public string Price { get; set; }
        public string DeliveryFee { get; set; }
        public string PickUpTime { get; set; }
        public string DropTime { get; set; }
        public string Rating { get; set; }

    }
    public class WalletResponse
    {
        public List<TransactionsHistory> transactionsHistory { get; set; }
    }

    public class TransactionsHistory
    {
        public long TransactionID { get; set; }
        public long OrderID { get; set; }
        public long UserID { get; set; }
        public decimal TransactionAmount { get; set; }
        public string TransactionFrom { get; set; }
        public string TransactionTo { get; set; }
        public string TransactionTime { get; set; }
        public string PaymentType { get; set; }
        public string TransactionFor { get; set; }
        public string TransactionStatus { get; set; }
    }

    public class AnalyticsResponse
    {

    }
}
