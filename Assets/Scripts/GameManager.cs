using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public bool server { get; private set; } = true;
    public PlayerController player { get; private set; }
    public List<Entity> players { get; private set; }

    void Awake()
    {
        instance = this;
        player = FindAnyObjectByType<PlayerController>();
        players = new List<Entity>();

        players.Add(player);
    }

    void Start()
    {
    }

    void Update()
    {
        
    }
}
