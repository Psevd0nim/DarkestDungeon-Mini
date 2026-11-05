using System;
using System.Collections;

namespace MyProject
{
    [Serializable]
    public class SkipTurnSkill : Skill
    {
        public SkipTurnSkill(SkipTurnSkillConfig config) : base(config)
        {

        }

        public override void Execute(Action onCompleted)
        {
            _skillExecuteCoroutine = _coroutineRunner.StartCoroutine(ExecuteCoroutine(onCompleted));
        }

        private IEnumerator ExecuteCoroutine(Action onCompleted)
        {
            Owner.ShowFeedback("Wait", UnityEngine.Color.white);
            yield return new UnityEngine.WaitForSeconds(1f);
            onCompleted?.Invoke();
        }
    }
}
