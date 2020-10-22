using System;

namespace BuildACastle
{
    using UnityEngine;

    public class LookAtCamera : MonoBehaviour
    {
        private Camera _mainCamera;
        private void Awake()=>_mainCamera = Camera.main;
        
        void Update()=>transform.LookAt(transform.position + _mainCamera.transform.forward);
        
    }
}
