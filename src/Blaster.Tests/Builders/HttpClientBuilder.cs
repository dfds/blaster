using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Blaster.WebApi;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Blaster.Tests.Builders
{
    public class HttpClientBuilder : IDisposable
    {
        private readonly LinkedList<IDisposable> _disposables = new LinkedList<IDisposable>();
        private readonly Dictionary<Type, ServiceDescriptor> _serviceDescriptors = new Dictionary<Type, ServiceDescriptor>();

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
            WithService<IAuthenticationHandlerProvider>(new StubAuthenticationHandlerProvider());

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

            customizations.Add(client =>
            {
                client.Timeout = TimeSpan.FromMinutes(1);
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

    public class StubAuthenticationHandlerProvider : IAuthenticationHandlerProvider
    {
        public Task<IAuthenticationHandler> GetHandlerAsync(HttpContext context, string authenticationScheme)
        {
            return Task.FromResult((IAuthenticationHandler) new StubAuthenticationRequestHandler());
        }

        private class StubAuthenticationRequestHandler : IAuthenticationRequestHandler
        {
            public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
            {
                throw new NotImplementedException();
            }

            public Task ChallengeAsync(AuthenticationProperties properties)
            {
                throw new NotImplementedException();
            }

            public Task ForbidAsync(AuthenticationProperties properties)
            {
                throw new NotImplementedException();
            }

            public Task<AuthenticateResult> AuthenticateAsync()
            {
                var identity = new ClaimsIdentity(
                    claims: Enumerable.Empty<Claim>(),
                    authenticationType: "not important"
                );

                var ticket = new AuthenticationTicket(
                    principal: new ClaimsPrincipal(identity),
                    authenticationScheme: "also not important!"
                );

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            public Task<bool> HandleRequestAsync()
            {
                return Task.FromResult(false);
            }
        }
    }
}