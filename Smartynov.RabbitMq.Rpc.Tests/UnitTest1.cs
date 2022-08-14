using Smartynov.RabbitMq.Rpc;

namespace Smartynov.RabbitMq.Rpc.Tests;

[TestFixture]
public class RabbitMqBasicServerTests
{
    private BasicRabbitMqRpcServer _server;

    [SetUp]
    public void Setup()
    {
        _server = new BasicRabbitMqRpcServer(new RabbitMqConnectionProperties());
    }

    [Test]
    public void Test1()
    {
        // TODO: implement
    }
}