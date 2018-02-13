Public Class move_or_Copy
    Private Sub move_or_Copy_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text_Existing.Text = add_package.existing_package_location
        Text_Final.Text = add_package.final_package_location
    End Sub

    Private Sub Button_Move_Click(sender As Object, e As EventArgs) Handles Button_Move.Click
        add_package.move_copy_choice = "Move"
        Me.Close()
    End Sub

    Private Sub Button_Copy_Click(sender As Object, e As EventArgs) Handles Button_Copy.Click
        add_package.move_copy_choice = "Copy"
        Me.Close()
    End Sub
End Class