using UnityEngine;
using System.Collections.Generic;

public static class CameraMatrixProvider
{
    static Dictionary<Camera, Matrix4x4> _currentVPMatrix = new Dictionary<Camera, Matrix4x4>();
    static Dictionary<Camera, Matrix4x4> _previousVPMatrix = new Dictionary<Camera, Matrix4x4>();
    static int _frameCount;

    public static Matrix4x4 GetVPMatrix(Camera camera)
    {
        if (Time.frameCount != _frameCount) SwapMatrixMap();

        Matrix4x4 m;

        if (!_currentVPMatrix.TryGetValue(camera, out m))
        {
            m = camera.nonJitteredProjectionMatrix * camera.worldToCameraMatrix;
            _currentVPMatrix.Add(camera, m);
        }

        return m;
    }

    public static Matrix4x4 GetPreviousVPMatrix(Camera camera)
    {
        if (Time.frameCount != _frameCount) SwapMatrixMap();

        Matrix4x4 m;

        if (_previousVPMatrix.TryGetValue(camera, out m))
            return m;
        else
            return GetVPMatrix(camera);
    }

    static void SwapMatrixMap()
    {
        var temp = _previousVPMatrix;
        _previousVPMatrix = _currentVPMatrix;
        temp.Clear();
        _currentVPMatrix = temp;
        _frameCount = Time.frameCount;
    }
}
