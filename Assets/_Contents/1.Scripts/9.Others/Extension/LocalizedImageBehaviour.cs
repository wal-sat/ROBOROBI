using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class LocalizedImageBehaviour : MonoBehaviour
{
    public string targetElementName;
    public LocalizedAsset<Texture2D> localizedTexture; // ✅ Texture2Dに変更

    private Extension.UIElement.Image _customImage;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _customImage = root.Q<Extension.UIElement.Image>(targetElementName);

        if (_customImage != null && localizedTexture != null)
        {
            localizedTexture.AssetChanged += OnTextureChanged;
            localizedTexture.LoadAssetAsync(); // 読み込み開始
        }
    }

    private void OnDisable()
    {
        if (localizedTexture != null)
        {
            localizedTexture.AssetChanged -= OnTextureChanged;
        }
    }

    private void OnTextureChanged(Texture2D texture)
    {
        if (_customImage != null && texture != null)
        {
            _customImage.ImageTexture = texture; // 🔁 ImageSprite ではなく Texture 用プロパティ
        }
    }
}
