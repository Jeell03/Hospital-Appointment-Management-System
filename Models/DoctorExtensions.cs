
namespace Hospital.Models // Keep the extension in the model namespace.
{
    public static class DoctorExtensions // Define a static class for extension methods.
    {
        public static decimal CalculateTotalCharge(this Doctor doctor) // Extend the Doctor class.
        {
            return doctor.ConsultationFee + 100m; // Add the ₹100 service charge.
        }
    }
}
