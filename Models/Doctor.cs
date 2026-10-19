
namespace Hospital.Models // Define the namespace for hospital models.
{
    public class Doctor // Represent a doctor in the hospital.
    {
        public int DoctorId { get; set; } // Store the unique doctor ID.

        public string DoctorName { get; set; } = string.Empty; // Store the doctor's name.

        public string Specialization { get; set; } = string.Empty; // Store the medical specialization.

        public int Experience { get; set; } // Store experience in years.

        public decimal ConsultationFee { get; set; } // Store the consultation fee in rupees.
    }
}
