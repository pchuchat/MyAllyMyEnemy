using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharcterSwitcher: MonoBehaviour
{
    int index = 0;
    [SerializeField] List<GameObject> fighters = new List<GameObject>();
    PlayerInputManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<PlayerInputManager>();
        index = Random.Range(0, fighters.Count);
        manager.playerPrefab = fighters[index];
    }

    public void SwitchNextSpawnCharacter(PlayerInput input)
    {
        index = Random.Range(0,fighters.Count);
        manager.playerPrefab = fighters[index];
    }
}
