using System;
using UnityEngine;

namespace MyProject
{
    public abstract class LevelManager : MonoBehaviour
    {
        public event Action<string> OnExitLevel;

        public abstract void Init(AppServices appServices);
        public abstract void StartLevel();

        protected void ExitLevel(string sceneName)
        {
            OnExitLevel?.Invoke(sceneName);
        }
    }
}
