using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Test : MonoBehaviour
{
  public string JSFile;

  List<Callback> callbacks = new List<Callback>();

  void Start()
  {
    JsPlugin.clearLogFile();
    JsPlugin.setLogToFile(true);

    Callback cb1 = new Callback("lol", (args) =>
    {
      Debug.Log("alles nice haha" + args);
      return new
      {
        lol = "alles geilooo",
        num = 10482
      };
    });

    Callback cb2 = new Callback("multiply", (args) =>
    {
      float a = (float)args[0];
      float b = (float)args[1];
      return a * b;
    });

    callbacks.Add(cb1);
    callbacks.Add(cb2);

    JsPlugin.printFunctionList();

    for (int i = 0; i < callbacks.Count; i++)
    {
      Callback cb = callbacks[i];
      JsPlugin.registerFunction(cb.name, (uint)(i + 1));
    }

    JsPlugin.printFunctionList();
    updateLogs();

    JsPlugin.InitFromPath(JSFile);
    updateLogs();
  }

  void Update()
  {
    updateLogs();

    handleFunctionCalls();
  }

  private Invocation pollPendingInvocations()
  {
    unsafe
    {
      byte* ptr = JsPlugin.pollPendingInvocations();

      byte id = *ptr;
      Debug.Log("id " + id);
      if (id == 0)
      {
        return new Invocation(0, "");
      }

      var length_bytes = new byte[] { *(ptr + 1), *(ptr + 2), *(ptr + 3), *(ptr + 4) };
      var length = BitConverter.ToUInt32(length_bytes);

      var text_bytes = new byte[length];

      for (byte i = 0; i < length; i++)
      {
        text_bytes[i] = *(ptr + 5 + i);
      }

      string args = System.Text.Encoding.UTF8.GetString(text_bytes, 0, text_bytes.Length);

      return new Invocation(id, args);
    }
  }

  private void handleFunctionCalls()
  {
    var invocation = pollPendingInvocations();

    while (invocation.id != 0)
    {
      var callback = callbacks[invocation.id - 1];
      Debug.Log("calling " + callback.name + " with args " + invocation.args);

      var args = JArray.Parse(invocation.args);

      var result = callback.fn(args);
      Debug.Log("result " + result);
      var json_result = JsonConvert.SerializeObject(result);

      Debug.Log("json result " + json_result);

      JsPlugin.sendResult(json_result);

      invocation = pollPendingInvocations();
    }
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
  public Func<JArray, object> fn;
  public string name;

  public Callback(string name, Func<JArray, object> fn)
  {
    this.fn = fn;
    this.name = name;
  }
}


struct Invocation
{
  public Invocation(byte id, string args)
  {
    this.id = id;
    this.args = args;
  }
  public byte id;
  public string args;
}
