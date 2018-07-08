using System.Net;
using System.Threading.Tasks;
using Blaster.WebApi.Controllers;
using Xunit;
using Moq;
using k8s;
using Microsoft.Rest;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;

namespace Blaster.Tests
{
    public class TestNamespacesController
    {
        private NamespacesController _namespacesController;
        private Mock<IKubernetes> _kubernetesClient;

        public TestNamespacesController()
        {
            _kubernetesClient = new Mock<IKubernetes>();
            _namespacesController = new NamespacesController(_kubernetesClient.Object);
            
        }

        [Fact]
        public async Task get_returns_action_result_with_ienumerable_string()
        {
            List<k8s.Models.V1Namespace> items = new List<k8s.Models.V1Namespace>();
            k8s.Models.V1ObjectMeta meta1 = new k8s.Models.V1ObjectMeta { Name = "default" };
            k8s.Models.V1Namespace item1 = new k8s.Models.V1Namespace { Metadata = meta1 };
            items.Add(item1);

            k8s.Models.V1ObjectMeta meta2 = new k8s.Models.V1ObjectMeta { Name = "kube-system" };
            k8s.Models.V1Namespace item2 = new k8s.Models.V1Namespace { Metadata = meta2 };
            items.Add(item2);

            System.Net.Http.HttpRequestMessage req = new System.Net.Http.HttpRequestMessage { };
            System.Net.Http.HttpResponseMessage res = new System.Net.Http.HttpResponseMessage { StatusCode = HttpStatusCode.OK };

            k8s.Models.V1NamespaceList ls = new k8s.Models.V1NamespaceList { ApiVersion = "V1", Items = items, Kind = "NamespaceList", Metadata = It.IsAny<k8s.Models.V1ListMeta>() };
            _kubernetesClient.Setup(x => x.ListNamespaceWithHttpMessagesAsync(null, null, null, null, null, null, null, null, null, null, It.IsAny<CancellationToken>())).ReturnsAsync(new HttpOperationResponse<k8s.Models.V1NamespaceList> { Body = ls, Request = req, Response = res });
            var results = _namespacesController.Get();

            var t1 = Assert.IsType<ActionResult<IEnumerable<string>>>(results);
        }

        [Fact]
        public async Task get_returns_multiple_namespaces()
        {
            List<k8s.Models.V1Namespace> items = new List<k8s.Models.V1Namespace>();
            k8s.Models.V1ObjectMeta meta1 = new k8s.Models.V1ObjectMeta { Name = "default" };
            k8s.Models.V1Namespace item1 = new k8s.Models.V1Namespace { Metadata = meta1 };
            items.Add(item1);

            k8s.Models.V1ObjectMeta meta2 = new k8s.Models.V1ObjectMeta { Name = "kube-system" };
            k8s.Models.V1Namespace item2 = new k8s.Models.V1Namespace { Metadata = meta2 };
            items.Add(item2);

            System.Net.Http.HttpRequestMessage req = new System.Net.Http.HttpRequestMessage { };
            System.Net.Http.HttpResponseMessage res = new System.Net.Http.HttpResponseMessage { StatusCode = HttpStatusCode.OK };

            k8s.Models.V1NamespaceList ls = new k8s.Models.V1NamespaceList { ApiVersion = "V1", Items = items, Kind = "NamespaceList", Metadata = It.IsAny<k8s.Models.V1ListMeta>() };
            _kubernetesClient.Setup(x => x.ListNamespaceWithHttpMessagesAsync(null, null, null, null, null, null, null, null, null, null, It.IsAny<CancellationToken>())).ReturnsAsync(new HttpOperationResponse<k8s.Models.V1NamespaceList> { Body = ls, Request = req, Response = res });
            var results = _namespacesController.Get();

            var t1 = Assert.IsType<ActionResult<IEnumerable<string>>>(results);
            List<string> vals = new List<string>(t1.Value);
            Assert.True(vals.Count > 1);
        }

        [Fact]
        public async Task get_returns_namespace_names()
        {
            List<k8s.Models.V1Namespace> items = new List<k8s.Models.V1Namespace>();
            k8s.Models.V1ObjectMeta meta1 = new k8s.Models.V1ObjectMeta { Name = "default" };
            k8s.Models.V1Namespace item1 = new k8s.Models.V1Namespace { Metadata = meta1 };
            items.Add(item1);

            k8s.Models.V1ObjectMeta meta2 = new k8s.Models.V1ObjectMeta { Name = "kube-system" };
            k8s.Models.V1Namespace item2 = new k8s.Models.V1Namespace { Metadata = meta2 };
            items.Add(item2);

            System.Net.Http.HttpRequestMessage req = new System.Net.Http.HttpRequestMessage { };
            System.Net.Http.HttpResponseMessage res = new System.Net.Http.HttpResponseMessage { StatusCode = HttpStatusCode.OK };

            k8s.Models.V1NamespaceList ls = new k8s.Models.V1NamespaceList { ApiVersion = "V1", Items = items, Kind = "NamespaceList", Metadata = It.IsAny<k8s.Models.V1ListMeta>() };
            _kubernetesClient.Setup(x => x.ListNamespaceWithHttpMessagesAsync(null, null, null, null, null, null, null, null, null, null, It.IsAny<CancellationToken>())).ReturnsAsync(new HttpOperationResponse<k8s.Models.V1NamespaceList> { Body = ls, Request = req, Response = res });
            var results = _namespacesController.Get();

            var t1 = Assert.IsType<ActionResult<IEnumerable<string>>>(results);
            List<string> vals = new List<string>(t1.Value);
            Assert.True(vals[0] == "default");
        }
    }
}