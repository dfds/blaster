using System.Threading.Tasks;
using Blaster.Tests.Builders;
using Blaster.Tests.TestDoubles;
using Blaster.WebApi.Features.Namespaces;
using Xunit;

namespace Blaster.Tests
{
    public class TestNamespaceController
    {
        [Fact]
        public async Task get_returns_expected_when_no_namespaces_are_available()
        {
            var emptyNamespaceList = new Namespace[0];

            var sut = new NamespaceController(new StubNamespaceRepository(emptyNamespaceList));
            var result = await sut.Get();
            
            Assert.Empty(result.Value.Items);
        }

        [Fact]
        public async Task get_returns_expected_when_multiple_namespaces_are_available()
        {
            var expectedNamespaces = new[]
            {
                new NamespaceBuilder().Build(), 
                new NamespaceBuilder().Build(), 
            };

            var sut = new NamespaceController(new StubNamespaceRepository(expectedNamespaces));
            var result = await sut.Get();
            
            Assert.Equal(expectedNamespaces, result.Value.Items);
        }
    }

    #region old test case

    //public class TestNamespacesController
    //{
    //    private NamespacesController _namespacesController;
    //    private Mock<IKubernetes> _kubernetesClient;

    //    public TestNamespacesController()
    //    {
    //        _kubernetesClient = new Mock<IKubernetes>();
    //        _namespacesController = new NamespacesController(_kubernetesClient.Object);

    //    }

    //    [Fact]
    //    public async Task get_returns_expected_when_multiple_namespaces_are_available()
    //    {
    //        var namespaces = new List<V1Namespace>();

    //        var item1 = new V1Namespace
    //        {
    //            Metadata = new V1ObjectMeta {Name = "default"}
    //        };
    //        namespaces.Add(item1);

    //        var item2 = new V1Namespace
    //        {
    //            Metadata = new V1ObjectMeta { Name = "kube-system" }
    //        };
    //        namespaces.Add(item2);

    //        var namespaceList = new V1NamespaceList
    //        {
    //            ApiVersion = "V1",
    //            Items = namespaces,
    //            Kind = "NamespaceList",
    //            Metadata = It.IsAny<V1ListMeta>()
    //        };

    //        var req = new HttpRequestMessage();
    //        var res = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

    //        _kubernetesClient
    //            .Setup(x => x.ListNamespaceWithHttpMessagesAsync(null, null, null, null, null, null, null, null, null, null, It.IsAny<CancellationToken>()))
    //            .ReturnsAsync(new HttpOperationResponse<V1NamespaceList> { Body = namespaceList, Request = req, Response = res });

    //        var results = await _namespacesController.Get();

    //        var t1 = Assert.IsType<OkObjectResult>(results);
    //        var t2 = Assert.IsType<List<string>>(t1.Value);
    //        Assert.True(t2.Count > 1);
    //    }

    //    [Fact]
    //    public async Task get_returns_namespace_names()
    //    {
    //        List<V1Namespace> items = new List<V1Namespace>();
    //        V1ObjectMeta meta1 = new V1ObjectMeta { Name = "default" };
    //        V1Namespace item1 = new V1Namespace { Metadata = meta1 };
    //        items.Add(item1);

    //        V1ObjectMeta meta2 = new V1ObjectMeta { Name = "kube-system" };
    //        V1Namespace item2 = new V1Namespace { Metadata = meta2 };
    //        items.Add(item2);

    //        HttpRequestMessage req = new HttpRequestMessage();
    //        HttpResponseMessage res = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

    //        V1NamespaceList ls = new V1NamespaceList { ApiVersion = "V1", Items = items, Kind = "NamespaceList", Metadata = It.IsAny<V1ListMeta>() };
    //        _kubernetesClient.Setup(x => x.ListNamespaceWithHttpMessagesAsync(null, null, null, null, null, null, null, null, null, null, It.IsAny<CancellationToken>())).ReturnsAsync(new HttpOperationResponse<V1NamespaceList> { Body = ls, Request = req, Response = res });
    //        var results = await _namespacesController.Get();

    //        var t1 = Assert.IsType<OkObjectResult>(results);
    //        var t2 = Assert.IsType<List<string>>(t1.Value);
    //        Assert.Contains("default", t2);
    //    }

    //    [Fact]
    //    public async Task get_returns_ok_status_code()
    //    {
    //        List<V1Namespace> items = new List<V1Namespace>();
    //        V1ObjectMeta meta1 = new V1ObjectMeta { Name = "default" };
    //        V1Namespace item1 = new V1Namespace { Metadata = meta1 };
    //        items.Add(item1);

    //        V1ObjectMeta meta2 = new V1ObjectMeta { Name = "kube-system" };
    //        V1Namespace item2 = new V1Namespace { Metadata = meta2 };
    //        items.Add(item2);

    //        HttpRequestMessage req = new HttpRequestMessage();
    //        HttpResponseMessage res = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

    //        V1NamespaceList ls = new V1NamespaceList { ApiVersion = "V1", Items = items, Kind = "NamespaceList", Metadata = It.IsAny<V1ListMeta>() };
    //        _kubernetesClient.Setup(x => x.ListNamespaceWithHttpMessagesAsync(null, null, null, null, null, null, null, null, null, null, It.IsAny<CancellationToken>())).ReturnsAsync(new HttpOperationResponse<V1NamespaceList> { Body = ls, Request = req, Response = res });
    //        var results = await _namespacesController.Get();

    //        var t1 = Assert.IsType<OkObjectResult>(results);

    //        Assert.True(200 == t1.StatusCode);
    //    }

    //    [Fact]
    //    public async Task get_returns_500_status_code()
    //    {
    //        V1NamespaceList ls = new V1NamespaceList { ApiVersion = "V1", Items = It.IsAny<IList<V1Namespace>>(), Kind = "NamespaceList", Metadata = It.IsAny<V1ListMeta>() };
    //        _kubernetesClient.Setup(x => x.ListNamespaceWithHttpMessagesAsync(null, null, null, null, null, null, null, null, null, null, It.IsAny<CancellationToken>())).ReturnsAsync(new HttpOperationResponse<V1NamespaceList>());
    //        var results = await _namespacesController.Get();

    //        //var t1 = Assert.IsType<ActionResult<IEnumerable<string>>>(results);
    //        var t1 = Assert.IsType<StatusCodeResult>(results);

    //        Assert.True(500 == t1.StatusCode);
    //    }
    //}

    #endregion
}