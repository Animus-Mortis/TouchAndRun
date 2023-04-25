using Game.Player;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Menu
{
    public class TurnManager : MonoBehaviour
    {
        private List<MirrorNetworker> players = new List<MirrorNetworker>();

        public void AddPlayer(MirrorNetworker player)
        {
            players.Add(player);
        }
    }
}