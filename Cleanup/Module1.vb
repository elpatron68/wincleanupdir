Imports System.IO

Module Module1

    Sub Main()
        If My.Application.CommandLineArgs.Count < 3 Then
            Console.WriteLine("Cleanup.exe <Directory> <Search pattern> <Number of newest files to keep>")
            Console.WriteLine("Example:")
            Console.WriteLine("Cleanup.exe *.tmp ""c:\path to my data"" 5")
            Console.WriteLine(" - Deletes all but the 5 newest *.tmp files in each of ""c:\path to my data"" 's subdirectories")
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

            Console.WriteLine($"Deleting files from '{sDir}' with pattern {sPattern} and keep the {iNum.ToString} newest.")
            Dim iValues() = _cleanup(sDir, sPattern, iNum)
            Console.WriteLine($"{iValues(0).ToString} file(s) deleted.")
            Console.WriteLine($"{iValues(1).ToString} file(s) skipped.")
        End If
    End Sub

    Private Function _cleanup(sDirectory As String, sPattern As String, iNumber As Integer) As Integer()
        Dim iSkippedCount As Integer = 0
        Dim iDeletedCount As Integer = 0
        Dim subdirs = Directory.EnumerateDirectories(sDirectory)
        For Each s In subdirs
            Dim files = New DirectoryInfo(s).EnumerateFiles(sPattern).OrderByDescending(Function(f) f.CreationTime).Skip(iNumber).ToList()
            For Each f In files
                Try
                    f.Delete()
                    iDeletedCount += 1
                    Console.WriteLine($"{s}\{f.Name} deleted")
                Catch ex As Exception
                    Console.WriteLine($"Failed deleting {s}\{f.Name}")
                    iSkippedCount += 1
                End Try
            Next
        Next
        Return {iDeletedCount, iSkippedCount}
    End Function

End Module
