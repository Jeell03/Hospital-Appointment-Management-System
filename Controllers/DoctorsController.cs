
using Hospital.Models; // Import Doctor and appointment models.
using Microsoft.AspNetCore.Mvc; // Import MVC controller features.
using Microsoft.Extensions.Caching.Memory; // Import in-memory caching.

namespace Hospital.Controllers // Define the controller namespace.
{
    public class DoctorsController : Controller // Handle doctor-related requests.
    {
        private readonly IMemoryCache _cache; // Store the memory cache service.

        public DoctorsController(IMemoryCache cache) // Receive the cache through dependency injection.
        {
            _cache = cache; // Save the cache service for later use.
        }

        private List<Doctor> GetDoctors() // Return the doctor list from cache or create it.
        {
            return _cache.GetOrCreate("DoctorList", entry => // Look up the cached doctor list.
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3); // Expire the list after 3 minutes.

                return new List<Doctor> // Create sample doctor records.
                {
                    new Doctor { DoctorId = 1, DoctorName = "Dr. Rajesh Patel", Specialization = "Cardiology", Experience = 12, ConsultationFee = 700m },
                    new Doctor { DoctorId = 2, DoctorName = "Dr. Priya Shah", Specialization = "Neurology", Experience = 8, ConsultationFee = 600m },
                    new Doctor { DoctorId = 3, DoctorName = "Dr. Amit Desai", Specialization = "Orthopedics", Experience = 10, ConsultationFee = 500m },
                    new Doctor { DoctorId = 4, DoctorName = "Dr. Neha Mehta", Specialization = "Dermatology", Experience = 6, ConsultationFee = 400m }
                };
            })!; // Return the cached list; initialize it if missing.
        }

        public IActionResult Index() // Display all doctors.
        {
            ViewBag.PatientName = Request.Cookies["PatientName"]; // Read the patient-name cookie.
            ViewBag.SelectedSpecialization = HttpContext.Session.GetString("Specialization"); // Read the session.
            return View(GetDoctors()); // Send the cached doctor list to the view.
        }

        [HttpPost] // Accept the specialization form submission.
        [ValidateAntiForgeryToken] // Protect the form against CSRF requests.
        public IActionResult SelectSpecialization(string specialization) // Save the selected specialization.
        {
            HttpContext.Session.SetString("Specialization", specialization); // Store the value in session.
            return RedirectToAction(nameof(Index)); // Return to the doctor list.
        }

        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "id" })] // Cache each doctor's details response for up to 60 seconds.
        public IActionResult Details(int id) // Read the DoctorId from the query string.
        {
            Doctor? doctor = GetDoctors().FirstOrDefault(d => d.DoctorId == id); // Find the requested doctor.

            if (doctor == null) // Check whether the doctor exists.
            {
                return NotFound(); // Return HTTP 404 when no doctor matches.
            }

            return View(doctor); // Display the doctor's details.
        }

        [HttpGet] // Display the appointment form.
        public IActionResult Appointment(int id) // Receive DoctorId through the query string.
        {
            Doctor? doctor = GetDoctors().FirstOrDefault(d => d.DoctorId == id); // Find the doctor.

            if (doctor == null) // Check whether the doctor exists.
            {
                return NotFound(); // Reject invalid doctor IDs.
            }

            var model = new AppointmentViewModel // Prepare the appointment form model.
            {
                DoctorId = doctor.DoctorId, // Set the selected doctor's ID.
                Specialization = doctor.Specialization, // Set the doctor's specialization.
                AppointmentDate = DateTime.Today.AddDays(1) // Set tomorrow as the initial date.
            };

            return View(model); // Display the appointment form.
        }

        [HttpPost] // Accept appointment form submissions.
        [ValidateAntiForgeryToken] // Protect the form against CSRF requests.
        public IActionResult Appointment(AppointmentViewModel model) // Receive the submitted appointment.
        {
            Doctor? doctor = GetDoctors().FirstOrDefault(d => d.DoctorId == model.DoctorId); // Validate the doctor ID.

            if (doctor == null) // Check whether the submitted doctor ID is valid.
            {
                return NotFound(); // Reject an invalid doctor ID.
            }

            if (model.AppointmentDate.Date <= DateTime.Today) // Require a future appointment date.
            {
                ModelState.AddModelError(nameof(model.AppointmentDate), "Choose a date after today."); // Add a validation error.
            }

            if (!string.Equals(model.Specialization, doctor.Specialization, StringComparison.Ordinal)) // Verify the submitted specialization.
            {
                ModelState.AddModelError(nameof(model.Specialization), "The specialization does not match the selected doctor."); // Reject mismatched data.
            }

            if (!ModelState.IsValid) // Check all validation results.
            {
                return View(model); // Redisplay the form with validation errors.
            }

            Response.Cookies.Append("PatientName", model.PatientName, new CookieOptions // Create the patient-name cookie.
            {
                Expires = DateTimeOffset.UtcNow.AddDays(7), // Keep the cookie for 7 days.
                HttpOnly = true, // Prevent JavaScript access to the cookie.
                Secure = Request.IsHttps, // Send the cookie securely over HTTPS.
                SameSite = SameSiteMode.Lax // Limit cross-site cookie sending.
            });

            HttpContext.Session.SetString("Specialization", doctor.Specialization); // Store the selected specialization.

            TempData["Message"] = $"Appointment request received for {model.PatientName} with {doctor.DoctorName}."; // Prepare a confirmation message.

            return RedirectToAction(nameof(Confirmation)); // Redirect to the confirmation page.
        }

        public IActionResult Confirmation() // Display appointment submission confirmation.
        {
            return View(); // Render the confirmation view.
        }
    }
}
