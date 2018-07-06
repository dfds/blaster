using System.Net;
using System.Threading.Tasks;
using Blaster.WebApi;
using Blaster.WebApi.Controllers;
using Xunit;
using Moq;
using k8s;

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
        public async Task get_returns_action_result()
        {
            //var results = new k8s.Models.V1NamespaceList
            _kubernetesClient.Setup(x => x.ListNamespace(null, null, null, null, null, null, null, null, null)).Returns(new k8s.Models.V1NamespaceList{ApiVersion="V1"});
            var results = _namespacesController.Get();

            Assert.IsType<Microsoft.AspNetCore.Mvc.ActionResult>(results);
        }

    }
}