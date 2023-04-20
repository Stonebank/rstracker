using Newtonsoft.Json;
using rstracker.Models;

namespace rstracker.Core
{
    public class ErrorLogHandler
    {

        public static void SaveErrorLog(ErrorModel error)
        {
            string _errorPath = Constants.ERROR_LOG_PATH;
            if (!Directory.Exists(_errorPath))
                Directory.CreateDirectory(_errorPath);

            var json = JsonConvert.SerializeObject(error, Formatting.Indented);
            File.WriteAllTextAsync(_errorPath + error.Id + ".json", json);
        }

    }
}
