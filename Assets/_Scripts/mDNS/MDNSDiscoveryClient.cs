using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zeroconf;
namespace SprayAR
{
    /// <summary>
    /// In the future, this class will be used to discover services on the local network using mDNS.
    /// It is currently not used in the project, since it's unclear whether or not the Meta Quest platform supports mDNS. 
    /// </summary>
    public class MDNSDiscoveryClient : MonoBehaviour
    {
        private List<string> _discoveredServices = new List<string>();
        private AndroidJavaObject multicastLock;

        void OnEnable()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            MulticastLock();
#endif

        }

        private async void Start()
        {
            await DiscoverServices();
        }

        //TODO: The correct service type (possibly user defined) should be used here. 
        private async Task DiscoverServices()
        {
            string serviceType = "_http._tcp.local.";
            Debug.Log("Discovering services...");
            // Discover services
            IReadOnlyList<IZeroconfHost> responses = await ZeroconfResolver.ResolveAsync(serviceType);
            Debug.Log($"Discovered {responses.Count} services");
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
#if UNITY_ANDROID && !UNITY_EDITOR
            ReleaseMulticastLock();
#endif
            foreach (var service in _discoveredServices)
            {
                Debug.Log($"Service: {service}");
            }
        }

        private async Task BrowseAllDomains()
        {
            ILookup<string, string> responses = await ZeroconfResolver.BrowseDomainsAsync();
            Debug.Log($"Discovered {responses.Count} domains");
            foreach (var domain in responses)
            {
                Debug.Log($"Domain: {domain.Key}");
                foreach (var service in domain)
                {
                    Debug.Log($"Service: {service}");
                }
            }
        }

        /// <summary>
        /// Acquires a multicast lock on Android to allow mDNS discovery to work.
        /// This is necessary ONLY on Android.
        /// </summary>
        void MulticastLock()
        {
            try
            {
                Debug.Log("Acquiring multicast lock...");
                using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (var wifiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi"))
                    {
                        multicastLock = wifiManager.Call<AndroidJavaObject>("createMulticastLock", "Zeroconf lock");
                        multicastLock.Call("acquire");
                        Debug.Log("Multicast lock acquired");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to acquire multicast lock: " + e.Message);
            }
        }

        /// <summary>
        /// Call this method to release the multicast lock. 
        /// Not releasing the lock likely causes battery drain.
        /// </summary>
        void ReleaseMulticastLock()
        {
            try
            {
                Debug.Log("Releasing multicast lock...");
                if (multicastLock != null)
                {
                    multicastLock.Call("release");
                    multicastLock = null;
                    Debug.Log("Multicast lock released");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to release multicast lock: " + e.Message);
            }
        }
    }
}
