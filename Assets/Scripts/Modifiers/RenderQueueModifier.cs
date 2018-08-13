using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Standard Shader doesn't expose its render queue value and adding this functionality would be a pain
// For opaque materials that need to be drawn in a certain order. Non-opaque materials will probably break
// WARNING: Only modifies the material from Renderer.material (only works on one material)
// TODO if needed: Add modifiers to different preset queue orders and allow script to function on multi-material objects
public class RenderQueueModifier : MonoBehaviour
{

    [SerializeField, Tooltip("Offset from Geometry. Negative values will be rendered before, positive values rendered after")]
    private int _modifier;

    public void Awake()
    {
        GetComponent<Renderer>().material.renderQueue = (int)RenderQueue.Geometry + _modifier;
    }
}
