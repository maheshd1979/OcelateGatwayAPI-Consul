using System.Net;
using System.Text.Json.Serialization;

namespace AuthService
{
    public class APIResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="APIResponse"/> class.
        /// </summary>
        public APIResponse()
        {
            // Initialize the error messages as an empty list
            ErrorMessages = new List<string>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the result object associated with the API response.
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// Gets or sets the HTTP status code associated with the API response.
        /// </summary>
        [JsonIgnore] // Exclude from JSON serialization
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or sets a list of error messages associated with the API response.
        /// </summary>
        public List<string> ErrorMessages { get; set; }
    }
}
