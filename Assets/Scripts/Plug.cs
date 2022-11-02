using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Firebase;

public class Plug : MonoBehaviour
{
    [SerializeField] private GameObject _game;
    [SerializeField] private GameObject _view;
    private string _path;
    private RuntimePlatform brandDevice;
    private bool simDevice;

    private void Awake() 
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
        var dependencyStatus = task.Result;
        if (dependencyStatus == Firebase.DependencyStatus.Available) {
        // Create and hold a reference to your FirebaseApp,
        // where app is a Firebase.FirebaseApp property of your application class.
        Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;

        // Set a flag here to indicate whether Firebase is ready to use by your app.
        } else {
        UnityEngine.Debug.LogError(System.String.Format(
        "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
        // Firebase Unity SDK is not safe to use here.
        }
        });

        if (PlayerPrefs.HasKey("path"))
        {
            _path = PlayerPrefs.GetString("path");
        }
    }

    private void OpenGame()
    {
        _game.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OpenView(string URI)
    {
        if (_path != null && !PlayerPrefs.HasKey("path"))
        PlayerPrefs.SetString("path", _path);

        _view.SetActive(true);
        gameObject.SetActive(false);
    }

    public void TryEnterView()
    {
        if (_path == null)
        {
            _path = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("path").ToString();
            brandDevice = Application.platform;
            simDevice = false;
        }

        if(_path == null || brandDevice != RuntimePlatform.Android  || !simDevice)
        {
            OpenGame();
        }
        else
        {
            OpenView(_path);
        }
    }
}