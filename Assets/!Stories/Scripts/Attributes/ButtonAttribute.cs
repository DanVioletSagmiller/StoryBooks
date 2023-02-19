using System;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class MethodButtonAttribute : PropertyAttribute { }

[CustomEditor(typeof(MonoBehaviour), true)]
public class MethodButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawProperties(serializedObject.GetIterator());

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawProperties(SerializedProperty property)
    {
        if (property.NextVisible(true))
        {
            do
            {
                if (property.propertyType == SerializedPropertyType.ObjectReference)
                {
                    var obj = property.objectReferenceValue;
                    if (obj != null && !SerializedProperty.EqualContents(property, property.serializedObject.FindProperty(property.propertyPath)))
                    {
                        DrawProperties(new SerializedObject(obj).GetIterator());
                    }
                }
                else
                {
                    EditorGUILayout.PropertyField(property, true);
                }

                var monoBehaviour = (MonoBehaviour)target;
                var methods = monoBehaviour.GetType().GetMethods();
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(MethodButtonAttribute), true);
                    foreach (var attribute in attributes)
                    {
                        if (GUILayout.Button(method.Name))
                        {
                            method.Invoke(monoBehaviour, null);
                        }
                    }
                }
            } while (property.NextVisible(false));
        }
    }
}

