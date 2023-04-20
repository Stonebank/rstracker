using rstracker.Utility;

namespace rstracker.Models
{
    public class ErrorModel
    {

        /* Id is the unique identifier for the error model */

        public readonly string Id = Guid.NewGuid().ToString();

        /* Timestamp is the date and time in which the error model was triggered */

        public readonly DateTime Timestamp = DateTime.Now;

        /* OSName is the name of the operating system that the error was triggered on. */

        public string OSName = Environment.OSVersion.VersionString;

        /* ProcessorCount is the number of processors that the error was triggered on. */

        public int ProcessorCount = Environment.ProcessorCount;

        /* Memory is the amount of memory that the error was triggered on. */

        public long Memory = Environment.WorkingSet;

        /* Name is the name of the user submitting the error. */

        public string Name { get; set; }

        /* Email is the email of the user submitting the error. */

        public string Email { get; set; }

        /* Comments is a string that can be used to add additional information about the error. */

        public string Comments { get; set; }

        /* ErrorMessage is the message that is returned from the http client or an exception. */

        public string ErrorMessage { get; set; }

        /* StatusCode is the response from the http client. */

        public int StatusCode { get; set; }

        /* RemoteAddress is the IP address of the device that the error was triggered on. */

        public string RemoteAddress { get; set; }

        /* ErrorUrl is the URL of which the error occured */

        public string ErrorUrl { get; set; }


        public ErrorModel(string errorMessage, int statusCode, string remoteAddress = "unknown", string comments = "none", string name = "unknown", string email = "unknown", string errorurl = "unknown")
        {
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
            Comments = comments;
            RemoteAddress = remoteAddress;
            Name = name;
            Email = email;
            ErrorUrl = errorurl;
        }

    }
}
