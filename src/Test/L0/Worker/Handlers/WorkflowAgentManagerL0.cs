using System;
using System.IO;
using System.Threading.Tasks;
using GitHub.Runner.Worker;
using GitHub.Runner.Worker.Handlers;
using Moq;
using Xunit;

namespace GitHub.Runner.Common.Tests.Worker.Handlers
{
    public sealed class WorkflowAgentManagerL0
    {
        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public async Task SyncFileToWorkflowPodAsync_ReturnsEarlyWhenNoSharedVolumeDisabled()
        {
            using (var hostContext = new TestHostContext(this))
            {
                var manager = new WorkflowAgentManager();
                manager.Initialize(hostContext);

                var ec = new Mock<IExecutionContext>();
                await manager.SyncFileToWorkflowPodAsync(ec.Object, "somepath");
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public async Task SyncDirectoryToWorkflowPodAsync_ReturnsEarlyWhenNoSharedVolumeDisabled()
        {
            using (var hostContext = new TestHostContext(this))
            {
                var manager = new WorkflowAgentManager();
                manager.Initialize(hostContext);

                var ec = new Mock<IExecutionContext>();
                await manager.SyncDirectoryToWorkflowPodAsync(ec.Object, "somedir");
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public void InitializeFileCommand_ReturnsEarlyWhenNoSharedVolumeDisabled()
        {
            using (var hostContext = new TestHostContext(this))
            {
                var manager = new WorkflowAgentManager();
                manager.Initialize(hostContext);

                var ec = new Mock<IExecutionContext>();
                manager.InitializeFileCommand(ec.Object, null, "somepath", "context");
            }
        }

        [Fact]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        public async Task SyncFileCommandsFromWorkflowPodAsync_ReturnsEarlyWhenNoSharedVolumeDisabled()
        {
            using (var hostContext = new TestHostContext(this))
            {
                var manager = new WorkflowAgentManager();
                manager.Initialize(hostContext);

                var ec = new Mock<IExecutionContext>();
                await manager.SyncFileCommandsFromWorkflowPodAsync(ec.Object, null, "dir", "suffix", null);
            }
        }

        [Theory]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        [InlineData("/w/repo/repo/ci_connection/", "/w/repo/repo")]
        [InlineData("/w/repo/repo/ci_connection", "/w/repo/repo")]
        [InlineData("/w/repo/repo/ci_connection//", "/w/repo/repo")]
        [InlineData("/w/repo/repo/./ci_connection/", "/w/repo/repo")]
        [InlineData("/w/repo/repo/a/b/", "/w/repo/repo/a")]
        public void GetHostExtractionDirectory_HandlesTrailingSlashes(string input, string expected)
        {
            Assert.Equal(Path.GetFullPath(expected), WorkflowAgentManager.GetHostExtractionDirectory(input));
        }

        [Theory]
        [Trait("Level", "L0")]
        [Trait("Category", "Worker")]
        [InlineData("/w/_actions", "/w/_actions/anchore/sbom-action/e22c38/download-syft", "/w/_actions/anchore/sbom-action/e22c38")]
        [InlineData("/w/_actions/", "/w/_actions/anchore/sbom-action/e22c38/download-syft/", "/w/_actions/anchore/sbom-action/e22c38")]
        [InlineData("/w/_actions", "/w/_actions/owner/repo/ref/nested/sub/path", "/w/_actions/owner/repo/ref")]
        [InlineData("/w/_actions", "/w/_actions/actions/checkout/v4", "/w/_actions/actions/checkout/v4")]
        [InlineData("/w/_actions", "/w/_actions/actions/checkout/v4/", "/w/_actions/actions/checkout/v4")]
        [InlineData("/w/_actions", "/w/repo/repo/local-action", "/w/repo/repo/local-action")]
        [InlineData("/w/_actions", "/w/_actions_other/owner/repo/ref/sub", "/w/_actions_other/owner/repo/ref/sub")]
        public void GetDirectoryToSync_ResolvesActionRepoRoot(string actionsDir, string input, string expected)
        {
            Assert.Equal(Path.GetFullPath(expected), WorkflowAgentManager.GetDirectoryToSync(actionsDir, input));
        }
    }
}
