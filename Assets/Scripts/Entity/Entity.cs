using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    [field: SerializeField] public GUID Guid { get; private set; }
    [field: SerializeField] public float Health { get; private set; }
    [SerializeField] public float MaxHeath;
    public UnityEvent onHeathChange = new UnityEvent();

    void Start()
    {
        Guid = GUID.Generate();
    }

    void Update()
    {
        
    }
}
