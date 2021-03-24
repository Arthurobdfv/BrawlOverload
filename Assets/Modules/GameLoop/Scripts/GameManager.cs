using BrawlServer;
using GameClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public GameObject playerPrefab;

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this);

        Client._sender = new BrawlClientSender();
        Client._handler = new BrawlClientHandler();
        Client.instance = new Client();
    }

    public void SpawnPlayer(int _id, string _userName, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        _player = Instantiate(playerPrefab, _position, _rotation);
        var plrMgr = _player.GetComponent<PlayerManager>();
        plrMgr.id = _id;
        plrMgr.userName = _userName;
        players.Add(_id, plrMgr);
    }

    private void OnApplicationQuit()
    {
        Client.instance.Disconnect();
    }

    public static void Disconnect(int id)
    {
        Destroy(players[id].gameObject);
        players.Remove(id);
    }

}
