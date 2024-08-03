using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inbora.FPSFramework
{
    public struct MathUtilities
    {
        public static Vector3 MultiplyVectors(Vector3 a, Vector3 b)
        {
            a.x *= b.x;
            a.y *= b.y;
            a.z *= b.z;

            return a;
        }

        public static Vector3 GetVector3Direction(Vector3Direction direction)
        {
            Vector3 vector = Vector3.zero;

            switch (direction)
            {
                case Vector3Direction.forward:
                    vector = Vector3.forward;
                    break;
                case Vector3Direction.back:
                    vector = Vector3.back;
                    break;
                case Vector3Direction.right:
                    vector = Vector3.right;
                    break;
                case Vector3Direction.left:
                    vector = Vector3.left;
                    break;
                case Vector3Direction.up:
                    vector = Vector3.up;
                    break;
                case Vector3Direction.down:
                    vector = Vector3.down;
                    break;
            }

            return vector;
        }

        public static Quaternion GetFromToRotation(RaycastHit raycastHit, Vector3Direction direction)
        {
            Quaternion result = new Quaternion();

            switch (direction)
            {
                case Vector3Direction.forward:
                    result = Quaternion.FromToRotation(Vector3.forward, raycastHit.normal);
                    break;

                case Vector3Direction.back:
                    result = Quaternion.FromToRotation(Vector3.back, raycastHit.normal);
                    break;

                case Vector3Direction.right:
                    result = Quaternion.FromToRotation(Vector3.right, raycastHit.normal);
                    break;

                case Vector3Direction.left:
                    result = Quaternion.FromToRotation(Vector3.left, raycastHit.normal);
                    break;

                case Vector3Direction.up:
                    result = Quaternion.FromToRotation(Vector3.up, raycastHit.normal);
                    break;

                case Vector3Direction.down:
                    result = Quaternion.FromToRotation(Vector3.down, raycastHit.normal);
                    break;
            }

            return result;
        }
    }
}