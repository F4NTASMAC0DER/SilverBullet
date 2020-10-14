using Extreme.Net;
using RuriLib.Functions.Requests;

namespace RuriLib.Functions.ImgToBase64
{
    /// <summary>
    /// Image to base64
    /// </summary>
    public class ImageToBase64
    {
        /// <summary>
        /// Convert image to base64 string
        /// </summary>
        /// <param name="url">Image url</param>
        /// <param name="data">Bot data</param>
        /// <returns>base64 string</returns>
        public static string Convert(string url, BotData data = null)
        {
            var request = new Request();
            // Set proxy
            if (data != null && data.UseProxies)
            {
                request.SetProxy(data.Proxy);
            }
            request.Perform(url, HttpMethod.GET, resToMemoryStream: true);
            return System.Convert.ToBase64String(request.GetMemoryStream().ToArray());
        }
    }
}
