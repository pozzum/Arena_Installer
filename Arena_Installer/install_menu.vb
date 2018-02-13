Imports System.IO    'Files
Public Class install_menu
    Public package_list As List(Of package_info)
    Private WithEvents copier As FileAndDirectoryCopy.Copy
    Private WithEvents Uninstaller As PackageUninstaller.Uninstall
    Dim pending_install As String
    Dim followup_install As String
    Dim followup_uninstall As String
    Private Sub install_menu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If File.Exists(Path.GetDirectoryName(Application.ExecutablePath) & "/packages.bin") Then
            package_list = Serialize.PackageFileDeserialize(Path.GetDirectoryName(Application.ExecutablePath) & "/packages.bin")
        Else
            package_list = New List(Of package_info)
            Dim temp_package As New package_info
            temp_package.menu_name = "Default"
            temp_package.folder_name = "Backup"
            temp_package.base_package = "Default"
            package_list.Add(temp_package)
            Serialize.FileSerialize(Path.GetDirectoryName(Application.ExecutablePath) & "/packages.bin", package_list, False)
        End If
        load_packages()
    End Sub
    Sub load_packages()
        ComboBoxPackage.Items.Clear()
        For Each temp_package As package_info In package_list
            ComboBoxPackage.Items.Add(temp_package.menu_name)
        Next
        ComboBoxPackage.SelectedIndex = 0
        Label_Current.Text = "Current: " & My.Settings.current_package
    End Sub
    Private Sub Button_Add_Click(sender As Object, e As EventArgs) Handles Button_Add.Click
        add_package.Show()
    End Sub
    Public Sub save_packages()
        Serialize.FileSerialize(Path.GetDirectoryName(Application.ExecutablePath) & "/packages.bin", package_list, False)
        load_packages()
    End Sub
    Function get_package(package_menu_name) As package_info
        For Each tested_package As package_info In package_list
            If tested_package.menu_name = package_menu_name Then
                Return tested_package
            End If
        Next
        Return Nothing
    End Function
    Private Sub Button_Install_Click(sender As Object, e As EventArgs) Handles Button_Install.Click
        If Button_Install.Text = "Install" Then
            '
            'Reinstall
            '
            If ComboBoxPackage.SelectedItem = My.Settings.current_package Then
                If MessageBox.Show("Package already installed, reinstall?", "Reinstall?", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                    Button_Install.Text = "Cancel"
                    pending_install = ComboBoxPackage.SelectedItem
                    copier = New FileAndDirectoryCopy.Copy
                    copier.BeginOperation(Path.GetDirectoryName(
                                       Application.ExecutablePath) & "\" & get_package(ComboBoxPackage.SelectedItem).folder_name,
                                          Path.GetDirectoryName(My.Settings.exe_location),
                                         FileAndDirectoryCopy.Copy.DirectoryParsing.IncludeSubfolders)
                End If
                '
                'Simple Install
                '
            ElseIf get_package(ComboBoxPackage.SelectedItem).base_package = My.Settings.current_package Then
                Button_Install.Text = "Cancel"
                pending_install = ComboBoxPackage.SelectedItem
                copier = New FileAndDirectoryCopy.Copy
                copier.BeginOperation(Path.GetDirectoryName(
                                   Application.ExecutablePath) & "\" & get_package(ComboBoxPackage.SelectedItem).folder_name,
                                      Path.GetDirectoryName(My.Settings.exe_location),
                                     FileAndDirectoryCopy.Copy.DirectoryParsing.IncludeSubfolders)
                '
                'Base to be installed then selected package
                '
            ElseIf get_package(get_package(ComboBoxPackage.SelectedItem).base_package).base_package = My.Settings.current_package Then
                If MessageBox.Show("Base Package " & get_package(ComboBoxPackage.SelectedItem).base_package & " To be installed first", "Install Base Package", MessageBoxButtons.OKCancel) =
                        DialogResult.Cancel Then
                    Exit Sub
                End If
                Button_Install.Text = "Cancel"
                pending_install = get_package(ComboBoxPackage.SelectedItem).base_package
                followup_install = ComboBoxPackage.SelectedItem
                copier = New FileAndDirectoryCopy.Copy
                copier.BeginOperation(Path.GetDirectoryName(
                                   Application.ExecutablePath) & "\" & get_package(get_package(ComboBoxPackage.SelectedItem).base_package).folder_name,
                                      Path.GetDirectoryName(My.Settings.exe_location),
                                     FileAndDirectoryCopy.Copy.DirectoryParsing.IncludeSubfolders)
                '
                'Simple Uninstall
                '
            ElseIf ComboBoxPackage.SelectedItem = get_package(My.Settings.current_package).base_package Then
                If MessageBox.Show("Current package to be uninstalled back to base " & ComboBoxPackage.SelectedItem, "Uninstall Package", MessageBoxButtons.OKCancel) =
                        DialogResult.Cancel Then
                    Exit Sub
                End If
                Button_Install.Text = "Cancel"
                pending_install = ComboBoxPackage.SelectedItem
                Uninstaller = New PackageUninstaller.Uninstall
                Uninstaller.BeginOperation(Path.GetDirectoryName(Application.ExecutablePath) & "\" & get_package(My.Settings.current_package).folder_name,
                                           Path.GetDirectoryName(Application.ExecutablePath) & "\" & get_package(get_package(My.Settings.current_package).base_package).folder_name,
                                          Path.GetDirectoryName(My.Settings.exe_location),
                                         FileAndDirectoryCopy.Copy.DirectoryParsing.IncludeSubfolders)
                '
                'Package and Base Uninstall
                '
            ElseIf ComboBoxPackage.SelectedItem = get_package(get_package(My.Settings.current_package).base_package).base_package Then
                If MessageBox.Show("Current package and base of that package to be uninstalled back to base " & ComboBoxPackage.SelectedItem, "Uninstall Package", MessageBoxButtons.OKCancel) =
                        DialogResult.Cancel Then
                    Exit Sub
                End If
                Button_Install.Text = "Cancel"
                pending_install = get_package(ComboBoxPackage.SelectedItem).base_package
                followup_uninstall = ComboBoxPackage.SelectedItem
                Uninstaller = New PackageUninstaller.Uninstall
                Uninstaller.BeginOperation(Path.GetDirectoryName(Application.ExecutablePath) & "\" & get_package(My.Settings.current_package).folder_name,
                                           Path.GetDirectoryName(Application.ExecutablePath) & "\" & get_package(get_package(My.Settings.current_package).base_package).folder_name,
                                          Path.GetDirectoryName(My.Settings.exe_location),
                                         FileAndDirectoryCopy.Copy.DirectoryParsing.IncludeSubfolders)
                '
                ' Uninstall of Pack and Install of New Pack
                '
            ElseIf get_package(ComboBoxPackage.SelectedItem).base_package = get_package(My.Settings.current_package).base_package Then
                If MessageBox.Show("Current package to be uninstalled prior to install.", "Uninstall then new install", MessageBoxButtons.OKCancel) =
                        DialogResult.Cancel Then
                    Exit Sub
                End If
                Button_Install.Text = "Cancel"
                pending_install = ComboBoxPackage.SelectedItem
                followup_install = ComboBoxPackage.SelectedItem
                Uninstaller = New PackageUninstaller.Uninstall
                Uninstaller.BeginOperation(Path.GetDirectoryName(Application.ExecutablePath) & "\" & get_package(My.Settings.current_package).folder_name,
                                           Path.GetDirectoryName(Application.ExecutablePath) & "\" & get_package(get_package(My.Settings.current_package).base_package).folder_name,
                                          Path.GetDirectoryName(My.Settings.exe_location),
                                         FileAndDirectoryCopy.Copy.DirectoryParsing.IncludeSubfolders)
            Else
                MessageBox.Show("Package too far from current, maximum of 2 install / uninstalls currently supported.")
            End If
        ElseIf Button_Install.Text = "Cancel" Then
            If Not copier Is Nothing Then
                copier.CancelOperation()
            ElseIf Not Uninstaller Is Nothing Then
                Uninstaller.CancelOperation()
            End If
        End If
    End Sub
    Shared Sub RebuildDef()
        File.Create(Path.GetDirectoryName(Path.GetDirectoryName(My.Settings.exe_location)) & "/Chunk0.def").Dispose()
        Dim DefFile As System.IO.StreamWriter
        DefFile = My.Computer.FileSystem.OpenTextFileWriter(Path.GetDirectoryName(My.Settings.exe_location) & "\Chunk0.def", True)
        DefFile.WriteLine(";PAC_LIST_ARC_EPAC")

        For Each TempFile In Directory.GetFiles(Path.GetDirectoryName(My.Settings.exe_location), "*", IO.SearchOption.AllDirectories)
            If Path.GetExtension(TempFile).ToLower = ".pac" Then
                TempFile = TempFile.Replace((Path.GetDirectoryName(My.Settings.exe_location) & "\"), "")
                DefFile.WriteLine(TempFile) 'TempFile.Replace((Path.GetDirectoryName(My.Application.MySettings.ExeFilePath) & "\"), ""))
            End If
        Next
        DefFile.Close()
    End Sub
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
        Button_Install.Text = "Install"
        If My.Settings.backup_save Then
            backup_menu.Backupsave()
        End If
        If My.Settings.rebuild_def Then
            RebuildDef()
        End If
        If e.OperationException IsNot Nothing Then
            MessageBox.Show(String.Format("An error occurred:{0}{0}{1}",
                                          vbCrLf, e.OperationException.Message),
                                          "Exception Thrown",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning)
        ElseIf e.UserCancelledOperation Then
            MessageBox.Show("Install canceled partway through!")
        Else
            If followup_install = "" Then
                MessageBox.Show("Install complete.")
                My.Settings.current_package = pending_install
                Label_Current.Text = "Current: " & My.Settings.current_package
            Else
                My.Settings.current_package = pending_install
                Label_Current.Text = "Current: " & My.Settings.current_package
                Button_Install.Text = "Cancel"
                pending_install = followup_install
                copier = New FileAndDirectoryCopy.Copy
                copier.BeginOperation(Path.GetDirectoryName(
                                   Application.ExecutablePath) & "\" & get_package(followup_install).folder_name,
                                      Path.GetDirectoryName(My.Settings.exe_location),
                                     FileAndDirectoryCopy.Copy.DirectoryParsing.IncludeSubfolders)
                followup_install = ""
            End If
        End If
    End Sub
    Private Sub Uninstaller_ProgressChanged(sender As Object, e As PackageUninstaller.Uninstall.ProgressChangedEventArgs) Handles Uninstaller.ProgressChanged

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
    Private Sub Uninstaller_OperationComplete(sender As Object, e As PackageUninstaller.Uninstall.OperationCompleteEventArgs) Handles Uninstaller.OperationComplete
        ProgressBar1.Value = 0
        Label_Progress.Text = " "
        Button_Install.Text = "Install"
        If My.Settings.backup_save Then
            backup_menu.Backupsave()
        End If
        If My.Settings.rebuild_def Then
            RebuildDef()
        End If
        If e.OperationException IsNot Nothing Then
            MessageBox.Show(String.Format("An error occurred:{0}{0}{1}",
                                          vbCrLf, e.OperationException.Message),
                                          "Exception Thrown",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning)
        ElseIf e.UserCancelledOperation Then
            MessageBox.Show("Uninstall canceled partway through!")
        Else
            If followup_install = "" Then
                If followup_uninstall = "" Then
                    MessageBox.Show("Uninstall complete.")
                    My.Settings.current_package = pending_install
                    Label_Current.Text = "Current: " & My.Settings.current_package
                Else
                    My.Settings.current_package = pending_install
                    Label_Current.Text = "Current: " & My.Settings.current_package
                    Button_Install.Text = "Cancel"
                    pending_install = followup_uninstall
                    Uninstaller.BeginOperation(Path.GetDirectoryName(Application.ExecutablePath) & "\" & get_package(My.Settings.current_package).folder_name,
                           Path.GetDirectoryName(Application.ExecutablePath) & "\" & get_package(get_package(My.Settings.current_package).base_package).folder_name,
                          Path.GetDirectoryName(My.Settings.exe_location),
                         FileAndDirectoryCopy.Copy.DirectoryParsing.IncludeSubfolders)
                    followup_uninstall = ""
                End If
            Else
                My.Settings.current_package = pending_install
                Label_Current.Text = "Current: " & My.Settings.current_package
                Button_Install.Text = "Cancel"
                pending_install = followup_install
                copier = New FileAndDirectoryCopy.Copy
                copier.BeginOperation(Path.GetDirectoryName(
                                   Application.ExecutablePath) & "\" & get_package(followup_install).folder_name,
                                      Path.GetDirectoryName(My.Settings.exe_location),
                                     FileAndDirectoryCopy.Copy.DirectoryParsing.IncludeSubfolders)
                followup_install = ""
            End If
        End If
    End Sub

#End Region
End Class