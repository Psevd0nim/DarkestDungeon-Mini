using System;
using System.Collections;
using UnityEngine;

namespace MyProject
{
    [Serializable]
    public class SkipTurnSkill : Skill
    {
        private SkillBoard_UI _skillBoard_UI;

        public SkipTurnSkill(SkipTurnSkillConfig config) : base(config)
        {

        }

        public void Prepare(SkillBoard_UI skillBoard_UI)
        {
            _skillBoard_UI = skillBoard_UI;
        }

        public override void Execute(Action onCompleted)
        {
            _skillExecuteCoroutine = _coroutineRunner.StartCoroutine(ExecuteCoroutine(onCompleted));
        }

        private IEnumerator ExecuteCoroutine(Action onCompleted)
        {
            _skillBoard_UI.SetInteractable(false);
            bool showingFeedbackIsEnd = false;
            Owner.ShowFeedback("Wait", Color.white, () => showingFeedbackIsEnd = true);
            yield return new WaitUntil(() => showingFeedbackIsEnd);
            onCompleted?.Invoke();
        }
    }
}
