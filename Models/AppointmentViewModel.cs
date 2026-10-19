
using System.ComponentModel.DataAnnotations; // Import validation attributes.

namespace Hospital.Models // Define the model namespace.
{
    public class AppointmentViewModel // Represent an appointment form.
    {
        [Required] // Require a doctor ID.
        public int DoctorId { get; set; } // Store the selected doctor's ID.

        [Required] // Require the patient's name.
        [StringLength(100)] // Limit the patient name to 100 characters.
        public string PatientName { get; set; } = string.Empty; // Store the patient's name.

        [Required] // Require a preferred appointment date.
        [DataType(DataType.Date)] // Render the date using a date-friendly input.
        public DateTime AppointmentDate { get; set; } = DateTime.Today; // Store the appointment date.

        [Required] // Require the selected specialization.
        public string Specialization { get; set; } = string.Empty; // Store the specialization.
    }
}
