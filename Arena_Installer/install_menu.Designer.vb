<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class install_menu
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
        Me.ComboBoxPackage = New System.Windows.Forms.ComboBox()
        Me.Button_Install = New System.Windows.Forms.Button()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Button_Add = New System.Windows.Forms.Button()
        Me.Label_Progress = New System.Windows.Forms.Label()
        Me.Label_Current = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(83, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Select Package"
        '
        'ComboBoxPackage
        '
        Me.ComboBoxPackage.FormattingEnabled = True
        Me.ComboBoxPackage.Location = New System.Drawing.Point(12, 25)
        Me.ComboBoxPackage.Name = "ComboBoxPackage"
        Me.ComboBoxPackage.Size = New System.Drawing.Size(121, 21)
        Me.ComboBoxPackage.TabIndex = 1
        '
        'Button_Install
        '
        Me.Button_Install.Location = New System.Drawing.Point(139, 23)
        Me.Button_Install.Name = "Button_Install"
        Me.Button_Install.Size = New System.Drawing.Size(50, 23)
        Me.Button_Install.TabIndex = 2
        Me.Button_Install.Text = "Install"
        Me.Button_Install.UseVisualStyleBackColor = True
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(12, 52)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(233, 23)
        Me.ProgressBar1.TabIndex = 3
        '
        'Button_Add
        '
        Me.Button_Add.Location = New System.Drawing.Point(195, 23)
        Me.Button_Add.Name = "Button_Add"
        Me.Button_Add.Size = New System.Drawing.Size(50, 23)
        Me.Button_Add.TabIndex = 4
        Me.Button_Add.Text = "Add"
        Me.Button_Add.UseVisualStyleBackColor = True
        '
        'Label_Progress
        '
        Me.Label_Progress.AutoSize = True
        Me.Label_Progress.Location = New System.Drawing.Point(12, 78)
        Me.Label_Progress.Name = "Label_Progress"
        Me.Label_Progress.Size = New System.Drawing.Size(10, 13)
        Me.Label_Progress.TabIndex = 5
        Me.Label_Progress.Text = " "
        '
        'Label_Current
        '
        Me.Label_Current.AutoSize = True
        Me.Label_Current.Location = New System.Drawing.Point(101, 9)
        Me.Label_Current.Name = "Label_Current"
        Me.Label_Current.Size = New System.Drawing.Size(44, 13)
        Me.Label_Current.TabIndex = 6
        Me.Label_Current.Text = "Current:"
        '
        'install_menu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(254, 131)
        Me.Controls.Add(Me.Label_Current)
        Me.Controls.Add(Me.Label_Progress)
        Me.Controls.Add(Me.Button_Add)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.Button_Install)
        Me.Controls.Add(Me.ComboBoxPackage)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "install_menu"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Install Menu"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents ComboBoxPackage As ComboBox
    Friend WithEvents Button_Install As Button
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents Button_Add As Button
    Friend WithEvents Label_Progress As Label
    Friend WithEvents Label_Current As Label
End Class
