using TMPro;
using UnityEngine;
using NativeWebSocket;
using System.Text;

public class LLMStreaming : MonoBehaviour {
    WebSocket websocket;
    public TMP_InputField inputField;
    public TMP_Text outputText;

    async void Start()
    {
        websocket = new WebSocket("ws://localhost:8000/ws");

        websocket.OnOpen += () => Debug.Log("WebSocket opened.");
        websocket.OnError += (e) => Debug.LogError("WebSocket error: " + e);
        websocket.OnClose += (e) => Debug.Log("WebSocket closed with code: " + e);
        websocket.OnMessage += (bytes) =>
        {
            string msg = Encoding.UTF8.GetString(bytes);

            // If the end signal is received, we don't append further chunks.
            if (msg == "[[END]]")
            {
                Debug.Log("Stream ended.");
                return; // End of stream
            }

            // Directly update output text without using StringBuilder
            outputText.text = msg; // Append each chunk as it comes in
        };

        await websocket.Connect();
    }

    public async void OnAskButtonPressed()
    {
        if (websocket.State != WebSocketState.Open)
        {
            Debug.LogWarning("WebSocket not open.");
            return;
        }

        outputText.text = ""; // Reset previous output

        var payload = new QuestionData { question = inputField.text };
        string json = JsonUtility.ToJson(payload);
        await websocket.SendText(json);
    }

    void Update()
    {
        if (websocket != null)
            websocket.DispatchMessageQueue();
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }

    [System.Serializable]
    public class QuestionData {
        public string question;
    }
}
