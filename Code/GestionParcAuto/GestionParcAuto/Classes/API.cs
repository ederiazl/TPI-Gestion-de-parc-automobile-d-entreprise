namespace GestionParcAuto.Classes
{
    public class API
    {
        /// <summary>
        /// Used to build a request
        /// </summary>
        /// <param name="parameters">parameters of the request</param>
        /// <param name="body">body of ther request</param>
        /// <param name="action">action in the url</param>
        /// <param name="urlComplement">complement in url f.ex. %20brewing%20company</param>
        /// <param name="method">method of the request</param>
        /// <returns></returns>
        public static HttpRequestMessage BuildRequest(Dictionary<string, string>? parameters, Dictionary<string, string>? body, Dictionary<string, string>? headers, string defaultUrl, string action, string urlComplement, HttpMethod method)
        {
            //Build Url
            string url = defaultUrl + action;

            if (parameters != null)
            {
                foreach (var item in parameters)
                {
                    if (parameters.ToList().IndexOf(item) == 0)
                        url += "?";
                    else
                        url += "&";
                    url += $"{item.Key}={item.Value}";
                }
            }

            url += urlComplement;

            //Build Body
            string bodyString = "{";
            if (body != null)
            {
                foreach (var item in body)
                {
                    bodyString += $"{item.Key}={item.Value},";
                }
            }
            bodyString = "}";
            JsonContent content = JsonContent.Create(bodyString);

            //Build Request With Info gotten
            HttpRequestMessage req = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = method,
                Content = content
            };

            foreach (var item in headers)
            {
                req.Headers.Add(item.Key, item.Value);
            }

            return req;
        }
    }
}
