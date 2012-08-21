using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace ssl_checker
{
    public class Utillty
    {
        public Utillty()
        {
        }

        /// <summary>
        /// Formats the URL, making sure it's pointing to HTTPS.
        /// </summary>
        /// <returns>
        /// A URI object pointing to HTTPS.
        /// </returns>
        /// <param name='url'>
        /// URL.
        /// </param>
        public static Uri FormatURL(string url)
        {
            UriBuilder endpoint = new UriBuilder(url);

            //we make sure we are going to connect to a secure endpoint
            endpoint.Scheme = "https";
            endpoint.Port = 443;

            return endpoint.Uri;
        }

        /// <summary>
        /// Prepares the service point manager.
        /// </summary>
        public static void PrepareServicePointManager()
        {
            // Accept any SSL certificate, don't validate it
            ServicePointManager.ServerCertificateValidationCallback += (object sender2, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors) => true;
        }

        /// <summary>
        /// Prepare the request and set cookies.
        /// </summary>
        /// <returns>
        /// The request.
        /// </returns>
        /// <param name='requestUri'>
        /// Request URI.
        /// </param>
        /// <param name='cookieCollection'>
        /// Cookie collection.
        /// </param>
        public static HttpWebRequest PrepareRequest(Uri requestUri, CookieCollection cookieCollection)
        {
            var request = PrepareRequest(requestUri);
            request.CookieContainer.Add(cookieCollection);
            return request;
        }

        /// <summary>
        /// Prepare the request and set some options.
        /// </summary>
        /// <returns>
        /// The request.
        /// </returns>
        /// <param name='requestUri'>
        /// Request URI.
        /// </param>
        public static HttpWebRequest PrepareRequest(Uri requestUri)
        {
            // Create a request with the Uri requestUri parameter.
            // we don't want all the content just the HEAD
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Method = "HEAD";
            request.AllowAutoRedirect = false;

            //default Accept header for Chrome Browser
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            //simulate a Chrome browser
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.79 Safari/537.1";

            //there could be some cookies in the response, so we need to havae a jar for them
            request.CookieContainer = new CookieContainer();
            var cookieCollection = new CookieCollection();
            request.CookieContainer.Add(cookieCollection);

            return request;
        }
    }
}

