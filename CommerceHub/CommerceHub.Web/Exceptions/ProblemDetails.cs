namespace CommerceHub.Web.Exceptions
{
    public class ProblemDetails
    {
        /*
         * type: A URI reference identifying the error type.
         * title: A short summary of the error.
         * status: The HTTP status code.
         * detail: Specific info about the error occurrence.
         * instance: A URI identifying the exact occurrence
         */

        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Status { get; set; } = StatusCodes.Status500InternalServerError;
        public string Detail { get; set; } = string.Empty;

        public string Instance { get; set; }

    }
}
