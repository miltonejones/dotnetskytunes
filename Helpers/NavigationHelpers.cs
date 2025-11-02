using System.Web;

namespace SkyTunesCsharp.Helpers
{
    public static class NavigationHelpers
    {
        public static string GetDetailUrl(string type, int id)
        {
            // URL encode the type to handle any special characters
            var encodedType = HttpUtility.UrlEncode(type);
            return $"/Detail/{encodedType}/{id}";
        }
 
        public static string GetImageUrl(string imagePath)
        {
            // Helper for image URLs - you can add CDN logic, fallbacks, etc.
            if (string.IsNullOrEmpty(imagePath))
                return "/images/placeholder.jpg";
            
            return imagePath.StartsWith("http") ? imagePath : $"/images/{imagePath}";
        }

        public static string TruncateText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
                return text;
            
            return text.Substring(0, maxLength) + "...";
        }
    }
}