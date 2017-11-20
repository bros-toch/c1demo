using System;

namespace Consoles
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class EditWorkflowAttribute : Attribute
    {
        public Type EditWorkflowType { get; set; }

        public EditWorkflowAttribute(Type type)
        {
            EditWorkflowType = type;
        }
    }
}