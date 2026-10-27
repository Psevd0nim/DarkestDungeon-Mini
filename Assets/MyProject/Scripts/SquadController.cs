using MyProject;
using System.Collections.Generic;
using UnityEngine;

public class SquadController : MonoBehaviour
{
    public List<Entity> Characters => _characters;

    [SerializeField] private List<Transform> _squadSlots;
    [SerializeField] private Transform _charactersParent;

    private Factory _factory;
    private List<Entity> _characters = new List<Entity>();

    public void Init(Factory factory)
    {
        _factory = factory;
    }

    public void FillSquadByRandom()
    {
        foreach (Transform slot in _squadSlots)
        {
            Entity entity = _factory.CreateRandomEntity();
            entity.transform.SetParent(_charactersParent, false);
            entity.transform.position = slot.position;
            _characters.Add(entity);
        }
    }
}
