//using Unity.Services.Lobbies.Models;
//using Unity.Services.Lobbies;
//using UnityEngine;
//using System.Threading.Tasks;
//using System.Collections.Generic;
//using System.Collections;
//using QFSW;
//using QFSW.QC;
//using Unity.Services.Authentication;
//using FishNet.Managing;

//public class LobbyManager : MonoBehaviour
//{
//    [SerializeField] int _hearbeatDelayInSeconds = 20;

//    public Lobby lobby;

//    [Command]
//    public async Task LeaveCurrentLobby()
//    {
//        try
//        {
//            await LobbyService.Instance.RemovePlayerAsync(lobby.Id, AuthenticationService.Instance.PlayerId);
//        }
//        catch (LobbyServiceException e)
//        {
//            Debug.Log(e);
//        }
//    }

//    [Command]
//    public async Task JoinLobbyById(string id)
//    {
//        try
//        {
//            lobby = await LobbyService.Instance.JoinLobbyByIdAsync(id);
//            Debug.Log($"Joined lobby: {lobby.Name} ({lobby.Id}) with {lobby.Players.Count} player(s)");
//        }
//        catch (LobbyServiceException e)
//        {
//            Debug.Log(e);
//        }
//    }

//    [Command]
//    public async Task QueryForLobbies()
//    {
//        try
//        {
//            QueryResponse response = await LobbyService.Instance.QueryLobbiesAsync();
//            Debug.Log("Lobby count: " + response.Results.Count);

//            foreach (var lobby in response.Results)
//            {
//                Debug.Log(lobby.Name);
//            }
//        }
//        catch (LobbyServiceException e)
//        { 
//            Debug.Log(e); 
//        }
//    }

//    [Command]
//    public async Task CreateLobbyAsync()
//    {
//        string ip = 
//        string lobbyName = "new lobby";
//        int maxPlayers = 4;

//        CreateLobbyOptions options = new()
//        {
//            IsPrivate = false
//        };

//        options.Data = new()
//        {
//            {
//                "ip", new(DataObject.VisibilityOptions.Member, value:)
//            }
//        }

//        lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

//        StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, _hearbeatDelayInSeconds));
//    }

//    IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds)
//    {
//        var delay = new WaitForSecondsRealtime(waitTimeSeconds);

//        while (true)
//        {
//            LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
//            yield return delay;
//        }
//    }

//    [Command]
//    public async Task CloseCurrentLobby()
//    {
//        if (lobby == null)
//        {
//            Debug.LogWarning("CloseCurrentLobby was called when the local lobby is unassigned.");
//            return;
//        }

//        try
//        {
//            await LobbyService.Instance.DeleteLobbyAsync(lobby.Id);
//        }
//        catch (LobbyServiceException e)
//        {
//            Debug.Log(e);
//        }
//    }

//    private async void OnApplicationQuit()
//    {
//        await CloseCurrentLobby();
//    }
//}
