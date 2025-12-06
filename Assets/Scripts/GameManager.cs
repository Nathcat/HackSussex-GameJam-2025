using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public bool server { get; private set; } = true;
    public PlayerController player { get; private set; }
    public List<Entity> players { get; private set; }

    void Start()
    {
        instance = this;
        player = FindFirstObjectByType<PlayerController>();
    }

    void Update()
    {
        
    }
}
