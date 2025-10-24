using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveLoadTrigger : MonoBehaviour
{
    [Header("<color=orange>Scene Managment</color>")]
    [SerializeField] private string _originalScene = "TerrainsScene";
    [SerializeField] private string _addedScene = "DungeonScene";
    [SerializeField] private Animation _doorAnimation;

    private bool _isActive = false;
    public bool IsActive { get { return _isActive; } }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerBehaviour>() && !_isActive)
        {
            StartCoroutine(LoadSceneAdditive(_addedScene));
        }
    }

    private IEnumerator LoadSceneAdditive(string name)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        asyncOp.completed += OpenDoor;

        while (!asyncOp.isDone)
        {
            yield return null;
        }

        _isActive = true;
    }

    private void OpenDoor(AsyncOperation asyncOp)
    {
        _doorAnimation.clip = _doorAnimation.GetClip("DungeonDoorOpen");
        _doorAnimation.Play();
    }

    private IEnumerator UnloadSceneAsync(string name)
    {
        _doorAnimation.clip = _doorAnimation.GetClip("DungeonDoorClose");
        _doorAnimation.Play();

        yield return new WaitForSeconds(_doorAnimation.clip.length);

        SceneManager.UnloadSceneAsync(name);

        _isActive = false;
    }
}
