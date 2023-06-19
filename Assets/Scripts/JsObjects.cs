using System.Collections.Generic;

namespace Js2
{

  public static class JsObjectPool
  {
    static uint nextId;
    public static uint getId()
    {
      return nextId++;
    }
  }

  public class JsObject
  {
    public JsObject()
    {
      this.id = JsObjectPool.getId();
    }

    private uint id = 0;
    private Dictionary<string, uint> methods = new Dictionary<string, uint>();

    public object buildReturnValue()
    {
      return new
      {
        ___type = "object_instance",
        id = this.id,
        methods = this.methods
      };
    }

    public uint getId()
    {
      return id;
    }

    public void addMethod(Callback callback)
    {
      methods[callback.name] = callback.id;
    }
  }

}
