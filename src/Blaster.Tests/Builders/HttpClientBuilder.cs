using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Blaster.Tests.TestDoubles;
using Blaster.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blaster.Tests.Builders
{
    public class HttpClientBuilder
    {
        private readonly Dictionary<Type, ServiceDescriptor> _serviceDescriptors = new Dictionary<Type, ServiceDescriptor>();
        private string _apiKey;

        public HttpClientBuilder()
        {
            _apiKey = "123";
            WithService(typeof(IApiKeyValidator), new StubApiKeyValidator(isValid: true));
        }

        public HttpClientBuilder WithApiKey(string apiKey)
        {
            _apiKey = apiKey;
            return this;
        }

        public HttpClientBuilder WithService(Type serviceType, object serviceInstance)
        {
            _serviceDescriptors.Remove(serviceType);
            _serviceDescriptors.Add(serviceType, ServiceDescriptor.Singleton(serviceType, serviceInstance));

            return this;
        }

        public HttpClient Build()
        {
            var webHostBuilder = new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureTestServices(services =>
                {
                    _serviceDescriptors
                        .Values
                        .ToList()
                        .ForEach(serviceOverride => services.Replace(serviceOverride));
                });

            var server = new TestServer(webHostBuilder);
            var client = server.CreateClient();

            if (!string.IsNullOrWhiteSpace(_apiKey))
            {
                client.DefaultRequestHeaders.Add("x-dfds-apikey", _apiKey);
            }

            return client;
        }
    }
}