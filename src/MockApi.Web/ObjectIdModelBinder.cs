using System.Web.Mvc;
using LiteDB;

namespace MockApi.Web
{
    public class ObjectIdModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName)?.AttemptedValue;
            return value == null ? null : new ObjectId(value);
        }
    }
}