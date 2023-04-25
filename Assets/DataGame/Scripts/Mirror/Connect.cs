using Mirror;
using UnityEngine;

namespace Game.Mirror
{
    public class Connect : MonoBehaviour
    {
        public NetworkManager networkManager;

        private void Start()
        {
            if (!Application.isBatchMode)
            {
                networkManager.StartClient();
            }
        }

        public void JoinClient()
        {
            networkManager.networkAddress = "localhost";
            networkManager.StartClient();
        }
    }
}