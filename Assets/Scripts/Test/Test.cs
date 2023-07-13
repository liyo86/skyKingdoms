using Managers;
using Service;
using UnityEngine;

namespace Test
{
    public class Test : MonoBehaviour
    {
        void Start()
        {
            ServiceLocator.GetService<MyLevelManager>().StartLevel();
        }
    }
}