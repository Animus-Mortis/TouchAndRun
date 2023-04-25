using System.Collections;
using UnityEngine;

namespace Game.Player
{
    public class ChangeColor : MonoBehaviour
    {
        [SerializeField] private Color touchColor;
        [SerializeField] private float timer;

        private PlayerState state;
        private Color standartColor;
        private MeshRenderer[] meshes;

        private void OnValidate()
        {
            if (!GetComponent<PlayerState>())
                gameObject.AddComponent<PlayerState>();
        }

        private void Awake()
        {
            meshes = GetComponentsInChildren<MeshRenderer>();
            standartColor = meshes[0].material.color;
            state = GetComponent<PlayerState>();
        }

        public void ChangeColorToCaught()
        {
            if (state.Status == PlayerStatus.Spurt || state.Status == PlayerStatus.Caught) return;

            state.NewStatus(PlayerStatus.Caught);

            foreach (var mesh in meshes)
            {
                mesh.material.color = touchColor;
            }

            StartCoroutine(TimerChangeColor());
        }

        private IEnumerator TimerChangeColor()
        {
            yield return new WaitForSeconds(timer);

            ChangeColorToStandart();
            state.SetNewState(PlayerStatus.Run);
        }

        private void ChangeColorToStandart()
        {
            foreach (var mesh in meshes)
            {
                mesh.material.color = standartColor;
            }
        }
    }
}