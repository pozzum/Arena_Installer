Imports System.IO    'Files
Public Class backup_menu
#Region "Controls"
    Dim filtered As Boolean = False
    Private WithEvents copier As FileAndDirectoryCopy.Copy
    Dim restore_pending As Boolean
    Private Sub backup_menu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckBox_Backup.Checked = My.Settings.backup_save
        Check_rebuild.Checked = My.Settings.rebuild_def
    End Sub
    Private Sub Button_backup_current_Click(sender As Object, e As EventArgs) Handles Button_backup_current.Click
        If Button_backup_current.Text = "Backup Game" Then
            If Button_Restore.Text = "Cancel" = False Then
                Dim result As Integer = MessageBox.Show("Would you like to backup the whole game?", "Only backup arenas?", MessageBoxButtons.YesNo)
                If result = DialogResult.No Then 'Filtered
                    If File.Exists(Path.GetDirectoryName(
                                   Application.ExecutablePath) &
                                   "\FileFilter.txt") = False Then 'we need to copy the txt from the embedded resource
                        File.WriteAllText(Path.GetDirectoryName(
                                   Application.ExecutablePath) &
                                   "\FileFilter.txt", My.Resources.FileFilter)
                    End If
                    MessageBox.Show("FileFilter.txt may be edited before continuing.")
                    filtered = True
                Else
                    filtered = False
                End If
                Button_Restore.Text = "Cancel"
                CheckBox_Backup.Visible = False
                Check_rebuild.Visible = False
                copier = New FileAndDirectoryCopy.Copy
                If Directory.Exists(Path.GetDirectoryName(Application.ExecutablePath) & "\Backup") = False Then
                    Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath) & "\Backup")
                End If
                If filtered Then
                    Dim FliteredFolders As List(Of String) = New List(Of String)
                    Dim BaseFolder As String = Path.GetDirectoryName(My.Settings.exe_location)
                    FliteredFolders.Add(Path.GetDirectoryName(My.Settings.exe_location)) 'first is the base directory to act as the 
                    Dim reader = File.OpenText(Path.GetDirectoryName(
                                   Application.ExecutablePath) &
                                   "\FileFilter.txt")
                    Dim line As String = Nothing
                    Dim lines As Integer = 0
                    While (reader.Peek() <> -1)
                        line = reader.ReadLine()
                        If line.StartsWith("*") Then
                            FliteredFolders.Add(line.Replace("*", BaseFolder))
                        Else
                            FliteredFolders.Add(line)
                        End If
                    End While
                    copier.BeginOperation(FliteredFolders,
                                 Path.GetDirectoryName(
                                   Application.ExecutablePath) & "\Backup",
                                 FileAndDirectoryCopy.Copy.DirectoryParsing.IncludeSubfolders)
                Else
                    copier.BeginOperation(Path.GetDirectoryName(My.Settings.exe_location),
                                 Path.GetDirectoryName(
                                   Application.ExecutablePath) & "\Backup",
                                 FileAndDirectoryCopy.Copy.DirectoryParsing.IncludeSubfolders)
                End If
            End If
        Else 'Text is cancel
            copier.CancelOperation()
        End If
    End Sub

    Private Sub Button_Restore_Click(sender As Object, e As EventArgs) Handles Button_Restore.Click
        If Button_Restore.Text = "Restore Backup" Then
            If Button_backup_current.Text = "Cancel" = False Then
                Button_backup_current.Text = "Cancel"
                CheckBox_Backup.Visible = False
                Check_rebuild.Visible = False
                copier = New FileAndDirectoryCopy.Copy
                copier.BeginOperation(Path.GetDirectoryName(Application.ExecutablePath) & "\Backup",
                                      Path.GetDirectoryName(My.Settings.exe_location),
                                FileAndDirectoryCopy.Copy.DirectoryParsing.IncludeSubfolders)
                restore_pending = True
            End If
        Else 'Text is cancel
            copier.CancelOperation()
        End If
    End Sub
    Private Sub CheckBox_Backup_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox_Backup.CheckedChanged
        My.Settings.backup_save = CheckBox_Backup.Checked
    End Sub

    Private Sub Check_rebuild_CheckedChanged(sender As Object, e As EventArgs) Handles Check_rebuild.CheckedChanged
        My.Settings.rebuild_def = Check_rebuild.Checked
    End Sub
#End Region
#Region "UI Updates"
    Private Sub copier_ProgressChanged(sender As Object, e As FileAndDirectoryCopy.Copy.ProgressChangedEventArgs) Handles copier.ProgressChanged

        If e.UserCancelledOperation Then
            Label_Progress.Text = "Cancelling Operation..."
        Else
            Label_Progress.Text = e.CurrentActivity_String

            Select Case e.CurrentActivity
                Case FileAndDirectoryCopy.Copy.OperationType.Copying
                    Label_Progress.Text =
                        String.Format("Copied {0} of {1} Files ({2} of {3}){4}{5} Completed",
                                      e.CurrentFileCount_String,
                                      e.TotalFileCount_String,
                                      e.CumulativeFileSize_String,
                                      e.TotalFilesSize_String,
                                      vbCrLf,
                                      e.CurrentProgress_String(1))

                    'lbl_TotalDirectoryCount.Text = e.TotalDirectoryCount_String

                    'lbl_TotalFileCount.Text =
                    'String.Format("{0} ({1})",
                    'e.TotalFileCount_String,
                    'e.TotalFilesSize_String)

                    If e.CurrentProgressForProgressBar > 0 Then
                        With ProgressBar1
                            .Visible = True
                            .Value = e.CurrentProgressForProgressBar
                            .Refresh()
                        End With
                    End If

                Case Else
                    Label_Progress.Text = " "
            End Select
        End If
        'lbl_StatusLeft.Text = "Elapsed: " & e.Elapsed_String
    End Sub
    Private Sub copier_OperationComplete(sender As Object, e As FileAndDirectoryCopy.Copy.OperationCompleteEventArgs) Handles copier.OperationComplete
        ProgressBar1.Value = 0
        Label_Progress.Text = " "
        Button_Restore.Text = "Restore Backup"
        Button_backup_current.Text = "Backup Game"
        CheckBox_Backup.Visible = True
        Check_rebuild.Visible = True
        If My.Settings.backup_save Then
            Backupsave()
        End If
        If e.OperationException IsNot Nothing Then
            MessageBox.Show(String.Format("An error occurred:{0}{0}{1}",
                                          vbCrLf, e.OperationException.Message),
                                          "Exception Thrown",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning)
        ElseIf e.UserCancelledOperation Then
            MessageBox.Show("Backup canceled partway through!")
        Else
            MessageBox.Show("Backup complete.")
            If restore_pending Then
                My.Settings.current_package = "Default"
            End If
        End If
    End Sub


    Shared Sub Backupsave()
        If My.Settings.save_location <> "" Then
            If Directory.Exists(Path.GetDirectoryName(Application.ExecutablePath) & "\Save") = False Then
                Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath) & "\Save")
            End If
            Dim foldername As String = My.Settings.current_package & " " & DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss")
            Dim newfolder As String = Path.GetDirectoryName(Application.ExecutablePath) & "\Save\" & foldername
            Directory.CreateDirectory(newfolder)
            My.Computer.FileSystem.CopyDirectory(Path.GetDirectoryName(My.Settings.save_location), newfolder, False)
        End If
    End Sub


#End Region
End Class
