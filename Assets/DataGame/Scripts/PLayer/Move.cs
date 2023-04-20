using UnityEngine;

namespace Game.Player
{
    public class Move : MonoBehaviour
    {
        [SerializeField] private float speed;

        private CharacterController controller;

        private void OnValidate()
        {
            if (GetComponent<CharacterController>())
                controller = GetComponent<CharacterController>();
            else
                Debug.LogError("Add CharacterController to Player");
        }

        private void FixedUpdate()
        {
            Run();
        }

        private void Run()
        {
            float translateX = Input.GetAxisRaw("Vertical")  * speed;
            float translateY = Input.GetAxisRaw("Horizontal")  * speed;

            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            controller.SimpleMove(forward * translateX + right * translateY);
        }
    }
}