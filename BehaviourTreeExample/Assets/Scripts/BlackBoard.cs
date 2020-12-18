using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlackBoard : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private List<VariableGameObject> ObjectVariables = new List<VariableGameObject>();
    [SerializeField] private List<VariableFloat> floatVariables = new List<VariableFloat>();
  //[SerializeField] private List<VariableFloat> boolVariables = new List<VariableFloat>();


    public Dictionary<VariableType, VariableFloat> VariableDictionary { get; private set; } = new Dictionary<VariableType, VariableFloat>();
    public Dictionary<string, VariableGameObject> VariableObjDictionary { get; private set; } = new Dictionary<string, VariableGameObject>();
    public void OnInitialize()
    {
      //      Debug.Log("BlackBoard values");
        VariableDictionary = new Dictionary<VariableType, VariableFloat>();
        foreach(var v in floatVariables)
        {
            VariableDictionary.Add(v.Type, v);
       //     Debug.Log(v.Value +" " + v.Type.ToString());
        }

        VariableObjDictionary = new Dictionary<string, VariableGameObject>();
        foreach (var v in ObjectVariables)
        {
            VariableObjDictionary.Add(v.name, v);
           //   Debug.Log(v.Value);
          //  Debug.Log(v.ToString());
        }
    }

    public VariableFloat GetFloatVariableValue(string name)
    {
        var res = floatVariables.Find(x => x.name == name);
        return res;
    }
    public VariableFloat GetFloatVariableValue(VariableType type)
    {
        if (VariableDictionary.ContainsKey(type))
        {
            return VariableDictionary[type];
        }
        return null;
    }

    //public VariableGameObject GetObjVariableValue(string name)
    //{
    //    var res = ObjectVariables.Find(x => x.name == name);
    //    return res;
    //}
    public VariableGameObject GetObjVariableValue(string type)
    {
        if (VariableObjDictionary.ContainsKey(type))
        {
            return VariableObjDictionary[type];
        }
        return null;
    }
    //public bool GetBoolVariableValue(string name, out bool result)
    //{
    //    var res = floatVariables.Find(x => x.name == name);
    //    if (res != null)
    //    {
    //        result = res.Value;
    //        return true;
    //    }
    //    else
    //    {
    //        result = 0;
    //        return false;
    //    }
    //}
}
