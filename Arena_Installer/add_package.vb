Imports System.IO
Public Class add_package
    Public existing_package_location As String
    Public final_package_location As String
    Public move_copy_choice As String
    Private WithEvents copier As FileAndDirectoryCopy.Copy
    Private WithEvents mover As FileAndDirectoryMove.Move
    Dim package_to_add As package_info
    Private Sub add_package_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        load_packages()
    End Sub
    Sub load_packages()
        ComboBoxPackage.Items.Clear()

        For Each temp_package As package_info In install_menu.package_list
            ComboBoxPackage.Items.Add(temp_package.menu_name)
        Next
        ComboBoxPackage.SelectedIndex = 0
    End Sub

    Private Sub Button_pick_folder_Click(sender As Object, e As EventArgs) Handles Button_pick_folder.Click
        Text_Folder.Text = ""
        If Open_package.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            If Not Directory.Exists(Path.GetDirectoryName(Open_package.FileName) & "\pac") Then
                MessageBox.Show("Pac folder not found!", "oh no")
                Exit Sub
            Else
                existing_package_location = Path.GetDirectoryName(Open_package.FileName)
                final_package_location = Path.GetDirectoryName(Application.ExecutablePath) &
                    Path.GetDirectoryName(Open_package.FileName).Replace(Directory.GetParent(Path.GetDirectoryName(Open_package.FileName)).ToString, "")
                If final_package_location = existing_package_location Then
                    move_copy_choice = "Exist"
                Else
                    move_or_Copy.ShowDialog()
                End If
            End If
            Text_Folder.Text = final_package_location
        End If
    End Sub

    Private Sub Text_Folder_TextChanged(sender As Object, e As EventArgs) Handles Text_Folder.TextChanged
        If Not Text_menu_name.Text = "" Then
            Button_Finalize.Enabled = True
        Else
            Button_Finalize.Enabled = False
        End If
    End Sub

    Private Sub Text_menu_name_TextChanged(sender As Object, e As EventArgs) Handles Text_menu_name.TextChanged
        If Not Text_Folder.Text = "" Then
            Button_Finalize.Enabled = True
        Else
            Button_Finalize.Enabled = False
        End If
    End Sub

    Private Sub Button_Finalize_Click(sender As Object, e As EventArgs) Handles Button_Finalize.Click
        If Button_Finalize.Text = "Finalize" Then
            package_to_add = New package_info
            package_to_add.menu_name = Text_menu_name.Text
            package_to_add.folder_name = Text_Folder.Text.Replace(Path.GetDirectoryName(Application.ExecutablePath) & "\", "")
            package_to_add.base_package = ComboBoxPackage.Text
            For Each exist_package As package_info In install_menu.package_list
                If exist_package.menu_name = package_to_add.menu_name Then
                    MessageBox.Show("Menu name must be unique.")
                    Exit Sub
                ElseIf exist_package.folder_name = package_to_add.folder_name Then
                    MessageBox.Show("Folder name must be unique.")
                    Exit Sub
                End If
            Next
            If Not Directory.Exists(final_package_location) Then
                Directory.CreateDirectory(final_package_location)
            End If
            Button_Finalize.Text = "Cancel"
            If move_copy_choice = "Copy" Then
                copier = New FileAndDirectoryCopy.Copy
                copier.BeginOperation(existing_package_location, final_package_location,
                                     FileAndDirectoryCopy.Copy.DirectoryParsing.IncludeSubfolders)
            ElseIf move_copy_choice = "Move" Then
                mover = New FileAndDirectoryMove.Move
                mover.BeginOperation(existing_package_location, final_package_location,
                                     FileAndDirectoryMove.Move.DirectoryParsing.IncludeSubfolders)
            ElseIf move_copy_choice = "Exist" Then
                ProgressBar1.Value = 0
                Label_Progress.Text = " "
                Button_Finalize.Text = "Finalize"
                add_to_list()
                'no copy needed
            End If
        ElseIf Button_Finalize.Text = "Cancel" Then
            Button_Finalize.Text = "Finalize"
            copier.CancelOperation()
            mover.CancelOperation()
        End If
    End Sub
    Private Sub Button_delete_Click(sender As Object, e As EventArgs) Handles Button_delete.Click
        If ComboBoxPackage.Text = "Default" Then
            MessageBox.Show("Default package not to be deleted!")
            Exit Sub
        End If
        For Each tested_package As package_info In install_menu.package_list
            If tested_package.base_package = ComboBoxPackage.Text Then
                MessageBox.Show("Package cannot be deleted, it is used as a base for package " & tested_package.menu_name & vbNewLine &
                                tested_package.menu_name & " must be deleted first.")
            End If
        Next
        If MessageBox.Show("Delete package " & ComboBoxPackage.Text & "?", "Remove Package", MessageBoxButtons.OKCancel) = DialogResult.OK Then
            install_menu.package_list.RemoveAt(get_index(ComboBoxPackage.Text))
            install_menu.save_packages()
            load_packages()
            MessageBox.Show("Files are not deleted.")
        End If
    End Sub
    Function get_index(package_menu_name) As Integer
        For i As Integer = 0 To install_menu.package_list.Count - 1
            If install_menu.package_list(i).menu_name = package_menu_name Then
                Return i
            End If
        Next
        Return Nothing
    End Function
    Sub add_to_list()
        install_menu.package_list.Add(package_to_add)
        install_menu.save_packages()
        Text_menu_name.Text = ""
        Text_Folder.Text = ""
        load_packages()
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
        Button_Finalize.Text = "Finalize"
        If e.OperationException IsNot Nothing Then
            MessageBox.Show(String.Format("An error occurred:{0}{0}{1}",
                                          vbCrLf, e.OperationException.Message),
                                          "Exception Thrown",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning)
        ElseIf e.UserCancelledOperation Then
            MessageBox.Show("Copy canceled partway through!")
        Else
            MessageBox.Show("Copy complete.")
            add_to_list()
        End If
    End Sub
    Private Sub mover_ProgressChanged(sender As Object, e As FileAndDirectoryMove.Move.ProgressChangedEventArgs) Handles mover.ProgressChanged

        If e.UserCancelledOperation Then
            Label_Progress.Text = "Cancelling Operation..."
        Else
            Label_Progress.Text = e.CurrentActivity_String

            Select Case e.CurrentActivity
                Case FileAndDirectoryCopy.Copy.OperationType.Copying
                    Label_Progress.Text =
                        String.Format("Moved {0} of {1} Files ({2} of {3}){4}{5} Completed",
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
    Private Sub mover_OperationComplete(sender As Object, e As FileAndDirectoryMove.Move.OperationCompleteEventArgs) Handles mover.OperationComplete
        ProgressBar1.Value = 0
        Label_Progress.Text = " "
        Button_Finalize.Text = "Finalize"
        If e.OperationException IsNot Nothing Then
            MessageBox.Show(String.Format("An error occurred:{0}{0}{1}",
                                          vbCrLf, e.OperationException.Message),
                                          "Exception Thrown",
                                          MessageBoxButtons.OK,
                                          MessageBoxIcon.Warning)
        ElseIf e.UserCancelledOperation Then
            MessageBox.Show("Move canceled partway through!")
        Else
            MessageBox.Show("Move complete.")
            add_to_list()
        End If
    End Sub


#End Region
End Class