using System.Collections;
using UnityEngine;

namespace MyProject
{
    public interface ICoroutineRunner
    {
        public Coroutine StartCoroutine(IEnumerator coroutine);
    }
}
