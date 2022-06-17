using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HaveIBeenPownedRetriever : MonoBehaviour
{
    private string _pwnedInformation;

    public string mail;

    public List<string> Result = new List<string>();

    bool Generated = false;

    private IEnumerator RequestPwnInfo(string mail)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get("https://haveibeenpwned.com/api/v3/breachedaccount/" + mail );
        webRequest.SetRequestHeader("hibp-api-key", "36c16a7b508844879ac003916dfcf3ad");
      
        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.LogError(webRequest.error);
            yield break;
        }
        Debug.Log(webRequest.downloadHandler.text);
        _pwnedInformation = webRequest.downloadHandler.text;

        generateList();
    }

    public void RetrieveMailInfo(string email)
    {
        StartCoroutine(RequestPwnInfo(email));
    }
    public string GetInfo()
    {
        return _pwnedInformation;
    }

    public string[] GetAll()
    {
        if (!Generated) {
            RetrieveMailInfo(Saves.SaveGameContoller.getMail());
        }

        if (Result.Count <= 0)
        {
            string[] e = { "geen resultaat" };

            return e;
        }

        return Result.ToArray();

       
    }

    void generateList()
    {

        _pwnedInformation = _pwnedInformation.Remove(1, 1);
        _pwnedInformation = _pwnedInformation.Remove(_pwnedInformation.Length - 1,1);

        List<string> workitems = new List<string>();
        workitems.AddRange(_pwnedInformation.Split(','));

        foreach (string item in workitems)
        {
            string E;

            E = item.Remove(0, 9);
            E = E.Remove(E.Length - 2);

            Debug.Log(E);

            Result.Add(E);
        }

        Generated = true;

    }

    public string randomInfo()
    {
        
        if (Result.Count >= 0)
        {
            int r = Random.Range(1, Result.Count);

            string s = Result[r];
            Result.Remove(s);

            return s;
        }

        return null;
    }
}
