using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GramophoneUtils
{
#if UNITY_EDITOR
    public static class SerializationUtilities
    {
        //public static System.Reflection.FieldInfo GetFieldViaPath(this System.Type type, string path)
        //{
        //    System.Type parentType = type;
        //    System.Reflection.FieldInfo fi = type.GetField(path);
        //    string[] perDot = path.Split('.');
        //    foreach (string fieldName in perDot)
        //    {
        //        fi = parentType.GetField(fieldName);
        //        if (fi != null)
        //            parentType = fi.FieldType;
        //        else
        //            return null;
        //    }
        //    if (fi != null)
        //        return fi;
        //    else return null;
        //}

        public static object GetValue(this SerializedProperty property)
        {
            System.Type parentType = property.serializedObject.targetObject.GetType();
            System.Reflection.FieldInfo fi = parentType.GetField(property.propertyPath);
            return fi.GetValue(property.serializedObject.targetObject);
        }
    }

    public static class TypeExtension
    {
        public static System.Reflection.FieldInfo GetFieldViaPath(this System.Type type, string path)
        {
            System.Type parentType = type;
            System.Reflection.FieldInfo fi = type.GetField(path);
            string[] perDot = path.Split('.');
            foreach (string fieldName in perDot)
            {
                fi = parentType.GetField(fieldName);
                if (fi != null)
                    parentType = fi.FieldType;
                else
                    return null;
            }
            if (fi != null)
                return fi;
            else return null;
        }

        public static System.Type NewGetType(SerializedProperty prop)
        {
            string[] slices = prop.propertyPath.Split('.');
            System.Type type = prop.serializedObject.targetObject.GetType();

            for (int i = 0; i < slices.Length; i++)
                if (slices[i] == "Array")
                {
                    i++; //skips "data[x]"
                    type = type.GetElementType(); //gets info on array elements
                }

                //gets info on field and its type
                else type = type.GetField(slices[i], BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance).FieldType;

            return type;
        }
    }
#endif
}

