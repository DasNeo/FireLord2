﻿using System;
using System.Reflection;

namespace FireLord.Utils
{
    public static class ReflectionUtil
    {
        public static void SetPrivateField(this object obj, string fieldName, object value) => obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(obj, value);

        public static T GetPrivateField<T>(this object obj, string fieldName) => (T)obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(obj);

        public static void SetPrivateProperty(this object obj, string propertyName, object value) => obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(obj, value, null);

        public static void InvokePrivateMethod(
          this object obj,
          string methodName,
          object[] methodParams)
        {
            obj.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(obj, methodParams);
        }

        public static void InvokeStaticPrivateMethod(
          this Type type,
          string methodName,
          object[] methodParams)
        {
            type.InvokeMember(methodName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, null, methodParams);
        }
    }
}
