using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NativeWebSocket;
using System.Text;
using System.Collections;
using System;

public class LLMMinigame : MonoBehaviour {

    public GameObject InputManager;

    public GameObject UIPanel;

    public int trustScore = 50;
    public int trustGoal = 80;
    public int trustMin = 0;
    public int trustMax = 100;

    WebSocket websocket;

    public TMP_InputField inputField;
    public TMP_Text npcResponseText;
    public TMP_Text statusText;
    public Image trustBarFill; 
    public TMP_Text trustChangeText;

    async void Start()
    {
        websocket = new WebSocket("ws://localhost:8000/npc_minigame");

        websocket.OnOpen += () => Debug.Log("WebSocket opened.");
        websocket.OnError += (e) => Debug.LogError("WebSocket error: " + e);
        websocket.OnClose += (e) => Debug.Log("WebSocket closed with code: " + e);

        websocket.OnMessage += (bytes) =>
        {
            string msg = Encoding.UTF8.GetString(bytes);

            if (msg == "[[WIN]]")
            {
                statusText.text = "🎉 You've gained the NPC's trust!";
                return;
            }

            try
            {
                MinigameResponse res = JsonUtility.FromJson<MinigameResponse>(msg);
                npcResponseText.text = res.npc_response;
                trustScore = res.trust_score;
                ShowTrustChange(res.trust_change);

                // Update trust bar fill
                float normalizedTrust = Mathf.InverseLerp(trustMin, trustMax, trustScore);
                trustBarFill.fillAmount = normalizedTrust;

                if (res.status == "win")
                {
                    statusText.text = "🎉 You've won the NPC's trust!";
                }
                else
                {
                    statusText.text = "Keep trying to convince the NPC...";
                }
            }
            catch
            {
                Debug.Log("Non-JSON chunk: " + msg);
            }
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

        npcResponseText.text = "";
        statusText.text = "Waiting for NPC...";

        var payload = new AnswerPayload { answer = inputField.text };
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

    [Serializable]
    public class AnswerPayload {
        public string answer;
    }

    [Serializable]
    public class MinigameResponse {
        public string npc_response;
        public int trust_change;
        public int trust_score;
        public string status;
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIPanel.SetActive(true);
            InputManager.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIPanel.SetActive(false);
        }
    }

    public void ExitMinigame()
    {
        UIPanel.SetActive(false);
        InputManager.SetActive(true);

    }

    public void ShowTrustChange(int amount)
    {
        trustChangeText.text = amount >= 0 ? $"+{amount}" : $"{amount}";
        trustChangeText.color = amount >= 0 ? Color.green : Color.red;
        trustChangeText.gameObject.SetActive(true);
        StartCoroutine(HideAfterSeconds(3f));
    }

    IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        trustChangeText.gameObject.SetActive(false);
    }
}
