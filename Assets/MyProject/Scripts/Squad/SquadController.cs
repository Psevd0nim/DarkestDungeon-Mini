using Sirenix.OdinInspector;
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

        [SerializeField, ListDrawerSettings(IsReadOnly = true, ShowIndexLabels = true)] 
        private SquadSlot[] _squadSlots = new SquadSlot[4];
        [SerializeField] private Transform _charactersParent;

        private CharactersManager _charactersManager;
        private ColorData _squadColor;
        private bool _needReverse;
        private List<Character> _characters = new List<Character>();
        private Character _frontlineCharacter;

        public void Init(CharactersManager charactersManager, ColorData squadColor, bool needReverse = false)
        {
            _charactersManager = charactersManager;
            _squadColor = squadColor;
            _needReverse = needReverse;
        }

        public void FillSquadByFixedConfig(SquadConfig playerSquadConfig)
        {
            CharacterConfig[] characterConfigs = playerSquadConfig.GetSquadFormation();
            for(int i=0; i < _squadSlots.Length; i++)
            {
                CharacterConfig characterConfig = characterConfigs[i];
                if (characterConfig == null)
                    continue;

                Character character = _charactersManager.GetCharacterByConfig(characterConfig);
                SquadSlot currentSlot = _squadSlots[i];
                character.transform.SetParent(_charactersParent, false);
                character.transform.position = currentSlot.transform.position;
                _characters.Add(character);
                character.AdditionalInit(this, i + 1, _squadColor, _needReverse);
                currentSlot.SetCharacter(character);
            }
            SetFrontlineCharacter();
        }

        public void FillSquadByRandom()
        {
            for (int i = 0; i < _squadSlots.Length; i++)
            {
                Character character = _charactersManager.GetRandomCharacter();
                SquadSlot currentSlot = _squadSlots[i];
                character.transform.SetParent(_charactersParent, false);
                character.transform.position = currentSlot.transform.position;
                _characters.Add(character);
                character.AdditionalInit(this, i + 1, _squadColor, _needReverse);
                currentSlot.SetCharacter(character);
            }
            SetFrontlineCharacter();
        }

        private void SetFrontlineCharacter()
        {
            _frontlineCharacter = _characters.First(x => x.IsAlive);
        }
    }
}
