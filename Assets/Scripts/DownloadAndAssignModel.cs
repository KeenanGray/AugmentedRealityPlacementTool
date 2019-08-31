using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityGoogleDrive;
using System.Text;
using System.IO;
using UnityEditor;
using TriLib;

public class DownloadAndAssignModel : MonoBehaviour
{
    private GoogleDriveFiles.DownloadRequest request;
    private string result = string.Empty;

    private string fileId;
    string fileName;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(DownloadModel);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetFileId(string newId)
    {
        fileId = newId;
    }
    public void SetFileName(string newName)
    {
        fileName = newName;
    }

    void DownloadModel()
    {
        var path = Application.persistentDataPath + "/" + fileName.Replace(" ", "");
        //EditorUtility.RevealInFinder(path);

        //first try to get the file by name
        try
        {
            using (var assetLoader = new AssetLoader())
            {
                var ops = AssetLoaderOptions.CreateInstance();   //Creates the AssetLoaderOptions instance.
                                                                 //AssetLoaderOptions let you specify options to load your model.
                                                                 //(Optional) You can skip this object creation and it's parameter or pass null.

                //You can modify assetLoaderOptions before passing it to LoadFromFile method. You can check the AssetLoaderOptions API reference at:
                //https://ricardoreis.net/trilib/manual/html/class_tri_lib_1_1_asset_loader_options.html
                ops.UseOriginalPositionRotationAndScale = true;
                ops.Scale = 1 / 1000f;

                var wrapperGameObject = gameObject;                             //Sets the game object where your model will be loaded into.
                                                                                //(Optional) You can skip this object creation and it's parameter or pass null.

                var myGameObject = assetLoader.LoadFromFile(path, ops, wrapperGameObject); //Loads the model synchronously and stores the reference in myGameObject.


                //successful retreival of file
                if (myGameObject != null)
                {
                    var ObjectToPlace = Instantiate(myGameObject);

                    //After instantiating object kill UI
                    GameObject.Find("Canvas").SetActive(false);

                    //Assign variable to TapToPlace
                    GameObject.Find("AR Session Origin").GetComponent<TapToPlace>().objectToPlace = ObjectToPlace;

                }
            }

        }
        catch
        {
            //if we don't get the file locally, we'll have to download it from the server

            request = GoogleDriveFiles.Download(fileId);

            //request = GoogleDriveFiles.Download(fileId, range.start >= 0 ? (RangeInt?)range : null);
            request.Send().OnDone += SetResult;
        }


    }

    private void SetResult(UnityGoogleDrive.Data.File file)
    {
        result = Encoding.UTF8.GetString(file.Content);
        var path = Application.persistentDataPath + "/" + fileName.Replace(" ", "");

        File.WriteAllBytes(path, file.Content);

        //Assign the file to the placement object
        //successful retreival of file
        using (var assetLoader = new AssetLoader())
        {
            var ops = AssetLoaderOptions.CreateInstance();   //Creates the AssetLoaderOptions instance.
                                                             //AssetLoaderOptions let you specify options to load your model.
                                                             //(Optional) You can skip this object creation and it's parameter or pass null.

            //You can modify assetLoaderOptions before passing it to LoadFromFile method. You can check the AssetLoaderOptions API reference at:
            //https://ricardoreis.net/trilib/manual/html/class_tri_lib_1_1_asset_loader_options.html
            ops.UseOriginalPositionRotationAndScale = true;
            ops.Scale = 1 / 1000f;

            var wrapperGameObject = gameObject;                             //Sets the game object where your model will be loaded into.
                                                                            //(Optional) You can skip this object creation and it's parameter or pass null.

            var myGameObject = assetLoader.LoadFromFile(path, ops, wrapperGameObject); //Loads the model synchronously and stores the reference in myGameObject.
            var ObjectToPlace = Instantiate(myGameObject);

            if (ObjectToPlace != null)
            {
                //After instantiating object kill UI
                GameObject.Find("Canvas").SetActive(false);

                //Assign variable to TapToPlace
                GameObject.Find("AR Session Origin").GetComponent<TapToPlace>().objectToPlace = ObjectToPlace;
            }
        }


        //EditorUtility.RevealInFinder(path);
    }


}
