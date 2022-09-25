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
        [HideInInspector] public Color color;
        
        private readonly string gizmosAssemblyName  = typeof(Gizmos).AssemblyQualifiedName;
        private readonly string handlesAssemblyName = typeof(Handles).AssemblyQualifiedName;

        private Dictionary<string, DebugRequest> _requests = new Dictionary<string, DebugRequest>();

        private string GetID(int line, string file) => string.Format("{0}<{1}>", file, line);

        private bool TryUpdateRequest(string id, float duration, params object[] parameters)
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
        private void CreateNewRequest(string id, float duration, string drawAssemblyName, string drawMethodName, params object[] parameters)
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
        private void DebugRequest(string id, float duration, string drawAssemblyName, string drawMethodName, params object[] parameters)
        {
            if (TryUpdateRequest(id, duration, parameters))
                return;
            
            CreateNewRequest(id, duration, drawAssemblyName, drawMethodName, parameters);
        }

        public void DrawAAConvexPolygon(float duration, Vector3[] points, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, handlesAssemblyName, nameof(Handles.DrawAAConvexPolygon),
                points);
        }
        public void DrawAAPolyLine(float duration, Texture2D lineTex, Vector3[] points, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, handlesAssemblyName, nameof(Handles.DrawAAPolyLine),
                lineTex, points);
        }
        public void DrawAAPolyLine(float duration, float width, Vector3[] points, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, handlesAssemblyName, nameof(Handles.DrawAAPolyLine),
                width, points);
        }
        public void DrawAAPolyLine(float duration, float width, int actualNumberOfPoints, Vector3[] points, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, handlesAssemblyName, nameof(Handles.DrawAAPolyLine),
                width, actualNumberOfPoints, points);
        }
        public void DrawAAPolyLine(float duration, Texture2D lineTex, float width, Vector3[] points, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, handlesAssemblyName, nameof(Handles.DrawAAPolyLine),
                lineTex, width, points);
        }
        public void DrawAAPolyLine(float duration, Vector3[] points, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, handlesAssemblyName, nameof(Handles.DrawAAPolyLine),
                points);
        }
        public void DrawBezier(float duration, Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent, Color color, Texture2D texture, float width, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, handlesAssemblyName, nameof(Handles.DrawBezier),
                startPosition, endPosition, startTangent, endTangent, color, texture, width);
        }
        public void DrawCamera(float duration, Rect position, Camera camera, DrawCameraMode drawMode, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, handlesAssemblyName, nameof(Handles.DrawCamera),
                position, camera, drawMode);
        }
        public void DrawCamera(float duration, Rect position, Camera camera, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, handlesAssemblyName, nameof(Handles.DrawCamera),
                position, camera);
        }
        public void DrawCube(float duration, Vector3 center, Vector3 size, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawCube), 
                center, size);
        }
        public void DrawDottedLine(float duration, Vector3 p1, Vector3 p2, float screenSpaceSize, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, handlesAssemblyName, nameof(Handles.DrawDottedLine),
                p1, p2, screenSpaceSize);
        }
        public void DrawDottedLines(float duration, Vector3[] points, int[] segmentIndices, float screenSpaceSize, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, handlesAssemblyName, nameof(Handles.DrawDottedLines),
                points, segmentIndices, screenSpaceSize);
        }
        public void DrawDottedLines(float duration, Vector3[] lineSegments, float screenSpaceSize, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, handlesAssemblyName, nameof(Handles.DrawDottedLines),
                lineSegments, screenSpaceSize);
        }
        public void DrawFrustum(float duration, Vector3 center, float fov, float maxRange, float minRange, float aspect, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawFrustum), 
                center, fov, maxRange, minRange, aspect);
        }
        public void DrawGUITexture(float duration, Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawGUITexture), 
                screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder);
        }
        public void DrawGUITexture(float duration, Rect screenRect, Texture texture, Material mat, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawGUITexture), 
                screenRect, texture, mat);
        }
        public void DrawGUITexture(float duration, Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Material mat, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawGUITexture), 
                screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder, mat);
        }
        public void DrawGUITexture(float duration, Rect screenRect, Texture texture, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawGUITexture), 
                screenRect, texture);
        }
        public void DrawIcon(float duration, Vector3 center, string name, bool allowScaling, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawIcon), 
                center, name, allowScaling);
        }
        public void DrawIcon(float duration, Vector3 center, string name, bool allowScaling, Color tint, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawIcon), 
                center, name, allowScaling, tint);
        }
        public void DrawIcon(float duration, Vector3 center, string name, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawIcon), 
                center, name);
        }
        public void DrawLine(float duration, Vector3 from, Vector3 to, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawLine), 
                from, to);
        }
        public void DrawLines(float duration, Vector3[] points, int[] segmentIndices, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, handlesAssemblyName, nameof(Handles.DrawLines),
                points, segmentIndices);
        }
        public void DrawLines(float duration, Vector3[] lineSegments, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, handlesAssemblyName, nameof(Handles.DrawLines),
                lineSegments);
        }
        public void DrawMesh(float duration, Mesh mesh, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawMesh), 
                mesh);
        }
        public void DrawMesh(float duration, Mesh mesh, int submeshIndex, Vector3 position, Quaternion rotation, Vector3 scale, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawMesh), 
                mesh, submeshIndex, position, rotation, scale);
        }
        public void DrawMesh(float duration, Mesh mesh, Vector3 position, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawMesh), 
                mesh, position);
        }
        public void DrawMesh(float duration, Mesh mesh, int submeshIndex, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawMesh), 
                mesh, submeshIndex);
        }
        public void DrawMesh(float duration, Mesh mesh, int submeshIndex, Vector3 position, Quaternion rotation, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawMesh), 
                mesh, submeshIndex, position, rotation);
        }
        public void DrawMesh(float duration, Mesh mesh, Vector3 position, Quaternion rotation, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawMesh), 
                mesh, position, rotation);
        }
        public void DrawMesh(float duration, Mesh mesh, Vector3 position, Quaternion rotation, Vector3 scale, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawMesh), 
                mesh, position, rotation, scale);
        }
        public void DrawMesh(float duration, Mesh mesh, int submeshIndex, Vector3 position, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawMesh), 
                mesh, submeshIndex, position);
        }
        public void DrawPolyLine(float duration, Vector3[] points, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, handlesAssemblyName, nameof(Handles.DrawPolyLine),
                points);
        }
        public void DrawRay(float duration, Vector3 from, Vector3 direction, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawRay), 
                from, direction);
        }
        public void DrawRay(float duration, Ray ray, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawRay), 
                ray);
        }
        public void DrawSolidArc(float duration, Vector3 center, Vector3 normal, Vector3 from, float angle, float radius, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, handlesAssemblyName, nameof(Handles.DrawSolidArc),
                center, normal, from, angle, radius);
        }
        public void DrawSolidDisc(float duration, Vector3 center, Vector3 normal, float radius, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, handlesAssemblyName, nameof(Handles.DrawSolidDisc),
                center, normal, radius);
        }
        public void DrawSolidRectangleWithOutline(float duration, Vector3[] verts, Color faceColor, Color outlineColor, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, handlesAssemblyName, nameof(Handles.DrawSolidRectangleWithOutline),
                verts, faceColor, outlineColor);
        }
        public void DrawSolidRectangleWithOutline(float duration, Rect rectangle, Color faceColor, Color outlineColor, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, handlesAssemblyName, nameof(Handles.DrawSolidRectangleWithOutline),
                rectangle, faceColor, outlineColor);
        }
        public void DrawSphere(float duration, Vector3 center, float radius, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawSphere), 
                center, radius);
        }
        public void DrawTexture3DSDF(float duration, Texture texture, float stepScale = 1, float surfaceOffset = 0, Gradient customColorRamp = null, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, handlesAssemblyName, nameof(Handles.DrawTexture3DSDF),
                texture, stepScale, surfaceOffset, customColorRamp);
        }
        public void DrawTexture3DSlice(float duration, Texture texture, Vector3 slicePositions, FilterMode filterMode = FilterMode.Bilinear, bool useColorRamp = false, Gradient customColorRamp = null, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, handlesAssemblyName, nameof(Handles.DrawTexture3DSlice),
                texture, slicePositions, filterMode, useColorRamp, customColorRamp);
        }
        public void DrawTexture3DVolume(float duration, Texture texture, float opacity = 1, float qualityModifier = 1, FilterMode filterMode = FilterMode.Bilinear, bool useColorRamp = false, Gradient customColorRamp = null, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, handlesAssemblyName, nameof(Handles.DrawTexture3DVolume),
                texture, opacity, qualityModifier, filterMode, useColorRamp, customColorRamp);
        }
        public void DrawWireArc(float duration, Vector3 center, Vector3 normal, Vector3 from, float angle, float radius, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, handlesAssemblyName, nameof(Handles.DrawWireArc),
                center, normal, from, angle, radius);
        }
        public void DrawWireCube(float duration, Vector3 center, Vector3 size, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawWireCube), 
                center, size);
        }
        public void DrawWireDisc(float duration, Vector3 center, Vector3 normal, float radius, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, handlesAssemblyName, nameof(Handles.DrawWireDisc),
                center, normal, radius);
        }
        public void DrawWireDisc(float duration, Vector3 center, Vector3 normal, float radius, float thickness, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, handlesAssemblyName, nameof(Handles.DrawWireDisc),
                center, normal, radius, thickness);
        }
        public void DrawWireMesh(float duration, Mesh mesh, Vector3 position, Quaternion rotation, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawWireMesh), 
                mesh, position, rotation);
        }
        public void DrawWireMesh(float duration, Mesh mesh, Vector3 position, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawWireMesh), 
                mesh, position);
        }
        public void DrawWireMesh(float duration, Mesh mesh, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawWireMesh), 
                mesh);
        }
        public void DrawWireMesh(float duration, Mesh mesh, Vector3 position, Quaternion rotation, Vector3 scale, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawWireMesh), 
                mesh, position, rotation, scale);
        }
        public void DrawWireMesh(float duration, Mesh mesh, int submeshIndex, Vector3 position, Quaternion rotation, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawWireMesh), 
                mesh, submeshIndex, position, rotation);
        }
        public void DrawWireMesh(float duration, Mesh mesh, int submeshIndex, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawWireMesh), 
                mesh, submeshIndex);
        }
        public void DrawWireMesh(float duration, Mesh mesh, int submeshIndex, Vector3 position, Quaternion rotation, Vector3 scale, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawWireMesh), 
                mesh, submeshIndex, position, rotation, scale);
        }
        public void DrawWireMesh(float duration, Mesh mesh, int submeshIndex, Vector3 position, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawWireMesh), 
                mesh, submeshIndex, position);
        }
        public void DrawWireSphere(float duration, Vector3 center, float radius, [CallerLineNumber] int line = 0, [CallerFilePath] string file = "")
        {
            DebugRequest(GetID(line, file), duration, gizmosAssemblyName, nameof(Gizmos.DrawWireSphere), 
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

            KeyValuePair<string, DebugRequest>[] requestsToRemove;
            if (Application.isPlaying)
                requestsToRemove = _requests.Where((r) => r.Value.HasExpired).ToArray();
            else
                requestsToRemove = _requests.Where((r) => r.Value.HasExpiredInEditor).ToArray();

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
