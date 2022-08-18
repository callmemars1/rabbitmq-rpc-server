using System.Reflection.PortableExecutable;
using Smartynov.RabbitMq.Rpc;

namespace Smartynov.RabbitMq.Rpc.Tests;

[TestFixture]
public class RabbitMqConnectionPropertiesTest
{
    [Test]
    [TestCase(
        true, 
        "sub.domain.com", 
        "myusername", 
        "mypassword", 
        null, 
        null, 
        "amqps://myusername:mypassword@sub.domain.com"
        )
    ]
    [TestCase(
        false, 
        "178.228.54.1", 
        "myusername", 
        "mypassword", 
        "myvhost", 
        56, 
        "amqp://myusername:mypassword@178.228.54.1:56/myvhost"
        )
    ]
    public void ToUri_Test
    (
        bool secure,
        string hostname,
        string username,
        string password,
        string? virtualhost,
        int? port,
        Uri result
    )
    {
        var properties = new RabbitMqConnectionProperties
        {
            Secure = secure,
            Hostname = hostname,
            Username = username,
            Password = password,
            VirtualHost = virtualhost,
            Port = port,
        };
        var uri = properties.ToUri();
        Assert.That(uri, Is.EqualTo(result));
    }
}