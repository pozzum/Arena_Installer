<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class backup_menu
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
        Me.Button_backup_current = New System.Windows.Forms.Button()
        Me.Button_Restore = New System.Windows.Forms.Button()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Label_Progress = New System.Windows.Forms.Label()
        Me.CheckBox_Backup = New System.Windows.Forms.CheckBox()
        Me.Check_rebuild = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'Button_backup_current
        '
        Me.Button_backup_current.Location = New System.Drawing.Point(12, 12)
        Me.Button_backup_current.Name = "Button_backup_current"
        Me.Button_backup_current.Size = New System.Drawing.Size(145, 23)
        Me.Button_backup_current.TabIndex = 0
        Me.Button_backup_current.Text = "Backup Game"
        Me.Button_backup_current.UseVisualStyleBackColor = True
        '
        'Button_Restore
        '
        Me.Button_Restore.Location = New System.Drawing.Point(163, 12)
        Me.Button_Restore.Name = "Button_Restore"
        Me.Button_Restore.Size = New System.Drawing.Size(150, 23)
        Me.Button_Restore.TabIndex = 1
        Me.Button_Restore.Text = "Restore Backup"
        Me.Button_Restore.UseVisualStyleBackColor = True
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(12, 41)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(301, 23)
        Me.ProgressBar1.TabIndex = 2
        '
        'Label_Progress
        '
        Me.Label_Progress.AutoSize = True
        Me.Label_Progress.Location = New System.Drawing.Point(12, 67)
        Me.Label_Progress.Name = "Label_Progress"
        Me.Label_Progress.Size = New System.Drawing.Size(10, 13)
        Me.Label_Progress.TabIndex = 3
        Me.Label_Progress.Text = " "
        '
        'CheckBox_Backup
        '
        Me.CheckBox_Backup.AutoSize = True
        Me.CheckBox_Backup.Location = New System.Drawing.Point(12, 70)
        Me.CheckBox_Backup.Name = "CheckBox_Backup"
        Me.CheckBox_Backup.Size = New System.Drawing.Size(91, 17)
        Me.CheckBox_Backup.TabIndex = 4
        Me.CheckBox_Backup.Text = "Backup Save"
        Me.CheckBox_Backup.UseVisualStyleBackColor = True
        '
        'Check_rebuild
        '
        Me.Check_rebuild.AutoSize = True
        Me.Check_rebuild.Location = New System.Drawing.Point(206, 70)
        Me.Check_rebuild.Name = "Check_rebuild"
        Me.Check_rebuild.Size = New System.Drawing.Size(107, 17)
        Me.Check_rebuild.TabIndex = 5
        Me.Check_rebuild.Text = "Auto Rebuild Def"
        Me.Check_rebuild.UseVisualStyleBackColor = True
        '
        'backup_menu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(319, 111)
        Me.Controls.Add(Me.Check_rebuild)
        Me.Controls.Add(Me.CheckBox_Backup)
        Me.Controls.Add(Me.Label_Progress)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.Button_Restore)
        Me.Controls.Add(Me.Button_backup_current)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "backup_menu"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Backup Menu"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button_backup_current As Button
    Friend WithEvents Button_Restore As Button
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents Label_Progress As Label
    Friend WithEvents CheckBox_Backup As CheckBox
    Friend WithEvents Check_rebuild As CheckBox
End Class
