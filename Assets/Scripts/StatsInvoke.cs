using JetBrains.Annotations;
using RESTfulHTTPServer.src.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;


namespace RESTfulHTTPServer.src.invoker
{
    public class StatsInvoke
    {
        private const string TAG = "Stats Invoke";

        public static Response SetBuildingCount(Request request)
        {
            Response response = new Response();
            string responseData = "";
            string json = request.GetPOSTData();
            string count = request.GetParameter("count");
            bool valid = true;

            UnityInvoker.ExecuteOnMainThread.Enqueue(() =>
            {

                try
                {

                    StatsData stats = JsonUtility.FromJson<StatsData>(json);
                    StatsData statsResult = new StatsData();

                    Stats.AddBuilding(int.Parse(count));
                    Stats.Instance.UpdateText();
                    responseData = JsonUtility.ToJson(stats);
                }
                catch (Exception e)
                {
                    valid = false;
                    string msg = "Failed to deseiralised JSON";
                    responseData = msg;

                    RESTfulHTTPServer.src.controller.Logger.Log(TAG, msg);
                    RESTfulHTTPServer.src.controller.Logger.Log(TAG, e.ToString());
                }
            });

            // Wait for the main thread
            while (responseData.Equals("")) { }

            // Filling up the response with data
            if (valid)
            {

                // 200 - OK
                response.SetContent(responseData);
                response.SetHTTPStatusCode((int)HttpStatusCode.OK);
                response.SetMimeType(Response.MIME_CONTENT_TYPE_JSON);
            }
            else
            {

                // 406 - Not acceptable
                response.SetContent("Failed to deseiralised JSON");
                response.SetHTTPStatusCode((int)HttpStatusCode.NotAcceptable);
                response.SetMimeType(Response.MIME_CONTENT_TYPE_HTML);
            }

            return response;
        }

        /// <summary>
        /// Get the color of an object
        /// </summary>
        /// <returns>The color.</returns>
        /// <param name="request">Request.</param>
        public static Response GetStats(Request request)
        {
            Response response = new Response();
            //string objname = request.GetParameter("objname");
            string responseData = "";

            // Verbose all URL variables
            foreach (string key in request.GetQuerys().Keys)
            {

                string value = request.GetQuery(key);
                RESTfulHTTPServer.src.controller.Logger.Log(TAG, "key: " + key + " , value: " + value);
            }

            StatsData data = new StatsData();
            data.BuildCount = Stats.BuildingCount;

            responseData = JsonUtility.ToJson(data);
            response.SetHTTPStatusCode((int)HttpStatusCode.OK);
            // Wait for the main thread
            while (responseData.Equals("")) { }

            // 200 - OK
            // Fillig up the response with data
            response.SetContent(responseData);
            
            response.SetMimeType(Response.MIME_CONTENT_TYPE_JSON);

            return response;
        }
    }
}
[Serializable]
public class StatsData
{
    public int BuildCount;
}