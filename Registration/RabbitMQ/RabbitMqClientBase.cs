using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace RabbitMQ
{
    public abstract class RabbitMqClientBase : IDisposable
    {
        protected IModel Channel { get; private set; }
        private IConnection _connection;
        private readonly ConnectionFactory _connectionFactory;
        private readonly ILogger<RabbitMqClientBase> _logger;

        protected RabbitMqClientBase(ConnectionFactory connectionFactory, ILogger<RabbitMqClientBase> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
            ConnectToRabbitMq();
        }

        private void ConnectToRabbitMq()
        {
            try
            {
                if (_connection == null || _connection.IsOpen == false)
                {
                    _connection = _connectionFactory.CreateConnection();
                }

                if (Channel == null || Channel.IsOpen == false)
                {
                    Channel = _connection.CreateModel();
                }
            }
            catch (Exception ex)
            {
               _logger.LogError(ex.Message, "Cannot dispose RabbitMQ channel or connection");
            }
        }

        public void Dispose()
        {
            try
            {
                Channel?.Close();
                Channel?.Dispose();
                Channel = null;

                _connection?.Close();
                _connection?.Dispose();
                _connection = null;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Cannot dispose RabbitMQ channel or connection");
            }
        }
    }
}
