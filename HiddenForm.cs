using System.Windows.Forms;

namespace NetworkDiagram
{
    // Форма, скрывающая себя при первом запуске.
    // Используется для фонового запуска без вспышек и появления в Alt+Tab.
    public class HiddenForm : Form
    {
        private bool firstShow = true;

        // Переопределяет логику отображения формы.
        // При первом вызове подавляет показ окна, если не в дизайнере.
        protected override void SetVisibleCore(bool value)
        {
            if (!DesignMode && firstShow) {
                firstShow = false;
                base.SetVisibleCore(false);
                return;
            }

            base.SetVisibleCore(value);
        }
    }
}
