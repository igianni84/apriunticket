Imports System.Data
Imports System.Data.SqlClient
Imports System.IO




Public Class backup
    Inherits System.Web.UI.Page
    Private MyGest As MNGestione
    Dim dbname As String = "advise_it_crm"
    Dim sqlcon As SqlConnection = New SqlConnection(CGlobal.cs)
    Dim sqlcmd As SqlCommand = New SqlCommand()
    Dim da As SqlDataAdapter = New SqlDataAdapter()
    Dim dt As DataTable = New DataTable()

    

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        
        Dim sqlConnectionString As String = "SERVER=CARLO-TOSH;DATABASE=advise_it_crm;user id=sa;password=123;" '"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Locations;Data Source=GIGABYTE-PC\SQLEXPRESS"

        Dim conn As New SqlConnection(sqlConnectionString)
        conn.Open()

        Dim cmd As New SqlCommand
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "BACKUP DATABASE advise_it_crm TO DISK='C:\Temp\location.BAK'"
        cmd.Connection = conn
        cmd.ExecuteNonQuery()

        conn.Close()
    End Sub

   
End Class