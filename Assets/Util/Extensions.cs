using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Internal;

namespace Custom.Extensions
{
    public delegate void MemberFunction<T>(T member);
    public delegate bool MemberFunctionCondition<T>(T member);
    public delegate T2 MemberInVariableOut<T1, T2>(T1 member);

    public delegate void ApplyToRB<T>(T component);

    public static class Extension
    {
        public static void ApplyToAllChildren(this Transform trans, MemberFunction<Transform> function, bool rootAsWell = false)
        {
            Transform[] children = trans.GetComponentsInChildren<Transform>();
            int len = children.Length;

            for (int i = 0; i < len; i++)
            {
                if ((rootAsWell || children[i] != trans))
                {
                    function?.Invoke(children[i]);
                }
            }
        }

        public static void ApplyIfNotNull<T>(this Component comp, ApplyToRB<T> function)
        {
            T component = comp.GetComponent<T>();

            if (component != null)
            {
                function?.Invoke(component);
            }
        }

        public static T2[] GetVariablesOfMembers<T1, T2>(this T1[] array, MemberInVariableOut<T1, T2> getVariable)
        {
            if (getVariable == null) return null;

            List<T2> outArr = new List<T2>();
            int count = array.Length;

            for (int i = 0; i < count; i++)
            {
                outArr.Add(getVariable.Invoke(array[i]));
            }

            return outArr.ToArray();
        }

        public static T2[] GetVariablesOfMembers<T1, T2>(this List<T1> array, MemberInVariableOut<T1, T2> getVariable)
        {
            if (getVariable == null) return null;

            List<T2> outArr = new List<T2>();
            int count = array.Count;

            for (int i = 0; i < count; i++)
            {
                outArr.Add(getVariable.Invoke(array[i]));
            }

            return outArr.ToArray();
        }

        public static void ApplyToAll<T>(this T[] array, MemberFunction<T> functionToApply)
        {
            if (functionToApply == null) return;

            int count = array.Length;

            for (int i = 0; i < count; i++)
            {
                functionToApply.Invoke(array[i]);
            }
        }

        public static T[] GetMembersWithCondition<T>(this T[] array, MemberFunctionCondition<T> functionToCheck)
        {
            if (functionToCheck == null) return default;

            List<T> retArr = new List<T>();
            int count = array.Length;

            for (int i = 0; i < count; i++)
            {
                if(functionToCheck.Invoke(array[i]))
                {
                    retArr.Add(array[i]);
                }
            }


            return retArr.ToArray();
        }

        public static List<T> GetMembersWithCondition<T>(this List<T> array, MemberFunctionCondition<T> functionToCheck)
        {
            if (functionToCheck == null) return default;

            List<T> retArr = new List<T>();
            int count = array.Count;

            for (int i = 0; i < count; i++)
            {
                if (functionToCheck.Invoke(array[i]))
                {
                    retArr.Add(array[i]);
                }
            }


            return retArr;
        }

        /// <summary>
        /// Vector3.x = x;
        /// </summary>
        public static Vector3 ChangeX(this Vector3 input, float x)
        {
            return new Vector3(x, input.y, input.z);
        }

        /// <summary>
        /// Vector3.y = y;
        /// </summary>
        public static Vector3 ChangeY(this Vector3 input, float y)
        {
            return new Vector3(input.x, y, input.z);
        }

        /// <summary>
        /// Vector3.z =z;
        /// </summary>
        public static Vector3 ChangeZ(this Vector3 input, float z)
        {
            return new Vector3(input.x, input.y, z);
        }

        /// <summary>
        /// Checks if the given renderer is in Camera view
        /// </summary>
        public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
            return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
        }

        /// <summary>
        /// Remaps given value from range to new range
        /// </summary>
        public static float Remap(this float value, float from1, float to1, float from2, float to2, bool clamp = false)
        {
            if(clamp) value = Mathf.Clamp(value, from1, to1);
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        /// <summary>
        /// Checks if mouse/touch is over an UI element
        /// </summary>
        public static bool IsPointerOverUIElement()
        {
            return IsPointerOverUIElement(GetEventSystemRaycastResults());
        }

        #region PoitnerOverElementFunctions

        public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
        {
            for (int index = 0; index < eventSystemRaysastResults.Count; index++)
            {
                RaycastResult curRaysastResult = eventSystemRaysastResults[index];
                if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                    return true;
            }
            return false;
        }

        static List<RaycastResult> GetEventSystemRaycastResults()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raysastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raysastResults);
            return raysastResults;
        }

        #endregion

        /// <summary>
        /// Clamps Vector3 to the given values
        /// </summary>
        public static Vector3 Clamp(this Vector3 value, Vector3 min, Vector3 max)
        {
            value.x = Mathf.Clamp(value.x, min.x, max.x);
            value.y = Mathf.Clamp(value.y, min.y, max.y);
            value.z = Mathf.Clamp(value.z, min.z, max.z);
            return value;
        }

        /// <summary>
        /// Clamps Vector3 to the given values
        /// </summary>
        public static Vector3 Clamp(this Vector3 value, float min, float max)
        {
            return value.Clamp(new Vector3(min, min, min), new Vector3(max, max, max));
        }

        /// <summary>
        /// Cap the maximum size of a vector
        /// </summary>
        public static Vector3 MaxMagnitude(this Vector3 value, float max)
        {
            if (value.magnitude > max)
            {
                return value.normalized * max;
            }

            return value;
        }

        #region InterpolationFunctinos
        public static void LerpTo(ref this float value, float target, float distToEqual, float lerpVal)
        {
            if (Mathf.Abs(value - target) > distToEqual)
            {
                value = Mathf.Lerp(value, target, lerpVal);
            }
            else
            {
                value = target;
            }
        }

        public static bool LerpTo(ref this Vector3 value, Vector3 target, float distToSnap, float lerpVal)
        {
            if (Vector3.Distance(value, target) > distToSnap)
            {
                value = Vector3.Lerp(value, target, lerpVal);
                return false;
            }

            value = target;
            return true;
        }

        public static bool LerpTo(ref this Quaternion value, Quaternion target, float angleToSnap, float lerpVal)
        {
            if (Quaternion.Angle(value, target) > angleToSnap)
            {
                value = Quaternion.Lerp(value, target, lerpVal);
                return false;
            }

            value = target;
            return true;
        }

        public static void MoveTo(ref this float value, float target, float distToEqual, float delta)
        {
            if (Mathf.Abs(value - target) > distToEqual)
            {
                value = Mathf.MoveTowards(value, target, delta);
            }
            else
            {
                value = target;
            }
        }

        public static bool RotateTo(ref this Quaternion value, Quaternion target, float angleToSnap, float slerpVal)
        {
            if (Quaternion.Angle(value, target) > angleToSnap)
            {
                value = Quaternion.RotateTowards(value, target, slerpVal);
                return false;
            }

            value = target;
            return true;
        }
        #endregion


        /// <summary>
        /// Check if layerMask (this) contains a given layer 
        /// </summary>
        /// <param name="layer"> The layer to be checked </param>
        /// <returns> True if is contained false if not </returns>
        public static bool Contains(this LayerMask layerMask, int layer)
        {
            return layerMask == (layerMask | (1 << layer));
        }

        /// <summary>
        /// Send Raycast from the mouse/touch point
        /// </summary>
        public static bool GetHitFromMouse(this Camera camera, out RaycastHit hit, LayerMask layer)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            return Physics.Raycast(ray, out hit, float.PositiveInfinity, layer);
        }

        /// <summary>
        /// Send Raycast from the mouse/touch point
        /// </summary>
        public static bool GetHitFromMouse(this Camera camera, out RaycastHit hit)
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            return Physics.Raycast(ray, out hit, float.PositiveInfinity);
        }

        /// <summary>
        /// Go up the parent structure to Find given component
        /// </summary>
        public static T FindParentWithComponent<T>(this Transform transform)
        {
            T ret = default(T);
            Transform onTrans = transform;

            while (true)
            {
                ret = onTrans.GetComponent<T>();
                if (ret != null) return ret;

                onTrans = onTrans.parent;

                if (onTrans == null) return default(T);
            }
        }

        /// <summary>
        /// Draw a line from this transform to the given direction
        /// </summary>
        public static void DrawLineTo(this Transform transform, Vector3 direction, Color color, float duration = 0)
        {
            Debug.DrawLine(transform.position, transform.position + direction, color, duration);
        }

        /// <summary>
        /// Draw an arrow from this transform to the given direction
        /// </summary>
        public static void DrawArrowTo(this Transform transform, Vector3 direction, Color color, float duration = 0)
        {
            Vector3 endPoint = transform.position + direction;
            Debug.DrawLine(transform.position, endPoint, color, duration);

            float arrowHeadAngle = 30;
            float arrowHeadLength = 0.6f;

            Vector3 right = ((direction != Vector3.zero) ? Quaternion.LookRotation(direction) : Quaternion.identity)
                            * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = ((direction != Vector3.zero) ? Quaternion.LookRotation(direction) : Quaternion.identity)
                           * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);

            Debug.DrawRay(endPoint, right * arrowHeadLength, color);
            Debug.DrawRay(endPoint, left * arrowHeadLength, color);
        }

        /// <summary>
        /// Draw free falling projectile Trajectory of this Rigidbody
        /// </summary>
        public static void DrawTrajectory(this Rigidbody rigidbody, Vector3 gravity, float durationOfTrajectory, Color color)
        {
            Vector3[] points = rigidbody.GetTrajectoryPoints(gravity, durationOfTrajectory);

            for (int i = 0; i < points.Length - 1; i++)
            {
                Debug.DrawLine(points[i], points[i + 1], color, durationOfTrajectory);
            }
        }

        /// <summary>
        /// Get the points this Rigidbody will be at every FixedUpdate
        /// </summary>
        public static Vector3[] GetTrajectoryPoints(this Rigidbody rigidbody, Vector3 gravity, float durationOfTrajectory)
        {
            Vector3 velocity = rigidbody.linearVelocity;
            Vector3 startPoint = rigidbody.position;
            int fixedUpdateFrames = Mathf.CeilToInt(durationOfTrajectory / Time.fixedDeltaTime);
            Vector3[] points = new Vector3[fixedUpdateFrames + 1];
            points[0] = startPoint;

            for (int i = 0; i < fixedUpdateFrames; i++)
            {
                velocity += gravity * Time.fixedDeltaTime;
                Vector3 endPoint = startPoint + velocity * Time.fixedDeltaTime;
                points[i + 1] = endPoint;
                startPoint = endPoint;
            }

            return points;
        }


        /// <summary>
        /// Get the sign of the value
        /// </summary>
        public static int Sign(this float value)
        {
            if (value > 0) return +1;
            if (value < 0) return -1;

            return 0;
        }

        /// <summary>
        /// Get the sign of the value
        /// </summary>
        public static int Sign(this int value)
        {
            if (value > 0) return +1;
            if (value < 0) return -1;

            return 0;
        }

        public static void LogAllMembers<T>(this T[] array)
        {
            string outstr = "";
            foreach (T VARIABLE in array)
            {
                outstr += $"{VARIABLE}, ";
            }

            Debug.Log(outstr);
        }

        public static void LogAllMembers<T>(this List<T> array)
        {
            array.ToArray().LogAllMembers();
        }

        public static void Set(this ref LayerMask layerMask, string name)
        {
            layerMask = 1 << LayerMask.NameToLayer(name);
        }
    }

}