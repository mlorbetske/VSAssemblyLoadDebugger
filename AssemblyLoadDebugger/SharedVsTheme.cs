using Microsoft.VisualStudio.PlatformUI;

namespace AssemblyLoadDebugger
{
    public static class SharedVsTheme
    {
        public static object ComboBoxBackgroundBrushKey
        {
            get { return CommonControlsColors.ComboBoxBackgroundBrushKey; }
        }

        public static object ComboBoxTextBrushKey
        {
            get { return CommonControlsColors.ComboBoxTextBrushKey; }
        }

        public static object ComboBoxTextHoverBrushKey
        {
            get { return CommonControlsColors.ComboBoxTextHoverBrushKey; }
        }

        public static object ComboBoxBorderBrushKey
        {
            get { return CommonControlsColors.ComboBoxBorderBrushKey; }
        }

        public static object ComboBoxSeparatorBrushKey
        {
            get { return CommonControlsColors.ComboBoxSeparatorBrushKey; }
        }

        public static object ComboBoxGlyphPressedBrushKey
        {
            get { return CommonControlsColors.ComboBoxGlyphPressedBrushKey; }
        }

        public static object ComboBoxBackgroundHoverBrushKey
        {
            get { return CommonControlsColors.ComboBoxBackgroundHoverBrushKey; }
        }

        public static object ComboBoxBorderHoverBrushKey
        {
            get { return CommonControlsColors.ComboBoxBorderHoverBrushKey; }
        }

        public static object ComboBoxSeparatorHoverBrushKey
        {
            get { return CommonControlsColors.ComboBoxSeparatorHoverBrushKey; }
        }

        public static object ComboBoxBackgroundPressedBrushKey
        {
            get { return CommonControlsColors.ComboBoxBackgroundPressedBrushKey; }
        }

        public static object ComboBoxBorderPressedBrushKey
        {
            get { return CommonControlsColors.ComboBoxBorderPressedBrushKey; }
        }

        public static object ComboBoxSeparatorPressedBrushKey
        {
            get { return CommonControlsColors.ComboBoxSeparatorPressedBrushKey; }
        }

        public static object ComboBoxGlyphDisabledBrushKey
        {
            get { return CommonControlsColors.ComboBoxGlyphDisabledBrushKey; }
        }

        public static object ComboBoxBackgroundDisabledBrushKey
        {
            get { return CommonControlsColors.ComboBoxBackgroundDisabledBrushKey; }
        }

        public static object ComboBoxBorderDisabledBrushKey
        {
            get { return CommonControlsColors.ComboBoxBorderDisabledBrushKey; }
        }

        public static object DataGridAlternatingRowBackgroundBrushKey
        {
            get
            {
                return EnvironmentColors.SystemWindowBrushKey;
            }
        }

        public static object DataGridBackgroundBrushKey
        {
            get
            {
                return EnvironmentColors.SystemWindowBrushKey;
            }
        }

        public static object DataGridBorderBrushKey
        {
            get
            {
                return EnvironmentColors.SystemActiveBorderBrushKey;
            }
        }

        public static object DataGridColumnHeaderBackgroundBrushKey
        {
            get
            {
                return EnvironmentColors.SystemActiveBorderBrushKey;
            }
        }

        public static object DataGridColumnHeaderTextForegroundBrushKey
        {
            get
            {
                return EnvironmentColors.BrandedUITextBrushKey;
            }
        }

        public static object SideTabActiveBackgroundBrushKey
        {
            get
            {
                return EnvironmentColors.StartPageSelectedItemBackgroundBrushKey;
            }
        }

        public static object SideTabInactiveBackgroundBrushKey
        {
            get { return WindowBackgroundBrushKey; }
        }

        public static object TextBlockPressedForegroundBrushKey
        {
            get
            {
                return EnvironmentColors.FileTabHotTextBrushKey;
            }
        }

        public static object TextBlockTextForegroundBrushKey
        {
            get
            {
                return EnvironmentColors.BrandedUITextBrushKey;
            }
        }

        public static object TextBoxBackgroundBrushKey
        {
            get { return WindowBackgroundBrushKey; }
        }

        public static object TextBoxBorderBrushKey
        {
            get
            {
                return EnvironmentColors.ComboBoxBorderBrushKey;
            }
        }

        public static object TextBoxForegroundBrushKey
        {
            get
            {
                return EnvironmentColors.BrandedUITextBrushKey;
            }
        }

        public static object TextBoxDisabledBackgroundBrushKey
        {
            get
            {
                return WindowBackgroundBrushKey;
            }
        }

        public static object TextBoxDisabledBorderBrushKey
        {
            get
            {
                return EnvironmentColors.ComboBoxMouseOverBorderBrushKey;
            }
        }

        public static object TextBoxDisabledForegroundBrushKey
        {
            get
            {
                return EnvironmentColors.BrandedUITextBrushKey;
            }
        }

        public static object TextBoxMouseOverBackgroundBrushKey
        {
            get { return WindowBackgroundBrushKey; }
        }

        public static object TextBoxMouseOverBorderBrushKey
        {
            get
            {
                return EnvironmentColors.ComboBoxMouseOverBorderBrushKey;
            }
        }

        public static object TextBoxMouseOverForegroundBrushKey
        {
            get
            {
                return EnvironmentColors.BrandedUITextBrushKey;
            }
        }

        public static object TextBoxPressedBackgroundBrushKey
        {
            get { return WindowBackgroundBrushKey; }
        }

        public static object TextBoxPressedBorderBrushKey
        {
            get
            {
                return EnvironmentColors.ComboBoxMouseOverBorderBrushKey;
            }
        }

        public static object TextBoxPressedForegroundBrushKey
        {
            get
            {
                return EnvironmentColors.BrandedUITextBrushKey;
            }
        }

        public static object TextHighlightBackgroundBrushKey
        {
            get
            {
                return EnvironmentColors.SystemHighlightBrushKey;
            }
        }

        public static object TextHighlightForegroundBrushKey
        {
            get
            {
                return EnvironmentColors.SystemHighlightTextBrushKey;
            }
        }

        public static object  WindowBackgroundBrushKey
        {
            get
            {
                return EnvironmentColors.StartPageTabBackgroundBrushKey;
            }
        }

        public static object WindowBorderBrushKey
        {
            get
            {
                return EnvironmentColors.MainWindowActiveDefaultBorderBrushKey;
            }
        }

        // Button background - ControlBrush
        // Button foreground - ControlTextBrush
        // Button hover Border - HighlightBrush.

        public static object WindowButtonBackgroundBrushKey
        {
            get
            {
                return EnvironmentColors.ComboBoxBackgroundBrushKey;
            }
        }

        public static object WindowButtonDownBackgroundBrushKey
        {
            get
            {
                return EnvironmentColors.ComboBoxItemMouseOverBorderBrushKey;
            }
        }

        public static object WindowButtonHoverBackgroundBrushKey
        {
            get
            {
                return EnvironmentColors.ComboBoxFocusedBackgroundBrushKey;
            }
        }

        public static object WindowButtonForegroundBrushKey
        {
            get
            {
                return EnvironmentColors.ComboBoxTextBrushKey;
            }
        }

        public static object WindowButtonDownForegroundBrushKey
        {
            get
            {
                return EnvironmentColors.ComboBoxItemMouseOverTextBrushKey;
            }
        }

        public static object WindowButtonHoverForegroundBrushKey
        {
            get
            {
                return EnvironmentColors.ComboBoxItemMouseOverTextBrushKey;
            }
        }

        public static object WindowButtonBorderBrushKey
        {
            get
            {
                return EnvironmentColors.ComboBoxBorderBrushKey;
            }
        }

        public static object WindowButtonDownBorderBrushKey
        {
            get
            {
                return EnvironmentColors.ComboBoxItemMouseOverBackgroundBrushKey;
            }
        }

        public static object WindowButtonHoverBorderBrushKey
        {
            get
            {
                return EnvironmentColors.ComboBoxBackgroundBrushKey;
            }
        }

        public static object WindowDisabledButtonBorderBrushKey
        {
            get
            {
                return EnvironmentColors.ComboBoxBackgroundBrushKey;
            }
        }

        public static object WindowFooterBackgroundBrushKey
        {
            get
            {
                return EnvironmentColors.ScrollBarBackgroundBrushKey;
            }
        }

        public static object LinkForegroundBrushKey
        {
            get
            {
                return EnvironmentColors.CommandBarMenuGroupHeaderLinkTextBrushKey;
            }
        }

        public static bool UseAccountPickerTheming
        {
            get { return false; }
        }
    }
}
