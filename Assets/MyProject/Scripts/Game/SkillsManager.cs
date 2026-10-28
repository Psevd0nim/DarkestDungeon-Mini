using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class SkillsManager : MonoBehaviour
    {
        private Game_UI_Manager _game_UI_Manager;
        private List<Skill> _targetSkills;
        private SkillsBoard_UI _skillsBoard_UI;

        private Skill_UI _targetSkill_UI;

        public void Init(Game_UI_Manager game_UI_Manager)
        {
            _game_UI_Manager = game_UI_Manager;
            _skillsBoard_UI = _game_UI_Manager.SkillsBoard_UI;
            _skillsBoard_UI.OnSkillPressed += AfterSkillPressed;
        }

        public void BeginPlayerTurn(Entity currentCharacter, Action callback)
        {
            ShowSkills(currentCharacter.SkillList);
        }

        private void ShowSkills(List<Skill> skills)
        {
            _targetSkills = skills;
            _skillsBoard_UI.ShowSkills(_targetSkills);
        }

        private void AfterSkillPressed(Skill_UI skill_UI)
        {
            if (_targetSkill_UI == skill_UI)
                return;
            if (_targetSkill_UI != null)
                _targetSkill_UI.SetSelectedActive(false);
            _targetSkill_UI = skill_UI;
            _targetSkill_UI.SetSelectedActive(true);
        }
    }
}
