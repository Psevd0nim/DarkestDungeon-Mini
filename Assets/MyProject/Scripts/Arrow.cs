using UnityEngine;

namespace MyProject
{
    public class Arrow : MonoBehaviour
    {
        private Vector3 _startPosition;
        private float _elapsed;

        private void Awake()
        {
            _startPosition = transform.localPosition;
        }

        private void Update()
        {
            _elapsed += Time.deltaTime;
            transform.localPosition = _startPosition + Vector3.up * (Mathf.Sin(_elapsed * 4) * 0.08f);
        }
    }
}
