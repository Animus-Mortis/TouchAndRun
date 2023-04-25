using UnityEngine;

namespace Game.Player
{
    public class CameraTilt : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Vector2 maximumAngleOfInclination;

        private float yRotation;

        private void LateUpdate()
        {
            Tilt();
        }

        private void Tilt()
        {
            yRotation += Input.GetAxis("Mouse Y") * speed;
            yRotation = Mathf.Clamp(yRotation, -maximumAngleOfInclination.x, -maximumAngleOfInclination.y);
            transform.localEulerAngles = new Vector3(-yRotation, 0, 0);
        }
    }
}