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
    public class HttpClientBuilder : IDisposable
    {
        private readonly LinkedList<IDisposable> _disposables = new LinkedList<IDisposable>();
        private readonly Dictionary<Type, ServiceDescriptor> _serviceDescriptors = new Dictionary<Type, ServiceDescriptor>();
        private string _apiKey;

        public HttpClientBuilder()
        {
            _apiKey = "123";
            WithService<IApiKeyValidator>(new StubApiKeyValidator(isValid: true));
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

        public HttpClientBuilder WithService<TService>(TService serviceInstance)
        {
            return WithService(typeof(TService), serviceInstance);
        }

        private IWebHostBuilder CreateWebHostBuilder()
        {
            return new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureTestServices(services =>
                {
                    _serviceDescriptors
                        .Values
                        .ToList()
                        .ForEach(serviceOverride => services.Replace(serviceOverride));
                });
        }

        private List<Action<HttpClient>> CreateCustomizations()
        {
            var customizations = new List<Action<HttpClient>>();

            if (!string.IsNullOrWhiteSpace(_apiKey))
            {
                customizations.Add(x =>
                {
                    x.DefaultRequestHeaders.Add("x-dfds-apikey", _apiKey);
                });
            }

            customizations.Add(x =>
            {
                x.Timeout = TimeSpan.FromMinutes(1);
            });

            return customizations;
        }

        public HttpClient Build()
        {
            var webHostBuilder = CreateWebHostBuilder();
            var testServer = new TestServer(webHostBuilder);
            _disposables.AddLast(testServer);

            var customizations = CreateCustomizations();
            var client = testServer.CreateClient();
            customizations.ForEach(x => x(client));
            _disposables.AddLast(client);

            return client;
        }

        public void Dispose()
        {
            foreach (var instance in _disposables.Reverse())
            {
                instance.Dispose();
            }
        }
    }
}