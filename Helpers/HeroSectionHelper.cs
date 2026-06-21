namespace NexsolCrmBackendVersion2.Helpers
{
    public static class HeroSectionHelper
    {
        public static string GetSource(string referrer)
        {
            if (string.IsNullOrEmpty(referrer) || referrer == "direct")
                return "Прямой переход";
            if (referrer.Contains("google", StringComparison.OrdinalIgnoreCase))
                return "Google";
            if (referrer.Contains("yandex", StringComparison.OrdinalIgnoreCase))
                return "Yandex";
            if (referrer.Contains("facebook", StringComparison.OrdinalIgnoreCase) ||
                referrer.Contains("fb", StringComparison.OrdinalIgnoreCase))
                return "Facebook";
            if (referrer.Contains("vk", StringComparison.OrdinalIgnoreCase) ||
                referrer.Contains("vkontakte", StringComparison.OrdinalIgnoreCase))
                return "VK";

            return "Другой источник";
        }

        public static string GetDevice(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
                return "Desktop";

            string ua = userAgent.ToLower();
            if (ua.Contains("mobile") || ua.Contains("android") || ua.Contains("iphone"))
                return "Mobile";
            if (ua.Contains("tablet") || ua.Contains("ipad"))
                return "Tablet";

            return "Desktop";
        }
    }
}
