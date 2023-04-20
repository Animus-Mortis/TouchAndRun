using System.Collections;
using UnityEngine;

namespace Game.Player
{
    public class Move : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float speedCaught;
        [SerializeField] private float DistanceToCaught;
        [SerializeField] private float timerRestartCaught;

        private CharacterController controller;
        private PlayerState state;
        private Coroutine caughtCororutine;
        private void OnValidate()
        {
            if (GetComponent<CharacterController>())
                controller = GetComponent<CharacterController>();
            else
                Debug.LogError("Add CharacterController to Player");

            if (!GetComponent<PlayerState>())
                gameObject.AddComponent<PlayerState>();

            state = GetComponent<PlayerState>();
        }

        private void FixedUpdate()
        {
            if (state.Status != PlayerStatus.Caught && Input.GetMouseButton(0) && caughtCororutine == null)
                caughtCororutine = StartCoroutine(Caught());
            else if (state.Status != PlayerStatus.Caught)
                Run();
        }

        private void Run()
        {
            float translateX = Input.GetAxisRaw("Vertical") * speed;
            float translateY = Input.GetAxisRaw("Horizontal") * speed;

            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            controller.SimpleMove(forward * translateX + right * translateY);
        }

        private IEnumerator Caught()
        {
            state.SetNewState(PlayerStatus.Caught);
            StartCoroutine(TimerCaught());

            Vector3 forward = transform.TransformDirection(Vector3.forward);

            while (state.Status == PlayerStatus.Caught)
            {
                controller.SimpleMove(forward * speedCaught);
                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForSeconds(timerRestartCaught);
            caughtCororutine = null;
        }

        private IEnumerator TimerCaught()
        {
            yield return new WaitForSeconds(DistanceToCaught / speedCaught);
            state.SetNewState(PlayerStatus.Run);
        }
    }
}