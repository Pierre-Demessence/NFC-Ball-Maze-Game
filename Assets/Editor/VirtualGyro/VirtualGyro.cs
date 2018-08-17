using UnityEditor;
using UnityEngine;

public class VirtualGyro : EditorWindow
{
    
    private PreviewRenderUtility _preview;

    private GameObject _proxy;
    private Transform _proxyTransform;
    private MeshFilter _proxyFilter;
    private MeshRenderer _proxyRenderer;

    private Vector2 _previewDir;
    private Vector3 _eulerAngles;
    
    [MenuItem("Tools/Virtual Gyro")]
    public static void Init()
    {
        VirtualGyro window = GetWindow<VirtualGyro>("Gyroscope");
        window.autoRepaintOnSceneChange = true;
        window.minSize = new Vector2(300, 300);
        window.position = new Rect(window.position.x, window.position.y, 300, 400);
        window.Show();
    }

    private void OnEnable()
    {
        _preview = new PreviewRenderUtility();
        _proxy = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Editor/VirtualGyro/Proxies/PhoneLandscape.prefab");
        _proxyTransform = _proxy.transform;
        _proxyTransform.eulerAngles = Vector3.zero;
        _proxyFilter = _proxy.GetComponent<MeshFilter>();
        _proxyRenderer = _proxy.GetComponent<MeshRenderer>();
        _previewDir = Vector2.zero;
        _preview.camera.clearFlags = CameraClearFlags.Nothing;
        _eulerAngles = Vector3.zero;
    }

    private void OnGUI()
    {
        
        EditorGUIUtility.labelWidth = 50;
        Rect previewRect = new Rect(0, 0, position.width, position.height / 2);
        
        
        EditorGUI.BeginChangeCheck();
        _previewDir = Drag2D(_previewDir, previewRect);
        if (EditorGUI.EndChangeCheck())
        {
            _eulerAngles.x = _previewDir.y;
            _eulerAngles.z = _previewDir.x;
        }
        else
        {
            _eulerAngles.x = Gyro.SignedAngle(EditorGUI.Slider(new Rect(0, previewRect.height, position.width, 20), "Pitch",
                Gyro.SignedAngle(_eulerAngles.x), -180f, 180));
            _eulerAngles.z = Gyro.SignedAngle(EditorGUI.Slider(
                new Rect(0, previewRect.height + position.height / 10, position.width, 20),
                "Roll", Gyro.SignedAngle(_eulerAngles.z), -180f, 180));
            _eulerAngles.y = Gyro.SignedAngle(EditorGUI.Slider(
                new Rect(0, previewRect.height + 2 * position.height / 10, position.width, 20), "Yaw",
                Gyro.SignedAngle(_eulerAngles.y), -180f, 180));
        }

        if (GUI.Button(new Rect(0, previewRect.height + 4 * position.height / 10, position.width, 20), "Reset"))
        {
            _eulerAngles = Vector3.zero;
            _previewDir = Vector2.zero;
        }

        _proxyTransform.eulerAngles = _eulerAngles;
        _preview.BeginPreview(previewRect, GUIStyle.none);
        
        Bounds bounds = _proxyFilter.sharedMesh.bounds;
        float halfSize = bounds.extents.magnitude;
        float distance = halfSize * 7f;

        _preview.camera.transform.position = Vector3.up * distance;
        _preview.camera.nearClipPlane = distance - halfSize * 1.1f;
        _preview.camera.farClipPlane = distance + halfSize * 1.1f;
        _preview.camera.transform.eulerAngles = Vector3.right * 90;

        _preview.lights[0].intensity = 1f;
        _preview.lights[0].transform.rotation = Quaternion.Euler(90,0,0);
        _preview.ambientColor = new Color(.1f, .1f, .1f, 0);
        
        _preview.DrawMesh(_proxyFilter.sharedMesh, _proxyTransform.position, _proxyTransform.rotation, _proxyRenderer.sharedMaterial, 0);
        _preview.Render();
        
        _preview.EndAndDrawPreview(previewRect);
        
        Gyro.VirtualTilt = Quaternion.Euler(Gyro.UnsignedAngle(-_eulerAngles.x), Gyro.UnsignedAngle(-_eulerAngles.z), _eulerAngles.y);
        EditorGUI.LabelField(new Rect(0, previewRect.height + 3 * position.height/10, position.width, 20), "Attitude: " + Gyro.VirtualTilt);
    }

    private void OnDisable()
    {
        _preview.Cleanup();
    }
    
    // Lifted from the CS Reference, darn internal classes
    private static Vector2 Drag2D(Vector2 scrollPosition, Rect position)
    {
        int id = GUIUtility.GetControlID(FocusType.Passive);
        Event evt = Event.current;
        switch (evt.GetTypeForControl(id))
        {
            case EventType.MouseDown:
                if (position.Contains(evt.mousePosition) && position.width > 50)
                {
                    GUIUtility.hotControl = id;
                    evt.Use();
                    EditorGUIUtility.SetWantsMouseJumping(1);
                }
                break;
            case EventType.MouseDrag:
                if (GUIUtility.hotControl == id)
                {
                    scrollPosition -= evt.delta * (evt.shift ? 3 : 1) / Mathf.Min(position.width, position.height) * 140.0f;
                    scrollPosition.y = Mathf.Clamp(scrollPosition.y, -90, 90);
                    evt.Use();
                    GUI.changed = true;
                }
                break;
            case EventType.MouseUp:
                if (GUIUtility.hotControl == id)
                    GUIUtility.hotControl = 0;
                EditorGUIUtility.SetWantsMouseJumping(0);
                break;
        }
        return scrollPosition;
    }
}
