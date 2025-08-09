using System.Media;

namespace LibMicroDesk.Windows;

/// <summary>
/// Representing the About dialog for the MicroDesk applications.
/// </summary>
public partial class WndAbout : IconlessWindow
{

    /// <summary>
    /// Creates a new instance of the <see cref="WndAbout"/> class.
    /// </summary>
    public WndAbout()
    {
        InitializeComponent();

        this.Loaded += (s, e) =>
        {
            SystemSounds.Beep.Play();
        };
    }
}