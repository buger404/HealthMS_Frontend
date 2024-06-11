using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;
using UnityEngine;

public class TestConnection : MonoBehaviour
{
    [Serializable]
    public class GetRandomRequest
    {
        public int MinValue, MaxValue;
    }
    [Serializable]
    public class GetRandomResponse
    {
        public int Result;
    }
    private void Awake()
    {
        var client = new RestClient("http://localhost:9371/");
        var request = new RestRequest("test/ping", Method.Get);
        request.AddQueryParameter("echo", 10);
        /**request.AddBody(JsonConvert.SerializeObject(new GetRandomRequest()
        {
            MinValue = 0,
            MaxValue = 100
        }), ContentType.Json);**/
        var response = client.Execute(request);
        //var data = JsonConvert.DeserializeObject<GetRandomResponse>(response.Content);
        Debug.Log(response.Content);
    }
}
