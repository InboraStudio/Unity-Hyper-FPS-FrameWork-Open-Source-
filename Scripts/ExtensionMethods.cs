using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;
using UnityEngine.UI;
using UnityEngine;

namespace Inbora.FPSFramework
{
    public static class ExtensionMethods
    {
        #region Component
        /// <summary>
        /// Tries to find T on game object then child then parent
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        /// <returns></returns>
        public static T SearchFor<T>(this Component component)
        {
            if (component.GetComponent<T>() != null) return component.GetComponent<T>();
            if (component.GetComponentInChildren<T>() != null) return component.GetComponentInChildren<T>();

            return component.GetComponentInParent<T>();
        }

        /// <summary>
        /// Tries to find Component on game object then child then parent
        /// </summary>
        /// <param name="component"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Component SearchFor(this Component component, Type type)
        {
            if (component.GetComponent(type) != null) return component.GetComponent(type);
            if (component.GetComponentInChildren(type) != null) return component.GetComponentInChildren(type);

            return component.GetComponentInParent(type);
        }
        #endregion

        #region Transform
        /// <summary>
        /// Sets transform position to given position
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="position">Target position</param>
        /// <param name="local">If true position is going to chnage in local space insted of global space</param>
        public static void SetPosition(this Transform transform, Vector3 position, bool local = false)
        {
            if (local) transform.localPosition = position;
            else transform.position = position;
        }

        /// <summary>
        /// Sets transform rotation to given rotation
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="rotation">Target rotation</param>
        /// <param name="local">If true position is going to chnage in local space insted of global space</param>
        public static void SetRotation(this Transform transform, Quaternion rotation, bool local = false)
        {
            if (local) transform.localRotation = rotation;
            else transform.rotation = rotation;
        }

        /// <summary>
        /// Resets transform position, rotation & scale
        /// </summary>
        /// <param name="transform"></param>
        public static void Reset(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        /// <summary>
        /// Adds a new game object as a child of the transform
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static Transform CreateChild(this Transform transform)
        {
            Transform children = new GameObject("GameObject").transform;

            children.parent = transform;
            children.Reset();

            return children;
        }

        /// <summary>
        /// Adds a new game object as a child of the transform
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="name">Name of the child</param>
        /// <returns></returns>
        public static Transform CreateChild(this Transform transform, string name)
        {
            Transform children = new GameObject(name).transform;

            children.parent = transform;
            children.Reset();

            return children;
        }


        /// <summary>
        /// Adds a new game object as a child of the transform
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="names">Target names the more names you have the more childern you get</param>
        /// <param name="parentAll">If true all childern will be child if each other</param>
        /// <returns></returns>
        public static Transform[] CreateChildren(this Transform transform, string[] names, bool parentAll = false)
        {
            List<Transform> transforms = new List<Transform>();

            for (int i = 0; i < names.Length; i++)
            {
                Transform child = CreateChild(transform, names[i]);
                transforms.Add(child);

                if (parentAll)
                {
                    if (i > 1)
                    {
                        transforms[1].SetParent(transforms[0]);

                        child.SetParent(transforms[transforms.Count - 2]);
                    }
                }
            }

            return transforms.ToArray();
        }

        /// <summary>
        /// destroies all childern in transform
        /// </summary>
        /// <param name="transform"></param>
        public static void ClearChildren(this Transform transform)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }
        }

        public static void SetPositionAndRotation(this Transform transform, Vector3 position, Quaternion rotation, bool local = false)
        {
            if (!local)
            {
                transform.position = position;
                transform.rotation = rotation;
            }
            else
            {
                transform.localPosition = position;
                transform.localRotation = rotation;
            }
        }

        public static void SetPositionAndRotation(this Transform transform, Vector3 position, Vector3 eulerAngles, bool local = false)
        {
            if (!local)
            {
                transform.position = position;
                transform.eulerAngles = eulerAngles;
            }
            else
            {
                transform.localPosition = position;
                transform.localEulerAngles = eulerAngles;
            }
        }

        public static Vector3 GetDirection(this Transform transform, Vector3Direction direction)
        {
            Vector3 dir = Vector3.zero;

            switch(direction)
            {
                case Vector3Direction.forward:
                    dir = transform.forward;
                    break;

                case Vector3Direction.back:
                    dir = -transform.forward;

                    break;

                case Vector3Direction.right:
                    dir = transform.right;

                    break;

                case Vector3Direction.left:
                    dir = -transform.right;

                    break;

                case Vector3Direction.up:
                    dir = transform.up;

                    break;

                case Vector3Direction.down:
                    dir = -transform.up;

                    break;
            }

            return dir;
        }
        #endregion

        #region Character Controller
        public static bool IsVelocityZero(this CharacterController characterController)
        {
            //check if player is standing still if yes set to true else set to false
            return characterController.velocity.magnitude <= 0;
        }
        #endregion

        #region Rigidbody
        public static bool IsVelocityZero(this Rigidbody rigidbody)
        {
            //check if rigidbody is not moving if yes set to true else set to false
            return rigidbody.velocity.magnitude <= 0;
        }
        #endregion

        #region Dropdown
        public static void AddOption(this Dropdown dropdown, string option)
        {
            dropdown.AddOptions(new List<Dropdown.OptionData>() { new Dropdown.OptionData() { text = option } });
        }
        #endregion

        #region Input Action
        /// <summary>
        /// Checks for douple clicks and sets targetValue to true if the user has douple clicked
        /// </summary>
        /// <param name="inputAction"></param>
        /// <param name="targetValue"></param>
        /// <param name="lastClickTime"></param>
        /// <returns></returns>
        public static void HasDoupleClicked(this InputAction inputAction, ref bool targetValue, ref float lastClickTime, float maxClickTime = 0.5f)
        {
            if (inputAction.triggered)
            {
                float timeSinceLastSprintClick = Time.time - lastClickTime;

                if (timeSinceLastSprintClick < maxClickTime)
                {
                    targetValue = true;
                }

                lastClickTime = Time.time;
            }

            if (inputAction.IsPressed() == false) targetValue = false;
        }
        #endregion

        #region Resolution
        public static string GetDetails(this Resolution resolution)
        {
            return $"{resolution.height}x{resolution.width} {resolution.refreshRate}Hz";
        }
        #endregion

        #region Animator
        /// <summary>
        /// Returns true if the animator is playing that animation state at the moment.
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="animationStateName">The animation state that is being checked to see if it is playing or not.</param>
        /// <returns></returns>
        public static bool IsPlaying(this Animator animator, string animationStateName)
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName(animationStateName);
        }
        #endregion
    }
}