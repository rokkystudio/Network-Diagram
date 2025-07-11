using System;
using System.Drawing;

namespace NetworkDiagram
{
    [Serializable]
    public class ComboBoxItem
    {
        private string _value;
        private Image _image;

        public string Value
        {
            get {
                return _value;
            }

            set {
                _value = value;
            }
        }

        public Image Image
        {
            get {
                return _image;
            }

            set {
                _image = value;
            }
        }

        public ComboBoxItem() {
            _value = string.Empty;
            _image = new Bitmap(1,1);
        }

        public ComboBoxItem(string value) {
            _value = value;
            _image = new Bitmap(1, 1);
        }

        public ComboBoxItem(string value, Image image) {
            _value = value;
            _image = image;
        }

        public override string ToString() {
            return _value;
        }
    }
}
