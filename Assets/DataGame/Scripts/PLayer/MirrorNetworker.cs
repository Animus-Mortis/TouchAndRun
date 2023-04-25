using Game.Menu;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Player
{
    public class MirrorNetworker : NetworkBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private Move move;
        [SerializeField] private Rotate rotate;
        [SerializeField] private CharacterController controller;

        public static MirrorNetworker localPlayer;
        public TextMeshPro nameDisplayText;
        [SyncVar(hook = "DisplayPlayerName")] public string PlayerDisplayName;
        [SyncVar] public string matchID;

        private NetworkMatch networkMatch;
        private Vector3 startPosition;

        private void Start()
        {
            startPosition = transform.position;

            networkMatch = GetComponent<NetworkMatch>();

            if (isLocalPlayer)
            {
                localPlayer = this;
                CmdSendName(MainMenu.instance.DisplayName);
            }
            else
                MainMenu.instance.SpawnPlayerUIPrefab(this);

            if (!isOwned)
            {
                camera.gameObject.SetActive(false);
                move.enabled = false;
                rotate.enabled = false;
                controller.enabled = false;
            }
        }

        public void DisplayPlayerName(string name, string playerName)
        {
            name = PlayerDisplayName;
            print($"Имя {name} : {playerName}");
            nameDisplayText.text = playerName;
        }

        [Command]
        public void CmdSendName(string name)
        {
            PlayerDisplayName = name;
        }

        public void HostGame()
        {
            string ID = MainMenu.GetRandomID();
            CmdHostGame(ID);
        }

        [Command]
        public void CmdHostGame(string ID)
        {
            matchID = ID;
            if (MainMenu.instance.HostGame(ID, gameObject))
            {
                print("Лобби было успешно создано");
                networkMatch.matchId = ID.ToGuid();
                TargetHostGame(true, ID);
            }
            else
            {
                print("Ошибка при создании лобби");
                TargetHostGame(false, ID);
            }
        }

        [TargetRpc]
        private void TargetHostGame(bool success, string ID)
        {
            matchID = ID;
            print($"ID {matchID} == {ID}");
            MainMenu.instance.HostSuccess(success, ID);
        }

        public void JoinGame(string inputID)
        {
            CmdJoinGame(inputID);
        }

        [Command]
        public void CmdJoinGame(string ID)
        {
            matchID = ID;
            if (MainMenu.instance.JoinGame(ID, gameObject))
            {
                print("Успешное подключение к лобби");
                networkMatch.matchId = ID.ToGuid();
                TargetJoinGame(true, ID);
            }
            else
            {
                print("Не удалось подключиться к лобби");
                TargetJoinGame(false, ID);
            }
        }

        [TargetRpc]
        private void TargetJoinGame(bool success, string ID)
        {
            matchID = ID;
            print($"ID {matchID} == {ID}");
            MainMenu.instance.JoinSuccess(success, ID);
        }

        public void BeginGame()
        {
            CmdBeginGame();
        }

        [Command]
        public void CmdBeginGame()
        {
            MainMenu.instance.BeginGame(matchID);
            print("Игра началась");
        }

        public void StartGame()
        {
            TargetBeginGame();
        }

        [TargetRpc]
        private void TargetBeginGame()
        {
            print($"ID {matchID} | начало");
            DontDestroyOnLoad(gameObject);
            MainMenu.instance.inGame = true;
            GetComponent<CharacterController>().enabled = true;
            transform.position = startPosition;
            SceneManager.LoadScene("MainScene");
        }
    }
}