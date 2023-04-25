using UnityEngine;

namespace Game.Player
{
    public class Rotate : MonoBehaviour
    {
        [SerializeField] private float speedRotation;

        private void Update()
        {
            RotationPlayer();
        }

        private void RotationPlayer()
        {
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * speedRotation);
        }
    }
}