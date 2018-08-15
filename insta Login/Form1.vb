Imports System.Net
Imports System.Text
Imports System.IO
Imports System.Threading
Imports Microsoft.VisualBasic.CompilerServices
Imports System.Security.Cryptography
Imports System.Text.RegularExpressions
Public Class Form1
    Private Counts As Integer
    Public xCookieContainer As CookieContainer
    Public xuid As String
    Sub New()
        Me.xCookieContainer = New CookieContainer
        Me.Counts = 0
        Me.InitializeComponent()
    End Sub
    Public Function login(ByVal user As String, ByVal pass As String) As Boolean
        Dim result As Boolean
        Try
            Dim httpWebRequest As HttpWebRequest = CType(WebRequest.Create("https://i.instagram.com/api/v1/accounts/login/"), HttpWebRequest)
            httpWebRequest.Method = "POST"
            httpWebRequest.UserAgent = "Instagram 22.0.0.15.68 Android"
            httpWebRequest.Accept = "*/*"
            httpWebRequest.Headers.Add("Accept-Language", "en;q=1, fr;q=0.9, de;q=0.8, zh-Hans;q=0.7, zh-Hant;q=0.6, ja;q=0.5")
            httpWebRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
            httpWebRequest.KeepAlive = True
            httpWebRequest.Proxy = Nothing
            httpWebRequest.CookieContainer = xCookieContainer
            Dim StringBuilder As StringBuilder = New StringBuilder()
            Dim text As String = String.Concat(New String() {"{""_uuid"":""", Guid.NewGuid().ToString(), """,""password"":""", pass, """,""username"":""", user, """,""device_id"":""", Guid.NewGuid().ToString(), """,""from_reg"":false,""_csrftoken"":""missing"",""login_attempt_count"":0}"})
            Try
                Dim HMACSHA256 As HMACSHA256 = New HMACSHA256(Encoding.UTF8.GetBytes("f372b2a5f14d1bebedaaa4ac6f8d506db30ffdd6185b8e0cdfa7dab42f5a9cc6"))
                Dim hash As Byte() = HMACSHA256.ComputeHash(Encoding.UTF8.GetBytes(text))
                For i As Integer = 0 To hash.Length - 1
                    StringBuilder.Append(hash(i).ToString("x2"))
                Next
            Catch expr_1CF As Exception
                ProjectData.SetProjectError(expr_1CF)
                ProjectData.ClearProjectError()
            End Try
            Dim text2 As String = String.Concat(New String() {"signed_body=", StringBuilder.ToString, ".", text, "&ig_sig_key_version=4"})
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(text2)
            httpWebRequest.ContentLength = CLng(bytes.Length)
            Dim Stream As Stream = httpWebRequest.GetRequestStream
            Stream.Write(bytes, 0, bytes.Length)
            Stream.Close()
            Dim HttpWebResponse As HttpWebResponse = DirectCast(httpWebRequest.GetResponse(), HttpWebResponse)
            Dim StreamReader As StreamReader = New StreamReader(HttpWebResponse.GetResponseStream())
            xuid = Regex.Match(StreamReader.ReadToEnd.ToString(), "pk"": (.*?), """).Groups(1).Value
            If Not Me.xuid = "" Then
                result = True
            End If
            StreamReader.Close()
            HttpWebResponse.Close()
        Catch expr_32E As Exception
            ProjectData.SetProjectError(expr_32E)
            result = False
            ProjectData.ClearProjectError()
        End Try
        Return result
    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TextBox1.Text & TextBox2.Text = Nothing Then
            MsgBox("Please enter username" + vbCrLf + " and password")
        ElseIf TextBox1.Text = Nothing Then
            MsgBox("Please enter username")
        ElseIf TextBox2.Text = Nothing Then
            MsgBox("Please enter password")
        Else
            Button1.Text = " Login... "
        End If

        If Button1.Text = " Login... " Then
            Try
                Dim flag As Boolean = Operators.ConditionalCompareObjectEqual(Me.login(Me.TextBox1.Text, Me.TextBox2.Text), True, False)
                If flag Then
                    MsgBox("Done")
                Else
                    MsgBox("username or password" + vbCrLf + "is incorrect or secure ")
                End If
            Catch expr_D7 As Exception
                ProjectData.SetProjectError(expr_D7)
                ProjectData.ClearProjectError()
            End Try
        ElseIf Button1.Text = "Hi..." Then
        End If
        If Button1.Text = "logout" Then
        End If
    End Sub
End Class
