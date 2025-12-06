using UnityEditor;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [field: SerializeField] public GUID Guid { get; private set; }
    [SerializeField] public float MaxHeath;
    [SerializeField] public float Health;

    void Start()
    {
        Guid = GUID.Generate();
    }

    void Update()
    {
        
    }
}
