using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MethodInvokerSystem
{
    public class MethodInvoker : MonoBehaviour
    {
        public List<MethodCall> methodCalls = new List<MethodCall>();

        public float interval;
        private BindingFlags _bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

        public void StartInvokingAll()
        {
            StartCoroutine(InvokeAllMethods());
        }
        
        public IEnumerator InvokeAllMethods()
        {
            foreach (var call in methodCalls)
            {
                if (!call.targetObject || string.IsNullOrEmpty(call.componentName) || string.IsNullOrEmpty(call.methodName))
                {
                    Debug.LogWarning($"Settings not valid. Check Target object , component and method.");
                    continue;
                }
                
                var component = call.targetObject.GetComponents<Component>().FirstOrDefault(c => c.GetType().Name == call.componentName);
                
                if (!component)
                {
                    print($"Component{call.componentName} not found"); 
                    continue;
                }

                var methodInfo = component.GetType().GetMethod(call.methodName, _bindingFlags);

                if (methodInfo == null) continue;
                try
                {
                    var parameters = methodInfo.GetParameters();
                    switch (parameters.Length)
                    {
                        case 0:
                            methodInfo.Invoke(component, null);
                            break;
                        case 1:
                        {
                            var parameterValue = GetParameterValue(call, parameters[0].ParameterType);
                            if (parameterValue != null)
                            {
                                methodInfo.Invoke(component, new object[] {parameterValue});
                            }
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    print(e);
                }
                yield return new WaitForSeconds(interval);
            }
        }
        
        public void InvokeMethod(int index)
        {
            var component = methodCalls[index].targetObject.GetComponents<Component>()
                .FirstOrDefault(c => c.GetType().Name == methodCalls[index].componentName);

            if (!component)
                print($"Component{methodCalls[index].componentName} not found");
                

            var methodInfo = component?.GetType().GetMethod(methodCalls[index].methodName, _bindingFlags);

            if (methodInfo == null) return;
            try
            {
                var parameters = methodInfo.GetParameters();
                switch (parameters.Length)
                {
                    case 0:
                        methodInfo.Invoke(component, null);
                        break;
                    case 1:
                    {
                        var parameterValue = GetParameterValue(methodCalls[index], parameters[0].ParameterType);
                        if (parameterValue != null)
                        {
                            methodInfo.Invoke(component, new [] {parameterValue});
                        }

                        break;
                    }
                }
            }
            catch (Exception e)
            {
                print(e);
            }

        }

        public void UpdateComponents(MethodCall call)
        {
            call.componentNames.Clear();
            call.methodNames.Clear();
            call.componentName = "";
            call.methodName = "";

            if (call.targetObject)
            {
                var components = call.targetObject.GetComponents<Component>();
                call.componentNames = components.Select(c => c.GetType().Name).ToList();
            }
            
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void UpdateMethods(MethodCall call, int index)
        {
            call.methodNames.Clear();
            call.methodName = "";
            call.parameterType = "";

            if (call.targetObject != null && !string.IsNullOrEmpty(call.componentName))
            {
                var component = call.targetObject.GetComponents<Component>().FirstOrDefault(c => c.GetType().Name == call.componentName);

                if (component != null)
                {
                    var methods = component.GetType()
                        .GetMethods(_bindingFlags)
                        .Where(m => !m.IsSpecialName && m.GetParameters().Length <= 1).ToArray();
                    
                    call.methodName = methods[index].Name;

                    call.methodNames.AddRange(methods.Select(m => m.Name));
                    
                    if (!string.IsNullOrEmpty(call.methodName))
                    {
                        var selectedMethod = methods.FirstOrDefault(m => m.Name == call.methodName);
                        if (selectedMethod != null && selectedMethod.GetParameters().Length == 1)
                        {
                            call.parameterType = selectedMethod.GetParameters()[0].ParameterType.Name;
                        }
                            
                    }
                }
            }
            
        }
        
        private object GetParameterValue(MethodCall call, Type parameterType)
        {
            if (parameterType == typeof(int)) return call.intParameter;
            if (parameterType == typeof(float)) return call.floatParameter;
            if (parameterType == typeof(string)) return call.stringParameter;
            if (parameterType == typeof(bool)) return call.boolParameter;
            if (typeof(Object).IsAssignableFrom(parameterType)) return call.objectParameter;
            return null;
        }
    }

    [System.Serializable]
    public class MethodCall
    {
        public GameObject targetObject;
        public string componentName;
        public string methodName;
        
        [HideInInspector] public List<string> componentNames = new List<string>();
        [HideInInspector] public List<string> methodNames = new List<string>();
        
        public string parameterType;
        public int intParameter;
        public float floatParameter;
        public bool boolParameter;
        public string stringParameter;
        public Object objectParameter;
    }
}