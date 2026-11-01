using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject
{
    public class SkillsManager : MonoBehaviour
    {
        [SerializeField] private SkipTurnSkillConfig _requiredSkillConfig;
        private CharactersManager _charactersManager;
        private SkillBoard_UI _skillBoard_UI;
        private SkillExecutionManager _skillExecutionManager;
        private Action _onTurnCompleted;
        private Skill _selectedSkill;
        private ITargetedSkill _targetedSkill;
        private List<Character> _targets = new List<Character>();
        private Coroutine _aiRoutine;
        private bool _acceptPlayerInput;
        public bool IsPaused { get; set; }
        public bool IsExecuting => _skillExecutionManager.IsExecuting;

        public void Init(Game_UI_Manager gameUIManager, CharactersManager charactersManager, SkillExecutionManager skillExecutionManager)
        {
            _charactersManager = charactersManager;
            _skillExecutionManager = skillExecutionManager;
            _skillBoard_UI = gameUIManager.SkillBoard_UI;
            _skillBoard_UI.OnSkillPressed += AfterSkillPressed;
        }

        public List<Skill> GetSkillListBySkillConfigs(List<SkillConfig> configs, Character owner)
        {
            var skills = new List<Skill>
            {
                _requiredSkillConfig.CreateSkill()
            };
            foreach (var config in configs)
            {
                skills.Add(config.CreateSkill());
            }

            foreach (var skill in skills)
            {
                skill.Init(owner, _skillExecutionManager);
            }

            return skills;
        }

        private void PrepareSkills(Character character)
        {
            foreach (var skill in character.SkillList)
            {
                switch (skill)
                {
                    case MeleeAttackSkill melee:
                        melee.Prepare(_charactersManager.GetOpponent(character));
                        break;
                    case RangedAttackSkill ranged:
                        ranged.Prepare(_charactersManager.GetOpponent(character));
                        break;
                    case HealSkill heal:
                        heal.Prepare(character.OwnSquad);
                        break;
                }
            }
        }

        public void BeginPlayerTurn(Character character, Action completed)
        {
            CancelSelection();
            _onTurnCompleted = completed;
            PrepareSkills(character);
            _acceptPlayerInput = true;
            _skillBoard_UI.ShowSkills(character.SkillList);
        }

        private void AfterSkillPressed(Skill_UI button)
        {
            if (!_acceptPlayerInput || IsPaused || _skillExecutionManager.IsExecuting || !button.Skill.CanUse)
            {
                return;
            }

            if (_selectedSkill == button.Skill)
            {
                return;
            }

            CancelSelection();
            _selectedSkill = button.Skill;
            button.SetSelectedActive(true);
            switch (_selectedSkill)
            {
                case IImmediateSkill immediate:
                    _acceptPlayerInput = false;
                    _skillBoard_UI.SetInteractable(false);
                    immediate.Execute(AfterSkillUsed);
                    break;
                case ITargetedSkill targeted:
                    _targetedSkill = targeted;
                    _targets = targeted.GetAvailableTargets();
                    foreach (var target in _targets)
                    {
                        target.SetTargetActive(true);
                    }

                    _charactersManager.OnCharacterPressed += AfterTargetPressed;
                    break;
            }
        }

        private void AfterTargetPressed(Character character)
        {
            if (IsPaused || !_acceptPlayerInput || !_targets.Contains(character) || !character.IsAlive)
            {
                return;
            }

            var skill = _targetedSkill;
            if (!skill.GetAvailableTargets().Contains(character))
            {
                return;
            }

            CancelSelection();
            _acceptPlayerInput = false;
            _skillBoard_UI.SetInteractable(false);
            skill.Execute(character, AfterSkillUsed);
        }

        public void CancelSelection()
        {
            _charactersManager.OnCharacterPressed -= AfterTargetPressed;
            foreach (var target in _targets)
            {
                if (target != null)
                {
                    target.SetTargetActive(false);
                }
            }

            _targets.Clear();
            _skillBoard_UI.ClearSelection();
            _selectedSkill = null;
            _targetedSkill = null;
        }

        public void Begin_AI_Turn(Character character, Action completed)
        {
            _onTurnCompleted = completed;
            _acceptPlayerInput = false;
            PrepareSkills(character);
            _skillBoard_UI.ClearBoard();
            _aiRoutine = StartCoroutine(AITurn(character));
        }

        private IEnumerator AITurn(Character character)
        {
            yield return new WaitForSeconds(0.55f);
            var available = new List<Skill>();
            foreach (var skill in character.SkillList)
            {
                if (!skill.CanUse || skill is SkipTurnSkill)
                {
                    continue;
                }

                if (skill is ITargetedSkill targeted)
                {
                    var targets = targeted.GetAvailableTargets();
                    if (skill is HealSkill)
                    {
                        targets.RemoveAll(target => target.Health == target.MaxHealth);
                    }

                    if (targets.Count == 0)
                    {
                        continue;
                    }
                }

                available.Add(skill);
            }

            // Passing is the fallback when no useful action is available.
            if (available.Count == 0)
            {
                available.Add(character.SkillList.Find(skill => skill is SkipTurnSkill));
            }

            Skill chosen = available[UnityEngine.Random.Range(0, available.Count)];
            _aiRoutine = null;
            switch (chosen)
            {
                case IImmediateSkill immediate:
                    immediate.Execute(AfterSkillUsed);
                    break;
                case ITargetedSkill targeted:
                    var targets = targeted.GetAvailableTargets();
                    if (chosen is HealSkill)
                    {
                        targets.RemoveAll(target => target.Health == target.MaxHealth);
                    }

                    targeted.Execute(targets[UnityEngine.Random.Range(0, targets.Count)], AfterSkillUsed);
                    break;
            }
        }

        private void AfterSkillUsed()
        {
            CancelSelection();
            _skillBoard_UI.ClearBoard();
            var completed = _onTurnCompleted;
            _onTurnCompleted = null;
            completed?.Invoke();
        }

        public void CancelTurn()
        {
            if (_aiRoutine != null)
            {
                StopCoroutine(_aiRoutine);
            }

            _aiRoutine = null;
            _acceptPlayerInput = false;
            _onTurnCompleted = null;
            CancelSelection();
            _skillBoard_UI.ClearBoard();
            _skillExecutionManager.Cancel();
        }

        private void OnDestroy()
        {
            if (_skillBoard_UI != null)
            {
                _skillBoard_UI.OnSkillPressed -= AfterSkillPressed;
            }

            if (_charactersManager != null)
            {
                _charactersManager.OnCharacterPressed -= AfterTargetPressed;
            }
        }
    }
}
