using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyProject
{
    public class SquadController : MonoBehaviour
    {
        public List<Character> Characters => _characters;
        public ColorData SquadColor => _squadColor;
        public Character FrontlineCharacter => _frontlineCharacter;

        [SerializeField] private List<Transform> _squadSlots;
        [SerializeField] private Transform _charactersParent;

        private List<Character> _characters = new List<Character>();

        private CharactersManager _charactersManager;
        private bool _needReverse;
        private ColorData _squadColor;
        private Character _frontlineCharacter;

        public void Init(CharactersManager charactersManager, ColorData squadColor, bool needReverse = false)
        {
            _charactersManager = charactersManager;
            _squadColor = squadColor;
            _needReverse = needReverse;
        }

        public void FillSquadByRandom()
        {
            for (int i = 0; i < _squadSlots.Count; i++)
            {
                Character character = _charactersManager.GetRandomCharacter();
                character.transform.SetParent(_charactersParent, false);
                character.transform.position = _squadSlots[i].position;
                _characters.Add(character);
                character.AdditionalInit(this, i + 1, _squadColor, _needReverse);
            }
            SetFrontlineCharacter();
        }

        private void SetFrontlineCharacter()
        {
            _frontlineCharacter = _characters.First(x => x.IsAlive);
        }

        public void FillSquadByFixedConfig()
        {

        }
    }
}
