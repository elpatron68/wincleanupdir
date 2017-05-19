Imports System.IO

Module Module1

    Sub Main()
        If My.Application.CommandLineArgs.Count < 3 Then
            Console.WriteLine("Cleanup.exe <Directory> <Search pattern> <Number of newest files to keep>")
            Console.WriteLine("Example:")
            Console.WriteLine("Cleanup.exe *.tmp ""c:\path to my data"" 5")
            Console.WriteLine("(Deletes all but the 5 newest files in ""c:\path to my data"" and each subdirectory)")
            Exit Sub
        Else
            Dim sDir As String = My.Application.CommandLineArgs(0)
            Dim sPattern As String = My.Application.CommandLineArgs(1)
            Dim sNum As Integer = My.Application.CommandLineArgs(2)
            Dim iNum As Integer = 0
            Try
                iNum = Convert.ToInt16(sNum)
            Catch ex As Exception
                Console.WriteLine("Failed parsing number of files to keep!")
                Exit Sub
            End Try

            Console.WriteLine("Deleting files from " + sDir + " with pattern " + sPattern + " and keep the " + iNum.ToString + " newest.")
            Dim iDeletedFiles = _cleanup(sDir, sPattern, iNum)
            Console.WriteLine(iDeletedFiles.ToString + " deleted.")
        End If
    End Sub

    Private Function _cleanup(sDirectory As String, sPattern As String, iNumber As Integer) As Integer
        Dim iCount As Integer = 0
        Dim subdirs = Directory.EnumerateDirectories(sDirectory)
        For Each s In subdirs
            Dim files = New DirectoryInfo(s).EnumerateFiles(sPattern).OrderByDescending(Function(f) f.CreationTime).Skip(iNumber).ToList()
            For Each f In files
                Try
                    f.Delete()
                    iCount += 1
                    Console.WriteLine(s + "\" + f.Name + " deleted")
                Catch ex As Exception
                    Console.WriteLine("Failed deleting " + s + "\" + f.Name)
                End Try
            Next
        Next
        Return iCount
    End Function

End Module
