using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inbora.FPSFramework.Animation
{
    [AddComponentMenu("Inbora/FPS Framework/Animation/Procedural Animator"), DisallowMultipleComponent]
    public class ProceduralAnimator : MonoBehaviour
    {
        public Transform layersHolder;
        [Range(0, 1)] public float weight = 1;
        [Range(0, 1)] public float positionWeight = 1;
        [Range(0, 1)] public float rotationWeight = 1;

        /// <summary>
        /// final position result from all clips
        /// </summary>
        public Vector3 targetPosition
        {
            get
            {
                Vector3 result = Vector3.zero;

                if (clips.Count <= 0) return result;
                foreach (ProceduralAnimation clip in clips)
                {
                    result += clip.targetPosition;
                }


                return Vector3.Lerp(Vector3.zero, result, weight * positionWeight);
            }
        }

        /// <summary>
        /// final rotation result from all clips
        /// </summary>
        public Vector3 targetRotation
        {
            get
            {
                Vector3 result = Vector3.zero;

                if (clips.Count <= 0) return result;
                foreach (ProceduralAnimation clip in clips)
                {
                    result += clip.targetRotation;
                }

                return Vector3.Lerp(Vector3.zero, result, weight * rotationWeight);
            }
        }

        /// <summary>
        /// all clips
        /// </summary>
        private List<ProceduralAnimation> clips = new List<ProceduralAnimation>();

        private Vector3 defaultPosition;
        private Vector3 defaultRotation;

        private void Awake()
        {
            defaultPosition = transform.localPosition;
            defaultRotation = transform.localEulerAngles;
        }

        private void OnEnable()
        {
            if (!layersHolder) layersHolder = transform;
            RefreshClips();
        }

        private void Update()
        {
            Vector3 position = defaultPosition + targetPosition;
            Quaternion rotation = Quaternion.Euler(defaultRotation + targetRotation);

            transform.localPosition = position;
            transform.localRotation = rotation;
        }

        /// <summary>
        /// returns all the animations clip for this animator in a List of ProceduralAnimationClip and refreshes the animtor clips 
        /// </summary>
        public List<ProceduralAnimation> RefreshClips()
        {
            clips = layersHolder.GetComponentsInChildren<ProceduralAnimation>().ToList();

            return clips.ToList();
        }

        public ProceduralAnimation GetClip(string name)
        {
            RefreshClips();
            return clips.Find(clip => clip.Name == name);
        }
    }
}