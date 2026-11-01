using UnityEngine;

namespace MyProject
{
    public class Factory : MonoBehaviour
    {
        [SerializeField] private Skill_UI _skillUIPrefab;
        [SerializeField] private Character_UI _character_UIPrefab;

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
