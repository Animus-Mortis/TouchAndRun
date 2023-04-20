using UnityEngine;

namespace Game.Player
{
    public enum PlayerStatus
    {
        Run,
        Spurt,
        Caught
    }

    public class PlayerState : MonoBehaviour
    {
        private PlayerStatus status;
        public PlayerStatus Status
        {
            get { return status; }
        }

        public void SetNewState(PlayerStatus newStatus)
        {
            status = newStatus;
        }
    }
}