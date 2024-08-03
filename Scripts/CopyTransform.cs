using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    [ExecuteAlways]
    [AddComponentMenu("Inbora/FPS Framework/Utility/Copy Transform")]
    public class CopyTransform : MonoBehaviour
    {
        public UpdateMode updateMode;
        public Transform target;
        [Space]
        public bool position;
        public bool rotation;

        private void Update()
        {
            if (updateMode == UpdateMode.Update) Copy();
        }

        private void FixedUpdate()
        {
            if (updateMode == UpdateMode.FixedUpdate) Copy();
        }

        private void LateUpdate()
        {
            if (updateMode == UpdateMode.LateUpdate) Copy();
        }

        private void Copy()
        {
            if (position)
                transform.position = target.position;

            if (rotation)
                transform.rotation = transform.rotation;
        }
    }

    public enum UpdateMode
    {
        Update,
        FixedUpdate,
        LateUpdate
    }
}