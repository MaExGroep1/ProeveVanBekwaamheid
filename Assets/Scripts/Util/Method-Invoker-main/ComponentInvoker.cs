using System.Reflection;
using UnityEngine;

namespace Util.Method_Invoker_main
{
    public class ComponentInvoker : MonoBehaviour
    {
        [SerializeField] private Component component;
        [SerializeField] private BindingFlags bindingFlags;
        [SerializeField] private string selectedMethod;
    }
} 