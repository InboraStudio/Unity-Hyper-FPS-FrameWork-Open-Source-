using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Utility/AutoRotate")]
    public class AutoRotate : MonoBehaviour
    {
        public float rotateSpeed = 1;
        public bool x;
        public bool y;
        public bool z;

        private void Update()
        {
            Vector3 rotation = new Vector3(x ? rotateSpeed : 0, y ? rotateSpeed : 0, z ? rotateSpeed : 0);
            transform.Rotate(rotation * rotateSpeed * Time.deltaTime);
        }
    }
}