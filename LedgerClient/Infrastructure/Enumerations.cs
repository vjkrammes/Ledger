using System;

namespace LedgerClient.Infrastructure
{
    #region ExplorerItemType

    public enum ExplorerItemType
    {
        [Description("Unspecified")]
        [ExplorerIcon("/resources/question-32.png")]
        Unspecified = 0,
        [Description("This Computer")]
        [ExplorerIcon("/resources/device-32.png")]
        ThisComputer = 1,
        [Description("Disk Drive")]
        [ExplorerIcon("/resources/save-32.png")]
        Drive = 2,
        [Description("Directory")]
        [ExplorerIcon("/resources/folder-32.png")]
        Directory = 3,
        [Description("File")]
        [ExplorerIcon("/resources/script-32.png")]
        File = 4,
        [Description("Placeholder")]
        [ExplorerIcon("/resources/ellipsis-32.png")]
        Placeholder = 999
    }

    #endregion

    #region PasswordStrength

    public enum PasswordStrength
    {
        [Description("Blank")]
        Blank = 0,
        [Description("Very Weak")]
        VeryWeak = 1,
        [Description("Weak")]
        Weak = 2,
        [Description("Medium")]
        Medium = 3,
        [Description("Strong")]
        Strong = 4,
        [Description("Very Strong")]
        VeryStrong = 5
    }

    #endregion

    #region PopupButtons

    [Flags]
    public enum PopupButtons
    {
        [Description("None")]
        None = 0,
        [Description("Ok")]
        Ok = 1,
        [Description("Cancel")]
        Cancel = 2,
        [Description("Yes")]
        Yes = 4,
        [Description("No")]
        No = 8,
        [Description("Ok and Cancel")]
        OkCancel = Ok | Cancel,
        [Description("Yes and No")]
        YesNo = Yes | No,
        [Description("Yes, No and Cancel")]
        YesNoCancel = Yes | No | Cancel
    }

    #endregion

    #region PopupImage

    public enum PopupImage
    {
        [Description("None")]
        None = 0,
        [Description("Question")]
        Question = 1,
        [Description("Information")]
        Information = 2,
        [Description("Warning")]
        Warning = 3,
        [Description("Stop")]
        Stop = 4,
        [Description("Error")]
        Error = 5
    }

    #endregion

    #region PopupResult

    public enum PopupResult
    {
        [Description("Unspecified")]
        Unspecified = 0,
        [Description("Yes")]
        Yes = 1,
        [Description("No")]
        No = 2,
        [Description("Ok")]
        Ok = 3,
        [Description("Cancel")]
        Cancel = 4
    }

    #endregion
}
