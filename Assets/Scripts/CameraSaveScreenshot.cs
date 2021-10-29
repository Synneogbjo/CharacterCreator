using TMPro;
using UnityEngine;

public class CameraSaveScreenshot : MonoBehaviour
{
    public int resWidth = 1920; //Width of exported img
    public int resHeight = 1080; //Width of exported img

    private float txtTime = 0f; //Used to show save location in TMP_Text element

    public static bool shot;
    [SerializeField] private TMP_Text _txt; //Add your TMP_Text file here

    //Creates name for the image file
    public static string ScreenShotName(int width, int height) {
        return string.Format("{0}/Screenshots/screen_{1}x{2}_{3}.png",
                            Application.dataPath, //This is where the image is saved, should be in the Unity project map -> assets -> screenshots !!! OBS !!! Lag en screenshots mappe her
                            width, height,
                            System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }
 
    //Call this function to take a screenshot
    public static void TakeHiResShot() {
        shot = true;
    }
    
    void LateUpdate() {
        //Removes text when txtTime is below 0
        if (txtTime > 0f) txtTime -= Time.deltaTime;
        else
        {
            _txt.text = "";
        }
        
        //Takes a screenshot
        if (shot)
        {
            RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
            GetComponent<Camera>().targetTexture = rt;
            Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            GetComponent<Camera>().Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
            GetComponent<Camera>().targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = ScreenShotName(resWidth, resHeight);
            System.IO.File.WriteAllBytes(filename, bytes);
            _txt.text = $"Took screenshot to: {filename}";
            txtTime = 5f;
            shot = false;
        }
    }
}