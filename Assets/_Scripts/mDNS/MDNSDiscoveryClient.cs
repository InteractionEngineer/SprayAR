using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zeroconf;
namespace SprayAR
{
    public class MDNSDiscoveryClient : MonoBehaviour
    {
        private List<string> _discoveredServices = new List<string>();

        private async void Start()
        {
            await DiscoverServices();
        }

        private async Task DiscoverServices()
        {
            string serviceType = "_http._tcp.local.";

            // Discover services
            IReadOnlyList<IZeroconfHost> responses = await ZeroconfResolver.ResolveAsync(serviceType);

            foreach (var host in responses)
            {
                foreach (var service in host.Services)
                {
                    string serviceName = service.Key;
                    if (!_discoveredServices.Contains(serviceName))
                    {
                        _discoveredServices.Add(serviceName);
                        Debug.Log($"Service discovered: {serviceName} on {host.IPAddress}");
                    }
                }
            }
        }

        void Update()
        {
            // TODO: Currently, removal of services are not detected
            foreach (var service in _discoveredServices)
            {
                Debug.Log($"Service: {service}");
            }
        }
    }
}
