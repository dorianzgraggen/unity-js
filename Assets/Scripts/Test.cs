using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Test : MonoBehaviour
{
  List<Callback> list = new List<Callback>();

  void Start()
  {
    Callback cb1 = new Callback("lol", () =>
    {
      Debug.Log("alles nice haha");
    });
    list.Add(cb1);

    JsPlugin.printFunctionList();

    for (int i = 0; i < list.Count; i++)
    {
      Callback cb = list[i];
      JsPlugin.registerFunction(cb.name, (uint)(i + 1));
    }

    JsPlugin.printFunctionList();

    Debug.Log("ok");



    Debug.Log("nice");

    updateLogs();
  }

  void Update()
  {
    updateLogs();
  }

  private void updateLogs()
  {
    unsafe
    {
      byte* a = JsPlugin.getRsLog();
      var length_bytes = new byte[] { *a, *(a + 1), *(a + 2), *(a + 3) };

      var length = BitConverter.ToUInt32(length_bytes);
      // Debug.Log("Length " + length);

      if (length == 0)
      {
        return;
      }

      var text_bytes = new byte[length];

      for (byte i = 0; i < length; i++)
      {
        text_bytes[i] = *(a + 4 + i);
      }

      string s = System.Text.Encoding.UTF8.GetString(text_bytes, 0, text_bytes.Length);
      Debug.Log("Rust Log: \n" + s);
    }
  }
}


struct Callback
{
  public Action action;
  public string name;

  public Callback(string name, Action action)
  {
    this.action = action;
    this.name = name;
  }
}
