# Project Structure

Active WPF application files are organized by role:

- `Views/` - WPF windows and code-behind (`MainWindow`, `SettingsWindow`).
- `ViewModels/` - WPF view models.
- `Controls/` - reusable WPF controls.
- `Core/` - small MVVM infrastructure helpers.
- `Models/` - domain/data objects, such as network adapters.
- `Services/` - localization, theme, assets, tray icon, placement, and icon generation services.
- `Themes/` - traffic color palette definitions.
- `Assets/App/` - application-level icons.
- `Assets/Icons/` - UI icons, such as theme toggle images.
- `Assets/Flags/` - language flag images.
- `Assets/TrayIcons/` - tray icon image frames referenced by `Properties/Resources.resx`.
- `Legacy/WinForms/` - old WinForms implementation kept for reference and excluded from compilation.

Tray icon arrow files use this naming:

- `tray-arrows-style-{style}-state-{state}.png`
- `style` is the selectable arrow set.
- `state` is the active-arrow state: `1` idle, `2` download, `3` upload, `4` both arrows active.
- Settings previews use `state-4`.

The active project is WPF. `Legacy/WinForms` files are intentionally excluded in `Network Diagram.csproj`.
