using UnityEngine;
using UnityEngine.UI;

public class PhonePhotoTaker : MonoBehaviour
{
    [SerializeField] StackChildren container;
    [SerializeField] Image prefab;
    [SerializeField] Camera captureCamera;
    [SerializeField] Camera realTimeCamera;
    [SerializeField] GameObject realTimeView;

    bool takingPhoto;
    bool inPhotoViewer = true;

    public void OnClick()
    {
        if (inPhotoViewer)
        {
            inPhotoViewer = false;

            realTimeCamera.gameObject.SetActive(true);
            realTimeView.SetActive(true);
        }
        else
        {
            inPhotoViewer = true;

            realTimeCamera.gameObject.SetActive(false);
            realTimeView.SetActive(false);

            TakePhoto();
        }
    }

    void TakePhoto()
    {
        if (takingPhoto)
            return;

        takingPhoto = true;

        var currentRT = RenderTexture.active;
        RenderTexture.active = captureCamera.targetTexture;

        captureCamera.Render();

        var rect = new Rect(0, 0, captureCamera.targetTexture.width, captureCamera.targetTexture.height);

        Texture2D image = new Texture2D(captureCamera.targetTexture.width, captureCamera.targetTexture.height);
        image.ReadPixels(rect, 0, 0);
        image.Apply();

        RenderTexture.active = currentRT;

        var newItem = Instantiate(prefab, container.transform);
        newItem.sprite = Sprite.Create(image, rect, rect.size / 2);
        container.Resize();

        takingPhoto = false;
    }

}
