using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    [SerializeField] public GameObject orbPrefab;
    public PlayerController player { get; private set; }

    void Awake()
    {
        instance = this;
        player = FindAnyObjectByType<PlayerController>();
    }

    void Start()
    {
    }

    void Update()
    {
        
    }
}
