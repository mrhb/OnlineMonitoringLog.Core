
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AlarmBase.DomainModel.generics
{
    public class OccSerialization
    {
        private const string Seperator_Replace_String = "-@-";
        private const string separator = "|";

     
        public static String ReplaceParamWithValues(string Template, object obj)
        {
            Type type = obj.GetType();
            try
            {          
                foreach (var prop in type.GetProperties().Where(p => p.IsMarkedWith<MessageParamAttribute>()))
                {  
                    Template = Template.Replace(prop.Name, prop.GetValue(obj).ToString());
                }
              
            }
            catch(Exception C)
            {
                Template = "error in TemplateToMessage"+C.ToString();
            }
            return Template;
        }
        

    }

    public class MessageParamAttribute : Attribute
    {
    }

    public static class ExtensionsOfPropertyInfo
    {
        public static IEnumerable<T> GetAttributes<T>(this PropertyInfo propertyInfo) where T : Attribute
        {
            return propertyInfo.GetCustomAttributes(typeof(T), true).Cast<T>();
        }
        public static bool IsMarkedWith<T>(this PropertyInfo propertyInfo) where T : Attribute
        {
            return propertyInfo.GetAttributes<T>().Any();
        }
    }

}