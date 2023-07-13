using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static DollarsToWords.Converter;

namespace DollarsToWords
{
    public class Program
    {
        public static void Main()
        {
            var httpListener = new HttpListener();
            httpListener.Prefixes.Add("http://localhost:5000/");

            httpListener.Start();

            Console.WriteLine("Waiting for incoming requests..");

            while (true)
            {
                HttpListenerContext context = httpListener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                string method = request.HttpMethod;

                Console.WriteLine($"HTTP-Methode: {method}");

                string error = string.Empty;

                // POST
                if (method == "POST")
                {
                    using (Stream stream = request.InputStream)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string requestBody = reader.ReadToEnd();

                            if(requestBody != null || !string.IsNullOrEmpty(requestBody))
                            {
                                Console.WriteLine($"Convert {requestBody} to text..");

                                string amountString;

                                decimal amount = ConvertStringToDecimal(requestBody, ref error);

                                if (!string.IsNullOrEmpty(error))
                                {
                                    amountString = error;
                                }
                                else
                                {
                                    amountString = ConvertDollarsToWords(amount, ref error);

                                    if(!string.IsNullOrEmpty(error))
                                    {
                                        amountString = error;
                                    }
                                }

                                Console.WriteLine($"{amount} = {amountString}");

                                // Antwort an den Client senden
                                byte[] responseBytes = Encoding.UTF8.GetBytes(amountString);
                                response.ContentType = "text/plain";
                                response.ContentLength64 = responseBytes.Length;
                                response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
                            }
                        }
                    }
                }
                // GET
                //else if (method == "GET")
                //{

                //}

                response.Close();
            }

            //string amountString = "100 500,09"; // Beispiel

            //decimal amount = ConvertStringToDecimal(amountString);

            //string amountInWords = ConvertDollarsToWords(amount);

            //Console.WriteLine(amountInWords);
        }
    }
}
