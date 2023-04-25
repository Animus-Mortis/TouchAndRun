using Game.Player;
using TMPro;
using UnityEngine;

namespace Game.Menu
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerText;

        public void SetPlayer(string name)
        {
            playerText.text = name;
        }
    }
}