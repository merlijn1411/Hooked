using System;
using UnityEditor;
using UnityEngine;

namespace MethodInvokerSystem
{
    [CustomEditor(typeof(MethodInvoker))]
    public class MethodInvokerEditor : Editor
    {
        private MethodInvoker _invoker;
        
        private int _componentIndex = 0;
        private int _methodIndex = 0;

        public override void OnInspectorGUI()
        {
            _invoker = (MethodInvoker)target;
            serializedObject.Update();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            EditorGUI.EndDisabledGroup();
            
            #region MethodList
            
            var methodCallsProperty = serializedObject.FindProperty("methodCalls");
            var interval = serializedObject.FindProperty("interval");
            
            EditorGUILayout.LabelField("Method Calls", EditorStyles.boldLabel);

            for (var i = 0; i < methodCallsProperty.arraySize; i++)
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);
                var callProp = methodCallsProperty.GetArrayElementAtIndex(i);
                var targetObject = callProp.FindPropertyRelative("targetObject");
                
                EditorGUI.BeginChangeCheck();
                EditorGUIUtility.labelWidth = 100f;
                EditorGUILayout.PropertyField(targetObject, new GUIContent($"Target Object {i + 1}"));
                
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_invoker, "Change Target Object");
                    var call = _invoker.methodCalls[i];
                    call.targetObject = targetObject.objectReferenceValue as GameObject; 
                    _invoker.UpdateComponents(call);
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(_invoker);
                }
                
                //Component Field
                EditorGUILayout.BeginHorizontal();

                var callData = _invoker.methodCalls[i];
                if (callData.targetObject != null && callData.componentNames.Count == 0) 
                {
                    _invoker.UpdateComponents(callData);
                    serializedObject.ApplyModifiedProperties();
                }
                
                if (callData.componentNames.Count > 0)
                {
                    EditorGUI.BeginChangeCheck();
                    EditorGUIUtility.labelWidth = 10f;
                    _componentIndex = Mathf.Max(0, callData.componentNames.IndexOf(callData.componentName));
                    _componentIndex = EditorGUILayout.Popup("", _componentIndex, callData.componentNames.ToArray());
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(_invoker, "Change Selected Component");
                        _invoker.UpdateMethods(callData, _componentIndex);
                        _invoker.UpdateComponents(callData);
                        callData.componentName = callData.componentNames[_componentIndex];
                        EditorUtility.SetDirty(_invoker);
                    }
                }

                //Methods PopUp
                if (!string.IsNullOrEmpty(callData.componentName))
                {
                    if (callData.methodNames.Count == 0)
                    {
                        _invoker.UpdateMethods(callData, 0);
                        serializedObject.ApplyModifiedProperties();
                    }
                    
                    if (callData.methodNames.Count > 0)
                    {
                        EditorGUI.BeginChangeCheck();
                        _methodIndex = Mathf.Max(0, callData.methodNames.IndexOf(callData.methodName));
                        _methodIndex = EditorGUILayout.Popup("", _methodIndex, callData.methodNames.ToArray());
                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(_invoker, "Change Selected Method");
                            _invoker.UpdateMethods(callData, _methodIndex);
                            callData.methodName = callData.methodNames[_methodIndex];
                            EditorUtility.SetDirty(_invoker);
                        }
                    }
                }
                
                //Method with parameter
                if (!string.IsNullOrEmpty(callData.parameterType))
                {
                    EditorGUI.indentLevel++;
                
                    void HandleChange<T>(string label, T currentValue, Action<T> onChange, Func<string, T, T> drawField)
                    {
                        EditorGUI.BeginChangeCheck();
                        var newValue = drawField(label, currentValue);
                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(_invoker, $"Change {typeof(T).Name} Parameter");
                            onChange(newValue);
                            EditorUtility.SetDirty(_invoker);
                        }
                    }
                    
                    switch (callData.parameterType)
                    {
                        case "Int32":
                            HandleChange<int>("Value", callData.intParameter, v => callData.intParameter = v,(label, val) => EditorGUILayout.IntField(label, val));
                            break;
                        case "Single":
                            HandleChange<float>("Value", callData.floatParameter, v => callData.floatParameter = v,(label, val) => EditorGUILayout.FloatField(label, val));
                            break;
                        case "String":
                            HandleChange<string>("Value", callData.stringParameter, v => callData.stringParameter = v,(label, val) => EditorGUILayout.TextField(label, val));
                            break;
                        case "Boolean":
                            HandleChange<bool>("Value", callData.boolParameter, v => callData.boolParameter = v,(label, val) => EditorGUILayout.Toggle(label, val));
                            break;
                        default:
                            var paramType = Type.GetType(callData.parameterType) ?? typeof(UnityEngine.Object);
                            HandleChange<UnityEngine.Object>("Value", callData.objectParameter, v => callData.objectParameter = v,(label, val) => EditorGUILayout.ObjectField(label, val, paramType, true));
                        break;
                    }
                    
                    EditorGUI.indentLevel--;
                }
                else
                {
                    EditorGUILayout.LabelField("Parameter not found");
                }
              
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginHorizontal();
                
                EditorGUIUtility.labelWidth = 10f;
                if (GUILayout.Button("Invoke Method"))
                {
                    _invoker.InvokeMethod(i);
                    break;
                }
                
                if (GUILayout.Button("Remove Method"))
                {
                    Undo.RecordObject(_invoker, "Remove Method Call");
                    _invoker.methodCalls.RemoveAt(i);
                    EditorUtility.SetDirty(_invoker);
                    break;
                }
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }
            
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Add Method"))
            {
                Undo.RecordObject(_invoker, "Add Method Call");
                _invoker.methodCalls.Add(new MethodCall());
                EditorUtility.SetDirty(_invoker);
            }

            if (_invoker.methodCalls.Count > 0 && GUILayout.Button("Invoke All Methods"))
            {
                _invoker.StartInvokingAll();
            }
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUIUtility.labelWidth = 50f;
            EditorGUILayout.PropertyField(interval, new GUIContent("interval"));
            
            #endregion
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}