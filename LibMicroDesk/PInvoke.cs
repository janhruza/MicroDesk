using System.Runtime.InteropServices;

namespace LibMicroDesk;

internal static class PInvoke
{
    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    public static extern int ShellAbout(nint hWnd,
                                        string szApp,
                                        string szOtherStuff,
                                        nint hIcon);
}
