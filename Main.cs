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

            //if no arguments
            if (0 == args.Length)
            {
                //show the help screen
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
                GetSslInfo(Utillty.PrepareRequest(Utillty.FormatURL(url)));
            }
			
        }

        public static void GetSslInfo(HttpWebRequest request)
        {

            HttpWebResponse response = null;

            try
            {
                // Get the response object
                response = (HttpWebResponse)request.GetResponse();

                ServicePoint currentServicePoint = request.ServicePoint;
                var SSLCertificate = currentServicePoint.Certificate;

                if (null == SSLCertificate)
                {
                    Console.WriteLine(string.Format("Couldn't find an SSL certificate for {0}", request.RequestUri.ToString()));
                    return;
                }

                Console.WriteLine(string.Format("SSL Certificate for {0}, expiration date: {1}", request.RequestUri.ToString(), SSLCertificate.GetExpirationDateString()));
            } 
            catch (WebException ex)
            {

                response = (HttpWebResponse)ex.Response;

                //for some sites behind CDN a Forbidden response with session cookies is returned,
                //we have to send another request with the cookies set in order to get the response
                if (null != response && response.StatusCode == HttpStatusCode.Forbidden && response.Cookies.Count > 0)
                {
                    GetSslInfo(Utillty.PrepareRequest(response.ResponseUri, response.Cookies));

                } else
                {         
                    Console.WriteLine(string.Format("{0}, URL: {1}", ex.Message, request.RequestUri.ToString()));         
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
