using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDiagram
{
    class Tools
    {
        public static String ColorToHex(Color color) {
            return ColorTranslator.ToHtml(color);
        }

        public static Color HexToColor(String hex)
        {
            try {
                return ColorTranslator.FromHtml(hex);
            }
            catch (Exception) {
                return Color.Empty;
            }
        }
    }
}
