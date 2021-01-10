using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace StEn.FinCalcR.WinUi.Behavior
{
    /// <summary>
    /// Calls a method on a specified object when invoked.
    /// That version is a copy of https://github.com/microsoft/XamlBehaviorsWpf/blob/e1629d5fa5a9c08a86a677de80ecb2f045616eb6/src/Microsoft.Xaml.Behaviors/Core/CallMethodAction.cs
    /// The copy was altered to handel async Task as target method.
    /// </summary>
    public class CustomCallMethodAction : TriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty TargetObjectProperty = DependencyProperty.Register("TargetObject", typeof(object), typeof(CustomCallMethodAction), new PropertyMetadata(OnTargetObjectChanged));
        public static readonly DependencyProperty MethodNameProperty = DependencyProperty.Register("MethodName", typeof(string), typeof(CustomCallMethodAction), new PropertyMetadata(OnMethodNameChanged));

        private List<MethodDescriptor> methodDescriptors;

        public CustomCallMethodAction()
        {
            this.methodDescriptors = new List<MethodDescriptor>();
        }

        /// <summary>
        /// Gets or sets the object that exposes the method of interest. This is a dependency property.
        /// </summary>
        public object TargetObject
        {
            get => this.GetValue(TargetObjectProperty);
            set => this.SetValue(TargetObjectProperty, value);
        }

        /// <summary>
        /// Gets or sets the name of the method to invoke. This is a dependency property.
        /// </summary>
        public string MethodName
        {
            get => (string)this.GetValue(MethodNameProperty);
            set => this.SetValue(MethodNameProperty, value);
        }

        private object Target => this.TargetObject ?? this.AssociatedObject;

        /// <summary>
        /// Invokes the action.
        /// </summary>
        /// <param name="parameter">The parameter of the action. If the action does not require a parameter, the parameter may be set to a null reference.</param>
        ///// <exception cref="ArgumentException">A method with <c cref="MethodName"/> could not be found on the <c cref="TargetObject"/>.</exception>
        protected override void Invoke(object parameter)
        {
            if (this.AssociatedObject == null)
            {
                return;
            }

            var methodDescriptor = this.FindBestMethod(parameter);
            if (methodDescriptor != null)
            {
                var parameters = methodDescriptor.Parameters;

                switch (parameters.Length)
                {
                    case 0:
                        methodDescriptor.MethodInfo.Invoke(this.Target, null);
                        break;
                    case 2 when this.AssociatedObject != null && parameter != null:
                        {
                            if (parameters[0].ParameterType.IsInstanceOfType(this.AssociatedObject)
                                && parameters[1].ParameterType.IsInstanceOfType(parameter))
                            {
                                methodDescriptor.MethodInfo.Invoke(this.Target, new[] { this.AssociatedObject, parameter });
                            }

                            break;
                        }
                }
            }
            else if (this.TargetObject != null)
            {
                throw new ArgumentException("No suitable method found!");
            }
        }

        /// <summary>
        /// Called after the action is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.UpdateMethodInfo();
        }

        /// <summary>
        /// Called when the action is getting detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>Override this to unhook functionality from the AssociatedObject.</remarks>
        protected override void OnDetaching()
        {
            this.methodDescriptors.Clear();
            base.OnDetaching();
        }

        private static bool AreMethodParamsValid(ParameterInfo[] methodParams)
        {
            if (methodParams.Length == 2)
            {
                if (methodParams[0].ParameterType != typeof(object))
                {
                    return false;
                }

                if (!typeof(EventArgs).IsAssignableFrom(methodParams[1].ParameterType))
                {
                    return false;
                }
            }
            else if (methodParams.Length != 0)
            {
                return false;
            }

            return true;
        }

        private static void OnMethodNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args) => OnTargetObjectChanged(sender, args);

        private static void OnTargetObjectChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var callMethodAction = (CustomCallMethodAction)sender;
            callMethodAction.UpdateMethodInfo();
        }

        private MethodDescriptor FindBestMethod(object parameter) => this.methodDescriptors.Find((methodDescriptor) => !methodDescriptor.HasParameters ||
                                                                                                                                                  (parameter != null &&
                                                                                                                                                   methodDescriptor.SecondParameterType.IsInstanceOfType(parameter)));

        private void UpdateMethodInfo()
        {
            this.methodDescriptors.Clear();

            if (this.Target != null && !string.IsNullOrEmpty(this.MethodName))
            {
                var targetType = this.Target.GetType();
                var methods = targetType.GetMethods(BindingFlags.Public | BindingFlags.Instance);

                for (var i = 0; i < methods.Length; i++)
                {
                    var method = methods[i];

                    if (!this.IsMethodValid(method))
                    {
                        continue;
                    }

                    var methodParams = method.GetParameters();

                    if (!AreMethodParamsValid(methodParams))
                    {
                        continue;
                    }

                    this.methodDescriptors.Add(new MethodDescriptor(method, methodParams));
                }

                this.methodDescriptors = this.methodDescriptors.OrderByDescending((methodDescriptor) =>
                {
                    var distanceFromBaseClass = 0;

                    if (!methodDescriptor.HasParameters)
                    {
                        return methodDescriptor.ParameterCount + distanceFromBaseClass;
                    }

                    var typeWalker = methodDescriptor.SecondParameterType;
                    while (typeWalker != typeof(EventArgs))
                    {
                        distanceFromBaseClass++;
                        typeWalker = typeWalker?.BaseType;
                    }

                    return methodDescriptor.ParameterCount + distanceFromBaseClass;
                }).ToList();
            }
        }

        private bool IsMethodValid(MethodInfo method)
        {
            if (!string.Equals(method.Name, this.MethodName, StringComparison.Ordinal))
            {
                return false;
            }

            if (method.ReturnType != typeof(void) && method.ReturnType != typeof(Task))
            {
                return false;
            }

            return true;
        }

        private class MethodDescriptor
        {
            public MethodDescriptor(MethodInfo methodInfo, ParameterInfo[] methodParams)
            {
                this.MethodInfo = methodInfo;
                this.Parameters = methodParams;
            }

            public MethodInfo MethodInfo { get; }

            public bool HasParameters => this.Parameters.Length > 0;

            public int ParameterCount => this.Parameters.Length;

            public ParameterInfo[] Parameters { get; }

            public Type SecondParameterType => this.Parameters.Length >= 2 ? this.Parameters[1].ParameterType : null;
        }
    }
}
