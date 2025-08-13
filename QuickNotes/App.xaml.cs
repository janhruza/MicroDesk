using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using LibMicroDesk;
using QuickNotes.Core;
using QuickNotes.Pages;

namespace QuickNotes;

/// <summary>
/// Representing the main application class.
/// </summary>
public partial class App : Application
{
    static App()
    {
        Notes = [];
        LoadNotesData();
    }

    #region Page definitions

    /// <summary>
    /// Representing the overview page.
    /// </summary>
    public static PgOverview OverviewPage => new PgOverview();

    /// <summary>
    /// Representing the New note page.
    /// </summary>
    public static PgNewNote NewNotePage => new PgNewNote();

    #endregion

    #region Static code

    #region Notes saving and loading

    private static void GenerateTestData()
    {
        Notes.AddRange([
            new Note("Create a new application.", DateTime.Now, DateTime.Now.AddDays(7)),
            new Note("Go to a barber shop.", DateTime.Now, DateTime.Now.AddDays(14)),
            new Note("Feed cat.", DateTime.Now, DateTime.Now.AddDays(-3)),
        ]);
    }

    private static string NotesFile => "Notes.bin";

    private static void LoadNotesData()
    {
        try
        {
            if (File.Exists(NotesFile) == false)
            {
                Log.Info("Notes file not found, using sample data instead.", nameof(LoadNotesData));
                GenerateTestData();
                return;
            }

            Notes.Clear();
            using (FileStream fs = File.OpenRead(NotesFile))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    // get notes count
                    int count = br.ReadInt32();

                    for (int i = 0; i < count; i++)
                    {
                        // get note data
                        DateTime dtCreation = DateTime.FromBinary(br.ReadInt64());
                        DateTime dtExpiration = DateTime.FromBinary(br.ReadInt64());
                        string text = br.ReadString();

                        Note note = new Note(text, dtCreation, dtExpiration);
                        Notes.Add(note);
                    }
                }
            }

            Log.Info("Notes were loaded.", nameof(LoadNotesData));
            return;
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(LoadNotesData));
            return;
        }
    }

    private static void SaveNotesData()
    {
        try
        {
            using (FileStream fs = File.Create(NotesFile))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    // write notes count
                    bw.Write(Notes.Count);

                    // write all notes to the file
                    foreach (var note in Notes)
                    {
                        bw.Write(note.Creation.ToBinary());
                        bw.Write(note.Expiration.ToBinary());
                        bw.Write(note.Text);
                    }
                }
            }

            Log.Info("Notes saved successfully.", nameof(SaveNotesData));
            return;
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(SaveNotesData));
            return;
        }
    }

    #endregion

    /// <summary>
    /// Representing a list of all loaded notes.
    /// </summary>
    public static List<Note> Notes { get; }

    /// <summary>
    /// Shows a message dialog box to the user.
    /// </summary>
    /// <param name="message">The message text.</param>
    /// <param name="title">The caption (title) of the message.</param>
    /// <param name="button">Available option buttons.</param>
    /// <param name="image">Message box icon.</param>
    public static void ShowMessage(string message, string title = "Message", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage image = MessageBoxImage.Information)
    {
        _ = MessageBox.Show(message, title, button, image);
        return;
    }

    #endregion

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        MainWindow mw = new MainWindow();
        MainWindow = mw;
        MainWindow.Show();
    }

    private void Application_Exit(object sender, ExitEventArgs e)
    {
        SaveNotesData();
        return;
    }
}
