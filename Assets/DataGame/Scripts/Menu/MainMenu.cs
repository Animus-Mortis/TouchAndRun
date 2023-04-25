using Game.Player;
using Mirror;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Menu
{
    [System.Serializable]
    public class Match : NetworkBehaviour
    {
        public string ID;
        public List<GameObject> players = new List<GameObject>();

        public Match(string ID, GameObject player)
        {
            this.ID = ID;
            players.Add(player);
        }
    }

    public class MainMenu : NetworkBehaviour
    {
        public static MainMenu instance;

        public readonly SyncList<Match> matches = new SyncList<Match>();
        public readonly SyncList<string> matchIDs = new SyncList<string>();

        [Header("MainMenu")]
        [SerializeField] private TMP_InputField joinInput;
        [SerializeField] private Button hostButton;
        [SerializeField] private Button joinButton;
        [SerializeField] private Canvas lobbyCanvas;

        [Header("Name")]
        [SyncVar] public string DisplayName;
        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private GameObject saveNameButton;

        [Header("Lobby")]
        [SerializeField] private Transform UIPlayerParent;
        [SerializeField] private GameObject UIPlayerPref;
        [SerializeField] private TextMeshProUGUI IDText;
        [SerializeField] private Button beginGameButton;
        [SerializeField] private GameObject turnManager;
        public bool inGame;

        private NetworkManager networkManager;

        private void Start()
        {
            instance = this;

            networkManager = FindObjectOfType<NetworkManager>();

            if (!PlayerPrefs.HasKey("Name"))
                return;

            string defultName = PlayerPrefs.GetString("Name");
            nameInput.text = defultName;
            DisplayName = defultName;
        }

        private void Update()
        {
            if (!inGame)
            {
                MirrorNetworker[] players = FindObjectsOfType<MirrorNetworker>();

                for (int i = 0; i < players.Length; i++)
                {
                    players[i].GetComponent<CharacterController>().enabled = false;
                }

                if (PlayerPrefs.HasKey("Name"))
                {
                    hostButton.gameObject.SetActive(true);
                    joinButton.gameObject.SetActive(true);
                    nameInput.interactable = false;
                    saveNameButton.SetActive(false);
                }
            }

        }
        
        public void Rename()
        {
            PlayerPrefs.DeleteKey("Name");
            hostButton.gameObject.SetActive(false);
            joinButton.gameObject.SetActive(false);
            nameInput.interactable = true;
            nameInput.text = "";
            saveNameButton.SetActive(true);
        }

        public void SaveName()
        {
            DisplayName = nameInput.text;
            PlayerPrefs.SetString("Name", nameInput.text);
            Invoke(nameof(Disconect), 1);
        }

        private void Disconect()
        {
            if(networkManager.mode == NetworkManagerMode.Host)
            {
                networkManager.StopHost();
            }
            else if (networkManager.mode == NetworkManagerMode.ClientOnly)
            {
                networkManager.StopClient();
            }
        }

        public void Host()
        {
            joinInput.interactable = false;
            hostButton.interactable = false;
            joinButton.interactable = false;

            MirrorNetworker.localPlayer.HostGame();
        }

        public void HostSuccess(bool success, string matchID)
        {
            if (success)
            {
                lobbyCanvas.enabled = true;

                SpawnPlayerUIPrefab(MirrorNetworker.localPlayer);
                IDText.text = matchID;
                beginGameButton.interactable = true;
            }
            else
            {
                joinInput.interactable = true;
                hostButton.interactable = true;
                joinButton.interactable = true;
            }
        }

        public void Join()
        {
            joinInput.interactable = false;
            hostButton.interactable = false;
            joinButton.interactable = false;

            MirrorNetworker.localPlayer.JoinGame(joinInput.text.ToUpper());
        }

        public void JoinSuccess(bool success, string matchID)
        {
            if (success)
            {
                lobbyCanvas.enabled = true;

                SpawnPlayerUIPrefab(MirrorNetworker.localPlayer);
                IDText.text = matchID;
                beginGameButton.interactable = false;
            }
            else
            {
                joinInput.interactable = true;
                hostButton.interactable = true;
                joinButton.interactable = true;
            }
        }

        public bool HostGame(string matchID, GameObject player)
        {
            if (!matchIDs.Contains(matchID))
            {
                matchIDs.Add(matchID);
                matches.Add(new Match(matchID, player));
                return true;
            }
            else
                return false;
        }

        public bool JoinGame(string matchID, GameObject player)
        {
            print(matchID);
            if (matchIDs.Contains(matchID))
            {
                for (int i = 0; i < matches.Count; i++)
                {
                    if (matches[i].ID == matchID)
                    {
                        matches[i].players.Add(player);
                        break;
                    }
                }
                return true;
            }
            else
                return false;
        }

        public static string GetRandomID()
        {
            string ID = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                int rand = Random.Range(0, 36);
                if (rand < 26)
                    ID += (char)(rand + 65);
                else
                    ID += (rand - 26).ToString();
            }
            return ID;
        }

        public void SpawnPlayerUIPrefab(MirrorNetworker player)
        {
            GameObject newUIPlayer = Instantiate(UIPlayerPref, UIPlayerParent);
            newUIPlayer.GetComponent<PlayerUI>().SetPlayer(player.PlayerDisplayName);
        }

        public void StartGame()
        {
            MirrorNetworker.localPlayer.BeginGame();
        }

        public void BeginGame(string matchID)
        {
            GameObject newTurnManager = Instantiate(turnManager);
            NetworkServer.Spawn(newTurnManager);
            newTurnManager.GetComponent<NetworkMatch>().matchId = matchID.ToGuid();
            TurnManager _turnManager = newTurnManager.GetComponent<TurnManager>();

            for (int i = 0; i < matches.Count; i++)
            {
                if(matches[i].ID == matchID)
                {
                    foreach (var player in matches[i].players)
                    {
                        MirrorNetworker player1 = player.GetComponent<MirrorNetworker>();
                        _turnManager.AddPlayer(player1);
                        player1.StartGame();
                    }
                    break;
                }
            }
        }
    }

    public static class MatchExtension
    {
        public static System.Guid ToGuid(this string id)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] inputBytes = Encoding.Default.GetBytes(id);
            byte[] hasBytes = provider.ComputeHash(inputBytes);

            return new System.Guid(hasBytes);
        }
    }
}