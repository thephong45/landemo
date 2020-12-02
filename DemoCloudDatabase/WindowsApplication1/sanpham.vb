Imports System.Data.SqlClient
Imports System.Data.DataSet
Public Class frmsanpham
    Dim db As New DataTable
    Dim chuoiketnoi As String = "Data Source=DESKTOP-JS828RS;Initial Catalog=Cloud_Database;Integrated Security=True"
    Dim conn As SqlConnection = New SqlConnection(chuoiketnoi)
    Private DBAccess As New DataBaseAccess
    'dinh nghia ham them san pham vao database
    Private Function themsp() As Boolean
        Dim lenhsql As String = "INSERT INTO SANPHAM(masp, tensp, giaca, maloai)"
        lenhsql += String.Format("VALUES('{0}','{1}','{2}','{3}')", _
                                 txtmasp.Text, txttensp.Text, txtgia.Text, txtmaloai.Text)
        Return DBAccess.ExecuteNoneQuery(lenhsql)
    End Function
    'dinh nghia ham kiem tra gia tri truoc khi insert
    Private Function rong() As Boolean
        Return (String.IsNullOrEmpty(txtmasp.Text) OrElse String.IsNullOrEmpty(txttensp.Text) OrElse _
                String.IsNullOrEmpty(txtmaloai.Text) OrElse String.IsNullOrEmpty(txtgia.Text))
    End Function
    Public Function Loadsanpham() As DataSet
        Dim chuoiketnoi As String = "Data Source=DESKTOP-JS828RS;Initial Catalog=Cloud_Database;Integrated Security=True"
        Dim conn As SqlConnection = New SqlConnection(chuoiketnoi)
        Dim LoadSP As New SqlDataAdapter("select SANPHAM.masp as 'Mã sản phẩm',SANPHAM.tensp as 'Tên sản phẩm', SANPHAM.maloai as 'Mã Loại', LOAISANPHAM.tenloai as 'Tên Loại', giaca as 'Gía Tiền' from SANPHAM inner join LOAISANPHAM on SANPHAM.MASP = LOAISANPHAM.maloai", conn)
        Dim db As New DataSet
        conn.Open()
        LoadSP.Fill(db)
        conn.Close()
        Return db
    End Function
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim sqlquery As String = String.Format("select masp as 'Mã sản phẩm', tensp as 'Tên sản phẩm', maloai as 'Mã Loại', giaca as 'Gía Tiền' from SANPHAM ")
        Dim dtable As DataTable = DBAccess.GetDataTable(sqlquery)
        Me.dgvsp.DataSource = dtable
        With Me.dgvsp
            .Columns(0).Width = 100
            .Columns(1).Width = 250
            .Columns(2).Width = 100
            .Columns(3).Width = 120
        End With
    End Sub


    Private Sub frmsanpham_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim sqlquery As String = String.Format("select masp as 'Mã sản phẩm', tensp as 'Tên sản phẩm', maloai as 'Mã Loại', giaca as 'Gía Tiền' from SANPHAM ")
        Dim dtable As DataTable = DBAccess.GetDataTable(sqlquery)
        Me.dgvsp.DataSource = dtable
        With Me.dgvsp
            .Columns(0).Width = 100
            .Columns(1).Width = 250
            .Columns(2).Width = 100
            .Columns(3).Width = 120
        End With
    End Sub

    Private Sub searchsp(value As String)
        Dim sqlquery As String = String.Format("select masp as 'Mã sản phẩm', tensp as 'Tên sản phẩm', maloai as 'Mã Loại', giaca as 'Gía Tiền' from SANPHAM ")
        If Me.cmbsearch.SelectedIndex = 0 Then 'tìm theo masp
            sqlquery += String.Format("WHERE masp LIKE '{0}%'", value)
        ElseIf Me.cmbsearch.SelectedIndex = 1 Then 'tìm theo tensp
            sqlquery += String.Format("WHERE tensp LIKE '{0}%'", value)
        End If
        Dim dtable As DataTable = DBAccess.GetDataTable(sqlquery)
        Me.dgvsp.DataSource = dtable
        With Me.dgvsp
            .Columns(0).Width = 100
            .Columns(1).Width = 250
            .Columns(2).Width = 100
            .Columns(3).Width = 120
        End With
    End Sub

    Private Sub txtsearch_TextChanged(sender As Object, e As EventArgs) Handles txtsearch.TextChanged
        searchsp(Me.txtsearch.Text)
    End Sub

    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        If rong() Then
            MessageBox.Show("Thiếu dữ liệu", "Error", MessageBoxButtons.OK)
        Else
            If themsp() Then
                MessageBox.Show("Thêm dữ liệu thành công", "OK", MessageBoxButtons.OK)
                Me.DialogResult = Windows.Forms.DialogResult.OK
                searchsp(Me.txtsearch.Text)
            Else
                MessageBox.Show("Lỗi", "Error", MessageBoxButtons.OK)
                Me.DialogResult = Windows.Forms.DialogResult.No
            End If
        End If
    End Sub

    Private Sub dgvsp_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvsp.CellContentClick
        Dim click As Integer = dgvsp.CurrentCell.RowIndex
        txtmasp.Text = dgvsp.Item(0, click).Value
        txttensp.Text = dgvsp.Item(1, click).Value
        txtmaloai.Text = dgvsp.Item(2, click).Value
        txtgia.Text = dgvsp.Item(3, click).Value
    End Sub

    Private Sub btnedit_Click(sender As Object, e As EventArgs) Handles btnedit.Click
        Dim conn As SqlConnection = New SqlConnection(chuoiketnoi)
        Dim updatequery As String = "update SANPHAM set masp=@MASP, tensp=@TENSP, giaca=@GIACA, maloai=@MALOAI where MASP=@MASP"
        Dim addupdate As SqlCommand = New SqlCommand(updatequery, conn)
        conn.Open()
        Try
            txtmasp.Focus()
            If txtmasp.Text = "" Then
                MessageBox.Show("Bạn chưa nhập mã sản phẩm", "Nhập thiếu", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                txtmasp.Focus()
                If txttensp.Text = "" Then
                    MessageBox.Show("Bạn chưa nhập tên sản phẩm", "Nhập thiếu", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Else
                    txttensp.Focus()
                    If txtmaloai.Text = "" Then
                        MessageBox.Show("Bạn chưa nhập mã loại", "Nhập thiếu", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Else
                        txtmaloai.Focus()
                        If txtgia.Text = "" Then
                            MessageBox.Show("Bạn chưa nhập số lượng", "Nhập thiếu", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Else
                            addupdate.Parameters.AddWithValue("@MASP", txtmasp.Text)
                            addupdate.Parameters.AddWithValue("@TENSP", txttensp.Text)
                            addupdate.Parameters.AddWithValue("@MALOAI", txtmaloai.Text)
                            addupdate.Parameters.AddWithValue("@GIACA", txtgia.Text)
                            addupdate.ExecuteNonQuery()
                            conn.Close() 'đóng kết nối
                            MessageBox.Show("Cập nhật thành công")
                            txtmasp.Text = Nothing
                            txttensp.Text = Nothing
                            txtmaloai.Text = Nothing

                            txtgia.Text = Nothing
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Không thành công", "Lỗi", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error)
        End Try

        'Sau khi cập nhật xong sẽ tự làm mới lại bảng
        db.Clear()
        dgvsp.DataSource = db
        dgvsp.DataSource = Nothing
        searchsp(Me.txtsearch.Text)
    End Sub

    Private Sub btnxoa_Click(sender As Object, e As EventArgs) Handles btnxoa.Click
        Dim delquery As String = "delete from SANPHAM where MASP=@MASP"
        Dim delete As SqlCommand = New SqlCommand(delquery, conn)
        Dim resulft As DialogResult = MessageBox.Show("Bạn muốn xóa không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        conn.Open()
        Try
            If txtmasp.Text = "" Then
                MessageBox.Show("Bạn cần nhập mã khách hàng", "Nhập thiếu", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                txtmasp.Focus()
            Else
                If resulft = Windows.Forms.DialogResult.Yes Then
                    delete.Parameters.AddWithValue("@MASP", txtmasp.Text)
                    delete.ExecuteNonQuery()
                    conn.Close()
                    MessageBox.Show("Xóa thành công")
                    'Sau khi xóa thành công, tự động làm mới các khung textbox
                    txtmasp.Text = Nothing
                    txttensp.Text = Nothing
                    txtmaloai.Text = Nothing
                    txtgia.Text = Nothing
                    txtmasp.Focus()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Nhập đúng mã sản phẩm cần xóa", "Lỗi", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error)
        End Try

        'làm mới bảng
        db.Clear()
        dgvsp.DataSource = db
        dgvsp.DataSource = Nothing
        searchsp(Me.txtsearch.Text)
    End Sub


End Class