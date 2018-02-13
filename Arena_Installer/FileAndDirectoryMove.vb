'https://social.msdn.microsoft.com/Forums/vstudio/en-US/1fa8dd55-5007-4edf-b70f-65bc073b6c01/copy-a-directory-with-a-progress-bar?forum=vbgeneral
'http://www.fls-online.com/VBNet_Forum/09-26-15/FileAndDirectoryCopy.htm
Option Strict On
Option Explicit On
Option Infer Off

Imports System.IO
Imports System.IO.Path
Imports System.Windows.Forms
'Creating a copy that has a loading bar for moving instead of just moving.
Namespace FileAndDirectoryMove
    Public Class Move
        'pozzum need to be able to pass original source folder
        Public OriginalSource As String
        Public Enum DirectoryParsing
            IncludeSubfolders
            TopLevelOnly
        End Enum

        Public Enum OperationType
            Idle
            EnumeratingDirectories
            EnumeratingFiles
            Moving
        End Enum

        Public ReadOnly Property IsBusy As Boolean
            Get
                If bgw Is Nothing Then
                    Return False
                Else
                    Return bgw.IsBusy
                End If
            End Get
        End Property

        Private Class BGW_Instructions
            Public SourceDirectoryInfoList As List(Of DirectoryInfo)
            Public TargetBaseDirectory As DirectoryInfo
            Public ParsingType As DirectoryParsing
        End Class

        Public Sub BeginOperation(ByVal sourceDirectoryPath As String,
                                  ByVal targetBaseDirectoryPath As String,
                                  ByVal parsingInstructions As DirectoryParsing)
            'Pozzum Adding reference to copy into background worker later
            OriginalSource = sourceDirectoryPath
            Try
                If String.IsNullOrEmpty(sourceDirectoryPath) OrElse sourceDirectoryPath.Trim = "" Then
                    Throw New ArgumentException("The directory path to move from cannot be null or empty.")

                ElseIf String.IsNullOrEmpty(targetBaseDirectoryPath) OrElse targetBaseDirectoryPath.Trim = "" Then
                    Throw New ArgumentException("The directory path to move to cannot be null or empty.")

                Else
                    Dim sourceDI As New DirectoryInfo(sourceDirectoryPath)
                    Dim targetDI As New DirectoryInfo(targetBaseDirectoryPath)

                    If Not sourceDI.Exists Then
                        Throw New DirectoryNotFoundException("The source directory path could not be located.")

                    ElseIf Not targetDI.Exists Then
                        Throw New DirectoryNotFoundException("The target directory path could not be located.")

                    Else
                        Dispose()

                        _operationException = Nothing
                        _userCancel = False
                        _totalDirectoryCount = 0
                        _totalFileCount = 0
                        _totalFilesSize = 0
                        _currentFileCount = 0
                        _cumulativeFileSize = 0

                        Dim instructions As New BGW_Instructions() With
                            {.SourceDirectoryInfoList = New List(Of DirectoryInfo)() From {sourceDI},
                             .TargetBaseDirectory = targetDI,
                             .ParsingType = parsingInstructions}

                        tmr = New System.Windows.Forms.Timer() With
                              {.Interval = 500,
                               .Enabled = True}

                        _operationSW = New Stopwatch

                        bgw = New System.ComponentModel.BackgroundWorker() With
                              {.WorkerSupportsCancellation = True}

                        bgw.RunWorkerAsync(instructions)
                    End If
                End If

            Catch ex As Exception

            End Try

        End Sub
        'pozzum overload to allow for lists of strings
        Public Sub BeginOperation(ByVal sourceDirectoryPath As List(Of String),
                                  ByVal targetBaseDirectoryPath As String,
                                  ByVal parsingInstructions As DirectoryParsing)
            'Pozzum Adding reference to copy into background worker later
            OriginalSource = sourceDirectoryPath(0)
            Try
                For i As Integer = 1 To sourceDirectoryPath.Count - 1
                    If String.IsNullOrEmpty(sourceDirectoryPath(i)) OrElse sourceDirectoryPath(i).Trim = "" Then
                        Throw New ArgumentException("The directory path to move from cannot be null or empty.")
                    End If
                Next
                If String.IsNullOrEmpty(targetBaseDirectoryPath) OrElse targetBaseDirectoryPath.Trim = "" Then
                    Throw New ArgumentException("The directory path to move to cannot be null or empty.")

                Else
                    Dim sourceDI As New List(Of DirectoryInfo)
                    For i As Integer = 1 To sourceDirectoryPath.Count - 1
                        sourceDI.Add(New DirectoryInfo(sourceDirectoryPath(i)))
                        If Not sourceDI(i - 1).Exists Then
                            MessageBox.Show(sourceDI(i - 1).ToString)
                            Throw New DirectoryNotFoundException("The source directory path could not be located.")
                        End If
                    Next
                    '(sourceDirectoryPath)
                    Dim targetDI As New DirectoryInfo(targetBaseDirectoryPath)

                    If Not targetDI.Exists Then
                        Throw New DirectoryNotFoundException("The target directory path could not be located.")

                    Else
                        Dispose()

                        _operationException = Nothing
                        _userCancel = False
                        _totalDirectoryCount = 0
                        _totalFileCount = 0
                        _totalFilesSize = 0
                        _currentFileCount = 0
                        _cumulativeFileSize = 0

                        Dim instructions As New BGW_Instructions() With
                            {.SourceDirectoryInfoList = sourceDI,
                             .TargetBaseDirectory = targetDI,
                             .ParsingType = parsingInstructions}

                        tmr = New System.Windows.Forms.Timer() With
                              {.Interval = 500,
                               .Enabled = True}

                        _operationSW = New Stopwatch

                        bgw = New System.ComponentModel.BackgroundWorker() With
                              {.WorkerSupportsCancellation = True}

                        bgw.RunWorkerAsync(instructions)
                    End If
                End If

            Catch ex As Exception

            End Try

        End Sub
        Public Sub CancelOperation()

            _userCancel = True
            OnProgressChanged()

        End Sub

        Private WithEvents bgw As New System.ComponentModel.BackgroundWorker
        Private WithEvents tmr As New System.Windows.Forms.Timer

        Private Shared _currentOperation As OperationType = OperationType.Idle
        Private Shared _operationSW As Stopwatch
        Private Shared _operationException As Exception
        Private Shared _userCancel As Boolean
        Private Shared _totalFileCount As Integer
        Private Shared _totalFilesSize As Long
        Private Shared _currentFileCount As Integer
        Private Shared _cumulativeFileSize As Long
        Private Shared _totalDirectoryCount As Integer

        Private Sub bgw_DoWork(sender As Object,
                               e As System.ComponentModel.DoWorkEventArgs) _
                               Handles bgw.DoWork

            Try
                If e.Argument IsNot Nothing Then
                    Dim instructions As BGW_Instructions = DirectCast(e.Argument, BGW_Instructions)

                    If instructions IsNot Nothing Then
                        _operationSW.Start()

                        Dim fileInfoList As New List(Of FileInfo)
                        Dim safeDirectoryPaths As New List(Of String)

                        _currentOperation = OperationType.EnumeratingDirectories

                        For Each di As DirectoryInfo In instructions.SourceDirectoryInfoList
                            If _userCancel Then
                                bgw.CancelAsync()
                            Else
                                safeDirectoryPaths.Add(di.FullName)

                                If instructions.ParsingType = DirectoryParsing.IncludeSubfolders Then
                                    Dim list As List(Of String) = GenerateSafeFolderList(di.FullName)

                                    If list IsNot Nothing Then
                                        For Each dirPath As String In list
                                            safeDirectoryPaths.Add(dirPath)
                                        Next
                                    End If

                                    safeDirectoryPaths.Sort()
                                End If
                            End If
                        Next

                        _totalDirectoryCount = safeDirectoryPaths.Count
                        _currentOperation = OperationType.EnumeratingFiles

                        For Each dirPath As String In safeDirectoryPaths
                            If _userCancel Then
                                bgw.CancelAsync()
                            Else
                                Dim files As String() = Directory.GetFiles(dirPath)

                                If files IsNot Nothing AndAlso files.Length > 0 Then
                                    For Each filePath As String In files
                                        If _userCancel Then
                                            bgw.CancelAsync()
                                        Else
                                            Dim fi As New FileInfo(filePath)

                                            If fi.Exists Then
                                                fileInfoList.Add(fi)
                                                _totalFileCount += 1
                                                _totalFilesSize += fi.Length
                                            End If
                                        End If
                                    Next
                                End If
                            End If
                        Next

                        _currentOperation = OperationType.Moving

                        For Each fi As FileInfo In fileInfoList
                            If _userCancel Then
                                bgw.CancelAsync()
                            Else
                                'Pozzum - I want this to not move all of the folders just the top level folder name.
                                'Dim sourceRoot As String = Directory.GetParent(OriginalSource).ToString
                                Dim targetPath As String = instructions.TargetBaseDirectory.FullName & "\" &
                                    fi.FullName.Replace(OriginalSource, "")

                                Dim targetDirectoryPath As String = GetDirectoryName(targetPath)

                                If Not Directory.Exists(targetDirectoryPath) Then
                                    Directory.CreateDirectory(targetDirectoryPath)

                                    _currentFileCount += 1
                                    _cumulativeFileSize += fi.Length

                                    File.Move(fi.FullName, targetPath)
                                Else
                                    Dim currentFI As New FileInfo(targetPath)

                                    If currentFI.Exists Then
                                        If currentFI.Length <> fi.Length OrElse currentFI.LastWriteTimeUtc <> fi.LastWriteTimeUtc Then
                                            File.Move(fi.FullName, targetPath)
                                        End If

                                        _currentFileCount += 1
                                        _cumulativeFileSize += fi.Length
                                    Else
                                        _currentFileCount += 1
                                        _cumulativeFileSize += fi.Length

                                        File.Move(fi.FullName, targetPath)
                                    End If
                                End If
                            End If
                        Next
                        'Pozzum having to clean up the remaining folders as moving doesn't delete the folders.
                        _totalFileCount = 0
                        _totalFilesSize = 0
                        For Each dirPath As String In safeDirectoryPaths
                            If _userCancel Then
                                bgw.CancelAsync()
                            Else
                                Dim files As String() = Directory.GetFiles(dirPath)

                                If files IsNot Nothing AndAlso files.Length > 0 Then
                                    For Each filePath As String In files
                                        If _userCancel Then
                                            bgw.CancelAsync()
                                        Else
                                            Dim fi As New FileInfo(filePath)

                                            If fi.Exists Then
                                                fileInfoList.Add(fi)
                                                _totalFileCount += 1
                                                _totalFilesSize += fi.Length
                                            End If
                                        End If
                                    Next
                                End If
                            End If
                        Next
                        If _totalFilesSize = 0 Then
                            For i As Integer = 0 To 10
                                For Each dirPath As String In safeDirectoryPaths
                                    If _userCancel Then
                                        bgw.CancelAsync()
                                    Else
                                        If Directory.Exists(dirPath) Then
                                            Dim directories As String() = Directory.GetDirectories(dirPath)

                                            If directories Is Nothing OrElse directories.Length = 0 Then
                                                Directory.Delete(dirPath)
                                            End If
                                        End If
                                    End If
                                Next
                            Next
                            'Directory.Delete(OriginalSource)
                        Else
                            MessageBox.Show("Folders not deleted.")
                        End If
                    End If
                End If

            Catch ex As Exception
                _operationException = ex
                bgw.CancelAsync()
            End Try

        End Sub

        Private Sub bgw_RunWorkerCompleted(sender As Object,
                                           e As System.ComponentModel.RunWorkerCompletedEventArgs) _
                                           Handles bgw.RunWorkerCompleted

            _operationSW.Stop()
            tmr.Enabled = False
            OnCompleted()

        End Sub

        Private Sub tmr_Tick(sender As Object,
                             e As System.EventArgs) _
                             Handles tmr.Tick

            OnProgressChanged()

        End Sub

        ''' <summary>
        ''' An event indicating that a change in the progress of the 
        ''' operation has occurred.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs)

        ''' <summary>
        ''' An event indicating that the operation has completed. 
        ''' Note that if an exception is thrown or if the user cancels 
        ''' the operation, this event is also raised.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Event OperationComplete(ByVal sender As Object, ByVal e As OperationCompleteEventArgs)

        ''' <summary>
        ''' An overridable method used to raise the ProgressChanged event.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overridable Sub OnProgressChanged()
            RaiseEvent ProgressChanged(Me, New ProgressChangedEventArgs)
        End Sub

        ''' <summary>
        ''' An overridable method used to raise the OperationComplete event.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overridable Sub OnCompleted()
            RaiseEvent OperationComplete(Me, New OperationCompleteEventArgs)
        End Sub

        Private Function GenerateSafeFolderList(ByVal folder As String) _
        As List(Of String)

            ' -------------------------------------------
            ' Based On A Function By John Wein As Posted:
            '
            ' http://social.msdn.microsoft.com/Forums/en-US/vbgeneral/thread/d6e64558-395b-4b48-8b64-0f5a7e3a7623
            '
            ' Thanks John!
            ' -------------------------------------------

            Dim retVal As New List(Of String)

            Dim Dirs As New Stack(Of String)
            Dirs.Push(folder)

            While Dirs.Count > 0
                If _userCancel Then
                    Exit While
                Else
                    Dim Dir As String = Dirs.Pop

                    Try
                        For Each D As String In System.IO.Directory.GetDirectories(Dir)
                            ' Do not include any that are either system or hidden

                            Dim dirInfo As New System.IO.DirectoryInfo(D)
                            If (((dirInfo.Attributes And System.IO.FileAttributes.Hidden) = 0) AndAlso
                                ((dirInfo.Attributes And System.IO.FileAttributes.System) = 0)) Then

                                If Not retVal.Contains(D) Then
                                    retVal.Add(D)
                                End If
                            End If

                            Dirs.Push(D)
                        Next

                    Catch ex As Exception

                        If retVal.Contains(Dir) Then
                            Dim indexToRemove As Integer = 0

                            For i As Integer = 0 To retVal.Count - 1
                                If retVal(i) = Dir Then
                                    indexToRemove = i
                                    Exit For
                                End If
                            Next

                            retVal.RemoveAt(indexToRemove)
                        End If
                        Continue While
                    End Try
                End If
            End While

            Return retVal

        End Function

        Private Delegate Function GenerateSafeFolderListDelegate(ByVal folder As String) _
            As List(Of String)





        Public Class ProgressChangedEventArgs
            Inherits EventArgs

            ''' <summary>
            ''' Gets the value of the current activity as a member of the 
            ''' Activity enumerations.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property CurrentActivity As OperationType
                Get
                    Return _currentOperation
                End Get
            End Property

            ''' <summary>
            ''' Gets a formatted string indicating the current activity.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property CurrentActivity_String As String
                Get
                    Select Case _currentOperation
                        Case OperationType.Moving
                            Return "Moving Directories/Files"
                        Case OperationType.EnumeratingDirectories
                            Return "Enumerating Directories"
                        Case OperationType.EnumeratingFiles
                            Return "Enumerating Files"
                        Case Else
                            Return _currentOperation.ToString
                    End Select
                End Get
            End Property

            ''' <summary>
            ''' Gets the total number of files found in all directory paths 
            ''' based on your specifications for finding them.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property TotalFileCount As Integer
                Get
                    Return _totalFileCount
                End Get
            End Property

            ''' <summary>
            ''' Gets a formatted string showing the total number of files found 
            ''' in all directory paths based on your specifications for finding them.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property TotalFileCount_String(Optional ByVal includeTotalSize As Boolean = False) As String
                Get
                    If _totalFileCount = 0 Then
                        Return "Unknown"
                    Else
                        If includeTotalSize Then
                            Return String.Format("{0:n0} ({1})",
                                                 _totalFileCount,
                                                 Utilities.ShowTotalFileSize(_totalFilesSize))
                        Else
                            Return _totalFileCount.ToString("n0")
                        End If
                    End If
                End Get
            End Property

            Public ReadOnly Property TotalFilesSize As Long
                Get
                    Return _totalFilesSize
                End Get
            End Property

            Public ReadOnly Property TotalFilesSize_String As String
                Get
                    Return Utilities.ShowTotalFileSize(_totalFilesSize)
                End Get
            End Property

            Public ReadOnly Property CurrentFileCount As Integer
                Get
                    Return _currentFileCount
                End Get
            End Property

            Public ReadOnly Property CurrentFileCount_String As String
                Get
                    Return _currentFileCount.ToString("n0")
                End Get
            End Property

            Public ReadOnly Property CumulativeFileSize As Long
                Get
                    Return _cumulativeFileSize
                End Get
            End Property

            Public ReadOnly Property CumulativeFileSize_String As String
                Get
                    Return Utilities.ShowTotalFileSize(_cumulativeFileSize)
                End Get
            End Property

            ''' <summary>
            ''' Gets the total number of directories found in all directory paths 
            ''' based on your specifications for finding them.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property TotalDirectoryCount() As Integer
                Get
                    Return _totalDirectoryCount
                End Get
            End Property

            ''' <summary>
            ''' Gets a formatted string showing the total number of directories 
            ''' found in all directory paths based on your specifications for finding them.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property TotalDirectoryCount_String As String
                Get
                    If _totalDirectoryCount = 0 Then
                        Return "Unknown"
                    Else
                        Return _totalDirectoryCount.ToString("n0")
                    End If
                End Get
            End Property

            ''' <summary>
            ''' Gets the value of the current progress as a percentage.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property CurrentProgress As Double
                Get
                    If _totalFileCount = 0 Then
                        Return 0
                    Else
                        Return _currentFileCount / _totalFileCount
                    End If
                End Get
            End Property

            ''' <summary>
            ''' Gets the value of the current progress as a percentage, formatted for 
            ''' use with a ProgressBar control. Note that you need to set the ProgressBar's 
            ''' .Minimum to 0 and .Maximum to 100 in order to use this value directly.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property CurrentProgressForProgressBar As Integer
                Get
                    Return CInt(CurrentProgress * 100)
                End Get
            End Property

            ''' <summary>
            ''' Gets a formatted string showing the value of the current progress as a percentage.
            ''' </summary>
            ''' <param name="numberOfDecimalPlaces">OPTIONAL: The number of decimal places to show 
            ''' in the string returned. Default: 0.</param>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property CurrentProgress_String(Optional ByVal numberOfDecimalPlaces As Integer = 0) As String
                Get
                    If numberOfDecimalPlaces < 0 OrElse numberOfDecimalPlaces > 8 Then
                        numberOfDecimalPlaces = 0
                    End If

                    Dim formatter As String = "p" & numberOfDecimalPlaces.ToString

                    Return CurrentProgress.ToString(formatter)
                End Get
            End Property

            ''' <summary>
            ''' Gets a boolean value indicating whether or not the user has elected to cancel the operation.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property UserCancelledOperation() As Boolean
                Get
                    Return _userCancel
                End Get
            End Property

            ''' <summary>
            ''' Gets the amount of time which has elapsed since the operation began.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property Elapsed() As TimeSpan
                Get
                    Return _operationSW.Elapsed
                End Get
            End Property

            ''' <summary>
            ''' Gets a formatted string showing the amount of time which has elapsed 
            ''' since the operation began.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks>
            ''' <para>Note that this function is set up to return the string 
            ''' formatted as mm:ss if the value is less than one hour, or as hh:mm:ss 
            ''' if the value is an hour or greater.</para>
            ''' </remarks>
            Public ReadOnly Property Elapsed_String() As String
                Get
                    If _operationSW.Elapsed.TotalSeconds > 3600 Then
                        Return Utilities.ConvertSecsToString(Elapsed.TotalSeconds, False, True)
                    Else
                        Return Utilities.ConvertSecsToString(Elapsed.TotalSeconds, False, False)
                    End If
                End Get
            End Property
        End Class





        Public Class OperationCompleteEventArgs
            Inherits EventArgs

            ''' <summary>
            ''' Gets the exception which was thrown (if any) during the operation.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property OperationException As Exception
                Get
                    Return _operationException
                End Get
            End Property

            ''' <summary>
            ''' Gets a boolean value indicating whether or not the user cancelled the operation.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property UserCancelledOperation() As Boolean
                Get
                    Return _userCancel
                End Get
            End Property

            ''' <summary>
            ''' Gets the total number of files found in all directory paths 
            ''' based on your specifications for finding them.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property TotalFileCount As Integer
                Get
                    Return _totalFileCount
                End Get
            End Property

            ''' <summary>
            ''' Gets a formatted string showing the total number of files found 
            ''' in all directory paths based on your specifications for finding them.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property TotalFileCount_String As String
                Get
                    If _totalFileCount = 0 Then
                        Return "Unknown"
                    Else
                        Return _totalFileCount.ToString("n0")
                    End If
                End Get
            End Property

            ''' <summary>
            ''' Gets the total number of directories found in all directory paths 
            ''' based on your specifications for finding them.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property TotalDirectoryCount() As Integer
                Get
                    Return _totalDirectoryCount
                End Get
            End Property

            ''' <summary>
            ''' Gets a formatted string showing the total number of directories 
            ''' found in all directory paths based on your specifications for finding them.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property TotalDirectoryCount_String As String
                Get
                    If _totalDirectoryCount = 0 Then
                        Return "Unknown"
                    Else
                        Return _totalDirectoryCount.ToString("n0")
                    End If
                End Get
            End Property

            Public ReadOnly Property TotalFilesSize As Long
                Get
                    Return _totalFilesSize
                End Get
            End Property

            Public ReadOnly Property TotalFilesSize_String As String
                Get
                    Return Utilities.ShowTotalFileSize(_totalFilesSize)
                End Get
            End Property

            ''' <summary>
            ''' Gets the amount of time which has elapsed since the operation began.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public ReadOnly Property Elapsed() As TimeSpan
                Get
                    Return _operationSW.Elapsed
                End Get
            End Property

            ''' <summary>
            ''' Gets a formatted string showing the amount of time which has elapsed 
            ''' since the operation began.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks>
            ''' <para>Note that this function is set up to return the string 
            ''' formatted as mm:ss if the value is less than one hour, or as hh:mm:ss 
            ''' if the value is an hour or greater.</para>
            ''' </remarks>
            Public ReadOnly Property Elapsed_String() As String
                Get
                    If _operationSW.Elapsed.TotalSeconds > 3600 Then
                        Return Utilities.ConvertSecsToString(Elapsed.TotalSeconds, False, True)
                    Else
                        Return Utilities.ConvertSecsToString(Elapsed.TotalSeconds, False, False)
                    End If
                End Get
            End Property
        End Class





        Private Sub Dispose()
            If bgw IsNot Nothing Then
                bgw.Dispose()
                bgw = Nothing
            End If

            If tmr IsNot Nothing Then
                tmr.Dispose()
                tmr = Nothing
            End If
        End Sub
    End Class





    Public NotInheritable Class Utilities
        Public Delegate Function ShowTotalFileSizeDelegate(ByVal bytes As Long) As String

        ''' <summary>
        ''' A formatting function to return the number of bytes using common names for units of measure.
        ''' </summary>
        ''' <param name="bytes">The size (type Long) of the file.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ShowTotalFileSize(ByVal bytes As Long) As String

            Dim stringToReturn As String = ""

            If bytes < 1024 Then
                stringToReturn = bytes.ToString("n0") & " bytes"

            ElseIf bytes < (1024 * 1024) Then
                stringToReturn = (bytes / 1024).ToString("n0") & " kB"

            ElseIf bytes >= (1024 * 1024) AndAlso bytes < (1024 * 1024 * 1024) Then
                stringToReturn = (bytes / (1024 * 1024)).ToString("n1") & " Megs"

            ElseIf bytes >= (1024 * 1024 * 1024) AndAlso bytes < 1099511627776 Then
                stringToReturn = (bytes / (1024 * 1024 * 1024)).ToString("n2") & " Gigs"

            Else
                stringToReturn = (bytes / 1099511627776).ToString("n1") & " TeraBytes"
            End If

            Return stringToReturn

        End Function

        ''' <summary>
        ''' A formatting function to return the number of bytes using common names for units of measure.
        ''' </summary>
        ''' <param name="bytes">The size (type Double) of the file.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ShowTotalFileSize(ByVal bytes As Double) As String

            Dim stringToReturn As String = ""

            If bytes < 1024 Then
                stringToReturn = bytes.ToString("n1") & " bytes"

            ElseIf bytes < (1024 * 1024) Then
                stringToReturn = (bytes / 1024).ToString("n0") & " kB"

            ElseIf bytes >= (1024 * 1024) AndAlso bytes < (1024 * 1024 * 1024) Then
                stringToReturn = (bytes / (1024 * 1024)).ToString("n1") & " Megs"

            ElseIf bytes >= (1024 * 1024 * 1024) AndAlso bytes < 1099511627776 Then
                stringToReturn = (bytes / (1024 * 1024 * 1024)).ToString("n2") & " Gigs"

            Else
                stringToReturn = (bytes / 1099511627776).ToString("n1") & " TeraBytes"
            End If

            Return stringToReturn

        End Function

        Public Delegate Function ConvertSecsToStringDelegate(ByVal totalSeconds As Double,
                                                             ByVal showDays As Boolean,
                                                             ByVal showHours As Boolean) As String

        ''' <summary>
        ''' A formatting function to return the amount of time elapsed, based on the number of seconds entered.
        ''' </summary>
        ''' <param name="totalSeconds">The number of seconds to evaluate (type: Double).</param>
        ''' <param name="showDays">Indicate whether or not to show the number of days in the result.</param>
        ''' <param name="showHours">Indicate whether or not to show the number of hours in the result.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ConvertSecsToString(ByVal totalSeconds As Double,
                                                   ByVal showDays As Boolean,
                                                   ByVal showHours As Boolean) As String

            Dim returnString As String = ""

            If totalSeconds >= 0 And totalSeconds <= Double.MaxValue Then

                Dim totalTime As TimeSpan = TimeSpan.FromSeconds(totalSeconds)
                Dim daysString As String = ""
                Dim hoursString As String = ""
                Dim minutesString As String = ""
                Dim secondsString As String = ""

                If showDays Then
                    daysString = totalTime.Days.ToString("f0")
                    hoursString = totalTime.Hours.ToString("00")
                    minutesString = totalTime.Minutes.ToString("00")
                    secondsString = totalTime.Seconds.ToString("00")
                    returnString = String.Format("{0}:{1}:{2}:{3}", daysString, hoursString, minutesString, secondsString)

                ElseIf showHours Then
                    hoursString = (totalTime.Days * 24 + totalTime.Hours).ToString("00")
                    minutesString = totalTime.Minutes.ToString("00")
                    secondsString = totalTime.Seconds.ToString("00")
                    returnString = String.Format("{0}:{1}:{2}", hoursString, minutesString, secondsString)

                Else
                    Dim additionalMinutes As Integer = (totalTime.Days * 24 + totalTime.Hours) * 60
                    minutesString = (totalTime.Minutes + additionalMinutes).ToString("00")
                    secondsString = totalTime.Seconds.ToString("00")
                    returnString = String.Format("{0}:{1}", minutesString, secondsString)
                End If
            Else
                returnString = "N/A"
            End If

            Return returnString

        End Function

        Public Delegate Function ReturnShortStringDelegate(ByVal str As String,
                                                           ByVal maxLength As Integer) _
                                                           As String

        ''' <summary>
        ''' This will return a shortened version of your string based on the maxLength you specify.
        ''' </summary>
        ''' <param name="str">The string to shorten.</param>
        ''' <param name="maxLength">The maximum characters to return.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ReturnShortString(ByVal str As String,
                                                 ByVal maxLength As Integer) _
                                                 As String

            If str Is Nothing Then
                Return ""
            Else
                If str.Length <= maxLength Then
                    Return str
                Else
                    Dim leftEndPosition As Integer = CInt((maxLength / 2) - 2.5)
                    Dim rightStartPosition As Integer = str.Length - (CInt((maxLength / 2) - 2.5))

                    Dim sb As New System.Text.StringBuilder

                    sb.AppendFormat("{0} ... {1}", str.Substring(0, leftEndPosition), str.Substring(rightStartPosition))

                    Return sb.ToString
                End If
            End If

        End Function
    End Class
End Namespace