using Mirror;
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
        [SerializeField] private PlayerStatus status;
        public PlayerStatus Status
        {
            get { return status; }
        }

        //[Server]
        public void SetNewState(PlayerStatus newStatus)
        {
            status = newStatus;
        }

        //[Command]
        //private void cmdSetNewState(PlayerStatus status)
        //{
        //    SetNewState(status);
        //}

        public void NewStatus(PlayerStatus status)
        {
            SetNewState(status);
        }
    }
}