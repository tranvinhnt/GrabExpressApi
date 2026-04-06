using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Options;

namespace GrabExpressApi.SDK
{
    using Configuration;

    /// <summary>
    /// Provider that manages multiple instances of GrabExpressClient for different environments
    /// </summary>
    public class GrabExpressProvider
    {
        private readonly IOptionsMonitor<GrabExpressConfig> _configMonitor;
        private readonly ConcurrentDictionary<string, GrabExpressClient> _clients = new ConcurrentDictionary<string, GrabExpressClient>(StringComparer.OrdinalIgnoreCase);

        public GrabExpressProvider(IOptionsMonitor<GrabExpressConfig> configMonitor)
        {
            _configMonitor = configMonitor ?? throw new ArgumentNullException(nameof(configMonitor));
        }

        /// <summary>
        /// Gets the GrabExpressClient for a specific environment name (e.g. "UAT", "Production")
        /// </summary>
        public GrabExpressClient GetClient(string environmentName)
        {
            if (string.IsNullOrEmpty(environmentName))
            {
                environmentName = "Production";
            }

            return _clients.GetOrAdd(environmentName, name =>
            {
                var config = _configMonitor.Get(name);
                if (string.IsNullOrEmpty(config.ClientId))
                {
                    // Fallback to default if named config is missing or empty
                    config = _configMonitor.CurrentValue;
                }
                
                return new GrabExpressClient(config);
            });
        }
    }
}
