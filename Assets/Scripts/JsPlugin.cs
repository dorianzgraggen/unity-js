using System.Collections;
using System.Collections.Generic;
using fts;
using System;


public delegate int MyCallbackDelegate(int a, int b);
public delegate string TaskCallbackDelegate(byte id, string args);

[PluginAttr("js_for_anything")]
public static unsafe class JsPlugin
{
  [PluginFunctionAttr("register_function_c_str")]
  public static register_function registerFunction = null;
  public delegate void register_function(string name, uint id, bool is_constructor);

  [PluginFunctionAttr("print_function_list")]
  public static print_function_list printFunctionList = null;
  public delegate void print_function_list();

  [PluginFunctionAttr("get_rs_log")]
  public static get_rs_log getRsLog = null;
  public delegate byte* get_rs_log();


  [PluginFunctionAttr("init_from_path")]
  public static init_from_path InitFromPath = null;
  public delegate void init_from_path(string path);

  [PluginFunctionAttr("poll_pending_invocations")]
  public static poll_pending_invocations pollPendingInvocations = null;
  public delegate byte* poll_pending_invocations();

  [PluginFunctionAttr("send_result_c_str")]
  public static send_result_c_str sendResult = null;
  public delegate void send_result_c_str(string path);

  [PluginFunctionAttr("set_log_to_file")]
  public static set_log_to_file setLogToFile = null;
  public delegate void set_log_to_file(bool logToFile);

  [PluginFunctionAttr("clear_log_file")]
  public static clear_log_file clearLogFile = null;
  public delegate void clear_log_file();

  [PluginFunctionAttr("send_event_c_str")]
  public static send_event_c_str sendEvent = null;
  public delegate void send_event_c_str(string type, string data);

  [PluginFunctionAttr("my_rust_function")]
  public static my_rust_function myRustFunction = null;
  public delegate void my_rust_function(MyCallbackDelegate callback);

  [PluginFunctionAttr("set_task_callback")]
  public static set_task_callback setTaskCallback = null;
  public delegate void set_task_callback(TaskCallbackDelegate callback);

  [PluginFunctionAttr("stop")]
  public static stop Stop = null;
  public delegate void stop();
}
