using UnityEngine;

namespace Game.Menu
{
    public class CanvasOn : MonoBehaviour
    {
        [SerializeField] private GameObject canvas;

        private void Start()
        {
            canvas.SetActive(true);
        }
    }
}