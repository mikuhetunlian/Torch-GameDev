using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class DataMgr 
{
    private static DataMgr instance => new DataMgr();
    public static DataMgr Instance
    {
        get
        {
            return instance;
        }
    }


    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="value"></param>
    /// <param name="key"></param>
    public void Save(object data,string key)
    {
        Type type = data.GetType();
        FieldInfo[] fieldInfos = type.GetFields();

        FieldInfo fieldinfo;
        for (int i = 0; i < fieldInfos.Length; i++)
        {
            fieldinfo = fieldInfos[i];
            Type fieldType = fieldinfo.FieldType;
            string fieldName = fieldinfo.Name;

            string keyName = key + "_" + fieldType.Name + "_" + fieldName;
            object value = fieldinfo.GetValue(data);

            SaveValue(value, keyName);
        }

    }

    /// <summary>
    /// ������PlayerPrefsд�����ݵĵط�
    /// </summary>
    /// <param name="value"></param>
    /// <param name="keyName"></param>
    private void SaveValue(object value,string keyName)
    {
        Type type = value.GetType();

        if (type == typeof(int))
        {
            PlayerPrefs.SetInt(keyName, (int)value);
        }
        else if (type == typeof(float))
        {
            PlayerPrefs.SetFloat(keyName, (float)value);
        }
        else if (type == typeof(string))
        {
            PlayerPrefs.SetString(keyName, (string)value);
        }
        else if (type.IsEnum)
        {
            PlayerPrefs.SetInt(keyName, (int)value);
        }
        //��������˵�����������Ա�IList���ܣ�Ҳ����˵����һ��List
        else if (typeof(IList).IsAssignableFrom(type))
        {

            IList list = value as IList;
            //�ȴ�list�ĳ��ȣ�Ϊ�˶�ȡ��ʱ����֪��list�ĳ���Ȼ���ٶ�ȡ
            PlayerPrefs.SetInt(keyName + "list_count", list.Count);

            int index = 0;
            foreach (object item in list)
            {
                string subKeyName = keyName + "_" + index;
                SaveValue(item, subKeyName);
                index++;
            }

        }
        //��������˵�����������Ա�IDictionary���ܣ�Ҳ����˵����һ��Dic
        else if (typeof(IDictionary).IsAssignableFrom(type))
        {
            IDictionary dic = value as IDictionary;

            //�ȴ�dic�ĳ��ȣ�Ϊ�˶�ȡ��ʱ����֪��dic�ĳ���Ȼ���ٶ�ȡ
            PlayerPrefs.SetInt(keyName + "dic_count", dic.Count);
            int index = 0;
            foreach (object obj in dic.Keys)
            {
                string dicKeyName = keyName + "_key_" + index;
                string dicValueName = keyName + "_value_" + index;

                SaveValue(obj, dicKeyName);
                SaveValue(dic[obj], dicValueName);

                index++;
            }
        }
        else
        {
            Save(value, keyName);
        }
    }


    /// <summary>
    /// ���ض�Ӧtype�����ݲ�ֱ�ӷ���object
    /// </summary>
    /// <param name="type"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public object Load(Type type, string key)
    {
        FieldInfo[] fieldInfos = type.GetFields();

        object obj = Activator.CreateInstance(type);

        for (int i = 0; i < fieldInfos.Length; i++)
        {
            FieldInfo fieldInfo = fieldInfos[i];
            Type fieldType = fieldInfo.FieldType;
            string fieldName = fieldInfo.Name;

            string keyName = key + "_" + fieldType.Name + "_" + fieldName;

            object value = LoadValue(fieldType, keyName);

            fieldInfo.SetValue(obj, value);
        }

        return obj;
    }

    /// <summary>
    /// ������PlayerPrefs��ȡ���ݵĵط�
    /// </summary>
    /// <param name="type"></param>
    /// <param name="keyName"></param>
    /// <returns></returns>
    private object LoadValue(Type type,string keyName)
    {
        if (type == typeof(int))
        {
            return PlayerPrefs.GetInt(keyName);
        }
        else if (type == typeof(float))
        {
            return PlayerPrefs.GetFloat(keyName);
        }
        else if (type == typeof(string))
        {
            return PlayerPrefs.GetString(keyName);
        }
        else if (type.IsEnum)
        {
            return PlayerPrefs.GetInt(keyName);
        }
        else if (typeof(IList).IsAssignableFrom(type))
        {
            int listCount = PlayerPrefs.GetInt(keyName + "list_count");
            IList list = Activator.CreateInstance(type) as IList;
            Type listType = list.GetType();
            //���list�еķ�����ʲô
            Type genericType = listType.GetGenericArguments()[0];

            for (int i = 0; i < listCount; i++)
            {
                string subKeyName = keyName + "_" + i;
                object value = LoadValue(genericType, subKeyName);
                list.Add(value);
            }
            return list;
        }
        else if (typeof(IDictionary).IsAssignableFrom(type))
        {
            IDictionary dic = Activator.CreateInstance(type) as IDictionary;
            Type dicType = dic.GetType();
            Type TKey = dicType.GetGenericArguments()[0];
            Type TValue = dicType.GetGenericArguments()[1];



            int dicCount = PlayerPrefs.GetInt(keyName + "dic_count");

            for (int i = 0; i < dicCount; i++)
            {
                string dicKeyName = keyName + "_key_" + i;
                string dicValueName = keyName + "_value_" + i;

                object key = LoadValue(TKey, dicKeyName);
                object value = LoadValue(TValue, dicValueName);

                dic.Add(key, value);
            }

            return dic;
        }
        else
        {
            return Load(type, keyName);
        }


    }




}
