using System.Collections;
using UnityEngine;

namespace MyProject
{
    public class FightManager : MonoBehaviour
    {
        public void Init(EntitiesManager entitiesManager)
        {

        }

        public void StartFight()
        {
            //StartCoroutine(FightCoroutine());
        }

        private void PrepareRound()
        {

        }

        private IEnumerator FightCoroutine()
        {
            yield return null;
        }
    }
}
