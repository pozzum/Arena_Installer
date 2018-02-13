'http://www.vbforums.com/showthread.php?661477-Easy-object-serialization-to-a-file-with-compression-and-encryption
Imports System.IO
Imports System.Runtime.Serialization

Public Class Serialize

#Region "Serialization classes for save features"

    <Serializable()>
    Public Class CompressedObject
        Public CompressedObject As Byte()
        Public Sub New(ByVal CompressedObject As Byte())
            Me.CompressedObject = CompressedObject
        End Sub
    End Class

    <Serializable()>
    Public Class EncryptedObject
        Public EncryptedObject As Byte()
        Public Sub New(ByVal EncryptedObject As Byte())
            Me.EncryptedObject = EncryptedObject
        End Sub
    End Class

#End Region

    Private Shared Sub StreamSerialize(ByRef s As Stream, ByVal Obj As Object, Optional ByVal Compress As Boolean = True, Optional ByVal EncryptKey As String = Nothing)
        Dim b As New Formatters.Binary.BinaryFormatter
        b.Serialize(s, Obj)
    End Sub

    Public Shared Sub FileSerialize(ByVal File As String, ByVal Obj As Object, Optional ByVal Compress As Boolean = True, Optional ByVal EncryptKey As String = Nothing)
        Using s As New FileStream(File, FileMode.Create, FileAccess.ReadWrite)
            StreamSerialize(s, Obj, Compress, EncryptKey)
            s.Close()
        End Using
    End Sub

    Public Shared Function FileDeserialize(ByVal File As String, Optional ByVal DecryptKey As String = Nothing) As Object
        FileDeserialize = Nothing
        Dim theError As Exception = Nothing
        Using s As New FileStream(File, FileMode.Open, FileAccess.Read)
            FileDeserialize = StreamDeserialize(s, DecryptKey)
            s.Close()
        End Using
    End Function

    Public Shared Function PackageFileDeserialize(ByVal File As String, Optional ByVal DecryptKey As String = Nothing) As List(Of package_info)
        PackageFileDeserialize = Nothing
        Dim theError As Exception = Nothing
        Using s As New FileStream(File, FileMode.Open, FileAccess.Read)
            PackageFileDeserialize = StreamDeserialize(s, DecryptKey)
            s.Close()
        End Using
    End Function

    Public Shared Function StreamDeserialize(ByVal s As Stream, Optional ByVal DecryptKey As String = Nothing) As Object
        Dim b As New Formatters.Binary.BinaryFormatter
        StreamDeserialize = b.Deserialize(s)
    End Function

    Private Shared Function ByteArrSerialize(ByVal Obj As Object) As Byte()
        Using MS As New MemoryStream
            Dim BF As New Formatters.Binary.BinaryFormatter
            BF.Serialize(MS, Obj)
            ByteArrSerialize = MS.ToArray
            MS.Close()
        End Using
    End Function

    Private Shared Function ByteArrDeserialize(ByVal SerializedData() As Byte) As Object
        Using MS As New MemoryStream(SerializedData)
            Dim BF As New Formatters.Binary.BinaryFormatter
            ByteArrDeserialize = BF.Deserialize(MS)
            MS.Close()
        End Using
    End Function

End Class
