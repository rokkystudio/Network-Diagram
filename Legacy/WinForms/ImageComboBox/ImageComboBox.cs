using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NetworkDiagram
{
    public class ImageComboBox : ComboBox
    {
        private ComboCollection _items;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ComboCollection Items {
            get { return _items; }
            set { _items = value; }
        }

        public ImageComboBox() {
            DropDownStyle = ComboBoxStyle.DropDownList; 
            DrawMode = DrawMode.OwnerDrawVariable;
            DrawItem += ComboBoxDrawItemEvent;
            MeasureItem += ComboBox_MeasureItem;
        }
              
        protected override ControlCollection CreateControlsInstance()
        {
            _items = new ComboCollection {
                ItemsBase = base.Items
            };

            _items.UpdateItems += UpdateItems;
            
            return base.CreateControlsInstance();
        }
      
        private void UpdateItems(object sender, EventArgs e) {}

        private void ComboBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            var g = CreateGraphics();
            var maxWidth = 0;
            foreach (var width in Items.ItemsBase.Cast<object>().Select(element => (int)g.MeasureString(element.ToString(), Font).Width).Where(width => width > maxWidth)) {
                maxWidth = width;
            }
            DropDownWidth = maxWidth + 20;
        }

        private void ComboBoxDrawItemEvent(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index != -1)
            {
                var comboboxItem = Items[e.Index];
                //Draw the image in combo box using its bound and ItemHeight
                e.Graphics.DrawImage(comboboxItem.Image, e.Bounds.X, e.Bounds.Y, ItemHeight, ItemHeight);

                //we need to draw the item as string because we made drawmode to ownervariable
                e.Graphics.DrawString(Items[e.Index].Value.ToString(), Font, Brushes.Black,
                    new RectangleF(e.Bounds.X + ItemHeight, e.Bounds.Y, DropDownWidth, ItemHeight));
            }
            
            e.DrawFocusRectangle();
        }
    } 
}
