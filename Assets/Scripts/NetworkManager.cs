using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using System.Threading.Tasks;

public class NetworkManager : Singleton<NetworkManager>
{
    private string adress = "http://localhost:3000/score";

    public async Task SaveScore(string playerName, int score)
    {
        if (score >= 0)
        {
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "playerName", playerName },
                { "score", score.ToString() }
            };

            await PostRequestAsync(data, adress);
        }
    }

    public async Task<ScoreLine[]> LoadScoreLines()
    {
        string json = JsonHelper.FixJson(await SendGetResponse(adress));
        ScoreLine[] lines = JsonHelper.FromJson<ScoreLine>(json);
        Array.Sort(lines);
        return lines;
    }

    private async Task<string> SendGetResponse(string adress)
    {
        string data = string.Empty;
        WebRequest request = WebRequest.Create(adress);
        request.Method = "GET";
        WebResponse response = await request.GetResponseAsync();
        using (Stream stream = response.GetResponseStream())
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                data = reader.ReadToEnd();
            }
        }

        response.Close();
        return data;
    }

    private async Task PostRequestAsync(Dictionary<string, string> data, string adress)
    {
        WebRequest request = WebRequest.Create(adress);
        request.Method = "POST";
        byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(DataToRequestString(data));
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = byteArray.Length;

        using (Stream dataStream = request.GetRequestStream())
        {
            dataStream.Write(byteArray, 0, byteArray.Length);
        }
        WebResponse response = await request.GetResponseAsync();
        using (Stream stream = response.GetResponseStream())
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                Debug.Log(reader.ReadToEnd());
            }
        }

        response.Close();
    }

    private string DataToRequestString(Dictionary<string, string> data)
    {
        string result = string.Empty;
        foreach (string key in data.Keys)
        {
            if (result.Length > 0)
            {
                result += '&';
            }

            result += $"{key}={data[key]}";
        }

        return result;
    }
}
