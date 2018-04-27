using System;
using System.ComponentModel;

#if ENABLE_UNET

namespace UnityEngine.Networking
{
	[AddComponentMenu("Network/NetworkManagerHUD")]
	[RequireComponent(typeof(NetworkManager))]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class NetworkManagerHUD : MonoBehaviour
	{


		public NetworkManager manager;

		void Awake()
		{
			manager = GetComponent<NetworkManager>();
		}

		void Update()
		{
			if (!manager.IsClientConnected() && !NetworkServer.active && manager.matchMaker == null)
			{
				if (UnityEngine.Application.platform != RuntimePlatform.WebGLPlayer)
				{
					manager.StartHost();
				}
			}
		}
	}
}
#endif //ENABLE_UNET