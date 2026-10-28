using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class Factory : MonoBehaviour
    {
        [SerializeField] private Entity _archerPrefab;
        [SerializeField] private Entity _swordsmanPrefab;
        [SerializeField] private Entity _healerPrefab;

        [SerializeField] private Skill_UI _skillUIPrefab;
        [SerializeField] private Entity_UI _entity_UIPrefab;

        public Entity CreateEntityByType(EntityType type)
        {
            Entity entity = null;
            switch (type)
            {
                case EntityType.Swordsman:
                    entity = Instantiate(_swordsmanPrefab);
                    break;
                case EntityType.Archer:
                    entity = Instantiate(_archerPrefab);
                    break;
                case EntityType.Healer:
                    entity = Instantiate(_healerPrefab);
                    break;
            }
            return entity;
        }

        public Skill_UI CreateSkill_UI()
        {
            return Instantiate(_skillUIPrefab);
        }

        public Entity_UI CreateEntity_UI()
        {
            return Instantiate(_entity_UIPrefab);
        }
    }
}
