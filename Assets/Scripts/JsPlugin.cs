using System.Collections;
using System.Collections.Generic;
using fts;

[PluginAttr("js_for_anything")]
public static unsafe class JsPlugin
{
  [PluginFunctionAttr("register_function")]
  public static register_function registerFunction = null;
  public delegate void register_function(string name, uint id);

  [PluginFunctionAttr("print_function_list")]
  public static print_function_list printFunctionList = null;
  public delegate void print_function_list();

  [PluginFunctionAttr("get_rs_log")]
  public static get_rs_log getRsLog = null;
  public delegate byte* get_rs_log();

}
