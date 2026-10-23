using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DownloadAssest : MonoBehaviour
{
    [SerializeField] string URL = "https://drive.google.com/uc?export=download&id=1DI0G4cIWFAVBrthApOeBVLiLRmoDTgFh";

    AssetBundle bundle;

    void Start()
    {
        StartCoroutine(SendRequest());
    }

    IEnumerator SendRequest()
    {
        using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(URL))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Download failed: {www.error}");
                yield break;
            }

            bundle = DownloadHandlerAssetBundle.GetContent(www);
        }

        if (bundle == null)
        {
            Debug.LogError("Bundle is null - file may not be a valid AssetBundle (Drive often returns an HTML page).");
            yield break;
        }

        string[] scenePaths = bundle.GetAllScenePaths();
        if (scenePaths.Length == 0)
        {
            Debug.LogError("No scenes in this bundle.");
            yield break;
        }

        string sceneName = Path.GetFileNameWithoutExtension(scenePaths[0]);
        Debug.Log($"Loading scene: {sceneName}");

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        //bundle.Unload(true);
        while (!op.isDone)
            yield return null;
    }

    void OnDestroy()
    {
        // Keep the bundle loaded while the scene is in use; unload when done.
        // if (bundle != null) bundle.Unload(false);
    }
}