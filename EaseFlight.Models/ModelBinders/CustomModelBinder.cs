using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace EaseFlight.Models.ModelBinders
{
    public class CustomModelBinder : DefaultModelBinder
    {
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {
            var propBindAttr = propertyDescriptor.Attributes.OfType<PropertyBinder>().FirstOrDefault();

            if (propBindAttr != null && propBindAttr.BindProperty(controllerContext, bindingContext, propertyDescriptor))
                return;

            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }
    }
}