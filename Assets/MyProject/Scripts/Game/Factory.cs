using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class Factory : MonoBehaviour
    {
        [SerializeField] private Character _archerPrefab;
        [SerializeField] private Character _swordsmanPrefab;
        [SerializeField] private Character _healerPrefab;

        [SerializeField] private Skill_UI _skillUIPrefab;
        [SerializeField] private Character_UI _character_UIPrefab;

        public Character CreateCharacterByType(CharacterType type)
        {
            Character character = null;
            switch (type)
            {
                case CharacterType.Swordsman:
                    character = Instantiate(_swordsmanPrefab);
                    break;
                case CharacterType.Archer:
                    character = Instantiate(_archerPrefab);
                    break;
                case CharacterType.Healer:
                    character = Instantiate(_healerPrefab);
                    break;
            }
            return character;
        }

        public Skill_UI CreateSkill_UI()
        {
            return Instantiate(_skillUIPrefab);
        }

        public Character_UI CreateCharacter_UI()
        {
            return Instantiate(_character_UIPrefab);
        }
    }
}
