using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

// Unity complains when you change the properties of a material in edit mode. This exists to do it "safely" without modifying the orignal script
// Pretty sure this will still cause a nasty leak, but it makes the console happy
[CustomEditor(typeof(RenderQueueModifier)), CanEditMultipleObjects]
public class RenderQueueModifierEditor : Editor
{
    private SerializedProperty _modifier;

    private void OnEnable()
    {
        _modifier = serializedObject.FindProperty("_modifier");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        // Don't do anything else if this is hidden from the hierarchy
        // All uninstantiated prefabs will have this flag set
        // Objects in the scene might have this set as well, which sucks
        if (serializedObject.targetObject.hideFlags.HasFlag(HideFlags.HideInHierarchy)) 
            return;
        
        if (serializedObject.isEditingMultipleObjects)
        {
            foreach (var obj in serializedObject.targetObjects)
            {
                UpdateRenderQueue(((RenderQueueModifier)obj).gameObject);
            }
        }
        else
        {
            UpdateRenderQueue((((RenderQueueModifier)serializedObject.targetObject).gameObject));
        }
    }

    private void UpdateRenderQueue(GameObject gameObject)
    {
        var renderer = gameObject.GetComponent<Renderer>();
        var material = new Material(renderer.sharedMaterial);
        material.renderQueue = (int)RenderQueue.Geometry + _modifier.intValue;
        renderer.sharedMaterial = material;
    }
}
