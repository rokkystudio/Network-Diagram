using System;
using System.Collections.Generic;
using System.Globalization;

namespace NetworkDiagram
{
    internal static class LocalizationService
    {
        public const string AutomaticLanguage = "auto";
        public const string EnglishLanguage = "en";
        public const string RussianLanguage = "ru";

        private static readonly Dictionary<string, string> EnglishTexts = new Dictionary<string, string>
        {
            { "app_name", "NETWORK DIAGRAM" },
            { "app_subtitle", "Traffic monitor" },
            { "settings_subtitle", "Settings" },
            { "sent", "Sent" },
            { "received", "Received" },
            { "open", "Open" },
            { "settings", "Settings" },
            { "reset", "Reset" },
            { "exit", "Exit" },
            { "compact_mode", "Compact Mode" },
            { "run_on_startup", "Run on system startup" },
            { "active_adapter", "Active network adapter" },
            { "always_on_top", "Keep diagram above all" },
            { "theme", "Theme" },
            { "opacity", "Window opacity" },
            { "sent_color", "Sent color" },
            { "received_color", "Received color" },
            { "pick", "Pick" },
            { "theme_light", "Light" },
            { "theme_dark", "Dark" },
            { "system_language", "System language - {0}" },
            { "tooltip_language", "Interface language" },
            { "tooltip_theme", "Switch theme" },
            { "tooltip_compact", "Toggle compact mode" },
            { "tooltip_settings", "Open settings" },
            { "tooltip_hide", "Hide window" },
            { "notify_text", "Network Diagram" }
        };

        private static readonly Dictionary<string, string> RussianTexts = new Dictionary<string, string>
        {
            { "app_name", "NETWORK DIAGRAM" },
            { "app_subtitle", "Монитор трафика" },
            { "settings_subtitle", "Настройки" },
            { "sent", "Отправлено" },
            { "received", "Получено" },
            { "open", "Открыть" },
            { "settings", "Настройки" },
            { "reset", "Сбросить" },
            { "exit", "Выход" },
            { "compact_mode", "Компактный режим" },
            { "run_on_startup", "Запускать вместе с Windows" },
            { "active_adapter", "Активный сетевой адаптер" },
            { "always_on_top", "Поверх остальных окон" },
            { "theme", "Тема" },
            { "opacity", "Прозрачность окна" },
            { "sent_color", "Цвет отправки" },
            { "received_color", "Цвет приёма" },
            { "pick", "Выбрать" },
            { "theme_light", "Светлая" },
            { "theme_dark", "Тёмная" },
            { "system_language", "Системный язык - {0}" },
            { "tooltip_language", "Язык интерфейса" },
            { "tooltip_theme", "Сменить тему" },
            { "tooltip_compact", "Переключить компактный режим" },
            { "tooltip_settings", "Открыть настройки" },
            { "tooltip_hide", "Скрыть окно" },
            { "notify_text", "Network Diagram" }
        };

        public static event EventHandler LanguageChanged;

        public static string CurrentLanguage
        {
            get { return ResolveLanguage(Properties.Settings.Default.Language); }
            set
            {
                string normalized = NormalizeLanguage(value);
                if (string.Equals(SelectedLanguage, normalized, StringComparison.Ordinal)) {
                    return;
                }

                Properties.Settings.Default.Language = normalized;
                Properties.Settings.Default.Save();
                OnLanguageChanged();
            }
        }

        public static string SelectedLanguage
        {
            get { return NormalizeLanguage(Properties.Settings.Default.Language); }
        }

        public static string DetectedLanguage
        {
            get { return ResolveLanguage(GetSystemLanguage()); }
        }

        public static bool IsAutomaticLanguageSelection
        {
            get { return string.Equals(SelectedLanguage, AutomaticLanguage, StringComparison.Ordinal); }
        }

        public static string NormalizeLanguage(string language)
        {
            if (string.IsNullOrWhiteSpace(language) ||
                string.Equals(language, AutomaticLanguage, StringComparison.OrdinalIgnoreCase)) {
                return AutomaticLanguage;
            }

            return string.Equals(language, RussianLanguage, StringComparison.OrdinalIgnoreCase)
                ? RussianLanguage
                : EnglishLanguage;
        }

        public static string ResolveLanguage(string language)
        {
            string normalized = NormalizeLanguage(language);
            if (string.Equals(normalized, AutomaticLanguage, StringComparison.Ordinal)) {
                return string.Equals(GetSystemLanguage(), RussianLanguage, StringComparison.Ordinal)
                    ? RussianLanguage
                    : EnglishLanguage;
            }

            return normalized;
        }

        public static bool IsRussianLanguage(string language)
        {
            return string.Equals(ResolveLanguage(language), RussianLanguage, StringComparison.Ordinal);
        }

        public static void ToggleLanguage()
        {
            CurrentLanguage = IsRussianLanguage(CurrentLanguage) ? EnglishLanguage : RussianLanguage;
        }

        public static string Text(string key)
        {
            Dictionary<string, string> dictionary = IsRussianLanguage(CurrentLanguage) ? RussianTexts : EnglishTexts;
            string value;
            return dictionary.TryGetValue(key, out value) ? value : key;
        }

        public static string LanguageDisplayName(string language)
        {
            return IsRussianLanguage(language) ? "Русский" : "English";
        }

        public static string SystemLanguageDisplayText()
        {
            return string.Format(Text("system_language"), LanguageDisplayName(DetectedLanguage));
        }

        public static string ThemeDisplayName(string theme)
        {
            return ThemeService.IsDarkTheme(theme) ? Text("theme_dark") : Text("theme_light");
        }

        private static string GetSystemLanguage()
        {
            return string.Equals(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, RussianLanguage, StringComparison.OrdinalIgnoreCase)
                ? RussianLanguage
                : EnglishLanguage;
        }

        private static void OnLanguageChanged()
        {
            EventHandler handler = LanguageChanged;
            if (handler != null) {
                handler(null, EventArgs.Empty);
            }
        }
    }
}
