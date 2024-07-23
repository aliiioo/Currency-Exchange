﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Reflection;

namespace Currency_Exchange.Security
{
    

        public class SanitizeInputFilter : IActionFilter
        {
            public void OnActionExecuting(ActionExecutingContext context)
            {
                foreach (var parameter in context.ActionArguments.Values)
                {
                    SanitizeObjectProperties(parameter);
                }
            }

            public void OnActionExecuted(ActionExecutedContext context)
            {
                // Do nothing
            }

            private void SanitizeObjectProperties(object obj)
            {
                if (obj == null) return;

                var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

                foreach (var property in properties)
                {
                    var value = property.GetValue(obj) as string;
                    if (!string.IsNullOrEmpty(value))
                    {
                        property.SetValue(obj, CleanInput(value));
                    }
                }
            }

            private string CleanInput(string input)
            {
                return input.Replace("--", "")
                    .Replace(";", "")
                    .Replace("#", "")
                    .Replace(",", "")
                    .Replace("%", "")
                    .Replace("=", "")
                    .Replace("*", "");
            }
        }
   

}
