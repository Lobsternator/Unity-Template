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
    /// <summary>
    /// Singleton wrapper class around <see cref="Handles"/>, and <see cref="Gizmos"/> which allows for many of their draw functions to be called from anywhere (instead of being limited to OnDrawGizmos).
    /// <b>Automatically created at the start of the program.</b>
    /// </summary>
    [PersistentRuntimeObject(RuntimeInitializeLoadType.BeforeSceneLoad, -1500)]
    public class DebugManager : PersistentRuntimeSingleton<DebugManager>
    {
        public static Color color;

        private static readonly string _gizmosAssemblyName        = typeof(Gizmos).AssemblyQualifiedName;
        private static readonly string _handlesAssemblyName       = typeof(Handles).AssemblyQualifiedName;
        private static Dictionary<string, DebugRequest> _requests = new Dictionary<string, DebugRequest>();

        private static bool CanMakeRequest()
        {
            if (!Instance)
            {
                Debug.LogWarning($"{nameof(DebugManager)} instance needs to exist in order to make a debug request!");
                return false;
            }

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

        public static void DrawAAConvexPolygon(float duration, Vector3[] points) => DrawAAConvexPolygon(duration, points, GUID.Generate().ToString());
        public static void DrawAAConvexPolygon(float duration, Vector3[] points, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _handlesAssemblyName, nameof(Handles.DrawAAConvexPolygon),
                points);
        }
        public static void DrawAAPolyLine(float duration, Texture2D lineTex, Vector3[] points) => DrawAAPolyLine(duration, lineTex, points, GUID.Generate().ToString());
        public static void DrawAAPolyLine(float duration, Texture2D lineTex, Vector3[] points, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _handlesAssemblyName, nameof(Handles.DrawAAPolyLine),
                lineTex, points);
        }
        public static void DrawAAPolyLine(float duration, float width, Vector3[] points) => DrawAAPolyLine(duration, width, points, GUID.Generate().ToString());
        public static void DrawAAPolyLine(float duration, float width, Vector3[] points, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _handlesAssemblyName, nameof(Handles.DrawAAPolyLine),
                width, points);
        }
        public static void DrawAAPolyLine(float duration, float width, int actualNumberOfPoints, Vector3[] points) => DrawAAPolyLine(duration, width, actualNumberOfPoints, points, GUID.Generate().ToString());
        public static void DrawAAPolyLine(float duration, float width, int actualNumberOfPoints, Vector3[] points, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _handlesAssemblyName, nameof(Handles.DrawAAPolyLine),
                width, actualNumberOfPoints, points);
        }
        public static void DrawAAPolyLine(float duration, Texture2D lineTex, float width, Vector3[] points) => DrawAAPolyLine(duration, lineTex, width, points, GUID.Generate().ToString());
        public static void DrawAAPolyLine(float duration, Texture2D lineTex, float width, Vector3[] points, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _handlesAssemblyName, nameof(Handles.DrawAAPolyLine),
                lineTex, width, points);
        }
        public static void DrawAAPolyLine(float duration, Vector3[] points) => DrawAAPolyLine(duration, points, GUID.Generate().ToString());
        public static void DrawAAPolyLine(float duration, Vector3[] points, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _handlesAssemblyName, nameof(Handles.DrawAAPolyLine),
                points);
        }
        public static void DrawBezier(float duration, Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent, Color color, Texture2D texture, float width) => DrawBezier(duration, startPosition, endPosition, startTangent, endTangent, color, texture, width, GUID.Generate().ToString());
        public static void DrawBezier(float duration, Vector3 startPosition, Vector3 endPosition, Vector3 startTangent, Vector3 endTangent, Color color, Texture2D texture, float width, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _handlesAssemblyName, nameof(Handles.DrawBezier),
                startPosition, endPosition, startTangent, endTangent, color, texture, width);
        }
        public static void DrawCamera(float duration, Rect position, Camera camera, DrawCameraMode drawMode) => DrawCamera(duration, position, camera, drawMode, GUID.Generate().ToString());
        public static void DrawCamera(float duration, Rect position, Camera camera, DrawCameraMode drawMode, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _handlesAssemblyName, nameof(Handles.DrawCamera),
                position, camera, drawMode);
        }
        public static void DrawCamera(float duration, Rect position, Camera camera) => DrawCamera(duration, position, camera, GUID.Generate().ToString());
        public static void DrawCamera(float duration, Rect position, Camera camera, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _handlesAssemblyName, nameof(Handles.DrawCamera),
                position, camera);
        }
        public static void DrawCube(float duration, Vector3 center, Vector3 size) => DrawCube(duration, center, size, GUID.Generate().ToString());
        public static void DrawCube(float duration, Vector3 center, Vector3 size, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawCube),
                center, size);
        }
        public static void DrawDottedLine(float duration, Vector3 p1, Vector3 p2, float screenSpaceSize) => DrawDottedLine(duration, p1, p2, screenSpaceSize, GUID.Generate().ToString());
        public static void DrawDottedLine(float duration, Vector3 p1, Vector3 p2, float screenSpaceSize, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _handlesAssemblyName, nameof(Handles.DrawDottedLine),
                p1, p2, screenSpaceSize);
        }
        public static void DrawDottedLines(float duration, Vector3[] points, int[] segmentIndices, float screenSpaceSize) => DrawDottedLines(duration, points, segmentIndices, screenSpaceSize, GUID.Generate().ToString());
        public static void DrawDottedLines(float duration, Vector3[] points, int[] segmentIndices, float screenSpaceSize, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _handlesAssemblyName, nameof(Handles.DrawDottedLines),
                points, segmentIndices, screenSpaceSize);
        }
        public static void DrawDottedLines(float duration, Vector3[] lineSegments, float screenSpaceSize) => DrawDottedLines(duration, lineSegments, screenSpaceSize, GUID.Generate().ToString());
        public static void DrawDottedLines(float duration, Vector3[] lineSegments, float screenSpaceSize, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _handlesAssemblyName, nameof(Handles.DrawDottedLines),
                lineSegments, screenSpaceSize);
        }
        public static void DrawFrustum(float duration, Vector3 center, float fov, float maxRange, float minRange, float aspect) => DrawFrustum(duration, center, fov, maxRange, minRange, aspect, GUID.Generate().ToString());
        public static void DrawFrustum(float duration, Vector3 center, float fov, float maxRange, float minRange, float aspect, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawFrustum),
                center, fov, maxRange, minRange, aspect);
        }
        public static void DrawGUITexture(float duration, Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder) => DrawGUITexture(duration, screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder, GUID.Generate().ToString());
        public static void DrawGUITexture(float duration, Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawGUITexture),
                screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder);
        }
        public static void DrawGUITexture(float duration, Rect screenRect, Texture texture, Material mat) => DrawGUITexture(duration, screenRect, texture, mat, GUID.Generate().ToString());
        public static void DrawGUITexture(float duration, Rect screenRect, Texture texture, Material mat, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawGUITexture),
                screenRect, texture, mat);
        }
        public static void DrawGUITexture(float duration, Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Material mat) => DrawGUITexture(duration, screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder, mat, GUID.Generate().ToString());
        public static void DrawGUITexture(float duration, Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Material mat, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawGUITexture),
                screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder, mat);
        }
        public static void DrawGUITexture(float duration, Rect screenRect, Texture texture) => DrawGUITexture(duration, screenRect, texture, GUID.Generate().ToString());
        public static void DrawGUITexture(float duration, Rect screenRect, Texture texture, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawGUITexture),
                screenRect, texture);
        }
        public static void DrawIcon(float duration, Vector3 center, string name, bool allowScaling) => DrawIcon(duration, center, name, allowScaling, GUID.Generate().ToString());
        public static void DrawIcon(float duration, Vector3 center, string name, bool allowScaling, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawIcon),
                center, name, allowScaling);
        }
        public static void DrawIcon(float duration, Vector3 center, string name, bool allowScaling, Color tint) => DrawIcon(duration, center, name, allowScaling, tint, GUID.Generate().ToString());
        public static void DrawIcon(float duration, Vector3 center, string name, bool allowScaling, Color tint, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawIcon),
                center, name, allowScaling, tint);
        }
        public static void DrawIcon(float duration, Vector3 center, string name) => DrawIcon(duration, center, name, GUID.Generate().ToString());
        public static void DrawIcon(float duration, Vector3 center, string name, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawIcon),
                center, name);
        }
        public static void DrawLine(float duration, Vector3 from, Vector3 to) => DrawLine(duration, from, to, GUID.Generate().ToString());
        public static void DrawLine(float duration, Vector3 from, Vector3 to, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawLine),
                from, to);
        }
        public static void DrawLines(float duration, Vector3[] points, int[] segmentIndices) => DrawLines(duration, points, segmentIndices, GUID.Generate().ToString());
        public static void DrawLines(float duration, Vector3[] points, int[] segmentIndices, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _handlesAssemblyName, nameof(Handles.DrawLines),
                points, segmentIndices);
        }
        public static void DrawLines(float duration, Vector3[] lineSegments) => DrawLines(duration, lineSegments, GUID.Generate().ToString());
        public static void DrawLines(float duration, Vector3[] lineSegments, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _handlesAssemblyName, nameof(Handles.DrawLines),
                lineSegments);
        }
        public static void DrawMesh(float duration, Mesh mesh) => DrawMesh(duration, mesh, GUID.Generate().ToString());
        public static void DrawMesh(float duration, Mesh mesh, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawMesh),
                mesh);
        }
        public static void DrawMesh(float duration, Mesh mesh, int submeshIndex, Vector3 position, Quaternion rotation, Vector3 scale) => DrawMesh(duration, mesh, submeshIndex, position, rotation, scale, GUID.Generate().ToString());
        public static void DrawMesh(float duration, Mesh mesh, int submeshIndex, Vector3 position, Quaternion rotation, Vector3 scale, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawMesh),
                mesh, submeshIndex, position, rotation, scale);
        }
        public static void DrawMesh(float duration, Mesh mesh, Vector3 position) => DrawMesh(duration, mesh, position, GUID.Generate().ToString());
        public static void DrawMesh(float duration, Mesh mesh, Vector3 position, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawMesh),
                mesh, position);
        }
        public static void DrawMesh(float duration, Mesh mesh, int submeshIndex) => DrawMesh(duration, mesh, submeshIndex, GUID.Generate().ToString());
        public static void DrawMesh(float duration, Mesh mesh, int submeshIndex, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawMesh),
                mesh, submeshIndex);
        }
        public static void DrawMesh(float duration, Mesh mesh, int submeshIndex, Vector3 position, Quaternion rotation) => DrawMesh(duration, mesh, submeshIndex, position, rotation, GUID.Generate().ToString());
        public static void DrawMesh(float duration, Mesh mesh, int submeshIndex, Vector3 position, Quaternion rotation, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawMesh),
                mesh, submeshIndex, position, rotation);
        }
        public static void DrawMesh(float duration, Mesh mesh, Vector3 position, Quaternion rotation) => DrawMesh(duration, mesh, position, rotation, GUID.Generate().ToString());
        public static void DrawMesh(float duration, Mesh mesh, Vector3 position, Quaternion rotation, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawMesh),
                mesh, position, rotation);
        }
        public static void DrawMesh(float duration, Mesh mesh, Vector3 position, Quaternion rotation, Vector3 scale) => DrawMesh(duration, mesh, position, rotation, scale, GUID.Generate().ToString());
        public static void DrawMesh(float duration, Mesh mesh, Vector3 position, Quaternion rotation, Vector3 scale, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawMesh),
                mesh, position, rotation, scale);
        }
        public static void DrawMesh(float duration, Mesh mesh, int submeshIndex, Vector3 position) => DrawMesh(duration, mesh, submeshIndex, position, GUID.Generate().ToString());
        public static void DrawMesh(float duration, Mesh mesh, int submeshIndex, Vector3 position, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawMesh),
                mesh, submeshIndex, position);
        }
        public static void DrawPolyLine(float duration, Vector3[] points) => DrawPolyLine(duration, points, GUID.Generate().ToString());
        public static void DrawPolyLine(float duration, Vector3[] points, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _handlesAssemblyName, nameof(Handles.DrawPolyLine),
                points);
        }
        public static void DrawRay(float duration, Vector3 from, Vector3 direction) => DrawRay(duration, from, direction, GUID.Generate().ToString());
        public static void DrawRay(float duration, Vector3 from, Vector3 direction, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawRay),
                from, direction);
        }
        public static void DrawRay(float duration, Ray ray) => DrawRay(duration, ray, GUID.Generate().ToString());
        public static void DrawRay(float duration, Ray ray, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawRay),
                ray);
        }
        public static void DrawSolidArc(float duration, Vector3 center, Vector3 normal, Vector3 from, float angle, float radius) => DrawSolidArc(duration, center, normal, from, angle, radius, GUID.Generate().ToString());
        public static void DrawSolidArc(float duration, Vector3 center, Vector3 normal, Vector3 from, float angle, float radius, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _handlesAssemblyName, nameof(Handles.DrawSolidArc),
                center, normal, from, angle, radius);
        }
        public static void DrawSolidDisc(float duration, Vector3 center, Vector3 normal, float radius) => DrawSolidDisc(duration, center, normal, radius, GUID.Generate().ToString());
        public static void DrawSolidDisc(float duration, Vector3 center, Vector3 normal, float radius, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _handlesAssemblyName, nameof(Handles.DrawSolidDisc),
                center, normal, radius);
        }
        public static void DrawSolidRectangleWithOutline(float duration, Vector3[] verts, Color faceColor, Color outlineColor) => DrawSolidRectangleWithOutline(duration, verts, faceColor, outlineColor, GUID.Generate().ToString());
        public static void DrawSolidRectangleWithOutline(float duration, Vector3[] verts, Color faceColor, Color outlineColor, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _handlesAssemblyName, nameof(Handles.DrawSolidRectangleWithOutline),
                verts, faceColor, outlineColor);
        }
        public static void DrawSolidRectangleWithOutline(float duration, Rect rectangle, Color faceColor, Color outlineColor) => DrawSolidRectangleWithOutline(duration, rectangle, faceColor, outlineColor, GUID.Generate().ToString());
        public static void DrawSolidRectangleWithOutline(float duration, Rect rectangle, Color faceColor, Color outlineColor, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _handlesAssemblyName, nameof(Handles.DrawSolidRectangleWithOutline),
                rectangle, faceColor, outlineColor);
        }
        public static void DrawSphere(float duration, Vector3 center, float radius) => DrawSphere(duration, center, radius, GUID.Generate().ToString());
        public static void DrawSphere(float duration, Vector3 center, float radius, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawSphere),
                center, radius);
        }
        public static void DrawTexture3DSDF(float duration, Texture texture, float stepScale = 1, float surfaceOffset = 0, Gradient customColorRamp = null) => DrawTexture3DSDF(duration, texture, stepScale, surfaceOffset, customColorRamp, GUID.Generate().ToString());
        public static void DrawTexture3DSDF(float duration, Texture texture, float stepScale = 1, float surfaceOffset = 0, Gradient customColorRamp = null, string id = null)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _handlesAssemblyName, nameof(Handles.DrawTexture3DSDF),
                texture, stepScale, surfaceOffset, customColorRamp);
        }
        public static void DrawTexture3DSlice(float duration, Texture texture, Vector3 slicePositions, FilterMode filterMode = FilterMode.Bilinear, bool useColorRamp = false, Gradient customColorRamp = null) => DrawTexture3DSlice(duration, texture, slicePositions, filterMode, useColorRamp, customColorRamp, GUID.Generate().ToString());
        public static void DrawTexture3DSlice(float duration, Texture texture, Vector3 slicePositions, FilterMode filterMode = FilterMode.Bilinear, bool useColorRamp = false, Gradient customColorRamp = null, string id = null)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _handlesAssemblyName, nameof(Handles.DrawTexture3DSlice),
                texture, slicePositions, filterMode, useColorRamp, customColorRamp);
        }
        public static void DrawTexture3DVolume(float duration, Texture texture, float opacity = 1, float qualityModifier = 1, FilterMode filterMode = FilterMode.Bilinear, bool useColorRamp = false, Gradient customColorRamp = null) => DrawTexture3DVolume(duration, texture, opacity, qualityModifier, filterMode, useColorRamp, customColorRamp, GUID.Generate().ToString());
        public static void DrawTexture3DVolume(float duration, Texture texture, float opacity = 1, float qualityModifier = 1, FilterMode filterMode = FilterMode.Bilinear, bool useColorRamp = false, Gradient customColorRamp = null, string id = null)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _handlesAssemblyName, nameof(Handles.DrawTexture3DVolume),
                texture, opacity, qualityModifier, filterMode, useColorRamp, customColorRamp);
        }
        public static void DrawWireArc(float duration, Vector3 center, Vector3 normal, Vector3 from, float angle, float radius) => DrawWireArc(duration, center, normal, from, angle, radius, GUID.Generate().ToString());
        public static void DrawWireArc(float duration, Vector3 center, Vector3 normal, Vector3 from, float angle, float radius, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _handlesAssemblyName, nameof(Handles.DrawWireArc),
                center, normal, from, angle, radius);
        }
        public static void DrawWireCube(float duration, Vector3 center, Vector3 size) => DrawWireCube(duration, center, size, GUID.Generate().ToString());
        public static void DrawWireCube(float duration, Vector3 center, Vector3 size, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawWireCube),
                center, size);
        }
        public static void DrawWireDisc(float duration, Vector3 center, Vector3 normal, float radius) => DrawWireDisc(duration, center, normal, radius, GUID.Generate().ToString());
        public static void DrawWireDisc(float duration, Vector3 center, Vector3 normal, float radius, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _handlesAssemblyName, nameof(Handles.DrawWireDisc),
                center, normal, radius);
        }
        public static void DrawWireDisc(float duration, Vector3 center, Vector3 normal, float radius, float thickness) => DrawWireDisc(duration, center, normal, radius, thickness, GUID.Generate().ToString());
        public static void DrawWireDisc(float duration, Vector3 center, Vector3 normal, float radius, float thickness, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _handlesAssemblyName, nameof(Handles.DrawWireDisc),
                center, normal, radius, thickness);
        }
        public static void DrawWireMesh(float duration, Mesh mesh, Vector3 position, Quaternion rotation) => DrawWireMesh(duration, mesh, position, rotation, GUID.Generate().ToString());
        public static void DrawWireMesh(float duration, Mesh mesh, Vector3 position, Quaternion rotation, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawWireMesh),
                mesh, position, rotation);
        }
        public static void DrawWireMesh(float duration, Mesh mesh, Vector3 position) => DrawWireMesh(duration, mesh, position, GUID.Generate().ToString());
        public static void DrawWireMesh(float duration, Mesh mesh, Vector3 position, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawWireMesh),
                mesh, position);
        }
        public static void DrawWireMesh(float duration, Mesh mesh) => DrawWireMesh(duration, mesh, GUID.Generate().ToString());
        public static void DrawWireMesh(float duration, Mesh mesh, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawWireMesh),
                mesh);
        }
        public static void DrawWireMesh(float duration, Mesh mesh, Vector3 position, Quaternion rotation, Vector3 scale) => DrawWireMesh(duration, mesh, position, rotation, scale, GUID.Generate().ToString());
        public static void DrawWireMesh(float duration, Mesh mesh, Vector3 position, Quaternion rotation, Vector3 scale, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawWireMesh),
                mesh, position, rotation, scale);
        }
        public static void DrawWireMesh(float duration, Mesh mesh, int submeshIndex, Vector3 position, Quaternion rotation) => DrawWireMesh(duration, mesh, submeshIndex, position, rotation, GUID.Generate().ToString());
        public static void DrawWireMesh(float duration, Mesh mesh, int submeshIndex, Vector3 position, Quaternion rotation, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawWireMesh),
                mesh, submeshIndex, position, rotation);
        }
        public static void DrawWireMesh(float duration, Mesh mesh, int submeshIndex) => DrawWireMesh(duration, mesh, submeshIndex, GUID.Generate().ToString());
        public static void DrawWireMesh(float duration, Mesh mesh, int submeshIndex, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawWireMesh),
                mesh, submeshIndex);
        }
        public static void DrawWireMesh(float duration, Mesh mesh, int submeshIndex, Vector3 position, Quaternion rotation, Vector3 scale) => DrawWireMesh(duration, mesh, submeshIndex, position, rotation, scale, GUID.Generate().ToString());
        public static void DrawWireMesh(float duration, Mesh mesh, int submeshIndex, Vector3 position, Quaternion rotation, Vector3 scale, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawWireMesh),
                mesh, submeshIndex, position, rotation, scale);
        }
        public static void DrawWireMesh(float duration, Mesh mesh, int submeshIndex, Vector3 position) => DrawWireMesh(duration, mesh, submeshIndex, position, GUID.Generate().ToString());
        public static void DrawWireMesh(float duration, Mesh mesh, int submeshIndex, Vector3 position, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawWireMesh),
                mesh, submeshIndex, position);
        }
        public static void DrawWireSphere(float duration, Vector3 center, float radius) => DrawWireSphere(duration, center, radius, GUID.Generate().ToString());
        public static void DrawWireSphere(float duration, Vector3 center, float radius, string id)
        {
            if (id is null) id = GUID.Generate().ToString();

            DebugRequest(id, duration, _gizmosAssemblyName, nameof(Gizmos.DrawWireSphere),
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
