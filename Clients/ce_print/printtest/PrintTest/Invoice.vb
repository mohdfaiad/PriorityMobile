﻿Imports CPCL
Imports btZebra

Public Class Invoice

    Private WithEvents prn As New btZebra.LabelPrinter( _
        New Point(300, 300), _
        New Size(576, 0), _
        "My Documents\prnimg\" _
    )

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim macaddress As String = "0022583cdd7e"

        If Not prn.Connected Then
            prn.BeginConnect(macaddress, , True)
        Else
            Print()
        End If

    End Sub

    Private Sub hConnectionEstablished() Handles prn.connectionEstablished
        Print()
    End Sub

    Private Sub Print()
        Dim headerFont As New PrinterFont(50, 5, 2) 'variable width. 
        Dim largeFont As New PrinterFont(30, 0, 3) '16 
        Dim smallFont As New PrinterFont(35, 0, 2) '8 

        Using lblInvoice As New Label(prn, eLabelStyle.receipt)

            'first receipt formatter
            'taking margins into account the receipt is 556px wide. 
            'font 0 - 8/16 for size 2/3 respectively, font 5 is variable width ~20-30.

            Dim docHead As New ReceiptFormatter(64, _
                                                New FormattedColumn(16, 0, eAlignment.Center), _
                                                New FormattedColumn(16, 16, eAlignment.Center), _
                                                New FormattedColumn(16, 32, eAlignment.Center), _
                                                New FormattedColumn(16, 48, eAlignment.Center))
            docHead.AddRow("Number:", "Date:", "Time:", "Van:")
            docHead.AddRow("593151", "29/01/13", "11:51:22", "WK11 BHW")

            Dim custDetails As New ReceiptFormatter(64, _
                                                    New FormattedColumn(13, 0, eAlignment.Right), _
                                                    New FormattedColumn(48, 16, eAlignment.Left))
            custDetails.AddRow("Customer:", "G00012")
            custDetails.AddRow("", "Goods returned Restock Van50")
            custDetails.AddRow("", "TR16 5BU")



            Dim invoicePartsList As New ReceiptFormatter(64, _
                                                  New FormattedColumn(3, 0, eAlignment.Right), _
                                                  New FormattedColumn(46, 4, eAlignment.Left), _
                                                  New FormattedColumn(7, 50, eAlignment.Right), _
                                                  New FormattedColumn(7, 57, eAlignment.Right))
            invoicePartsList.AddRow("No:", "Description:", "Price:", "Total:")
            invoicePartsList.AddRow("2", "56g (2oz) CLOTTED CREAM", "0.39", "0.78")
            invoicePartsList.AddRow("8", "Blue 1ltr Whole Milk", "0.60", "4.80")
            invoicePartsList.AddRow("12", "Green 1ltr Semi-Skimmed Milk", "0.60", "7.20")




            Dim total As New ReceiptFormatter(64, _
                                              New FormattedColumn(6, 10, eAlignment.Right), _
                                              New FormattedColumn(47, 16, eAlignment.Right))
            total.AddRow("Total:", "#12.78")


            With lblInvoice
                .CharSet(eCountry.UK)
                'logo
                .AddImage("roddas.pcx", New Point(186, prn.Dimensions.Height + 10), 147)

                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 10, 15)

                'header = 174px wide
                .AddText("INVOICE", New Point((prn.Dimensions.Width / 2) - 87, prn.Dimensions.Height + 10), _
                         headerFont)
                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 10)

                'address
                .AddMultiLine("A.E. Rodda & Son Ltd." & vbCrLf & "The Creamery" & vbCrLf & "Scorrier" _
                                & vbCrLf & "Redruth" & vbCrLf & "Cornwall" & vbCrLf & "TR165BU", _
                                             New Point(10, prn.Dimensions.Height + 10), largeFont, 30)
                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 2)

                'document header 
                For Each StrVal In docHead.FormattedText
                    .AddText(StrVal, New Point(22, prn.Dimensions.Height), smallFont)
                Next

                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 2)

                'customer details 
                For Each StrVal In custDetails.FormattedText
                    .AddText(StrVal, New Point(22, prn.Dimensions.Height), smallFont)
                Next

                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 2)

                'itemised invoice box
                For Each StrVal In invoicePartsList.FormattedText
                    .AddText(StrVal, New Point(22, prn.Dimensions.Height), smallFont)
                Next

                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 5)

                'total 
                For Each StrVal In total.FormattedText
                    .AddText(StrVal, New Point(22, prn.Dimensions.Height), smallFont)
                Next

                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 5)

                'itemisation
                Dim totals As String = " ( 1 lines 6 units ) " 'this will, of course, be calculated.
                .AddText(totals, New Point((prn.Dimensions.Width / 2 - (totals.Length / 2) * 16), _
                                           prn.Dimensions.Height + 10), largeFont)

                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 20), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 20), 5)

                'vat number 
                Dim vat As String = "V.A.T. No.  131 7759 63"
                .AddText(vat, New Point((prn.Dimensions.Width / 2 - (vat.Length / 2) * 16), _
                                           prn.Dimensions.Height + 10), largeFont)

                'For any remittance.... 
                .AddMultiLine("For any remittance queries please contact" & vbCrLf & "accounts@roddas.co.uk".PadLeft(32, " "), _
                              New Point(prn.Dimensions.Width / 2 - 168, prn.Dimensions.Height + 10), smallFont, 30)

                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 5)

                'bank details header
                .AddText("Bank Details", New Point(prn.Dimensions.Width / 2 - 96, prn.Dimensions.Height + 10), largeFont)
                'bank details 
                .AddMultiLine("HSBC" & vbCrLf & "Branch Location" & vbCrLf & "Account Number" & vbCrLf & "Sort Code", _
                              New Point(10, prn.Dimensions.Height + 10), smallFont, 30)

                'line
                .AddLine(New Point(10, prn.Dimensions.Height + 10), _
                         New Point(prn.Dimensions.Width - 10, prn.Dimensions.Height + 10), 10)

                'please quote
                .AddText("Please quote account number in all correspondence.", _
                         New Point(prn.Dimensions.Width / 2 - 200, _
                                   prn.Dimensions.Height + 10), _
                         smallFont)

                'tear 'n' print!
                .AddTearArea(New Point(0, prn.Dimensions.Height))
                prn.Print(.toByte)


            End With
        End Using
    End Sub

End Class