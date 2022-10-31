using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerManager : MonoBehaviour
{
    public PlayerInputManager playerInputManager;

    public void OnPlayerJoined()
    {
        GameManager.numberOfPlayers++;
    }

}
