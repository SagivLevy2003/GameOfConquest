using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;

public class AuthManager
{
    public async void AuthenticatePlayer()
    {
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
        }
        catch (AuthenticationException ex)
        {
            Debug.LogError("Authentication failed: " + ex.Message);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogError("Unity Services request failed: " + ex.Message);
        }
    }
}