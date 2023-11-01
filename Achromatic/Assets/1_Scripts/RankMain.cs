using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft;
using UnityEngine.UI;
using Newtonsoft.Json;

public class RankMain : MonoBehaviour
{
    public string host;
    public string port;
    public string idurl;
    public string top3url;
    public string posturl;
    public Text idtext;
    public Text scoretext;

    public Text[] top3text;

    public IEnumerator RankTop3()
    {
        var url = string.Format("{0}:{1}/{2}", host, port, top3url);
        Debug.Log(url);

        yield return StartCoroutine(this.GetTop3(url, (raw) =>
        {
            var res = JsonConvert.DeserializeObject<Protocols.Packets.res_scores_top3>(raw);
            Debug.LogFormat("{0}, {1}", res.cmd, res.result.Length);
            for(int i = 0; i < res.result.Length; i++)
            {
                top3text[i].text = res.result[i].id.ToString() + " / " + res.result[i].score.ToString();
                Debug.LogFormat("{0} : {1}", res.result[i].id, res.result[i].score);
            }
        }));
    }

    public void post()
    {
        if (idtext.text == "") return;

        var url = string.Format("{0}:{1}/{2}", host, port, posturl);
        Debug.Log(url); //http://localhost:3030/scores

        var req = new Protocols.Packets.req_scores();
        req.cmd = 1000; //(int)Protocols.eType.POST_SCORE;
        req.id = idtext.text;
        req.score = int.Parse(scoretext.text);
        //직렬화  (오브젝트 -> 문자열)
        var json = JsonConvert.SerializeObject(req);
        Debug.Log(json);
        //{"id":"hong@nate.com","score":100,"cmd":1000}

        StartCoroutine(this.PostScore(url, json, (raw) => {
            Protocols.Packets.res_scores res = JsonConvert.DeserializeObject<Protocols.Packets.res_scores>(raw);
            Debug.LogFormat("{0}, {1}", res.cmd, res.message);
        }));
    }

    private IEnumerator GetTop3(string url, System.Action<string> callback)
    {
        var webRequest = UnityWebRequest.Get(url);
        yield return webRequest.SendWebRequest();
        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("네트워크 환경이 안좋아서 통신을 할수 없습니다.");
        }
        else
        {
            callback(webRequest.downloadHandler.text);
        }
    }


    private IEnumerator PostScore(string url, string json, System.Action<string> callback)
    {

        var webRequest = new UnityWebRequest(url, "POST");
        var bodyRaw = Encoding.UTF8.GetBytes(json); //직렬화 (문자열 -> 바이트 배열)

        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("네트워크 환경이 안좋아서 통신을 할수 없습니다.");
        }
        else
        {
            Debug.LogFormat("{0}\n{1}\n{2}", webRequest.responseCode, webRequest.downloadHandler.data, webRequest.downloadHandler.text);
            callback(webRequest.downloadHandler.text);
        }
    }

}