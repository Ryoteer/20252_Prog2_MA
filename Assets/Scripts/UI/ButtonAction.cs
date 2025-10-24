using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAction : MonoBehaviour
{
    public void LoadSceneAsync(string name)
    {
        AsyncLoadManager.Instance.LoadScene(name);
    }
}
