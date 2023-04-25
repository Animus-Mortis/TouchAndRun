using UnityEngine;

namespace Game.Player
{
    public class Touching : MonoBehaviour
    {
        private PlayerState state;
        private ChangeColor color;

        private void OnValidate()
        {
            if (!GetComponent<PlayerState>())
                gameObject.AddComponent<PlayerState>();
        }

        private void Awake()
        {
            state = GetComponent<PlayerState>();
            color = GetComponent<ChangeColor>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (state.Status == PlayerStatus.Caught || collision.transform == transform) return;

            if (collision.transform.GetComponent<PlayerState>())
            {
                PlayerState target = collision.transform.GetComponent<PlayerState>();

                //switch (target.Status)
                //{
                //    case PlayerStatus.Run:
                //        target.GetComponent<ChangeColor>().ChangeColorToCaught();
                //        break;
                //    case PlayerStatus.Spurt:
                //        if(state.Status == PlayerStatus.Run)
                //            color.ChangeColorToCaught();
                //        break;
                //}

                if(target.Status == PlayerStatus.Run)
                    target.GetComponent<ChangeColor>().ChangeColorToCaught();

                if(target.Status != PlayerStatus.Caught && state.Status == PlayerStatus.Run)
                    color.ChangeColorToCaught();

            }
        }
    }
}
