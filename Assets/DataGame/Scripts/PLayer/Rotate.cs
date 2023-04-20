using UnityEngine;

namespace Game.Player
{
    public class Rotate : MonoBehaviour
    {
        [SerializeField] private float speedRotation;

        private void FixedUpdate()
        {
            RotationPlayer();
        }

        private void RotationPlayer()
        {
            float mx = Input.GetAxis("Mouse X");

            transform.Rotate(Vector3.up * mx * speedRotation);
        }
    }
}