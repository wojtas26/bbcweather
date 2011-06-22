Imports Jayrock.Json
Imports MediaPortal.Configuration
Imports MediaPortal.Profile

Imports System.Net
Imports System.Web
Imports System.Windows.Forms


Public Class PluginSetupForm
    '==============================================================================================================
    ' http://news.bbc.co.uk/weather/util/search/WeatherSuggestJSON.json?region=uk&search=stoke = json loction check
    '
    ' region can be uk or world
    '==============================================================================================================
    Private _ja As JsonArray
    Private _areaCode As String = String.Empty
    Private _areaName As String = String.Empty
    Private _tempUnit As String = String.Empty
    Private _windUnit As String = String.Empty
    Private _overRide As Boolean = False

    Private Sub PluginSetupForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        AppDomain.CurrentDomain.AppendPrivatePath(String.Format("{0}\Plugins\Windows\BBCWeather", AppDomain.CurrentDomain.BaseDirectory))

        Using xmlReader As Settings = New Settings(Config.GetFile(Config.Dir.Config, "MediaPortal.xml"))
            _areaCode = xmlReader.GetValueAsString("BBCWeather", "areaCode", "8")
            _areaName = xmlReader.GetValueAsString("BBCWeather", "areaName", "London")
            _tempUnit = xmlReader.GetValueAsString("BBCWeather", "tempUnit", "degC")
            _windUnit = xmlReader.GetValueAsString("BBCWeather", "windUnit", "mph")
            _overRide = xmlReader.GetValueAsBool("BBCWeather", "overRide", False)
        End Using

        If _areaName.Length > 0 Then AreaLookup(_areaName)

        If _tempUnit.ToLower = "degc" Then
            rbnDegC.Checked = True
            rbnDegF.Checked = False
        Else
            rbnDegC.Checked = False
            rbnDegF.Checked = True
        End If

        If _windUnit.ToLower = "mph" Then
            rbnMph.Checked = True
            rbnKph.Checked = False
        Else
            rbnMph.Checked = False
            rbnKph.Checked = True
        End If

        cbxInfoService.Checked = _overRide

    End Sub

    Private Sub btnLookup_Click(sender As System.Object, e As System.EventArgs) Handles btnLookup.Click

        AreaLookup(tbxInput.Text)

    End Sub

    Private Sub AreaLookup(ByVal area As String)
        Cursor = Windows.Forms.Cursors.WaitCursor
        Dim URL As String = String.Format("http://news.bbc.co.uk/weather/util/search/WeatherSuggestJSON.json?region=uk&search={0}", HttpUtility.UrlEncode(area))
        Dim response As String = New WebClient().DownloadString(URL)

        'Populate checklistbox with options
        _ja = CType(Conversion.JsonConvert.Import(response), JsonArray)
        clbResults.Items.Clear()
        Dim i As Integer = 0
        For Each obj In _ja(1)
            clbResults.Items.Add(String.Format("{0} / {1}", obj, _ja(2)(i)))
            i += 1
        Next

        If clbResults.Items.Count = 0 Then
            MessageBox.Show(String.Format("No results found for {0}.{1}Try a different location or postcode.", tbxInput.Text, vbCrLf), "BBC Weather", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            CheckSelectedListItem()
        End If

        Cursor = Windows.Forms.Cursors.Arrow
    End Sub

    Private Sub CheckSelectedListItem()
        Dim itemToBeChecked As Integer = 0

        For Each areaItem In clbResults.Items
            If _areaCode = _ja(3)(clbResults.Items.IndexOf(areaItem)) Then
                itemToBeChecked = clbResults.Items.IndexOf(areaItem)
            End If
        Next
        clbResults.SetItemChecked(itemToBeChecked, True)
    End Sub

    'Only allow a single listitem to be ticked
    Private Sub clbResults_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles clbResults.ItemCheck
        If e.NewValue = CheckState.Checked Then
            For i As Integer = 0 To clbResults.Items.Count - 1
                If e.Index <> i Then
                    clbResults.SetItemChecked(i, False)
                End If
            Next
        End If
    End Sub

    Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
        For Each checkedItem In clbResults.CheckedItems
            _areaCode = _ja(3)(clbResults.Items.IndexOf(checkedItem))
            _areaName = _ja(1)(clbResults.Items.IndexOf(checkedItem))
        Next
        If _areaCode <> "" Then
            Using xmlReader As Settings = New Settings(Config.GetFile(Config.Dir.Config, "MediaPortal.xml"))
                xmlReader.SetValue("BBCWeather", "areaCode", _areaCode)
                xmlReader.SetValue("BBCWeather", "areaName", _areaName)
                xmlReader.SetValue("BBCWeather", "tempUnit", IIf(rbnDegF.Checked, "degF", "degC"))
                xmlReader.SetValue("BBCWeather", "windUnit", IIf(rbnKph.Checked, "kph", "mph"))
                xmlReader.SetValueAsBool("BBCWeather", "overRide", cbxInfoService.Checked)
                Settings.SaveCache()
            End Using
        End If
        Me.Close()
    End Sub


    Private Sub PictureBox1_Click(sender As System.Object, e As System.EventArgs) Handles PictureBox1.Click
        System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=WDNKXZFMKR4MY")
    End Sub

End Class