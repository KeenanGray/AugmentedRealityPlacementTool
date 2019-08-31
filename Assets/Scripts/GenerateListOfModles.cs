using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGoogleDrive;

public class GenerateListOfModles : MonoBehaviour
{

    private GoogleDriveFiles.ListRequest request;
    private Dictionary<string, string> results;
    private string query = string.Empty;

    // Start is called before the first frame update
    void Start()
    {
        ListFiles();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ListFiles(string nextPageToken = null)
    {
        request = GoogleDriveFiles.List();
        request.Fields = new List<string> { "nextPageToken, files(id, name, size, createdTime)" };
        //		request.PageSize = ResultsPerPage;
        if (!string.IsNullOrEmpty(query))
            request.Q = string.Format("name contains '{0}'", query);
        if (!string.IsNullOrEmpty(nextPageToken))
            request.PageToken = nextPageToken;
        request.Send().OnDone += BuildResults;

    }


    private void BuildResults(UnityGoogleDrive.Data.FileList fileList)
    {
        results = new Dictionary<string, string>();

        foreach (var file in fileList.Files)
        {
            var fileInfo = string.Format("Name: {0}, Size: {1:0.00}MB, Created: {2:dd.MM.yyyy}",
                file.Name,
                file.Size * .000001f,
                file.CreatedTime);
            results.Add(file.Id, fileInfo);
        }

        GenerateScrollMenuButtons(results);


    }


    void GenerateScrollMenuButtons(Dictionary<string, string> res)
    {
        var objPool = GameObject.Find("ScrollMenuPool");
        var scrollRect = GetComponentInChildren<ScrollRect>();

        foreach (string s in res.Keys)
        {
            string s1 = null;
            res.TryGetValue(s, out s1);
            s1 = ParseInfo("Name", s1);

            if (s1 != null)
            {
                var btn = objPool.transform.GetChild(0);
                btn.transform.SetParent(scrollRect.content);
                btn.GetComponentInChildren<TextMeshProUGUI>().text = ParseInfo("Name", s1);
                btn.GetComponent<DownloadAndAssignModel>().SetFileId(s);
                btn.GetComponent<DownloadAndAssignModel>().SetFileName(ParseInfo("Name", s1));
            }
        }
    }

    private string ParseInfo(string v, string s1)
    {
        var val = s1.Split(',');

        int index = -1;

        for (int i = 0; i < val.Length; i++)
        {
            if (val[i].Split(':')[0] == v)
            {
                index = i;
                break;
            }
        }

        if (index == -1)
        {
            ///	Debug.LogError("string not found");
        }
        else
        {
            s1 = val[index].Split(':')[1];
        }

        if (s1.Split('.').Length > 0) //Does the string have a file extension
        {
            try
            {
                if (s1.Split('.')[1] == "fbx" || s1.Split('.')[1] == "obj") //is the extension fbx or obj
                    return s1;
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(NullReferenceException))
                {

                }
            }
        }
        return null;//return an empty string for files that we don't need
    }
}
