<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class main_menu
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Button_Backup = New System.Windows.Forms.Button()
        Me.Button_Install = New System.Windows.Forms.Button()
        Me.Open_File_Exe = New System.Windows.Forms.OpenFileDialog()
        Me.Open_File_Save = New System.Windows.Forms.OpenFileDialog()
        Me.SuspendLayout()
        '
        'Button_Backup
        '
        Me.Button_Backup.Location = New System.Drawing.Point(12, 12)
        Me.Button_Backup.Name = "Button_Backup"
        Me.Button_Backup.Size = New System.Drawing.Size(75, 23)
        Me.Button_Backup.TabIndex = 0
        Me.Button_Backup.Text = "Backup"
        Me.Button_Backup.UseVisualStyleBackColor = True
        '
        'Button_Install
        '
        Me.Button_Install.Location = New System.Drawing.Point(93, 12)
        Me.Button_Install.Name = "Button_Install"
        Me.Button_Install.Size = New System.Drawing.Size(75, 23)
        Me.Button_Install.TabIndex = 1
        Me.Button_Install.Text = "Install Pack"
        Me.Button_Install.UseVisualStyleBackColor = True
        '
        'Open_File_Exe
        '
        Me.Open_File_Exe.FileName = "WWE2K18_x64.exe"
        Me.Open_File_Exe.Filter = "WWE EXE File|WWE*.exe"
        Me.Open_File_Exe.InitialDirectory = "C:\Steam\steamapps\common\"
        Me.Open_File_Exe.Title = "Select WWE 2K18 EXE"
        '
        'Open_File_Save
        '
        Me.Open_File_Save.FileName = "remotecache.vdf"
        Me.Open_File_Save.Filter = "remotecache.vdf|remotecache.vdf"
        Me.Open_File_Save.InitialDirectory = "C:\Steam\userdata\"
        Me.Open_File_Save.Title = "Select Save 664430\remotecache.vdf"
        '
        'main_menu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(177, 47)
        Me.Controls.Add(Me.Button_Install)
        Me.Controls.Add(Me.Button_Backup)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "main_menu"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Main Menu"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Button_Backup As Button
    Friend WithEvents Button_Install As Button
    Friend WithEvents Open_File_Exe As OpenFileDialog
    Friend WithEvents Open_File_Save As OpenFileDialog
End Class
