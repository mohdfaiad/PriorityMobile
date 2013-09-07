﻿Imports System.Runtime.InteropServices
Imports System.IO
Imports PrioritySFDC

Module StartUp

    Public StartFlags As New StartFlags

#Region " API Declarations "

    Public Const SW_HIDE As Integer = &H0
    Public Const SW_SHOW As Integer = &H1

    <DllImport("coredll.dll", CharSet:=CharSet.Auto)> _
    Public Function FindWindow(ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    End Function

    <DllImport("coredll.dll", CharSet:=CharSet.Auto)> _
    Public Function ShowWindow(ByVal hwnd As IntPtr, ByVal nCmdShow As Integer) As Boolean
    End Function

    <DllImport("coredll.dll", CharSet:=CharSet.Auto)> _
    Public Function EnableWindow(ByVal hwnd As IntPtr, ByVal enabled As Boolean) As Boolean
    End Function

    <DllImport("coredll.dll", SetLastError:=True)> _
    Public Function GetCapture() As IntPtr
    End Function

    <DllImport("aygshell.dll")> _
    Public Function SHFullScreen(ByVal hwndRequester As IntPtr, ByVal dwState As Integer) As Integer
    End Function

    Public Enum SHFS As Integer
        SHOWTASKBAR = &H1
        HIDETASKBAR = &H2
        SHOWSIPBUTTON = &H4
        HIDESIPBUTTON = &H8
        SHOWSTARTICON = &H10
        HIDESTARTICON = &H20
    End Enum

#End Region

    Public Sub Main(ByVal args As String())

        For Each arg As String In args
            Select Case arg.ToLower
                Case "clearcache"
                    StartFlags.ClearCache = True
                Case "wipedata"
                    StartFlags.WipeData = True
                Case "noprovision"
                    StartFlags.NoProvision = True
            End Select
        Next

        Try
            ue = New UserEnv("PrioritySFDC", "cHandler.dll")
            With ue
                .SaveProvision = Not (StartFlags.NoProvision)
                .Add("forms", _
                    New OfflineXML( _
                       ue, "sfdc.xml", "sfdc.ashx", _
                            New StartFlags( _
                                StartFlags.ClearCache, _
                                StartFlags.WipeData, _
                                StartFlags.NoProvision _
                            ) _
                    ) _
                )

            End With

            Application.Run(Menu)

        Catch ex As Exception
            Using sw As New StreamWriter("appcrash.txt", True)
                sw.WriteLine("{0}: {1}", Now.ToString, ex.Message)
                sw.Write(ex.StackTrace)
            End Using
        End Try

    End Sub

    Private _ue As UserEnv
    Public Property ue() As UserEnv
        Get
            Return _ue
        End Get
        Set(ByVal value As UserEnv)
            _ue = value
        End Set
    End Property

End Module

