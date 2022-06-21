Imports System
Imports System.IO.Ports
Imports System.IO

Public Class Form1
    Dim fileName As String
    Dim arduinoFlash As String
    Dim arduinoBackup As String
    Dim getDate As String = Date.Today
    Dim MyAvrudeConfPathName As String = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "avrude.conf")
    Dim MyAvrudeExePathName As String = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "avrude.exe")
    Dim MyLibusbPathName As String = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "libusb0.dll")
    'Dim MyBackupMsg As String = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), " !!!!!")

    'array of devices "devices.txt"
    Dim substringsArray As New List(Of String)()









    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        ' Declare Ports 
        Dim Ports As String() = IO.Ports.SerialPort.GetPortNames()
        ' Add port name Into a comboBox control
        Dim test As Integer = Ports.GetLength(test)
        If test <> 0 Then
            For Each Port In Ports
                ComboBox1.Items.Add(Port)
            Next Port
            ' Select an item in the combobox 
            ComboBox1.SelectedIndex = 0
        End If




        Label3.Text = "Disconnected, Please Select Correct COM and press Connect"
        Button2.Text = "Connect"

        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = 100
        ProgressBar1.Value = 0


        ComboBox3.Items.Clear()
        ' Declare Ports 
        Dim Ports2 As String() = IO.Ports.SerialPort.GetPortNames()
        ' Add port name Into a comboBox control
        Dim test2 As Integer = Ports2.GetLength(test2)
        If test2 <> 0 Then
            For Each Port In Ports2
                ComboBox3.Items.Add(Port)
            Next Port
            ' Select an item in the combobox 
            ComboBox3.SelectedIndex = 0
        End If

        'IO.File.WriteAllBytes(MyAvrudeConfPathName, My.Resources.avrdude_conf)
        'IO.File.WriteAllBytes(MyAvrudeExePathName, My.Resources.avrdude_exe)
        'IO.File.WriteAllBytes(MyLibusbPathName, My.Resources.libusb0)


        'ComboBox2.Items.AddRange(System.IO.File.ReadAllLines("devices.txt"))
        Dim reader = File.OpenText("devices.txt")
        Dim line As String = ""

        While (reader.Peek() <> -1)
            line = reader.ReadLine()
            substringsArray.Add(line)
            Dim substrings() As String = line.Split(";")
            ComboBox2.Items.Add(substrings(0))
        End While

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ComboBox1.Items.Clear()
        ' Declare Ports 
        Dim Ports As String() = IO.Ports.SerialPort.GetPortNames()
        ' Add port name Into a comboBox control
        Dim test As Integer = Ports.GetLength(test)
        If test <> 0 Then
            For Each Port In Ports
                ComboBox1.Items.Add(Port)
            Next Port
            ' Select an item in the combobox 
            ComboBox1.SelectedIndex = 0
        End If

        Label3.Text = "Updating COM Ports, Please Select Correct COM and press Connect"

    End Sub




    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        If ComboBox1.SelectedText.ToString() = "-1" Then
            Label3.Text = "Connected to COM, Please wait progress may take aprox 30 minutes."
            Button2.Text = "Abort"
            ProgressBar1.Value = 1

            Do

                Threading.Thread.Sleep(25000)
                ProgressBar1.PerformStep()

            Loop Until ProgressBar1.Value >= ProgressBar1.Maximum

        Else
            Label3.Text = "No COM Port Selected, Please select correct COM and Press Connect"

        End If

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Dim myFileDlog As New OpenFileDialog()

        'look for files in the c drive
        myFileDlog.InitialDirectory = "c:\"

        'specifies what type of data files to look for
        myFileDlog.Filter = "firmware (*.hex)|*.hex"

        'specifies which data type is focused on start up
        myFileDlog.FilterIndex = 2

        'Gets or sets a value indicating whether the dialog box restores the current directory before closing.
        myFileDlog.RestoreDirectory = True

        'seperates message outputs for files found or not found
        If myFileDlog.ShowDialog() =
            DialogResult.OK Then
            If Dir(myFileDlog.FileName) <> "" Then
                fileName = myFileDlog.FileName
                MsgBox("File to flash: " &
                       myFileDlog.FileName,
                       MsgBoxStyle.Information)
            Else
                MsgBox("File Not Found",
                       MsgBoxStyle.Critical)
            End If
        End If
        TextBox3.Text = fileName
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged

    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        Dim int As Integer = 0
        int = ComboBox2.SelectedIndex
        Dim line As String = substringsArray(int)
        Dim substrings() As String = line.Split(";")
        TextBox2.Text = substrings(3)
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ComboBox3.Items.Clear()
        ' Declare Ports 
        Dim Ports As String() = IO.Ports.SerialPort.GetPortNames()
        ' Add port name Into a comboBox control
        Dim test As Integer = Ports.GetLength(test)
        If test <> 0 Then
            For Each Port In Ports
                ComboBox3.Items.Add(Port)
            Next Port
            ' Select an item in the combobox 
            ComboBox3.SelectedIndex = 0
        End If
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click

        Dim int As Integer = 0
        int = ComboBox2.SelectedIndex

        If int <> -1 Then
            If fileName <> "" Then
                Dim line As String = substringsArray(int)
                Dim substrings() As String = line.Split(";")
                arduinoFlash = MyAvrudeExePathName + " -C " + MyAvrudeConfPathName + " -v -p " + substrings(1) + " -c " + substrings(2) + " -P " + ComboBox1.Text + " -b " + TextBox2.Text + " -D -Uflash:w:" + fileName + ":i"
                If fileName <> "" Then
                    Shell(arduinoFlash, vbNormalFocus)
                    TextBox3.Text = " Flashing device"

                End If
            Else
                If int <> -1 Then
                    TextBox3.Text = ""
                    TextBox3.Text = " Select COM Port and flash"

                Else
                    TextBox3.Text = ""
                    TextBox3.Text = " Select device and file"
                End If
            End If
        Else
            If fileName <> "" Then
                TextBox3.Text = ""
                TextBox3.Text = " Select COM Port and device"
            Else
                TextBox3.Text = ""
                TextBox3.Text = " Select device and file"
            End If
        End If

        'TextBox1.Text = arduinoFlash



    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        Dim int As Integer = 0
        int = ComboBox2.SelectedIndex

        If int <> -1 Then
            If ComboBox1.Text <> "" Then

                Dim line As String = substringsArray(int)
                Dim substrings() As String = line.Split(";")
                arduinoBackup = MyAvrudeExePathName + " -C " + MyAvrudeConfPathName + " -v -p " + substrings(1) + " -c " + substrings(2) + " -P " + ComboBox1.Text + " -b " + TextBox2.Text + " -D -U flash:r:backup_" + getDate + ".hex:i"

                If ComboBox1.Text <> "" Then
                    Shell(arduinoBackup, vbNormalFocus)
                    TextBox3.Text = " Backup saved to backup_" + getDate + ".hex"
                End If
            Else
                If int <> -1 Then
                    TextBox3.Text = ""
                    TextBox3.Text = " Select COM Port"

                Else
                    TextBox3.Text = ""
                    TextBox3.Text = " --------"
                End If
            End If
        Else
            If ComboBox1.Text <> "" Then
                TextBox3.Text = ""
                TextBox3.Text = " Select device"
            Else
                TextBox3.Text = ""
                TextBox3.Text = " Select device and COM Port"
            End If
        End If

        'TextBox1.Text = arduinoFlash


    End Sub

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged

    End Sub
End Class
