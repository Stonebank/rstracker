using rstracker.Enums;

namespace rstracker.Core
{
    public class CookieHandler
    {

        private static readonly Dictionary<CookieKeys, string> _keys = new Dictionary<CookieKeys, string>();

        public static void Init()
        {
            foreach (CookieKeys key in Enum.GetValues(typeof(CookieKeys)))
                _keys.Add(key, key.ToString().ToLower());
        }

        public static void SetCookie(HttpResponse response, CookieKeys cookiekey, string value, int? expireDays = null)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = expireDays.HasValue ? DateTime.Now.AddDays(expireDays.Value) : DateTime.Now.AddDays(365),
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = true
            };

            response.Cookies.Append(GetCookie(cookiekey), value, cookieOptions);
        }

        public static string GetCookie(CookieKeys cookiekey)
        {
            return _keys[cookiekey];
        }

    }
}
