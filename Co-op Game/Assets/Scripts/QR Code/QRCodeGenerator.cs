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
        var encoded = new Texture2D (256, 256);
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
        return writer.Write(textForEncoding);
    }
}
