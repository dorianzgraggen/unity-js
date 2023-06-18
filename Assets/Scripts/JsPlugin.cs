using System.Collections;
using System.Collections.Generic;
using fts;
using System;

[PluginAttr("js_for_anything")]
public static unsafe class JsPlugin
{
  [PluginFunctionAttr("register_function_c_str")]
  public static register_function registerFunction = null;
  public delegate void register_function(string name, uint id);

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
}
