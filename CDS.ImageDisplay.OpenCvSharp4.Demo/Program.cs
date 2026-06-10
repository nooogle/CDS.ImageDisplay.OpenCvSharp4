using System;
using System.Windows.Forms;

namespace CDS.ImageDisplay.OpenCvSharp4.Demo;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
#if !NETFRAMEWORK
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
#endif
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        using var form = new FormTestLauncher();
        Application.Run(form);
    }
}
