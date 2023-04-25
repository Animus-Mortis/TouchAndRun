using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Menu
{
    public class EnterName : NetworkBehaviour
    {
        [SerializeField] private NetworkManager networkManager;
        [SerializeField] private InputField nameInput;
        [SerializeField] private int firstTime = 1;

        [SyncVar] public string DisplayName;

        public void SaveName()
        {
            DisplayName = nameInput.text;
        }
    }
}