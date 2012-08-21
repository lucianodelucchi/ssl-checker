using System;
using System.Net;
using CommandLine;
using CommandLine.Text;


namespace ssl_checker
{
	class MainClass
	{
		
		public static void Main (string[] args)
		{

            var options = new Options();

            if (0 == args.Length)
            {
                Console.WriteLine(options.GetUsage());
                Environment.Exit(1);
            }


            if (!CommandLineParser.Default.ParseArguments(args, options))
                Environment.Exit(1);

            try
            {
                DoCoreTask(options);

                Environment.Exit(0);

            } catch (Exception ex)
            {
                Console.WriteLine (ex.Message);
                Environment.Exit(1); 
            }

		}

		private static void DoCoreTask(Options options)
        {
            Utillty.PrepareServicePointManager();

            foreach (var url in options.Urls)
            {
                GetSslInfo(Utillty.FormatURL(url));
            }
			
        }

		static void GetSslInfo(Uri requestUri, CookieCollection cookieCollection = null)
        {
            HttpWebResponse response = null;

            try
            {
                                         
                var request = Utillty.PrepareRequest(requestUri, cookieCollection);

                // Get the response object
                response = (HttpWebResponse)request.GetResponse();

                ServicePoint currentServicePoint = request.ServicePoint;
                var SSLCertificate = currentServicePoint.Certificate;

                if (null == SSLCertificate)
                {
                    Console.WriteLine(string.Format("Couldn't find a SSL certificate for {0}", requestUri));
                    return;
                }

                Console.WriteLine(string.Format("SSL Certificate for {0}, expiration date: {1}", requestUri, SSLCertificate.GetExpirationDateString()));
            } 
            catch (WebException ex)
            {

                response = (HttpWebResponse)ex.Response;

                //for some sites behind CDN a Forbidden response with session cookies is returned,
                //we have to send another request with the cookies set in order to get the response
                if (null != response && response.StatusCode == HttpStatusCode.Forbidden && response.Cookies.Count > 0)
                {
                    GetSslInfo(response.ResponseUri, response.Cookies);

                } else
                {         
                    Console.WriteLine(string.Format("{0}, URL: {1}", ex.Message, requestUri));         
                }
                   
            } 
            finally
            {
                if (null != response)
                {
                    response.Close();
                }
            }

		}

	}
}
