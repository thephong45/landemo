Imports System
Imports System.Data
'Imports System.Data.Types
Imports System.Configuration
Imports System.Data.SqlClient
Public Class DataBaseAccess
    ' Represents an open connection to a SQL database
    Protected SqlCon As SqlConnection
    ' Represents a SQL statement or stored
    ' procedure to execute against a SQL database
    Protected SqlCom As SqlCommand
    ' Provides a way of reading a forward-only
    ' stream of rows from a SQL database
    Protected Sqldreader As SqlDataReader
    ' Represents a set of data commands and a database
    ' connection that are used to fill the 
    Protected SqlDa As SqlDataAdapter
    ' Automatically generates single-table commands that are
    ' used to reconcile changes made to a 
    Protected SqlComb As SqlCommandBuilder

    Private connectionstring As String = "Data Source=DESKTOP-JS828RS;Initial Catalog=Cloud_Database;Integrated Security=True"

    'Purpose: Class constructor.
    Public Sub New()
        'Initialize the class' members.
        SqlCon = New SqlConnection
        ' Set connection string of the SqlConnection object
        SqlCon.ConnectionString = ConnectionString
    End Sub


    ' Purpose: Opens a database connection
    ' if SQL connection open successful then return true
    ' else occur exception return false
    Public Function OpenConnection() As Boolean
        Try ' open connection
            If SqlCon.State <> ConnectionState.Open Then
                SqlCon.ConnectionString = ConnectionString
                SqlCon.Open()
                Return True
            Else
                Return False
            End If
        Catch ex As SqlException
            MessageBox.Show("Can not connect to database, please choose a database!", "Connect Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return True
    End Function
    ' Purpose: Close a database connection. 
    ' if SQL connection close successful then return true else
    ' occur exception return false
    Protected Function CloseConnection() As Boolean
        Try ' Close connection
            If SqlCon.State <> ConnectionState.Closed Then
                SqlCon.Close()
                Return True
            Else
                Return False
            End If
        Catch ex As SqlException
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Close Database")
            Return False
        End Try
    End Function
    ' Purpose: ExecuteNoneQuery command
    ' if SQL statement execute successful then return true else
    ' occur exception retun false
    Public Function ExecuteNoneQuery(ByVal v_sSqlString As String, Optional ByVal ShowErrorMessage As Boolean = False) As Boolean
        If OpenConnection() Then
            Try
                'sets the SqlConnection used by this SqlCom of the SqlCommand
                SqlCom = New SqlCommand(v_sSqlString, SqlCon)
                ' Executes a SQL statement against the connection and returns
                ' the number of rows affected.
                SqlCom.ExecuteNonQuery()
                Return True
            Catch ex As SqlException
                If ShowErrorMessage Then
                    MsgBox(ex.Message, MsgBoxStyle.Critical, "Execute!")
                End If

                Return False
            Finally
                ' Close database connection.
                CloseConnection()
                SqlCom.Dispose()
            End Try
        End If
        Return True
    End Function
    

    ' Purpose: Provides a way of reading a forward-only stream of rows from a SQL database
    ' if SQL statement execute successful then return SqlDataReader else
    ' occur exception return nothing
    Public Function GetDataReader(ByVal v_sSqlString As String) As SqlDataReader
        Try
            If OpenConnection() Then
                'sets the SqlConnection used by this SqlCom of the SqlCommand
                SqlCom = New SqlCommand(v_sSqlString, SqlCon)
                ' Sends the CommandText to the Connection and builds a SqlDataReader.
                Sqldreader = SqlCom.ExecuteReader
            End If
            Return Sqldreader
        Catch ex As SqlException

            MsgBox(ex.Message, MsgBoxStyle.Critical, "Get DataReader")
            Return Nothing
        Finally
            ' Close database connection.
            CloseConnection()
            SqlCom.Dispose()
        End Try
    End Function
    'Purpose: return the first column of the first row in the result set,
    ' or a nothing reference if the result set is empty
    Public Function GetScalar(ByVal v_sSqlString As String) As Object
        Dim Result As Object = Nothing
        Try
            If OpenConnection() Then
                'sets the SqlConnection used by this SqlCom of the SqlCommand
                SqlCom = New SqlCommand(v_sSqlString, SqlCon)
                ' Executes the query, and returns the first column of the first row in the
                ' result set returned by the query. Extra columns or rows are ignored.
                Result = SqlCom.ExecuteScalar
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Get Scalar")
        Finally
            ' Close database connection.
            CloseConnection()
            SqlCom.Dispose()
        End Try
        Return Result
    End Function
    ' Purpose: Return one table of in-memory data.
    Public Function GetDataTable(ByVal v_sSqlString As String) As DataTable
        Dim sTableName As String = String.Empty
        Try
            sTableName = v_sSqlString.Substring(14)
            sTableName = sTableName.Substring(0, sTableName.IndexOf(" ") + 1)
        Catch ex As Exception
        End Try

        Dim dtable As New DataTable(sTableName)
        SqlDa = New SqlDataAdapter(v_sSqlString, SqlCon)
        If OpenConnection() Then
            Try
                ' Configures the schema to match that in the data source based on 
                ' the specified System.Data.SchemaType
                'SqlDa.FillSchema(dtable, SchemaType.Source)
                SqlDa.Fill(dtable)
            Catch ex As SqlException
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Get Datatable")
            Finally
                ' Close database connection.
                CloseConnection()
                SqlDa.Dispose()
            End Try
        End If
        Return dtable
    End Function
    Public Function GetDataTable(ByVal TableName As String, ByVal Condition As String) As DataTable

        Dim dtable As New DataTable(TableName)
        SqlDa = New SqlDataAdapter("Select * from " & TableName & " Where " & Condition, SqlCon)
        If OpenConnection() Then
            Try
                SqlDa.Fill(dtable)
            Catch ex As SqlException
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Get DataTable")
            Finally
                ' Close database connection.
                CloseConnection()
                SqlDa.Dispose()
            End Try
        End If
        Return dtable
    End Function
  
    ' Calls the respective INSERT, UPDATE, or DELETE statements for each inserted,
    ' updated, or deleted row in the specified DataTable. if execute success
    ' return true else return false
    Public Function UpdateDataTable(ByVal v_sSqlString As String, ByVal v_dtable As  _
    System.Data.DataTable, Optional ByVal ShowErrorMessage As Boolean = False) As Boolean
        SqlDa = New SqlDataAdapter
        SqlDa.SelectCommand = New SqlCommand(v_sSqlString, SqlCon)
        ' Without the SQLCommandBuilder this line would fail.
        SqlComb = New SqlCommandBuilder(SqlDa)
        Try
            If OpenConnection() Then
                SqlDa.Update(v_dtable)
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            If ShowErrorMessage Then
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Warning")
            End If
            Return False
        Finally
            ' Close database connection.
            CloseConnection()
            SqlDa.Dispose()
            SqlComb.Dispose()
        End Try
    End Function
   
    'Purpose: Implements the IDispose' method Dispose.
    Protected Overloads Sub Dispose()
        CloseConnection()
        GC.SuppressFinalize(Me)
    End Sub

End Class
