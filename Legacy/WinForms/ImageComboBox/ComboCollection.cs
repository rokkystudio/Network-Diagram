using System;
using System.Collections;
using System.Windows.Forms;

namespace NetworkDiagram
{
    [Serializable]
    public class ComboCollection : CollectionBase
    {
        public EventHandler UpdateItems;
        public ComboBox.ObjectCollection ItemsBase { get; set; }

        public ComboBoxItem this[int index]
        {
            get {
                return ((ComboBoxItem)ItemsBase[index]);
            }
            
            set {
                ItemsBase[index] = value;
            }
        }

        public int Add(ComboBoxItem value) {
            var result = ItemsBase.Add(value);
            UpdateItems.Invoke(this, null);
            return result;
        }

        public int IndexOf(ComboBoxItem value) {
            return (ItemsBase.IndexOf(value));
        }

        public void Insert(int index, ComboBoxItem value) {
            ItemsBase.Insert(index, value);
            UpdateItems.Invoke(this, null);
        }

        public void Remove(ComboBoxItem value) {
            ItemsBase.Remove(value);
            UpdateItems.Invoke(this, null);
        }

        public bool Contains(ComboBoxItem value) {
            return (ItemsBase.Contains(value));
        }
    }
}
