using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    [CreateAssetMenu]
    public class EntityHubConfig : ScriptableObject
    {
        public List<EntityConfig> entityConfigs;

        public EntityConfig GetRandomCharacterConfig()
        {
            int randomIndex = Random.Range(0, entityConfigs.Count);
            return entityConfigs[randomIndex];
        }
    }
}