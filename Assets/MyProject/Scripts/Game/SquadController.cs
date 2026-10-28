using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class SquadController : MonoBehaviour
    {
        public List<Entity> Characters => _characters;
        public ColorData SquadColor => _squadColor;

        [SerializeField] private List<Transform> _squadSlots;
        [SerializeField] private Transform _charactersParent;

        private List<Entity> _characters = new List<Entity>();

        private EntitiesManager _entitiesManager;
        private bool _needReverse;
        private ColorData _squadColor;

        public void Init(EntitiesManager entitiesManager, ColorData squadColor, bool needReverse = false)
        {
            _entitiesManager = entitiesManager;
            _squadColor = squadColor;
            _needReverse = needReverse;
        }

        public void FillSquadByRandom()
        {
            for (int i = 0; i < _squadSlots.Count; i++)
            {
                Entity entity = _entitiesManager.GetRandomCharacter();
                entity.transform.SetParent(_charactersParent, false);
                entity.transform.position = _squadSlots[i].position;
                _characters.Add(entity);
                entity.AdditionalInit(i + 1, _squadColor, _needReverse);
            }
        }

        public void FillSquadByFixedConfig()
        {

        }
    }
}
