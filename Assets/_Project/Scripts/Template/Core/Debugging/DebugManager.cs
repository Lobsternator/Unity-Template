#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using Type = System.Type;

namespace Template.Core
{
    [PersistentRuntimeObject(RuntimeInitializeLoadType.BeforeSceneLoad, -1500)]
    public class DebugManager : PersistentRuntimeSingleton<DebugManager>
    {
        public static Color color;

        private static readonly string _gizmosAssemblyName        = typeof(Gizmos).AssemblyQualifiedName;
        private static readonly string _handlesAssemblyName       = typeof(Handles).AssemblyQualifiedName;
        private static Dictionary<string, DebugRequest> _requests = new Dictionary<string, DebugRequest>();

        private static string GetID(int line, string file) => $"{file}<{line}>";

        private static bool CanMakeRequest()
        {
#if UNITY_EDITOR
            if (!Instance)
            {
                Debug.LogWarning($"{nameof(DebugManager)} instance needs to exist in order to make a debug request!");
                return false;
            }
#endif

            return true;
        }
        private static bool TryUpdateRequest(string id, float duration, params object[] parameters)
        {
            if (_requests.TryGetValue(id, out DebugRequest request))
            {
                request.startTime  = Time.time;
                request.duration   = duration;
                request.parameters = parameters;
                request.color      = color;

                return true;
            }

            return false;
        }
        private static void CreateNewRequest(string id, float duration, string drawAssemblyName, string drawMethodName, params object[] parameters)
        {
            DebugRequest request  = new DebugRequest();
            Type[] parameterTypes = parameters.Select((p) => p.GetType()).ToArray();

            request.startTime  = Time.time;
            request.duration   = duration;
            request.drawMethod = Type.GetType(drawAssemblyName).GetMethod(drawMethodName, parameterTypes);
            request.parameters = parameters;
            request.color      = color;

            _requests.Add(id, request);
        }
        private static void DebugRequest(string id, float duration, string drawAssemblyName, string drawMethodName, params object[] parameters)
        {
            if (!CanMakeRequest())
                return;

            if (TryUpdateRequest(id, duration, parameters))
                return;
            
            CreateNewRequest(id, duration, drawAssemblyName, drawMethodName, parameters);
        }

        public static void DrawAAConvexPolygon(float duration, Vector3[] points, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _handlesAssemblyName, nameof(Handles.DrawAAConvexPolygon),
                points);
        }
        public static void DrawAAPolyLine(float duration, Texture2D lineTex, Vector3[] points, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _handlesAssemblyName, nameof(Handles.DrawAAPolyLine),
                lineTex, points);
        }
        public static void DrawAAPolyLine(float duration, float width, Vector3[] points, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _handlesAssemblyName, nameof(Handles.DrawAAPolyLine),
                width, points);
        }
        public static void DrawAAPolyLine(float duration, float width, int actualNumberOfPoints, Vector3[] points, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _handlesAssemblyName, nameof(Handles.DrawAAPolyLine),
                width, actualNumberOfPoints, points);
        }
        public static void DrawAAPolyLine(float duration, Texture2D lineTex, float width, Vector3[] points, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _handlesAssemblyName, nameof(Handles.DrawAAPolyLine),
                lineTex, width, points);
        }
        public static void DrawAAPolyLine(float duration, Vector3[] points, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _handlesAssemblyName, nameof(Handles.DrawAAPolyLine),
                points);
        }
        public static void DrawBezier(float duration, Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent, Color color, Texture2D texture, float width, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _handlesAssemblyName, nameof(Handles.DrawBezier),
                startPosition, endPosition, startTangent, endTangent, color, texture, width);
        }
        public static void DrawCamera(float duration, Rect position, Camera camera, DrawCameraMode drawMode, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _handlesAssemblyName, nameof(Handles.DrawCamera),
                position, camera, drawMode);
        }
        public static void DrawCamera(float duration, Rect position, Camera camera, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _handlesAssemblyName, nameof(Handles.DrawCamera),
                position, camera);
        }
        public static void DrawCube(float duration, Vector3 center, Vector3 size, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawCube),
                center, size);
        }
        public static void DrawDottedLine(float duration, Vector3 p1, Vector3 p2, float screenSpaceSize, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _handlesAssemblyName, nameof(Handles.DrawDottedLine),
                p1, p2, screenSpaceSize);
        }
        public static void DrawDottedLines(float duration, Vector3[] points, int[] segmentIndices, float screenSpaceSize, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _handlesAssemblyName, nameof(Handles.DrawDottedLines),
                points, segmentIndices, screenSpaceSize);
        }
        public static void DrawDottedLines(float duration, Vector3[] lineSegments, float screenSpaceSize, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _handlesAssemblyName, nameof(Handles.DrawDottedLines),
                lineSegments, screenSpaceSize);
        }
        public static void DrawFrustum(float duration, Vector3 center, float fov, float maxRange, float minRange, float aspect, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawFrustum),
                center, fov, maxRange, minRange, aspect);
        }
        public static void DrawGUITexture(float duration, Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawGUITexture),
                screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder);
        }
        public static void DrawGUITexture(float duration, Rect screenRect, Texture texture, Material mat, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawGUITexture),
                screenRect, texture, mat);
        }
        public static void DrawGUITexture(float duration, Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Material mat, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawGUITexture),
                screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder, mat);
        }
        public static void DrawGUITexture(float duration, Rect screenRect, Texture texture, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawGUITexture),
                screenRect, texture);
        }
        public static void DrawIcon(float duration, Vector3 center, string name, bool allowScaling, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawIcon),
                center, name, allowScaling);
        }
        public static void DrawIcon(float duration, Vector3 center, string name, bool allowScaling, Color tint, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawIcon),
                center, name, allowScaling, tint);
        }
        public static void DrawIcon(float duration, Vector3 center, string name, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawIcon),
                center, name);
        }
        public static void DrawLine(float duration, Vector3 from, Vector3 to, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawLine),
                from, to);
        }
        public static void DrawLines(float duration, Vector3[] points, int[] segmentIndices, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _handlesAssemblyName, nameof(Handles.DrawLines),
                points, segmentIndices);
        }
        public static void DrawLines(float duration, Vector3[] lineSegments, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _handlesAssemblyName, nameof(Handles.DrawLines),
                lineSegments);
        }
        public static void DrawMesh(float duration, Mesh mesh, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawMesh),
                mesh);
        }
        public static void DrawMesh(float duration, Mesh mesh, int submeshIndex, Vector3 position, Quaternion rotation, Vector3 scale, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawMesh),
                mesh, submeshIndex, position, rotation, scale);
        }
        public static void DrawMesh(float duration, Mesh mesh, Vector3 position, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawMesh),
                mesh, position);
        }
        public static void DrawMesh(float duration, Mesh mesh, int submeshIndex, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawMesh),
                mesh, submeshIndex);
        }
        public static void DrawMesh(float duration, Mesh mesh, int submeshIndex, Vector3 position, Quaternion rotation, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawMesh),
                mesh, submeshIndex, position, rotation);
        }
        public static void DrawMesh(float duration, Mesh mesh, Vector3 position, Quaternion rotation, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawMesh),
                mesh, position, rotation);
        }
        public static void DrawMesh(float duration, Mesh mesh, Vector3 position, Quaternion rotation, Vector3 scale, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawMesh),
                mesh, position, rotation, scale);
        }
        public static void DrawMesh(float duration, Mesh mesh, int submeshIndex, Vector3 position, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawMesh),
                mesh, submeshIndex, position);
        }
        public static void DrawPolyLine(float duration, Vector3[] points, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _handlesAssemblyName, nameof(Handles.DrawPolyLine),
                points);
        }
        public static void DrawRay(float duration, Vector3 from, Vector3 direction, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawRay),
                from, direction);
        }
        public static void DrawRay(float duration, Ray ray, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawRay),
                ray);
        }
        public static void DrawSolidArc(float duration, Vector3 center, Vector3 normal, Vector3 from, float angle, float radius, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _handlesAssemblyName, nameof(Handles.DrawSolidArc),
                center, normal, from, angle, radius);
        }
        public static void DrawSolidDisc(float duration, Vector3 center, Vector3 normal, float radius, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _handlesAssemblyName, nameof(Handles.DrawSolidDisc),
                center, normal, radius);
        }
        public static void DrawSolidRectangleWithOutline(float duration, Vector3[] verts, Color faceColor, Color outlineColor, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _handlesAssemblyName, nameof(Handles.DrawSolidRectangleWithOutline),
                verts, faceColor, outlineColor);
        }
        public static void DrawSolidRectangleWithOutline(float duration, Rect rectangle, Color faceColor, Color outlineColor, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _handlesAssemblyName, nameof(Handles.DrawSolidRectangleWithOutline),
                rectangle, faceColor, outlineColor);
        }
        public static void DrawSphere(float duration, Vector3 center, float radius, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawSphere),
                center, radius);
        }
        public static void DrawTexture3DSDF(float duration, Texture texture, float stepScale = 1, float surfaceOffset = 0, Gradient customColorRamp = null, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _handlesAssemblyName, nameof(Handles.DrawTexture3DSDF),
                texture, stepScale, surfaceOffset, customColorRamp);
        }
        public static void DrawTexture3DSlice(float duration, Texture texture, Vector3 slicePositions, FilterMode filterMode = FilterMode.Bilinear, bool useColorRamp = false, Gradient customColorRamp = null, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _handlesAssemblyName, nameof(Handles.DrawTexture3DSlice),
                texture, slicePositions, filterMode, useColorRamp, customColorRamp);
        }
        public static void DrawTexture3DVolume(float duration, Texture texture, float opacity = 1, float qualityModifier = 1, FilterMode filterMode = FilterMode.Bilinear, bool useColorRamp = false, Gradient customColorRamp = null, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _handlesAssemblyName, nameof(Handles.DrawTexture3DVolume),
                texture, opacity, qualityModifier, filterMode, useColorRamp, customColorRamp);
        }
        public static void DrawWireArc(float duration, Vector3 center, Vector3 normal, Vector3 from, float angle, float radius, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _handlesAssemblyName, nameof(Handles.DrawWireArc),
                center, normal, from, angle, radius);
        }
        public static void DrawWireCube(float duration, Vector3 center, Vector3 size, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawWireCube),
                center, size);
        }
        public static void DrawWireDisc(float duration, Vector3 center, Vector3 normal, float radius, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _handlesAssemblyName, nameof(Handles.DrawWireDisc),
                center, normal, radius);
        }
        public static void DrawWireDisc(float duration, Vector3 center, Vector3 normal, float radius, float thickness, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _handlesAssemblyName, nameof(Handles.DrawWireDisc),
                center, normal, radius, thickness);
        }
        public static void DrawWireMesh(float duration, Mesh mesh, Vector3 position, Quaternion rotation, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawWireMesh),
                mesh, position, rotation);
        }
        public static void DrawWireMesh(float duration, Mesh mesh, Vector3 position, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawWireMesh),
                mesh, position);
        }
        public static void DrawWireMesh(float duration, Mesh mesh, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawWireMesh),
                mesh);
        }
        public static void DrawWireMesh(float duration, Mesh mesh, Vector3 position, Quaternion rotation, Vector3 scale, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawWireMesh),
                mesh, position, rotation, scale);
        }
        public static void DrawWireMesh(float duration, Mesh mesh, int submeshIndex, Vector3 position, Quaternion rotation, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawWireMesh),
                mesh, submeshIndex, position, rotation);
        }
        public static void DrawWireMesh(float duration, Mesh mesh, int submeshIndex, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawWireMesh),
                mesh, submeshIndex);
        }
        public static void DrawWireMesh(float duration, Mesh mesh, int submeshIndex, Vector3 position, Quaternion rotation, Vector3 scale, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawWireMesh),
                mesh, submeshIndex, position, rotation, scale);
        }
        public static void DrawWireMesh(float duration, Mesh mesh, int submeshIndex, Vector3 position, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawWireMesh),
                mesh, submeshIndex, position);
        }
        public static void DrawWireSphere(float duration, Vector3 center, float radius, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, _gizmosAssemblyName, nameof(Gizmos.DrawWireSphere),
                center, radius);
        }

        private void OnEnable()
        {
            _requests.Clear();
        }
        private void OnDisable()
        {
            _requests.Clear();
        }

        private void OnDrawGizmos()
        {
            foreach (DebugRequest request in _requests.Values)
            {
                Handles.color = request.color;
                Gizmos.color  = request.color;

                request.drawMethod.Invoke(null, request.parameters);

                Handles.color = Color.white;
                Gizmos.color  = Color.white;
            }

            var requestsToRemove = _requests.Where((r) => r.Value.HasExpired).ToArray();
            foreach (var request in requestsToRemove)
                _requests.Remove(request.Key);

            color = Color.white;
        }

        protected override void Awake()
        {
            base.Awake();
            if (IsDuplicate)
                return;

            color = Color.white;
        }
    }
}
#endif
