using UnityEngine;
using UnityEngine.Rendering;

public class DrawMeshWithMotionVectors: MonoBehaviour
{
    [SerializeField] Mesh _mesh;
    [SerializeField] Material _material;
    [SerializeField] Shader _motionVectorShader;

    Matrix4x4 _previousModelMatrix;
    Material _motionVectorMaterial;
    CommandBuffer _commandBuffer;

    void Start()
    {
        _motionVectorMaterial = new Material(_motionVectorShader);
        _commandBuffer = new CommandBuffer();
    }

    void OnDestroy()
    {
        Destroy(_motionVectorMaterial);
        _commandBuffer.Dispose();
    }

    void Update()
    {
        _previousModelMatrix = transform.localToWorldMatrix;
    }

    void LateUpdate()
    {
        Graphics.DrawMesh(_mesh, transform.localToWorldMatrix, _material, gameObject.layer);
    }

    void OnRenderObject()
    {
        _motionVectorMaterial.SetMatrix("_PreviousM", _previousModelMatrix);
        _motionVectorMaterial.SetMatrix("_PreviousVP", CameraMatrixProvider.GetPreviousVPMatrix(Camera.current));
        _motionVectorMaterial.SetMatrix("_NonJitteredVP", CameraMatrixProvider.GetVPMatrix(Camera.current));

        _commandBuffer.Clear();
        _commandBuffer.SetRenderTarget(BuiltinRenderTextureType.MotionVectors, BuiltinRenderTextureType.CameraTarget);
        _commandBuffer.DrawMesh(_mesh, transform.localToWorldMatrix, _motionVectorMaterial, 0, 0);

        Graphics.ExecuteCommandBuffer(_commandBuffer);
    }
}
