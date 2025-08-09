using LibMicroDesk.Windows;

namespace LibMicroDesk
{
    /// <summary>
    /// Representing the core functionality of the MicroDesk library.
    /// </summary>
    public static class MDCore
    {
        /// <summary>
        /// Shows the About dialog for the MicroDesk applications.
        /// </summary>
        public static void AboutBox()
        {
            _ = new WndAbout().ShowDialog();
        }
    }
}
