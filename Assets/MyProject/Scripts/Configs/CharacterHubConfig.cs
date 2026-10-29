using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    [CreateAssetMenu]
    public class CharacterHubConfig : ScriptableObject
    {
        public List<CharacterConfig> characterConfigs;

        public CharacterConfig GetRandomCharacterConfig()
        {
            int randomIndex = Random.Range(0, characterConfigs.Count);
            return characterConfigs[randomIndex];
        }
    }
}