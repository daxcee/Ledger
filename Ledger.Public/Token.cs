using System.Configuration;

namespace Ledger.Public
{
    public class Token
    {
        public static bool IsAuthorized(string token)
        {
            var authtoken = ConfigurationManager.AppSettings["AuthToken"];
            return token == authtoken;
        }
    }
}