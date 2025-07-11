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
        DontDestroyOnLoad(gameObject);
        StartPythonServer();
    }

    void OnApplicationQuit()
    {
        StopPythonServer();
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

    void StopPythonServer()
    {
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
