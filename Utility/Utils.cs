using System.Diagnostics;

namespace rstracker.Utility
{
    public class Utils
    {

        public static string Capitalize(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            string[] words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < words.Length; i++)
            {
                string firstChar = words[i][0].ToString().ToUpper();
                string restOfChars = words[i][1..];
                words[i] = $"{firstChar}{restOfChars}";
            }

            return string.Join(' ', words);
        }

        public static void PrintLine(string input)
        {
            if (input is null)
                return;
            Debug.WriteLine($"[{DateTime.Now}]: {input}");
        }

        public static string GetErrorMessage(int statusCode)
        {
            return statusCode switch
            {
                404 => "The page you requested could not be found.",
                500 => "An error occured while processing your request.",
                503 => "Service unavailable at the moment. We're working on it!",
                _ => "An error has occurred."
            };
        }

    }
}
