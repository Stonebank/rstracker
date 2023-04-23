using Newtonsoft.Json;
using rstracker.Utility;

namespace rstracker.Models
{
    public class ClanMemberModel
    {

        public bool IsSuffix { get; set; }

        public bool Recruiting { get; set; }

        public string Name { get; set; }

        public string Clan { get; set; }

        public string Title { get; set; }

        public string World { get; set; }

        public bool Online { get; set; }

        public ClanMemberModel(bool isSuffix, bool recruiting, string name, string clan, string title, string world, bool online)
        {
            IsSuffix = isSuffix;
            Recruiting = recruiting;
            Name = name;
            Clan = clan;
            Title = title;
            World = world;
            Online = online;
        }

        public static string ExtractJsonData(string jsonpResponse)
        {
            int startIndex = jsonpResponse.IndexOf('{');
            int endIndex = jsonpResponse.LastIndexOf('}');
            if (startIndex >= 0 && endIndex >= 0)
                return jsonpResponse.Substring(startIndex, endIndex - startIndex + 1);
            return string.Empty;
        }
    }
}
