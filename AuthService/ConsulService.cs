using Consul;

namespace AuthService
{
    public class ConsulService : IHostedService
    {
        private readonly IConsulClient _consulClient;
        private readonly IConfiguration _configuration;
        private string _registrationId;

        public ConsulService(IConsulClient consulClient, IConfiguration configuration)
        {
            _consulClient = consulClient;
            _configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var uri = new Uri(_configuration["Consul:Address"]);
            var registration = new AgentServiceRegistration()
            {
                ID = _configuration["Consul:ServiceId"],
                Name = _configuration["Consul:ServiceName"],
                Address = uri.Host,
                Port = uri.Port,
                Check = new AgentServiceCheck()
                {
                    HTTP = $"{uri.Scheme}://{uri.Host}:{uri.Port}{_configuration["Consul:HealthCheck"]}",
                    Interval = TimeSpan.Parse(_configuration["Consul:HealthCheckInterval"]),
                    Timeout = TimeSpan.FromSeconds(5),
                    // Uncomment if using self-signed certificates
                   // TLSSkipVerify = true
                }
            };

            _registrationId = registration.ID;
            await _consulClient.Agent.ServiceRegister(registration, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_registrationId != null)
            {
                await _consulClient.Agent.ServiceDeregister(_registrationId, cancellationToken);
            }
        }
    }


}
