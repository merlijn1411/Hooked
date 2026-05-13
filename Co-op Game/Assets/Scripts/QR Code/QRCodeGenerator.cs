using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class QRCodeGenerator : MonoBehaviour
{
    [SerializeField] private PhoneInputManager phoneInputManager; 
    [SerializeField] private RawImage image;

    private void Start()
    {
        // Pak het opgeslagen adres van de PhoneInputManager
        string url = phoneInputManager.ServerURL.Replace("ws://", "http://");

        var myQR = generateQR(url);
        
        image.texture = myQR;
    }
    
    private Texture2D generateQR(string text) {
        var encoded = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        var color32 = Encode(text, encoded.width, encoded.height);
        encoded.SetPixels32(color32);
        encoded.Apply();
        return encoded;
    }
    
    private static Color32[] Encode(string textForEncoding, 
        int width, int height) {
        var writer = new BarcodeWriter {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions {
                Height = height,
                Width = width
            }
        };
        
        var color32 = writer.Write(textForEncoding);
        
        // Loop through all pixels and make white ones transparent
        for (int i = 0; i < color32.Length; i++)
        {
            if (color32[i].r == 255 && color32[i].g == 255 && color32[i].b == 255)
            {
                color32[i] = new Color32(255, 255, 255, 0);
            }
        }
        
        return color32;
    }
}
