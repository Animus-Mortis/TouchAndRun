using UnityEngine;

namespace Game.Managers
{
    public class Spawner : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }

}