using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    [CreateAssetMenu]
    public class CharacterConfig : ScriptableObject
    {
        public string displayName;
        public Sprite portrait;
        public CharacterType characterType;
        public Character characterPrefab;
        public CharacterData characterData;
        public List<SkillConfig> skillConfigList;
    }
}
