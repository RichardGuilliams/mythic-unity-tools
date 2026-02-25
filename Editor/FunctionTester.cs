using UnityEditor;
using UnityEngine;

public class MyToolWindow : EditorWindow
{
    private FunctionInvokerSettings settings;
    private SerializedObject so;
    private SerializedProperty onInvokeProp;

    private AbstractObject targetObject;
    private string methodName = "DoTheThing";

    [MenuItem("MythicTools/Function Invoker")]
    public static void Open()
    {
        GetWindow<MyToolWindow>("Function Invoker");
    }

    private void OnEnable()
    {
        // Create an in-memory settings object (you can also load/create an asset instead)
        settings = CreateInstance<FunctionInvokerSettings>();

        so = new SerializedObject(settings);
        onInvokeProp = so.FindProperty("onInvoke");
    }

    private void OnGUI()
    {
        GUILayout.Label("Invoker Tool", EditorStyles.boldLabel);

        targetObject = (AbstractObject)EditorGUILayout.ObjectField(
            "Target Object",
            targetObject,
            typeof(AbstractObject),
            true
        );

        methodName = EditorGUILayout.TextField("Method Name", methodName);

        // Draw UnityEvent properly
        so.Update();
        EditorGUILayout.PropertyField(onInvokeProp);
        so.ApplyModifiedProperties();

        GUILayout.Space(10);

        using (new EditorGUI.DisabledScope(targetObject == null))
        {
            if (GUILayout.Button("Invoke (UnityEvent)"))
            {
                settings.onInvoke?.Invoke();
            }

            if (GUILayout.Button("Invoke (Reflection)"))
            {
                InvokeByName(targetObject, methodName);
            }
        }
    }

    private void InvokeByName(AbstractObject obj, string name)
    {
        var comps = obj.GetComponents<MonoBehaviour>();
        foreach (var c in comps)
        {
            var m = c.GetType().GetMethod(name,
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic);

            if (m != null)
            {
                m.Invoke(c, null);
                Debug.Log($"Invoked {name} on {c.GetType().Name}");
                return;
            }
        }

        Debug.LogWarning($"No method '{name}' found on {obj.name}");
    }
}
