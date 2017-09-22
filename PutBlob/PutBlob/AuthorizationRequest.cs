using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PutBlob
{
    public class AuthorizationRequest
    {
        private readonly string _username;
        private readonly string _password;

        public AuthorizationRequest(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public async Task<Response> Authorize()
        {
            var request = WebRequest.Create("https://miniauth.azurewebsites.net/api/account/login");
            request.ContentType = "application/json; charset=utf-8";
            request.Method = "POST";
            using (var writer = new StreamWriter(await request.GetRequestStreamAsync()))
            {
                writer.Write("{ \"Username\" : \"" + _username + "\", \"Password\" : \"" + _password + "\" }");
                writer.Flush();
            }
            using (var response = await request.GetResponseAsync())
                using (var stream = response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                        return new Response { ErrorMessage = 
                            JsonConvert.DeserializeObject<AuthorizationResponse>(await reader.ReadToEndAsync()).ErrorMessage };
        }
    }
}
