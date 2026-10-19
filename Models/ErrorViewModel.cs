namespace Hospital.Models
{
    // Represent information displayed on the error page.
    public class ErrorViewModel
    {
        // Store the request identifier for troubleshooting.
        public string? RequestId { get; set; }

        // Indicate whether a request identifier is available.
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}