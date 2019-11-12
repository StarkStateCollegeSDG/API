using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HttpClientSample
{



    class Type
    {
        public string chooseType()
        {
            string type = "";

            try
            {
                Console.WriteLine("Please enter the number for the type of query you are trying to make:\n\n" +
                    "[1. Email] [2. Username] [3. Password]");

                int queryChoice = Convert.ToInt32(Console.ReadLine());


                switch (queryChoice)
                {
                    case 1:
                        type = "email";
                        break;
                    case 2:
                        type = "username";
                        break;
                    case 3:
                        type = "password";
                        break;
                    default:
                        Console.WriteLine("Error");
                        chooseType();
                        break;
                }
            }
            catch
            {
                Console.WriteLine("Invalid input.");
                chooseType();

            }
            return type;

        }



    }

    class Query
    {
        public string defineQuery(string type)
        {
            string example = "";
            if (type == "email")
                example = "[i.e. john.smith@email.com]";
            else if (type == "username")
                example = "[i.e. johnnyboy123]";
            else if (type == "password")
                example = "[i.e. password1]";

            Console.WriteLine("Please type the " + type + " you are trying to locate in the database:\n" + example);
            string query = (Console.ReadLine().ToString());
            return query;
        }

    }

    class Program
    {




        // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
        static readonly HttpClient client = new HttpClient();


        static async Task Main()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {



                string type = ""; // query parameter type
                string query = ""; // query parameter query

                var t = new Type();
                type = t.chooseType();

                var q = new Query();
                query = q.defineQuery(type);






                string url = $"https://api.weleakinfo.com/v3/public/{ type }/{ query }?details=false"; // api url with query parameters set to variables "type" and "query"


                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "095b1889420b93b711c8b5643949751f883837a0"); //public API key BEARER TOKEN Authentication Header REQUIRED
                client.DefaultRequestHeaders.UserAgent.ParseAdd("User - Agent C# App"); // User-Agent - Name Header REQUIRED



                string rawResult = await client.GetStringAsync(url); //calling the api with the GET request

                dynamic dsResult = JsonConvert.DeserializeObject(rawResult);  //used to deserialize JSON into a dynamic object that can now be parsed through. class uses a "dynamic" variable type

                dynamic result = dsResult.Data; // .data stricly pulls the results from the DATA section of the JSON so you only output that instead of the entire object

                result = result.ToString(); // allows for str.contains function to find any matching strings

                if (result == "[]") // check for no results
                {
                    result = "No results found.";
                }

                Console.WriteLine("Matched leaks for " + query + " found in the following data breaches: \n" + result.Trim(new Char[] { '{', '}' }) + " "); // outputting the result // trimming away json brackets



                /* if (result.Contains("Roll20") == true)
                 {
                     Console.WriteLine("This database was leaked in 20xx.\n");  // For finding matching strings with leak data
                 }
                 */
                 

                Console.ReadLine();
            }
            catch (HttpRequestException e) // Throws an error for a bad html request
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                Console.ReadLine();
            }
        }
    }
}