﻿Public Class calc
    Dim btn(11) As Button
    Dim _Max As Integer
    Dim _Value As Integer

    Private DNum As Double = 0.0
    Private mclosing As Boolean = True
    Private _Handler As Object
    Private PressDot As Boolean = False
    Public Event SetNumber(ByVal MyValue As Double)

    Public Property Max() As Integer
        Get
            Return _Max
        End Get
        Set(ByVal value As Integer)
            _Max = value
        End Set
    End Property

    Public Property isClosing() As Boolean
        Get
            Return mclosing
        End Get
        Set(ByVal value As Boolean)
            mclosing = value
        End Set
    End Property

    Public Property Value() As Double
        Get
            Return CDbl(DNum)
        End Get
        Set(ByVal value As Double)
            DNum = value.ToString
        End Set
    End Property


    Private Sub ct_number_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        'If mclosing Then Exit Sub
        Dim fs As Single

        If Me.Width > 319 Then
            fs = 14
        ElseIf Me.Width > 268 Then
            fs = 12
        ElseIf Me.Width > 258 Then
            fs = 11
        ElseIf Me.Width > 241 Then
            fs = 10
        ElseIf Me.Width > 214 Then
            fs = 9
        ElseIf Me.Width > 199 Then
            fs = 8
        Else
            fs = 8
        End If

        Dim f As New Font("Microsoft Sans Serif", fs + 2, FontStyle.Bold)
        Dim c As New Font("Microsoft Sans Serif", fs + 4, FontStyle.Regular)

        txt_Number.Font = c

        Dim i As Integer = 0
        For y As Integer = 0 To 3
            For x As Integer = 0 To 2
                If IsNothing(btn(i)) Then
                    btn(i) = New Button
                End If
                btn(i).Width = Me.Width / 3
                btn(i).Height = (Me.Height - txt_Number.Height) / 4
                btn(i).Font = f
                btn(i).Top = (txt_Number.Top + txt_Number.Height) + (y * btn(i).Height)
                btn(i).Left = x * btn(i).Width
                i = i + 1
            Next
        Next

    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        For i As Integer = 0 To 11
            btn(i) = New Button
            btn(i).Width = Me.Width / 3
            btn(i).Height = (Me.Height - txt_Number.Height) / 4
            Select Case i
                Case 0, 1, 2, 3, 4, 5, 6, 7, 8
                    btn(i).Text = CStr(i + 1)
                Case 9
                    btn(i).Text = "<"
                Case 10
                    btn(i).Text = "0"
                Case 11
                    btn(i).Text = "Ok"
            End Select
            AddHandler btn(i).Click, AddressOf Btn_Click
            AddHandler btn(i).KeyPress, AddressOf hKeyPress
            AddHandler btn(i).KeyDown, AddressOf hKeyDown
            Me.Controls.Add(btn(i))
        Next
        DNum = 0
        Me.txt_Number.Text = DNum.ToString
        Me.btn(9).Focus()

    End Sub

    Private Sub Btn_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim btn As Button = sender
        Select Case Strings.Left(btn.Text, 1)
            Case "O"
                PressDot = False
                Me.isClosing = True
                RaiseEvent SetNumber(CDbl(DNum))
            Case "C"
                PressDot = False
                DNum = 0
            Case "<"
                PressDot = False
                If Len(DNum.ToString) > 1 Then
                    DNum = CDbl(Strings.Left(DNum.ToString, Len(DNum.ToString) - 1))
                Else
                    DNum = 0
                End If
            Case Else
                If PressDot Then
                    DNum = CDbl(DNum.ToString & "." & btn.Text)
                    PressDot = False
                Else
                    DNum = CDbl(DNum.ToString & btn.Text)
                End If
        End Select

        If Not IsNumeric(DNum) Then
            DNum = 0
        Else
            If DNum > Max Then DNum = Max            
        End If

        txt_Number.Text = DNum.ToString

        Me.btn(9).Focus()

    End Sub

    Private Sub txt_Number_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Number.GotFocus
        Me.btn(9).Focus()
    End Sub

    Private Sub hKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_Number.KeyDown
        e.Handled = True
        Select Case e.KeyValue
            Case Keys.Back
                Btn_Click(Me.btn(9), New System.EventArgs)
            Case Keys.D0
                Btn_Click(Me.btn(10), New System.EventArgs)
            Case Keys.D1
                Btn_Click(Me.btn(0), New System.EventArgs)
            Case Keys.D2
                Btn_Click(Me.btn(1), New System.EventArgs)
            Case Keys.D3
                Btn_Click(Me.btn(2), New System.EventArgs)
            Case Keys.D4
                Btn_Click(Me.btn(3), New System.EventArgs)
            Case Keys.D5
                Btn_Click(Me.btn(4), New System.EventArgs)
            Case Keys.D6
                Btn_Click(Me.btn(5), New System.EventArgs)
            Case Keys.D7
                Btn_Click(Me.btn(6), New System.EventArgs)
            Case Keys.D8
                Btn_Click(Me.btn(7), New System.EventArgs)
            Case Keys.D9
                Btn_Click(Me.btn(8), New System.EventArgs)
            Case 190
                If InStr(txt_Number.Text, ".") = 0 Then
                    PressDot = True
                    txt_Number.Text = DNum.ToString & "."
                End If
        End Select
    End Sub

    Private Sub hKeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Number.KeyPress, txt_Number.KeyPress


    End Sub


End Class
