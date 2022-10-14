// Decompiled with JetBrains decompiler
// Type: FireLord.Utils.ReflectionUtil
// Assembly: FireLord, Version=1.1.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 51633F12-6A5F-46B9-B9AF-55B0B570B321
// Assembly location: C:\Users\andre\Documents\FireLord.dll

using System;
using System.Reflection;

namespace FireLord.Utils
{
    public static class ReflectionUtil
    {
        public static void SetPrivateField(this object obj, string fieldName, object value) => obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(obj, value);

        public static T GetPrivateField<T>(this object obj, string fieldName) => (T)obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(obj);

        public static void SetPrivateProperty(this object obj, string propertyName, object value) => obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).SetValue(obj, value, (object[])null);

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
            type.InvokeMember(methodName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod, (Binder)null, (object)null, methodParams);
        }
    }
}
