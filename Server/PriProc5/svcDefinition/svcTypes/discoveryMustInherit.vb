﻿Public MustInherit Class discoveryMustInherit : Inherits ServiceMustInherit

    Private ReadOnly Property thisSvc() As eServicePorts
        Get
            Return eServicePorts.discovery
        End Get
    End Property

    Public Overrides ReadOnly Property ServiceType() As String
        Get
            Return ServiceName(thisSvc)
        End Get
    End Property

    Public Overrides ReadOnly Property ServicePort() As Integer
        Get
            Return thisSvc
        End Get
    End Property

    Public Overrides ReadOnly Property udpListener() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides ReadOnly Property tcpListener() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides ReadOnly Property BroadcastPort() As Integer
        Get
            Return eServicePorts.prisql
        End Get
    End Property

End Class
