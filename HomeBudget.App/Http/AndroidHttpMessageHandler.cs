namespace HomeBudget.App.Http
{
    public class AndroidHttpMessageHandler : IPlatformHttpMessageHandler
    {
        //public HttpMessageHandler GetHttpMessageHandler() => new HttpClientHandler
        //{
        //    ServerCertificateCustomValidationCallback = (httpRequestMessage, certificate, chain, sslPolicyErrors) =>
        //        certificate?.Issuer == "CN=localhost" || sslPolicyErrors == SslPolicyErrors.None
        //};

        public HttpMessageHandler GetHttpMessageHandler()
        {
            return new HttpClientHandler();
        }
    }
}