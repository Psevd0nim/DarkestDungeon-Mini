using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    [CreateAssetMenu]
    public class CharacterConfig : ScriptableObject
    {
        public CharacterType characterType;
        public Character characterPrefab;
        public CharacterData characterData;
        public List<SkillConfig> skillConfigList;
    }
}
