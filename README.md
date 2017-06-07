DrawMeshWithMotionVectors
=========================

This is an example showing how to generate per-object motion vectors correctly
while using Graphics.DrawMesh to draw objects.

![gif](http://i.imgur.com/iKeWAAG.gif)

The sphere in the gif above is drawn with using Graphics.DrawMesh. The previous
versions of Unity can't generate per-object motion vectors on this sphere, so
that only camera motion is applied in motion effects (motion blur, TAA, etc.).

Unity 5.6 introduced [BuiltinRenderTextureType.MotionVectors] that provides
access to the internal motion vectors buffer. One can overwrite it with using
command buffers to generate per-object motion vectors on objects drawn with
DrawMesh and its variants.

[Graphics.DrawMesh]: https://docs.unity3d.com/ScriptReference/Graphics.DrawMesh.html
[BuiltinRenderTextureType.MotionVectors]: https://docs.unity3d.com/ScriptReference/Rendering.BuiltinRenderTextureType.MotionVectors.html
