using System;
using System.ComponentModel;
using System.Web.Mvc;

namespace EaseFlight.Models.ModelBinders
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public abstract class PropertyBinder : Attribute
    {
        public abstract bool BindProperty(ControllerContext controllerContext,
        ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor);
    }
}