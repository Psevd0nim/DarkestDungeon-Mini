using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    [CreateAssetMenu]
    public class EntityConfig : ScriptableObject
    {
        public EntityType entityType;
        public EntityData entityData;
        public List<SkillConfig> skillConfigList;
    }
}
