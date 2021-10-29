using UnityEngine;

public class CameraSaveScreenshot : MonoBehaviour
{
    public int resWidth = 1920;
    public int resHeight = 1080;

    public static bool shot;

    //Creates name for the image file
    public static string ScreenShotName(int width, int height) {
        return string.Format("{0}/Screenshots/screen_{1}x{2}_{3}.png",
                            Application.dataPath,
                            width, height,
                            System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }
 
    public static void TakeHiResShot() {
        shot = true;
    }
    
    void LateUpdate() {
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
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
            shot = false;
        }
    }
}
