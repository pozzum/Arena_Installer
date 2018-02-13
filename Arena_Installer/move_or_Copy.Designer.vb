<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class move_or_Copy
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
        Me.Text_Existing = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Text_Final = New System.Windows.Forms.TextBox()
        Me.Button_Move = New System.Windows.Forms.Button()
        Me.Button_Copy = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(206, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Would you like to move or copy the folder:"
        '
        'Text_Existing
        '
        Me.Text_Existing.Location = New System.Drawing.Point(12, 25)
        Me.Text_Existing.Name = "Text_Existing"
        Me.Text_Existing.ReadOnly = True
        Me.Text_Existing.Size = New System.Drawing.Size(206, 20)
        Me.Text_Existing.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 48)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(124, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "The new location will be:"
        '
        'Text_Final
        '
        Me.Text_Final.Location = New System.Drawing.Point(12, 64)
        Me.Text_Final.Name = "Text_Final"
        Me.Text_Final.ReadOnly = True
        Me.Text_Final.Size = New System.Drawing.Size(206, 20)
        Me.Text_Final.TabIndex = 3
        '
        'Button_Move
        '
        Me.Button_Move.Location = New System.Drawing.Point(12, 90)
        Me.Button_Move.Name = "Button_Move"
        Me.Button_Move.Size = New System.Drawing.Size(100, 23)
        Me.Button_Move.TabIndex = 4
        Me.Button_Move.Text = "Move"
        Me.Button_Move.UseVisualStyleBackColor = True
        '
        'Button_Copy
        '
        Me.Button_Copy.Location = New System.Drawing.Point(118, 90)
        Me.Button_Copy.Name = "Button_Copy"
        Me.Button_Copy.Size = New System.Drawing.Size(100, 23)
        Me.Button_Copy.TabIndex = 5
        Me.Button_Copy.Text = "Copy"
        Me.Button_Copy.UseVisualStyleBackColor = True
        '
        'move_or_Copy
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(225, 121)
        Me.Controls.Add(Me.Button_Copy)
        Me.Controls.Add(Me.Button_Move)
        Me.Controls.Add(Me.Text_Final)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Text_Existing)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "move_or_Copy"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Move or Copy?"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Text_Existing As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Text_Final As TextBox
    Friend WithEvents Button_Move As Button
    Friend WithEvents Button_Copy As Button
End Class
