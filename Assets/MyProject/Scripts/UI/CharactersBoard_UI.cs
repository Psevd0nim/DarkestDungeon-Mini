using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class CharactersBoard_UI : MonoBehaviour
    {
        public List<Entity_UI> TargetCharacters => _targetCharacters;

        private List<Entity_UI> _targetCharacters = new List<Entity_UI>();

        public void ShowCharacterSequence(List<Entity> characters, Factory factory)
        {
            foreach (var character in characters)
            {
                Entity_UI entity_UI = factory.CreateEntity_UI();
                entity_UI.transform.SetParent(transform);
                entity_UI.Init(character);
                _targetCharacters.Add(entity_UI);
            }
        }

        public void SetCharacter_UI_Active(Entity currentCharacter, bool isActive)
        {
            Entity_UI targerCard_UI = _targetCharacters.Find(x => x.Entity == currentCharacter);
            if(targerCard_UI != null)
                targerCard_UI.SetCardActvie(isActive);
        }
    }
}
