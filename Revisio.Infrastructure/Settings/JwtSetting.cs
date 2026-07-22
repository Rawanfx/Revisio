
namespace Revisio.Infrastructure.Settings
{
    public class JwtSetting
    {
        public string Audience { get; set; }
        public string Key { get; set; }
        public string Issuer { get; set; }
        public int Expire { get; set; }
    }
}
