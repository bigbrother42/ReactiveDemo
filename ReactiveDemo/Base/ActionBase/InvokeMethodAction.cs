using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace ReactiveDemo.Base.ActionBase
{
    public class InvokeMethodAction : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty MethodNameProperty = DependencyProperty.Register(
            "MethodName", typeof(string), typeof(InvokeMethodAction));

        public string MethodName
        {
            get => (string)GetValue(MethodNameProperty);
            set => SetValue(MethodNameProperty, value);
        }

        public static readonly DependencyProperty InvokeSourceProperty = DependencyProperty.Register(
            "InvokeSource", typeof(DependencyObject), typeof(InvokeMethodAction), new PropertyMetadata(null));

        public DependencyObject InvokeSource
        {
            get => (DependencyObject)GetValue(InvokeSourceProperty);
            set => SetValue(InvokeSourceProperty, value);
        }

        public static readonly DependencyProperty ParameterProperty = DependencyProperty.Register(
            "Parameter", typeof(DependencyObject), typeof(InvokeMethodAction), new PropertyMetadata(null));

        public DependencyObject Parameter
        {
            get => (DependencyObject)GetValue(ParameterProperty);
            set => SetValue(ParameterProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            var hasNullParam = false;
            MethodNotification methodNotification = null;
            var parameters = new object[0];

            if (Parameter != null)
            {
                parameters = new object[] { Parameter };
            }
            else if (parameter is InteractionRequestedEventArgs arg)
            {
                methodNotification = arg.Context as MethodNotification;
                parameters = methodNotification?.Parameters;
            }

            Type type;
            DependencyObject invoker;
            if (InvokeSource != null)
            {
                type = InvokeSource.GetType();
                invoker = InvokeSource;
            }
            else
            {
                type = AssociatedObject.GetType();
                invoker = AssociatedObject;
            }

            Type[] types;
            if (parameters == null || parameters.Length == 0)
            {
                types = new Type[0];
            }
            else
            {
                types = new Type[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (parameters[i] == null)
                    {
                        hasNullParam = true;
                        break;
                    }

                    types[i] = parameters[i].GetType();
                }
            }

            var methodInfo = hasNullParam ? GetFirstMatchMethod(type, MethodName, parameters) : type.GetMethod(MethodName, types);

            if (methodInfo != null)
            {
                var result = methodInfo.Invoke(invoker, parameters);

                if (methodInfo.ReturnParameter?.ParameterType.Name != "Void" && methodNotification != null)
                {
                    methodNotification.ReturnParameter = result;
                }
            }
        }

        private MethodInfo GetFirstMatchMethod(Type type, string methodName, object[] parameters)
        {
            var methods = type.GetMethods();
            if (methods.Length == 0) return null;

            var correctMethods = methods.Where(m => m.Name == methodName);

            foreach (var method in correctMethods)
            {
                var notMatch = false;
                var ps = method.GetParameters();
                if (ps.Length != parameters.Length)
                {
                    if (ps.Length < parameters.Length)
                    {
                        continue;
                    }
                    if (!ParameterHasDefaultValueFromIndex(ps, parameters.Length)) continue;
                }

                for (var i = 0; i < parameters.Length; i++)
                {
                    var methodParamInfo = ps[i];
                    var param = parameters[i];
                    if (param == null)
                    {
                        if (methodParamInfo.ParameterType.IsSubclassOf(typeof(ValueType))) notMatch = true;
                    }
                    else
                    {
                        if (!(param.GetType() == methodParamInfo.ParameterType)) notMatch = true;
                    }
                }

                if (!notMatch)
                {

                    return method;
                }
            }

            return null;

        }

        private bool ParameterHasDefaultValueFromIndex(ParameterInfo[] ps, int index)
        {
            var allHasDefaultValue = true;
            for (var i = index; i < ps.Length; i++)
            {
                if (!ps[index].HasDefaultValue) allHasDefaultValue = false;
            }

            return allHasDefaultValue;
        }
    }
}
