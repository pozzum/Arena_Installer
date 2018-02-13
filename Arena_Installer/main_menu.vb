Imports System.IO
Public Class main_menu
    Private Sub Button_Backup_Click(sender As Object, e As EventArgs) Handles Button_Backup.Click
        backup_menu.Show()
    End Sub

    Private Sub Button_Install_Click(sender As Object, e As EventArgs) Handles Button_Install.Click
        If Not Directory.Exists(Path.GetDirectoryName(Application.ExecutablePath) & "\Backup") Then
            If MessageBox.Show("Would you like to back up your game first?", "", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                backup_menu.Show()
                Exit Sub
            End If
            Directory.CreateDirectory(Path.GetDirectoryName(Application.ExecutablePath) & "\Backup")
        End If
        install_menu.Show()
    End Sub

    Private Sub main_menu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.exe_location = "" Then
            MessageBox.Show("Select WWE2K18_x64.exe")
            If Open_File_Exe.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                If Open_File_Exe.FileName.Contains("WWE2K18_x64.exe") Then
                    My.Settings.exe_location = Open_File_Exe.FileName
                Else
                    MessageBox.Show("Please Select WWE2K18_x64.exe")
                End If
            End If
        End If
        If My.Settings.save_location = "" Then
            If Directory.Exists("C:\Steam\userdata") Then
                Dim UserDirectories() As String = Directory.GetDirectories("C:\Steam\userdata")
                For Each TestDirectory As String In UserDirectories
                    If Directory.Exists(TestDirectory & "\" & "664430") Then
                        Open_File_Save.InitialDirectory = TestDirectory & "\" & "664430"
                    End If
                Next
            ElseIf Directory.Exists("C:\Program Files (x86)\Steam\userdata") Then
                Dim UserDirectories() As String = Directory.GetDirectories("C:\Program Files (x86)\Steam\userdata")
                For Each TestDirectory As String In UserDirectories
                    If Directory.Exists(TestDirectory & "\" & "664430") Then
                        Open_File_Save.InitialDirectory = TestDirectory & "\" & "664430"
                    End If
                Next
            End If
            MessageBox.Show("Select 664430\remotecache.vdf")
            If Open_File_Save.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                If Open_File_Save.FileName.Contains("remotecache.vdf") Then
                    My.Settings.save_location = Open_File_Save.FileName
                Else
                    MessageBox.Show("Please Select WWE2K18_x64.exe")
                End If
            End If
        End If
    End Sub

End Class
