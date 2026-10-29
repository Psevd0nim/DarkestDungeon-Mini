using UnityEngine;

namespace MyProject
{
    public class TargetSign : MonoBehaviour
    {
        public void SetSignActive(bool status)
        {
            gameObject.SetActive(status);
        }
    }
}