using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class SquadController : MonoBehaviour
    {
        public List<Character> Characters => _characters;
        public Color SquadColor => _squadColor;
        public CharacterConfig[] InitialFormationData => _initialFormationData;

        public Character FrontlineCharacter { get; private set; }

        [SerializeField] private SquadSlot[] _squadSlots = new SquadSlot[4];
        [SerializeField] private Transform _charactersParent;

        private CharactersManager _charactersManager;
        private Color _squadColor;
        private bool _needReverse;
        private List<Character> _characters = new List<Character>();
        private CharacterConfig[] _initialFormationData;

        public void Init(CharactersManager charactersManager, Color color, bool reversed = false)
        {
            _charactersManager = charactersManager;
            _squadColor = color;
            _needReverse = reversed;
        }

        public void FillFormation(CharacterConfig[] formation)
        {
            Clear();
            _initialFormationData = formation;
            for (int i = 0; i < _squadSlots.Length; i++)
            {
                if (formation[i] == null)
                {
                    continue;
                }

                SquadSlot squadSlot = _squadSlots[i];
                Character character = _charactersManager.GetCharacterByConfig(formation[i]);
                character.transform.SetParent(_charactersParent, false);
                character.transform.position = squadSlot.transform.position;
                character.AdditionalInit(this, squadSlot, _squadColor, _needReverse);
                character.OnDeath += AfterDeath;
                squadSlot.SetCharacter(character);
                _characters.Add(character);
            }

            RefreshFrontline();
        }

        public List<Character> GetAliveCharacters()
        {
            return _characters.FindAll(character => character.IsAlive);
        }

        public void Clear()
        {
            foreach (Character character in _characters)
            {
                character.OnDeath -= AfterDeath;
                character.gameObject.SetActive(false);
                Destroy(character.gameObject);
            }

            _characters.Clear();
            foreach (SquadSlot slot in _squadSlots)
            {
                slot.SetCharacter(null);
            }

            FrontlineCharacter = null;
        }

        private void AfterDeath(Character character)
        {
            RefreshFrontline();
        }

        private void RefreshFrontline()
        {
            FrontlineCharacter = null;
            foreach (SquadSlot slot in _squadSlots)
            {
                if (slot.CharacterInSlot != null && slot.CharacterInSlot.IsAlive)
                {
                    FrontlineCharacter = slot.CharacterInSlot;
                    break;
                }
            }
        }
    }
}
