using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Js2;

public class Test : MonoBehaviour
{
  public string JSFile;

  List<Callback> callbacks = new List<Callback>();


  static ConcurrentQueue<Action> pendingFuncs = new ConcurrentQueue<Action>();

  static Dictionary<uint, GameObject> gameObjects = new Dictionary<uint, GameObject>();

  // Define the callback function
  public static int MyCallback(int a, int b)
  {
    // Handle the callback logic in C#
    // ...
    return a + b;
  }
  static System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

  public static string TaskCallback(byte id, string jsonArgs)
  {
    var callback = Callback.dict[id];
    // Debug.Log("calling " + callback.name + " with args " + jsonArgs);

    var args = JArray.Parse(jsonArgs);

    var result = callback.fn(args);
    // Debug.Log("result " + result);
    stopwatch.Restart();
    var json_result = JsonConvert.SerializeObject(result);
    stopwatch.Stop();

    // Debug.Log("json result " + json_result);

    // Debug.Log("Received from Rust " + args);

    var micros = stopwatch.ElapsedTicks * 1000 * 1000 / System.Diagnostics.Stopwatch.Frequency;
    // Debug.Log("elapsed micros:" + micros);
    return json_result;
  }

  Color backgroundColor = Color.gray;

  void Start()
  {
    JsPlugin.clearLogFile();
    JsPlugin.setLogToFile(true);
    JsPlugin.setTaskCallback(TaskCallback);

    Callback cb1 = new Callback("lol", false, (args) =>
    {
      Debug.Log("alles nice haha" + args);
      return new
      {
        lol = "alles geilooo",
        num = 10482
      };
    });

    Callback cb2 = new Callback("multiply", false, (args) =>
    {
      float a = (float)args[0];
      float b = (float)args[1];
      return a * b;
    });

    Callback setBackgroundColorHSV = new Callback("setBackgroundColorHSV", false, (args) =>
    {
      float h = (float)args[0];
      float s = (float)args[1];
      float v = (float)args[2];
      this.backgroundColor = Color.HSVToRGB(h, s, v);
      pendingFuncs.Enqueue(() =>
      {
        var camera = FindObjectOfType<Camera>();
        if (camera != null)
        {
          camera.backgroundColor = Color.HSVToRGB(h, s, v);
        }
      });
      return "";
    });


    Callback camera = new Callback("Camera", true, (args) =>
    {
      var jsCam = new JsObject();
      var objId = jsCam.getId();

      float fov = (float)args[0];
      float near = 1;
      if (args.Count > 1)
      {
        near = (float)args[1];

      }
      float far = 1000;
      if (args.Count > 2)
      {
        far = (float)args[2];
      }

      pendingFuncs.Enqueue(() =>
      {
        var go = new GameObject("Camera");
        gameObjects[objId] = go;
        var camera = go.AddComponent<Camera>();
        camera.fieldOfView = fov;
        camera.nearClipPlane = near;
        camera.farClipPlane = far;
        camera.backgroundColor = backgroundColor;
        camera.clearFlags = CameraClearFlags.SolidColor;
      });

      var setPosition = new Callback("setPosition", false, (args) =>
      {
        float x = (float)args[0];
        float y = (float)args[1];
        float z = (float)args[2];
        pendingFuncs.Enqueue(() =>
        {
          gameObjects[objId].transform.position = new Vector3(x, y, z);
        });
        return "";
      });
      jsCam.addMethod(setPosition);

      return jsCam.buildReturnValue();
    });


    Callback cube = new Callback("Cube", true, (args) =>
    {
      Debug.Log("called cube");
      var jsCube = new JsObject();
      var objId = jsCube.getId();

      float x = (float)args[0];
      float y = (float)args[1];
      float z = (float)args[2];
      string name = "Cube";

      if (args.Count > 3)
      {
        name = (string)args[3];
      }

      pendingFuncs.Enqueue(() =>
      {
        Debug.Log(args);
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameObjects[objId] = cube;
        cube.transform.localScale = new Vector3(x, y, z);
        cube.name = name;
      });


      var setPosition = new Callback("setPosition", false, (args) =>
      {
        float x = (float)args[0];
        float y = (float)args[1];
        float z = (float)args[2];
        pendingFuncs.Enqueue(() =>
        {
          gameObjects[objId].transform.position = new Vector3(x, y, z);
        });
        return "";
      });
      jsCube.addMethod(setPosition);


      var getX = new Callback("x", false, (args) =>
      {
        return 222222;
      });
      jsCube.addGetter(setPosition);

      var setHSV = new Callback("setHSV", false, (args) =>
      {
        float h = (float)args[0];
        float s = (float)args[1];
        float v = (float)args[2];
        pendingFuncs.Enqueue(() =>
        {

          var go = gameObjects[objId];
          go.GetComponent<MeshRenderer>().material.color = Color.HSVToRGB(h, s, v);
        });
        return "";
      });
      jsCube.addMethod(setHSV);

      var enableGravity = new Callback("enableGravity", false, (args) =>
      {
        bool enable = (bool)args[0];
        pendingFuncs.Enqueue(() =>
        {
          var go = gameObjects[objId];
          var rb = go.GetComponent<Rigidbody>();
          if (rb != null)
          {
            rb.useGravity = enable;
            if (!enable)
            {
              rb.velocity = Vector3.zero;
              rb.angularVelocity = Vector3.zero;
            }
            return;
          }

          if (enable)
          {
            go.AddComponent<Rigidbody>();
          }
        });

        return "";
      });
      jsCube.addMethod(enableGravity);

      var addForce = new Callback("addForce", false, (args) =>
      {
        float x = (float)args[0];
        float y = (float)args[1];
        float z = (float)args[2];
        pendingFuncs.Enqueue(() =>
        {
          var go = gameObjects[objId];
          var rb = go.GetComponent<Rigidbody>();
          if (rb != null)
          {
            rb.AddForce(x, y, z);
            return;
          }
          else
          {
            // TODO: add option to log to js console
          }
        });

        return "";
      });
      jsCube.addMethod(addForce);


      var setVelocity = new Callback("setVelocity", false, (args) =>
      {
        float x = (float)args[0];
        float y = (float)args[1];
        float z = (float)args[2];
        pendingFuncs.Enqueue(() =>
        {
          var go = gameObjects[objId];
          var rb = go.GetComponent<Rigidbody>();
          if (rb != null)
          {
            rb.velocity = new Vector3(x, y, z);
            return;
          }
          else
          {
            // TODO: add option to log to js console
          }
        });

        return "";
      });
      jsCube.addMethod(setVelocity);


      var ret = jsCube.buildReturnValue();
      return ret;
    });


    JsPlugin.printFunctionList();

    // for (int i = 0; i < callbacks.Count; i++)
    // {
    //   Callback cb = callbacks[i];
    //   JsPlugin.registerFunction(cb.name, (uint)(i + 1));
    // }

    JsPlugin.printFunctionList();
    updateLogs();

    JsPlugin.InitFromPath(JSFile);
    updateLogs();
    keyCodes = Enum.GetValues(typeof(KeyCode));
  }

  void Update()
  {
    Action result;
    while (pendingFuncs.TryDequeue(out result))
    {
      result();
    }

    updateLogs();

    sendEvents();

    handleKeys();
  }

  Array keyCodes;

  private void handleKeys()
  {
    foreach (KeyCode key in keyCodes)
    {
      if (Input.GetKeyDown(key))
      {
        Debug.Log("KeyCode down: " + key);
        string data = JsonConvert.SerializeObject(new { key = key.ToString() });
        JsPlugin.sendEvent("keydown", data);
      }

      if (Input.GetKeyUp(key))
      {
        Debug.Log("KeyCode up: " + key);
        string data = JsonConvert.SerializeObject(new { key = key.ToString() });
        JsPlugin.sendEvent("keyup", data);
      }
    }
  }

  private void OnDisable()
  {
    JsPlugin.Stop();
  }

  private void sendEvents()
  {
    if (UnityEngine.Random.Range(0f, 1f) > 0.99f)
    {
      return;
      Debug.Log("will send event lucky");
      // JsEvent evt = new JsEvent("lucky", new { num = 22.22f });

      string data = JsonConvert.SerializeObject(new { okok = "haha", num = 22.22f });
      JsPlugin.sendEvent("lucky", data);
    }
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
      var callback = Callback.dict[invocation.id];
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


public struct Callback
{
  private static uint nextId = 1;
  public Func<JArray, object> fn;
  public string name;
  public uint id;

  private bool isConstructor;

  public Callback(string name, bool isConstructor, Func<JArray, object> fn)
  {
    this.id = nextId;
    nextId++;
    this.fn = fn;
    this.name = name;
    this.isConstructor = isConstructor;

    JsPlugin.registerFunction(this.name, this.id, this.isConstructor);
    dict[this.id] = this;
  }

  public static Dictionary<uint, Callback> dict = new Dictionary<uint, Callback>();
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

