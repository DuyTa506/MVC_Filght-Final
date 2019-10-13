using System;
using System.IO;

namespace EaseFlight.Common.Utilities
{
    public static class CommonMethods
    {
        #region Object property handle functions
        public static object GetPropertyValue(object src, string propName)
        {
            object value = null;

            if (src != null)
                value = src.GetType().GetProperty(propName).GetValue(src, null);

            return value;
        }

        public static ValueType GetPropertyValue<ValueType>(object src, string propName)
        {
            var value = GetPropertyValue(src, propName);

            return (ValueType)value;
        }

        public static int GetEntityId(object entityModel)
        {
            var value = GetPropertyValue<int>(entityModel, Constants.Constant.CONST_DB_COLUMN_ID);

            return value;
        }

        public static void SetPropertyValue(object src, string propName, object value)
        {
            var property = src.GetType().GetProperty(propName);

            if (property != null)
                property.SetValue(src, value);
        }

        public static void CopyObjectProperties(object from, object to)
        {
            if (from == null || to == null)
                return;

            foreach (var prop in from.GetType().GetProperties())
            {
                var propValue = GetPropertyValue(from, prop.Name);
                SetPropertyValue(to, prop.Name, propValue);
            }
        }
        #endregion

        #region File I/O functions
        public static string ServerMapPath(string path)
        {
            var rootPath = AppDomain.CurrentDomain.BaseDirectory;

            return Path.Combine(rootPath, path);
        }
        #endregion
    }
}
