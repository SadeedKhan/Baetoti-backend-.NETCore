using Baetoti.Core.Entites.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baetoti.Core.Entites
{
    [Table("Driver", Schema = "baetoti")]
    public class Driver : BaseEntity
    {
        public long UserID { get; set; }
        public int DriverStatus { get; set; }
        public string Nationality { get; set; }
        public string DOB { get; set; }
        public string IDNumber { get; set; }
        public string IDExpiryDate { get; set; }
        public string FrontSideofIDPic { get; set; }
        public string Gender { get; set; }
        public string PersonalPic { get; set; }
        public string DrivingLicensePic { get; set; }
        public string ExpirayDateofLicense { get; set; }
        public string VehicleRegistrationPic { get; set; }
        public string ExpiryDateofVehicleRegistration { get; set; }
        public string VehicleAuthorizationPic { get; set; }
        public string ExpiryDateofVehicleAuthorization { get; set; }
        public string MedicalCheckupPic { get; set; }
        public string ExpiryDateofMedicalcheckup { get; set; }
        public string VehicleInsurancePic { get; set; }
        public string ExpiryDateofVehicleInsurance { get; set; }
    }
}
