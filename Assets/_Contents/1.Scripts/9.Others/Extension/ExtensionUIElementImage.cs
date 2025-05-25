using UnityEngine;
using UnityEngine.UIElements;

namespace Extension.UIElement
{
    [UxmlElement]
    public partial class Image : VisualElement
    {
        [UxmlAttribute] 
        public Texture2D ImageTexture
        {
            get => _image.image as Texture2D;
            set => _image.image = value;
        }

        private readonly UnityEngine.UIElements.Image _image;

        public Image()
        {
            _image = new UnityEngine.UIElements.Image();
            Add(_image);
        }
    }

}
