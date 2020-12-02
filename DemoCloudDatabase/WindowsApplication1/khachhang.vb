Imports System.Data.SqlClient
Imports System.Data.DataSet
Public Class frmkhachhang
    Dim db As New DataTable
    Dim chuoiketnoi As String = "Data Source=DESKTOP-JS828RS;Initial Catalog=Cloud_Database;Integrated Security=True"
    Dim conn As SqlConnection = New SqlConnection(chuoiketnoi)
    Private DBAccess As New DataBaseAccess

    'dinh nghia ham them san pham vao database
    Private Function themsp() As Boolean
        Dim lenhsql As String = "INSERT INTO KHACHHANG(makh, hoten, diachi, sdt)"
        lenhsql += String.Format("VALUES('{0}','{1}','{2}','{3}')", _
                                 txtmakh.Text, txttenkh.Text, txtdiachi.Text, txtsodt.Text)
        Return DBAccess.ExecuteNoneQuery(lenhsql)
    End Function
    'dinh nghia ham kiem tra gia tri truoc khi insert
    Private Function rong() As Boolean
        Return (String.IsNullOrEmpty(txtmakh.Text) OrElse String.IsNullOrEmpty(txttenkh.Text) OrElse _
                String.IsNullOrEmpty(txtdiachi.Text) OrElse String.IsNullOrEmpty(txtsodt.Text))
    End Function
    Public Function Loadkhachang() As DataSet
        Dim chuoiketnoi As String = "Data Source=DESKTOP-JS828RS;Initial Catalog=Cloud_Database;Integrated Security=True"
        Dim conn As SqlConnection = New SqlConnection(chuoiketnoi)
        Dim LoadKH As New SqlDataAdapter("select makh as 'Mã KH' ,hoten as 'Tên Khách Hàng', diachi as 'Địa chỉ', sdt as 'SĐT' from KHACHANG", conn)
        Dim db As New DataSet
        conn.Open()
        LoadKH.Fill(db)
        conn.Close()
        Return db
    End Function
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnhienthi.Click
        Dim sqlquery As String = String.Format("select makh as 'Mã khách hàng',hoten as 'Tên khách hàng', diachi as 'Địa chỉ', sdt as 'Số điện thoại' from KHACHHANG")
        Dim dtable As DataTable = DBAccess.GetDataTable(sqlquery)
        Me.dgvkh.DataSource = dtable
        With Me.dgvkh
            .Columns(0).Width = 100
            .Columns(1).Width = 200
            .Columns(2).Width = 200
            .Columns(3).Width = 100
        End With
    End Sub


    Private Sub frmkhachhang_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim sqlquery As String = String.Format("select makh as 'Mã khách hàng',hoten as 'Tên khách hàng', diachi as 'Địa chỉ', sdt as 'Số điện thoại' from KHACHHANG")
        Dim dtable As DataTable = DBAccess.GetDataTable(sqlquery)
        Me.dgvkh.DataSource = dtable
        With Me.dgvkh
            .Columns(0).Width = 100
            .Columns(1).Width = 200
            .Columns(2).Width = 200
            .Columns(3).Width = 100
        End With
    End Sub

    Private Sub searchkh(value As String)
        Dim sqlquery As String = String.Format("select makh as 'Mã khách hàng',hoten as 'Tên khách hàng', diachi as 'Địa chỉ', sdt as 'Số điện thoại' from KHACHHANG ")
        If Me.cmbsearch.SelectedIndex = 0 Then 'tìm theo masp
            sqlquery += String.Format("WHERE makh LIKE '{0}%'", value)
        ElseIf Me.cmbsearch.SelectedIndex = 1 Then 'tìm theo tensp
            sqlquery += String.Format("WHERE hoten LIKE '{0}%'", value)
        End If
        Dim dtable As DataTable = DBAccess.GetDataTable(sqlquery)
        Me.dgvkh.DataSource = dtable
        With Me.dgvkh
            .Columns(0).Width = 100
            .Columns(1).Width = 200
            .Columns(2).Width = 200
            .Columns(3).Width = 100
        End With
    End Sub

    Private Sub txtsearch_TextChanged(sender As Object, e As EventArgs) Handles txtsearch.TextChanged
        searchkh(Me.txtsearch.Text)
    End Sub

    Private Sub btnadd_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        If rong() Then
            MessageBox.Show("Thiếu dữ liệu", "Error", MessageBoxButtons.OK)
        Else
            If themsp() Then
                MessageBox.Show("Thêm dữ liệu thành công", "OK", MessageBoxButtons.OK)
                Me.DialogResult = Windows.Forms.DialogResult.OK
                searchkh(Me.txtsearch.Text)
            Else
                MessageBox.Show("Lỗi", "Error", MessageBoxButtons.OK)
                Me.DialogResult = Windows.Forms.DialogResult.No
            End If
        End If
    End Sub

    Private Sub dgvkh_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvkh.CellContentClick
        Dim click As Integer = dgvkh.CurrentCell.RowIndex
        txtmakh.Text = dgvkh.Item(0, click).Value
        txttenkh.Text = dgvkh.Item(1, click).Value
        txtdiachi.Text = dgvkh.Item(2, click).Value
        txtsodt.Text = dgvkh.Item(3, click).Value
    End Sub

    Private Sub btnedit_Click(sender As Object, e As EventArgs) Handles btnedit.Click
        Dim conn As SqlConnection = New SqlConnection(chuoiketnoi)
        Dim updatequery As String = "update KHACHHANG set makh=@MAKH, hoten=@TENKH, diachi=@DIACHI, sdt=@SDT where makh=@MAKH"
        Dim addupdate As SqlCommand = New SqlCommand(updatequery, conn)
        conn.Open()
        Try
            txtmakh.Focus()
            If txtmakh.Text = "" Then
                MessageBox.Show("Bạn chưa nhập mã sản phẩm", "Nhập thiếu", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
            Else
                txtmakh.Focus()
                If txttenkh.Text = "" Then
                    MessageBox.Show("Bạn chưa nhập tên sản phẩm", "Nhập thiếu", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                Else
                    txttenkh.Focus()
                    If txtdiachi.Text = "" Then
                        MessageBox.Show("Bạn chưa nhập mã loại", "Nhập thiếu", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                    Else
                        txtdiachi.Focus()
                        If txtsodt.Text = "" Then
                            MessageBox.Show("Bạn chưa nhập số lượng", "Nhập thiếu", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                        Else
                            addupdate.Parameters.AddWithValue("@MAKH", txtmakh.Text)
                            addupdate.Parameters.AddWithValue("@TENKH", txttenkh.Text)
                            addupdate.Parameters.AddWithValue("@DIACHI", txtdiachi.Text)
                            addupdate.Parameters.AddWithValue("@SDT", txtsodt.Text)
                            addupdate.ExecuteNonQuery()
                            conn.Close() 'đóng kết nối
                            MessageBox.Show("Cập nhật thành công")
                            txtmakh.Text = Nothing
                            txttenkh.Text = Nothing
                            txtdiachi.Text = Nothing
                            txtsodt.Text = Nothing
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Không thành công", "Lỗi", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error)
        End Try

        'Sau khi cập nhật xong sẽ tự làm mới lại bảng
        db.Clear()
        dgvkh.DataSource = db
        dgvkh.DataSource = Nothing
        searchkh(Me.txtsearch.Text)
    End Sub

    Private Sub btnxoa_Click(sender As Object, e As EventArgs) Handles btnxoa.Click
        Dim conn As SqlConnection = New SqlConnection(chuoiketnoi)
        Dim delquery As String = "delete from KHACHHANG where makh=@MAKH"
        Dim delete As SqlCommand = New SqlCommand(delquery, conn)
        Dim resulft As DialogResult = MessageBox.Show("Bạn muốn xóa không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        conn.Open()
        Try
            If txtmakh.Text = "" Then
                MessageBox.Show("Bạn cần nhập mã khách hàng", "Nhập thiếu", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)
                txtmakh.Focus()
            Else
                If resulft = Windows.Forms.DialogResult.Yes Then
                    delete.Parameters.AddWithValue("@MAKH", txtmakh.Text)
                    delete.ExecuteNonQuery()
                    conn.Close()
                    MessageBox.Show("Xóa thành công")
                    'Sau khi xóa thành công, tự động làm mới các khung textbox
                    txtmakh.Text = Nothing
                    txttenkh.Text = Nothing
                    txtdiachi.Text = Nothing
                    txtsodt.Text = Nothing
                    txtmakh.Focus()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Nhập đúng ma khach hang cần xóa", "Lỗi", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error)
        End Try

        'làm mới bảng
        db.Clear()
        dgvkh.DataSource = db
        dgvkh.DataSource = Nothing
        searchkh(Me.txtsearch.Text)
    End Sub
End Class