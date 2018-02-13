<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class add_package
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Text_menu_name = New System.Windows.Forms.TextBox()
        Me.Button_pick_folder = New System.Windows.Forms.Button()
        Me.Text_Folder = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ComboBoxPackage = New System.Windows.Forms.ComboBox()
        Me.Button_Finalize = New System.Windows.Forms.Button()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Label_Progress = New System.Windows.Forms.Label()
        Me.Open_package = New System.Windows.Forms.OpenFileDialog()
        Me.Button_delete = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(84, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Package Name:"
        '
        'Text_menu_name
        '
        Me.Text_menu_name.Location = New System.Drawing.Point(12, 25)
        Me.Text_menu_name.Name = "Text_menu_name"
        Me.Text_menu_name.Size = New System.Drawing.Size(260, 20)
        Me.Text_menu_name.TabIndex = 1
        '
        'Button_pick_folder
        '
        Me.Button_pick_folder.Location = New System.Drawing.Point(12, 51)
        Me.Button_pick_folder.Name = "Button_pick_folder"
        Me.Button_pick_folder.Size = New System.Drawing.Size(75, 23)
        Me.Button_pick_folder.TabIndex = 2
        Me.Button_pick_folder.Text = "Pick Folder"
        Me.Button_pick_folder.UseVisualStyleBackColor = True
        '
        'Text_Folder
        '
        Me.Text_Folder.Location = New System.Drawing.Point(12, 80)
        Me.Text_Folder.Name = "Text_Folder"
        Me.Text_Folder.ReadOnly = True
        Me.Text_Folder.Size = New System.Drawing.Size(260, 20)
        Me.Text_Folder.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 103)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(77, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Base Package"
        '
        'ComboBoxPackage
        '
        Me.ComboBoxPackage.FormattingEnabled = True
        Me.ComboBoxPackage.Location = New System.Drawing.Point(12, 119)
        Me.ComboBoxPackage.Name = "ComboBoxPackage"
        Me.ComboBoxPackage.Size = New System.Drawing.Size(260, 21)
        Me.ComboBoxPackage.TabIndex = 5
        '
        'Button_Finalize
        '
        Me.Button_Finalize.Enabled = False
        Me.Button_Finalize.Location = New System.Drawing.Point(197, 51)
        Me.Button_Finalize.Name = "Button_Finalize"
        Me.Button_Finalize.Size = New System.Drawing.Size(75, 23)
        Me.Button_Finalize.TabIndex = 6
        Me.Button_Finalize.Text = "Finalize"
        Me.Button_Finalize.UseVisualStyleBackColor = True
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(12, 146)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(260, 23)
        Me.ProgressBar1.TabIndex = 7
        '
        'Label_Progress
        '
        Me.Label_Progress.AutoSize = True
        Me.Label_Progress.Location = New System.Drawing.Point(12, 172)
        Me.Label_Progress.Name = "Label_Progress"
        Me.Label_Progress.Size = New System.Drawing.Size(10, 13)
        Me.Label_Progress.TabIndex = 8
        Me.Label_Progress.Text = " "
        '
        'Open_package
        '
        Me.Open_package.CheckFileExists = False
        Me.Open_package.FileName = "Select Folder Pac Folder"
        '
        'Button_delete
        '
        Me.Button_delete.Location = New System.Drawing.Point(93, 51)
        Me.Button_delete.Name = "Button_delete"
        Me.Button_delete.Size = New System.Drawing.Size(98, 23)
        Me.Button_delete.TabIndex = 9
        Me.Button_delete.Text = "Delete Base"
        Me.Button_delete.UseVisualStyleBackColor = True
        '
        'add_package
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 223)
        Me.Controls.Add(Me.Button_delete)
        Me.Controls.Add(Me.Label_Progress)
        Me.Controls.Add(Me.Button_Finalize)
        Me.Controls.Add(Me.ComboBoxPackage)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Text_Folder)
        Me.Controls.Add(Me.Button_pick_folder)
        Me.Controls.Add(Me.Text_menu_name)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ProgressBar1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "add_package"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Install Package"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Text_menu_name As TextBox
    Friend WithEvents Button_pick_folder As Button
    Friend WithEvents Text_Folder As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents ComboBoxPackage As ComboBox
    Friend WithEvents Button_Finalize As Button
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents Label_Progress As Label
    Friend WithEvents Open_package As OpenFileDialog
    Friend WithEvents Button_delete As Button
End Class
