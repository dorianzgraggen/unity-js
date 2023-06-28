using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using B83.Win32;

public class DragAndDrop : MonoBehaviour
{
  List<string> log = new List<string>();

  void OnEnable()
  {
    UnityDragAndDropHook.InstallHook();
    UnityDragAndDropHook.OnDroppedFiles += OnFiles;
  }

  void OnDisable()
  {
    UnityDragAndDropHook.UninstallHook();
  }

  void OnFiles(List<string> aFiles, POINT aPos)
  {
    string str = "Dropped " + aFiles.Count + " files at: " + aPos + "\n\t" +
        aFiles.Aggregate((a, b) => a + "\n\t" + b);
    Log(str);

    if (aFiles.Count != 1)
    {
      return;
    }

    var file = aFiles[0];
    Log("first file" + file);

    var ending = file.Substring(file.Length - 3);
    Log("ending" + ending);

    if (ending != ".js")
    {
      return;
    }

    Log("js file");

    var camera = FindObjectOfType<Camera>();
    if (camera != null)
    {
      camera.backgroundColor = Color.HSVToRGB(1, 1, 1);
    }
  }

  private void Update()
  {

  }

  private void OnGUI()
  {
    if (GUILayout.Button("clear log"))
      log.Clear();
    foreach (var s in log)
      GUILayout.Label(s);
  }

  void Log(string s)
  {
    Debug.Log(s);
    log.Add(s);
  }
}
