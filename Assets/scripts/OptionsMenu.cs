using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using TMPro;
using Cinemachine;

public class OptionsMenu : MonoBehaviour
{
    static string brightness_PPrefsTag = "Brightness";
    static string gamma_PPrefsTag = "Gamma";
    static string music_PPrefsTag = "Music";
    static string sfx_PPrefsTag = "SFX";
    static string resolution_PPrefsTag = "Resolution";
    static string camera_PPrefsTag = "Cameras";
    static string fullscreen_PPrefsTag = "FullScreen";
    static string quality_PPrefsTag = "Quality";

    [SerializeField] Canvas canvas;
    [SerializeField] AudioMixer myMixer;
    [SerializeField] Volume volume;


    [SerializeField] TMP_Dropdown resolution;
    [SerializeField] TMP_Dropdown fullscreen;
    [SerializeField] TMP_Dropdown quality;
    [SerializeField] TMP_Dropdown cameras;

    public GameObject[] camerasGO;
    public static GameObject current_camera;

    public bool menuIsOpen = false;
    public bool goMenu = false;
    public bool afterMenu = false;
    
    private MenuManager menuManager;

    [SerializeField] Slider sliMusic;
    [SerializeField] Slider sliSFX;
    [SerializeField] Slider sliBrightness;
    [SerializeField] Slider sliGamme;
    //[SerializeField] Slider sliZoom;

    public GameObject canvasHUD;
    public GameObject canvasPausa;
    public GameObject canvasTutorial;

    ColorAdjustments colorAdjustments;
    LiftGammaGain liftGammaGain;

    public CinemachineFreeLook freeLookNear;
    public CinemachineFreeLook freeLookMiddle;
    public CinemachineFreeLook freeLookTop;



    void Start()
    {

        volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        volume.profile.TryGet<LiftGammaGain>(out liftGammaGain);
        

        goMenu = false;
        menuManager = FindObjectOfType<MenuManager>();

        //sliBrightness.value = PlayerPrefs.GetFloat(zoom_PPrefsTag, 4f);
        //sliZoom.value = PlayerPrefs.GetFloat(brightness_PPrefsTag, 0.75f);
        sliGamme.value = PlayerPrefs.GetFloat(gamma_PPrefsTag, 0.1f);
        sliMusic.value = PlayerPrefs.GetFloat(music_PPrefsTag, 0.5f);
        sliSFX.value = PlayerPrefs.GetFloat(sfx_PPrefsTag, 0.5f);


        {
            resolution.ClearOptions();
            List<string> options = new List<string>();
            for (int i = 6; i < Screen.resolutions.Length; i++)
            {
                //options.Add("" + Screen.resolutions[i].width + " x " + Screen.resolutions[i].height + " " + Screen.resolutions[i].refreshRate + "hz");
                //options.Add("" + Screen.resolutions[i].width + " x " + Screen.resolutions[i].height + " " + Screen.resolutions[i].refreshRate + "hz");
                //options.Add(" 1280 x 720 ");
                options.Add("" + Screen.resolutions[i].width + " x " + Screen.resolutions[i].height + " " + Screen.resolutions[i].refreshRate + "hz");

            }

            int currentResolution = PlayerPrefs.GetInt(resolution_PPrefsTag, -1);
            if (currentResolution == -1)
            {
                for (int i = 6; i < Screen.resolutions.Length; i++)
                {
                    if ((Screen.resolutions[i].width == Screen.currentResolution.width) &&
                         (Screen.resolutions[i].height == Screen.currentResolution.height) &&
                         (Screen.resolutions[i].refreshRate == Screen.currentResolution.refreshRate))
                    {
                        currentResolution = i;
                    }
                }
            }
            resolution.AddOptions(options);
            resolution.SetValueWithoutNotify(currentResolution);
        }

        {
            quality.ClearOptions();
            List<string> options = new List<string>();
            for (int i = 0; i < QualitySettings.names.Length; i++)
            {
                options.Add(QualitySettings.names[i]);
            }
            quality.AddOptions(options);

            int qualityLevel = PlayerPrefs.GetInt(quality_PPrefsTag, -1);
            if (qualityLevel != -1)
            {
                quality.value = qualityLevel;
            }
            else
            {
                quality.SetValueWithoutNotify(QualitySettings.GetQualityLevel());
            }
        }

        {
            int fullscreenValue = PlayerPrefs.GetInt(fullscreen_PPrefsTag, -1);
            if (fullscreenValue != -1)
            {
                fullscreen.value = fullscreenValue;
            }
            else
            {
                fullscreen.SetValueWithoutNotify(Screen.fullScreen == true ? 0 : 1);
            }
        }

        {
            
            cameras.ClearOptions();
            List<string> optionsCamera = new List<string>();
            for (int i = 0; i < camerasGO.Length; i++)
            {
                if (i == 0)
                {
                    optionsCamera.Add("Camera Near");
                }
                else
                if (i == 1)
                {
                    optionsCamera.Add("Camera Middle");
                }
                else
                if (i == 2)
                {
                    optionsCamera.Add("Camera Far");
                }
                else { 
                    optionsCamera.Add("Camara " + i+1);
                }
            }
            cameras.AddOptions(optionsCamera);

            int camerasLevel = PlayerPrefs.GetInt(camera_PPrefsTag, -1);
            if(camerasLevel != -1)
            {
                //UnityEngine.Debug.Log("hola2");
                cameras.value = camerasLevel;
            }
            else
            {
               // UnityEngine.Debug.Log("hola");
                cameras.SetValueWithoutNotify(1);
            }
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnOpenMenu();
        }

    }


    public void OnSliBrightnessValue(float newValue)
    {
        PlayerPrefs.SetFloat(brightness_PPrefsTag, newValue);
        colorAdjustments.postExposure.value = NormalizedToRange(newValue, -5f, 5f);
    }
    
    public void OnSliGammaValue(float newValue)
    {
        PlayerPrefs.SetFloat(gamma_PPrefsTag, newValue);
        liftGammaGain.gamma.value = Vector4.one * NormalizedToRange(newValue, -0.5f, 2f);
    }

    public void OnSliMusicValue(float newValue)
    {
        PlayerPrefs.SetFloat(music_PPrefsTag, newValue);
        myMixer.SetFloat("Music", LinearToDecibel(newValue));
    }

    public void OnSliSFXValue(float newValue)
    {
        PlayerPrefs.SetFloat(sfx_PPrefsTag, newValue);
        myMixer.SetFloat("SFX", LinearToDecibel(newValue));
    }


    float oldTimeScale = 0f;
    
    public void OnOpenMenu()
    {
        if (!menuIsOpen)
        {
            canvasHUD.SetActive(false);
            canvasTutorial.SetActive(false);
            canvas.gameObject.SetActive(true);
            oldTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            menuIsOpen = true;
        }
    }

    public void cdtime()
    {
        UnityEngine.Debug.Log(menuIsOpen);
        Invoke(nameof(OnButBackToGame), .5f);
    }

    public void OnButBackToGame()
    {
       
        if (menuIsOpen)
        {
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            canvas.gameObject.SetActive(false);
            canvasPausa.gameObject.SetActive(false);

            Time.timeScale = oldTimeScale;
            menuIsOpen = false;
            PlayerPrefs.Save();
            canvasHUD.SetActive(true);
            canvasTutorial.SetActive(true);

        }
    }

    public void OnGoOptions()
    {
        canvas.gameObject.SetActive(false);
        canvasPausa.gameObject.SetActive(true);

    }

    public void cdtime2()
    {
        Invoke(nameof(OnButGotoMainMenu), .5f);
    }

    public void OnButGotoMainMenu()
    {
        if (menuIsOpen && !afterMenu)
        {
            goMenu = true;
            canvas.gameObject.SetActive(false);
            Time.timeScale = oldTimeScale;
            menuIsOpen = false;
            PlayerPrefs.Save();
            menuManager.MainMenu();

        }else if(menuIsOpen && afterMenu)
        {
            
            menuIsOpen = false;
            PlayerPrefs.Save();
            menuManager.MainMenu();
        }
    }

    public static float LinearToDecibel(float linear)
    {
        float dB;

        if (linear != 0)
            dB = 20.0f * Mathf.Log10(linear);
        else
            dB = -144.0f;

        return dB;
    }

    public static float DecibelToLinear(float dB)
    {
        float linear = Mathf.Pow(10.0f, dB / 20.0f);

        return linear;
    }

    public float NormalizedToRange(float value, float min, float max)
    {
        float range = max - min;
        float rangedValue = min + (value * range);

        return rangedValue;
    }

    public void OnResolutionChange(int option)
    {
        PlayerPrefs.SetInt(resolution_PPrefsTag, option);
        ApplyResolution();
    }

    public void OnFullscreenChange(int option)
    {
        PlayerPrefs.SetInt(fullscreen_PPrefsTag, option);
        ApplyResolution();
    }

    public void OnCameraChange(int option2)
    {
        PlayerPrefs.SetInt(camera_PPrefsTag, option2);
        
        if (option2 == 0)
        {
            camerasGO[0].SetActive(true);
            camerasGO[1].SetActive(false);
            camerasGO[2].SetActive(false);
            current_camera = camerasGO[0].gameObject;
        }
        else if(option2 == 1)
        {
            camerasGO[0].SetActive(false);
            camerasGO[1].SetActive(true);
            camerasGO[2].SetActive(false);
            current_camera = camerasGO[1].gameObject;
        }
        else if(option2 == 2)
        {
            camerasGO[0].SetActive(false);
            camerasGO[1].SetActive(false);
            camerasGO[2].SetActive(true);
            current_camera = camerasGO[2].gameObject;
        }
        else
        {
            camerasGO[0].SetActive(false);
            camerasGO[1].SetActive(true);
            camerasGO[2].SetActive(false);
        }
    }

    public void OnQualityChange(int option)
    {
        PlayerPrefs.SetInt(quality_PPrefsTag, option);
        QualitySettings.SetQualityLevel(option, true);
    }

    void ApplyResolution()
    {
        Screen.SetResolution(
            Screen.resolutions[resolution.value].width,
            Screen.resolutions[resolution.value].height,
            fullscreen.value == 0,
            Screen.resolutions[resolution.value].refreshRate
        );
    }

    public void ChangeAxisX(bool invert)
    {
        if (invert == false)
        {
            
            freeLookNear.m_XAxis.m_InvertInput = false;
            freeLookMiddle.m_XAxis.m_InvertInput = false;
            freeLookTop.m_XAxis.m_InvertInput = false;

        }
        else
        {
            freeLookNear.m_XAxis.m_InvertInput = true;
            freeLookMiddle.m_XAxis.m_InvertInput = true;
            freeLookTop.m_XAxis.m_InvertInput = true;

        }
    }
    public void ChangeAxisY(bool invert)
    {
        if (invert == false)
        {
            freeLookNear.m_YAxis.m_InvertInput = false;
            freeLookMiddle.m_YAxis.m_InvertInput = false;
            freeLookTop.m_YAxis.m_InvertInput = false;
        }
        else
        {
            freeLookNear.m_YAxis.m_InvertInput = true;
            freeLookMiddle.m_YAxis.m_InvertInput = true;
            freeLookTop.m_YAxis.m_InvertInput = true;
        }
        
    }
}
