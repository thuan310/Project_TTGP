using UnityEngine;
using System.Diagnostics;

public class PythonServerManager : MonoBehaviour
{
    private Process pythonProcess;

    public static PythonServerManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    void Start()
    {
        StartPythonServer();
        SceneNavigationManager.instance.currentSceneIndex.OnValueChanged += StopPythonServer;
    }

    void OnApplicationQuit()
    {
        StopPythonServer(0,0);
    }

    void StartPythonServer()
    {
        string pythonExePath = Application.streamingAssetsPath + "/llm_npc_minigame.exe";

        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = pythonExePath;
        //startInfo.CreateNoWindow = true;
        //startInfo.UseShellExecute = false;

        pythonProcess = new Process();
        pythonProcess.StartInfo = startInfo;
        pythonProcess.Start();

        print("Đã chạy Python server.");
    }

    private void StopPythonServer(int oldnum, int newnum)
    {
        try
        {
            SceneNavigationManager.instance.currentSceneIndex.OnValueChanged -= StopPythonServer;
        }
        catch
        { }
        if (pythonProcess != null && !pythonProcess.HasExited)
        {
            UnityEngine.Debug.Log("Killing Python server...");
            Process.Start(new ProcessStartInfo
            {
                FileName = "taskkill",
                Arguments = $"/PID {pythonProcess.Id} /F /T",  // /T kills entire tree
                CreateNoWindow = true,
                UseShellExecute = false
            });
            pythonProcess.Dispose();
            print("Đã dừng Python server.");
        }
    }
}
