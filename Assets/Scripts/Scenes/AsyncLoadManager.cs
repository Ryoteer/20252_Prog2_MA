using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoadManager : MonoBehaviour
{
    #region Instance
    public static AsyncLoadManager Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [Header("<color=orange>UI</color>")]
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _loadBarBackground;
    [SerializeField] private Image _loadBarFill;
    [SerializeField] private TextMeshProUGUI _progressText;
    [SerializeField] private float _fadeSpeed = 1.0f;

    private bool _isLoading = false;
    public bool IsLoading { get { return _isLoading; } }

    private Color _opaque = new Color(1.0f, 1.0f, 1.0f, 1.0f), _transparent = new Color(1.0f, 1.0f, 1.0f, 0.0f);

    private void Start()
    {
        _backgroundImage.color = _transparent;
        _loadBarBackground.color = _transparent;
        _loadBarFill.color = _transparent;
        _progressText.color = _transparent;
    }

    public void LoadScene(string name)
    {
        if (!_isLoading)
        {
            StartCoroutine(LoadSceneAsync(name));
        }
    }

    private IEnumerator LoadSceneAsync(string name)
    {
        _isLoading = true;

        float t = 0.0f;

        _loadBarFill.fillAmount = 0.0f;
        _progressText.text = "Loading...";

        while (t < 1.0f)
        {
            t += Time.deltaTime / _fadeSpeed;

            _backgroundImage.color = Color.Lerp(_transparent, _opaque, t);
            _loadBarBackground.color = Color.Lerp(_transparent, Color.gray, t);
            _loadBarFill.color = Color.Lerp(_transparent, _opaque, t);            
            _progressText.color = Color.Lerp(_transparent, _opaque, t);
            
            yield return null;
        }

        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(name);

        asyncOp.allowSceneActivation = false;

        while(asyncOp.progress < 0.9f)
        {
            _loadBarFill.fillAmount = asyncOp.progress / 0.9f;

            yield return null;
        }

        _loadBarFill.fillAmount = 1.0f;
        _progressText.text = $"Press any key to continue.";

        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        asyncOp.allowSceneActivation = true;

        t = 0.0f;

        while (t < 1.0f)
        {
            t += Time.deltaTime / _fadeSpeed;

            _backgroundImage.color = Color.Lerp(_opaque, _transparent, t);
            _loadBarBackground.color = Color.Lerp(Color.gray, _transparent, t);
            _loadBarFill.color = Color.Lerp(_opaque, _transparent, t);
            _progressText.color = Color.Lerp(_opaque, _transparent, t);

            yield return null;
        }

        _isLoading = false;
    }
}
