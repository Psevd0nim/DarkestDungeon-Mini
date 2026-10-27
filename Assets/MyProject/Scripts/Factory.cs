using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class Factory : MonoBehaviour
    {
        public List<Entity> _entitiesPrefabs;

        [SerializeField] private EntityHubConfig _entityHubConfig;

        public Entity CreateEntityByType(EntityType type)
        {
            Entity entity = Instantiate(_entitiesPrefabs[Random.Range(0, _entitiesPrefabs.Count)]);
            return entity;
        }

        public Entity CreateRandomEntity()
        {
            Entity entity = Instantiate(_entitiesPrefabs[Random.Range(0, _entitiesPrefabs.Count)]);
            return entity;
        }
    }

    public class EntityHubConfig
    {
        public List<EntityConfig> entityConfigs;
    }

    public class EntityConfig : ScriptableObject
    {
        public EntityType entityType;
        public EntityData entityData;
        public Entity entityPrefab;
    }

    public enum EntityType
    {
        Swordsman, Archer, Healer
    }
}
