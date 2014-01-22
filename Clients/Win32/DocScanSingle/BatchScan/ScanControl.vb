﻿Imports System.ComponentModel
Imports System.Text
Imports DTI.ImageMan.Twain
Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf
Imports System.IO
Imports System.Collections
Imports System
Imports System.Drawing
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Configuration.ConfigurationSettings

'/f  "1 PORD ../../scandocs/demo/PORD PO12000017"
Public Class ScanControl
    ' Single Scan Console
    ' Version 1.0
    ' Andy Mackintosh
    ' 14/12/2012
    ' This application is intended to be used from within Priority. It accepts one parameter /f {filename with full path but no filetype}
    ' Once ran it will call the scanner and grab whatever document is loaded, then it will convert it to pdf and save it as the filename provided
    ' I have set the needed scanner contols up as application level variables in the ScanSettings class, these are read from an XML file and can
    ' therefore be changed by simply editing the file

    Public Class ScanSettings
        Private ptype As DTI.ImageMan.Twain.PixelTypes
        Private mpAG As Integer
        Private ui As DTI.ImageMan.Twain.UserInterfaces
        Private resol As Integer
        Public Property PagType() As DTI.ImageMan.Twain.PixelTypes
            Get
                Return ptype
            End Get
            Set(ByVal value As DTI.ImageMan.Twain.PixelTypes)
                ptype = value
            End Set
        End Property
        Public Property mpg() As Integer
            Get
                Return mpAG
            End Get
            Set(ByVal value As Integer)
                mpAG = value
            End Set
        End Property
        Public Property uid() As DTI.ImageMan.Twain.UserInterfaces
            Get
                Return ui
            End Get
            Set(ByVal value As DTI.ImageMan.Twain.UserInterfaces)
                ui = value
            End Set
        End Property
        Public Property resolu() As Integer
            Get
                Return resol
            End Get
            Set(ByVal value As Integer)
                resol = value
            End Set
        End Property
        Public Sub New(ByVal pt As DTI.ImageMan.Twain.PixelTypes, ByVal pgs As Integer, ByVal uids As DTI.ImageMan.Twain.UserInterfaces, ByVal res As Integer)
            PagType = pt
            mpg = pgs
            uid = uids
            resol = res
        End Sub

        Public Shared Function writesettings(ByVal PixType As Integer, ByVal MaxPage As Integer, ByVal UsrIntFc As Integer, ByVal Res As Integer) As Boolean
            Try
                My.Settings.PixType = PixType
                My.Settings.MaxPage = MaxPage
                My.Settings.UserIntface = UsrIntFc
                My.Settings.Resolut = Res
                My.Settings.Save()
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function
    End Class
    Public Class ScanDocument
        Private SCAN_DOC_DIR As String
        Private SCAN_FILE_NAME As String
        Private SCAN_DATE As Integer
        Private SCAN_PROCESSED As Char
        Private SCAN_BATCH_NO As Integer
        'Private SCAN_TYPE_CODE As String
        'Private SCAN_USER As Integer
        'Private SCAN_COMPANY As String
        Private SCAN_Full_File As String
        Public Property doc_dir() As String
            Get
                Return SCAN_DOC_DIR
            End Get
            Set(ByVal value As String)
                SCAN_DOC_DIR = value
            End Set
        End Property
        Public Property file_name() As String
            Get
                Return SCAN_FILE_NAME
            End Get
            Set(ByVal value As String)
                SCAN_FILE_NAME = value
            End Set
        End Property
        Public Property doc_date() As Integer
            Get
                Return SCAN_DATE
            End Get
            Set(ByVal value As Integer)
                SCAN_DATE = value
            End Set
        End Property
        'Public Property user() As Integer
        '    Get
        '        Return SCAN_USER
        '    End Get
        '    Set(ByVal value As Integer)
        '        SCAN_USER = value
        '    End Set
        'End Property
        Public Property processed() As Char
            Get
                Return SCAN_PROCESSED
            End Get
            Set(ByVal value As Char)
                SCAN_PROCESSED = value
            End Set
        End Property
        Public Property batch_no() As Integer
            Get
                Return SCAN_BATCH_NO
            End Get
            Set(ByVal value As Integer)
                SCAN_BATCH_NO = value
            End Set
        End Property
        Public Property full_file() As String
            Get
                Return SCAN_Full_File
            End Get
            Set(ByVal value As String)
                SCAN_Full_File = value
            End Set
        End Property
        'Public Property type_code() As String
        '    Get
        '        Return SCAN_TYPE_CODE
        '    End Get
        '    Set(ByVal value As String)
        '        SCAN_TYPE_CODE = value
        '    End Set
        'End Property
        'Public Property Company() As String
        '    Get
        '        Return SCAN_COMPANY
        '    End Get
        '    Set(ByVal value As String)
        '        SCAN_COMPANY = value
        '    End Set
        'End Property
        Public Sub New(ByVal dir As String, ByVal name As String, ByVal sdate As Integer, ByVal proc As Char, ByVal bno As Integer)
            'Company = S_Company
            doc_dir = dir
            file_name = name
            doc_date = sdate
            processed = proc
            batch_no = bno
            'type_code = tcode
            'user = usr
        End Sub
    End Class
    Public Shared Function scannall(ByVal f As ScanDocument)
        Dim D As ScanControl.ScanSettings
        D = New ScanControl.ScanSettings(My.Settings("pixtype"), My.Settings("maxpage"), My.Settings("userintface"), My.Settings("resolut"))
        Dim imgs As New ArrayList
        'The arraylist will hold all the pages scanned in as bitmap images. I have used arraylist as I dont have to specify any bounds as I dont know them
        Dim tw As New DTI.ImageMan.Twain.TwainControl
        Using tw


            With tw
                .PixelType = D.PagType
                .MaxPages = D.mpg
                .UserInterface = D.uid
                .Resolution = D.resolu
            End With
            'Instantiates the twain control
            Dim img As Image
            'Creates a holder image to capture each page
            imgs = New ArrayList
            'instantiate the arraylist
            '
            'Dim capabilityValue As Object
            'Dim dataType As DTI.ImageMan.Twain.DataType

            'Dim retVal As Boolean = tw.GetCapability(DTI.ImageMan.Twain.Capabilities.FeederLoaded, capabilityValue, dataType)

            'If retVal AndAlso CInt(capabilityValue) <> 0 Then

            '    If retVal AndAlso CInt(capabilityValue) <> 0 Then
            '        'MessageBox.Show("Feeder is loaded with paper")
            '    End If
            'Else

            'End If



            Try
                img = tw.ScanPage()
            Catch ex As Exception
                write_error(ex.ToString, 1)
                Return False
            End Try

            'grab the first page
            While Not (img Is Nothing)
                Try
                    imgs.Add(img)
                    img = tw.ScanPage()

                Catch ex As Exception
                    Console.WriteLine(ex.ToString)
                    Return False
                End Try
            End While
        End Using
        'The above loop will keep scanning until there are no pages left to scan
        Dim doc As New PdfDocument
        'next we create an instance of a pdf document to feed the images into we have to do this on an image by image basis merrily creating pages as we go
        Dim count As Integer = 0
        'I use count to keep track on whichpage we are adding. Ready cool off we go then!
        doc.Pages.Add(New PdfPage)
        'Add the pages control into the pdf
        For Each img In imgs
            'loop through the images

            Try
                doc.AddPage()
                'add a blank page to the pdf
                Dim xgr As XGraphics
                xgr = XGraphics.FromPdfPage(doc.Pages(count))
                'Turn the page into a graphic to allow the image to be added
                Dim imgx As XImage
                imgx = XImage.FromGdiPlusImage(img)
                'As the pdfer only accepts the ximage format I need to convert my image from an in memory image to an ximage
                xgr.DrawImage(imgx, 0, 0)
                'draw the image to the page
            Catch ex As Exception
                Console.WriteLine(ex.Message)
                Return False

                'if any bad things happen (and they can be careful out there kids!!) tell the users about what went wrong
            End Try
            count += 1

        Next
        Try
            If Not Directory.Exists(f.doc_dir) Then
                Directory.CreateDirectory(f.doc_dir)
            End If
            doc.Save(f.full_file)
            write_log(f.doc_dir, f.file_name, f.doc_date, f.batch_no)
            doc = Nothing
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
            Return False
        End Try

        Return True

    End Function
    Public Shared Sub write_error(ByVal errmsg As String, ByVal usr As Integer)
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        With con
            .ConnectionString = My.Settings.PriorityDB.ToString 'GetConnectionString("PriorityDB")

        End With
        Dim sdate As DateTime
        sdate = FormatDateTime("1/1/1988", DateFormat.ShortDate)
        con.Open()
        cmd.Connection = con
        cmd.CommandText = "insert into ZEMG_ERRMSGLOG (LOGGEDBYPROCNAME,LOGGEDDATE,[MESSAGE],T$USER) values ( 'Scanner',@ddate,@message,@user)"
        cmd.Parameters.AddWithValue("ddate", DateDiff(DateInterval.Minute, sdate, Now))
        cmd.Parameters.AddWithValue("message", errmsg)
        cmd.Parameters.AddWithValue("user", usr)
        cmd.ExecuteNonQuery()
    End Sub
    Public Shared Sub write_log(ByVal DOC_DIR As String, ByVal File_Name As String, ByVal S_Date As Integer, ByVal S_Batch_NO As Integer)
        Try
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            With con
                .ConnectionString = GetConnectionString("PriorityDB")
            End With
            con.Open()
            cmd.Connection = con
            cmd.CommandText = "insert into ZEMG_SCANNINGLOG (SCAN_DOC_DIR,SCAN_FILE_NAME,SCAN_DATE,SCAN_PROCESSED,SCAN_BATCH_NO) values ( @docdir,@filename ,@ddate,'N',@batchno)"

            cmd.Parameters.AddWithValue("ddate", S_Date)
            cmd.Parameters.AddWithValue("docdir", DOC_DIR)
            cmd.Parameters.AddWithValue("filename", File_Name)
            cmd.Parameters.AddWithValue("batchno", S_Batch_NO)
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            write_error(ex.ToString, 1)
        End Try

    End Sub
    Public Shared Function GetConnectionString(ByVal connectionS As String) As String
        'this function will check on the existence of the app settings connection string named priority db, if it cant find it it will write an error and quit. The function will also scheck the company used in the connection string, if it is different it will update the string.
        'variable to hold our connection string for returning it (Data Source=POTTER\PRI;Initial Catalog=demo;Integrated Security=True)

        Dim strReturn As New String("")

        'check to see if the user provided a company name  


        If Not String.IsNullOrEmpty(connectionS) Then

            strReturn = ConfigurationManager.ConnectionStrings(connectionS).ConnectionString

            'Next we check the existing connection strinbg to see if the company names match
          
        Else
            'no connection string name was provided  
            'get the default connection string  
            strReturn = ConfigurationManager.ConnectionStrings("YourConnectionName").ConnectionString
        End If
        'return the connection string to the caller 
        Return strReturn

    End Function
    Public Shared Function ChecktConnectionString(ByVal company As String) As Boolean
        'this function will check on the existence of the app settings connection string named priority db, if it cant find it it will write an error and quit. The function will also scheck the company used in the connection string, if it is different it will update the string.
        'variable to hold our connection string for returning it (Data Source=POTTER\PRI;Initial Catalog=demo;Integrated Security=True)

        Dim strReturn As New String("")

        'check to see if the user provided a company name  


        If Not String.IsNullOrEmpty(company) Then
            'a company name was provided  
            'get the connection string by the name provided  
            Try
                strReturn = ConfigurationManager.ConnectionStrings("priorityd").ConnectionString
            Catch ex As Exception
                Console.WriteLine("Database connection string not set in the settings file")
                write_error("Database connection not found in the settings file.", 1)
                Return ""
            End Try


            'Next we check the existing connection strinbg to see if the company names match
            Dim connectionparts() As String = strReturn.Split(";")
            Dim initcat() As String = connectionparts(1).Split("=")
            Dim cat As String = initcat(1)
            If cat <> company Then
                Dim cons As New StringBuilder(connectionparts(0))


            End If
        Else
            'no connection string name was provided  
            'get the default connection string  
            strReturn = ConfigurationManager.ConnectionStrings("YourConnectionName").ConnectionString
        End If
        'return the connection string to the caller 
        Return strReturn

    End Function
End Class
