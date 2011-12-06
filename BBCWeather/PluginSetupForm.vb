Imports Jayrock.Json
Imports MediaPortal.Configuration
Imports MediaPortal.Profile

Imports System.Net
Imports System.Web
Imports System.Windows.Forms

Public Class PluginSetupForm

    Private _searchNames As New List(Of String)
    Private _areaCodes As New List(Of String)
    Private _areaNames As New List(Of String)
    Private _areaCodeList As New List(Of String)
    Private _tempUnit As String = String.Empty
    Private _windUnit As String = String.Empty
    Private _locations As String = String.Empty
    Private _overRide As Boolean = False
    Private _interval As Integer = 0

    Private Sub PluginSetupForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        AppDomain.CurrentDomain.AppendPrivatePath(String.Format("{0}\Plugins\Windows\BBCWeather", AppDomain.CurrentDomain.BaseDirectory))

        Using xmlReader As Settings = New Settings(Config.GetFile(Config.Dir.Config, "MediaPortal.xml"))
            _tempUnit = xmlReader.GetValueAsString("BBCWeather", "tempUnit", "degC")
            _windUnit = xmlReader.GetValueAsString("BBCWeather", "windUnit", "mph")
            _interval = xmlReader.GetValueAsInt("BBCWeather", "interval", 15)
            _overRide = xmlReader.GetValueAsBool("BBCWeather", "overRide", False)
            _locations = xmlReader.GetValueAsString("BBCWeather", "locations", "")
        End Using

        Dim jo As JsonObject
        jo = CType(Conversion.JsonConvert.Import(_locations), JsonObject)

        For i As Integer = 0 To jo.Count - 1
            Dim location As JsonObject = jo.Item(jo.Names(i))
            _searchNames.Add(location.Item("searchName"))
            _areaCodes.Add(location.Item("areaCode"))
            _areaNames.Add(location.Item("areaName"))
            clbLocations.Items.Add(_areaNames(i))
        Next

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

        numInterval.Value = _interval

    End Sub

    Private Sub btnLookup_Click(sender As System.Object, e As System.EventArgs) Handles btnLookup.Click
        AreaLookup(tbxInput.Text)
    End Sub

    Private Sub AreaLookup(ByVal area As String)

        If Not MediaPortal.Util.Win32API.IsConnectedToInternet() Then
            MessageBox.Show("Cannot detect an Internet connection.", "BBC Weather", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End If

        Cursor = Windows.Forms.Cursors.WaitCursor

        Dim URL As String = String.Format("http://www.bbc.co.uk/locator/client/weather/en-GB/search.json?search={0}", HttpUtility.UrlEncode(area))
        Dim response As String = New WebClient().DownloadString(URL)
        Dim jo As JsonObject = CType(Conversion.JsonConvert.Import(response), JsonObject)

        Dim noOfResults As Integer = CType(jo("noOfResults"), JsonNumber)
        If noOfResults = 0 Then
            MessageBox.Show(String.Format("No results found for {0}.", area), "BBC Weather", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Cursor = Windows.Forms.Cursors.Arrow
            Exit Sub
        ElseIf noOfResults >= 100 Then
            MessageBox.Show(String.Format("More than 100 results returned for {0}. Displaying the first 100.{1}You may want to narrow your search.", area, vbCrLf), "BBC Weather", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

        clbResults.Items.Clear()
        _areaCodeList.Clear()

        Dim locationString As String = CType(jo("results"), String)
        Dim doc As New HtmlAgilityPack.HtmlDocument
        doc.LoadHtml(locationString)

        Dim nodes As HtmlAgilityPack.HtmlNodeCollection = doc.DocumentNode.SelectNodes("//li")
        For Each node As HtmlAgilityPack.HtmlNode In nodes
            clbResults.Items.Add(node.InnerText)
            _areaCodeList.Add(Split(node.ChildNodes(0).Attributes("href").Value, "/")(2))
        Next

        Dim noOfPages As Integer = System.Math.Ceiling(noOfResults / 10)
        If noOfPages > 1 Then
            For i As Integer = 2 To noOfPages
                URL = String.Format("http://www.bbc.co.uk/locator/client/weather/en-GB/search.json?search={0}&page={1}", HttpUtility.UrlEncode(area), i.ToString)
                response = New WebClient().DownloadString(URL)
                jo = CType(Conversion.JsonConvert.Import(response), JsonObject)
                locationString = CType(jo("results"), String)
                doc.LoadHtml(locationString)
                nodes = doc.DocumentNode.SelectNodes("//li")
                For Each node As HtmlAgilityPack.HtmlNode In nodes
                    clbResults.Items.Add(node.InnerText)
                    _areaCodeList.Add(Split(node.ChildNodes(0).Attributes("href").Value, "/")(2))
                Next
            Next
        End If

        'CheckSelectedListItem()

        Cursor = Windows.Forms.Cursors.Arrow
    End Sub

    Private Sub CheckSelectedListItem(areaName)
        Dim itemToBeChecked As Integer = 0

        For Each areaItem In clbResults.Items
            If areaName = areaItem Then
                itemToBeChecked = clbResults.Items.IndexOf(areaItem)
            End If
        Next
        clbResults.SetItemChecked(itemToBeChecked, True)
        clbResults.TopIndex = itemToBeChecked
    End Sub

    Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
        Dim areaCode As String = String.Empty
        Dim searchName As String = String.Empty
        Dim areaName As String = String.Empty
        For Each checkedItem In clbResults.CheckedItems
            areaCode = _areaCodes(clbResults.Items.IndexOf(checkedItem))
            areaName = checkedItem
        Next

        If areaCode <> "" Then
            Using jw As New JsonTextWriter
                jw.PrettyPrint = False
                jw.WriteStartObject()
                For i = 1 To 3
                    jw.WriteMember(i.ToString)
                    jw.WriteStartObject()
                    jw.WriteMember("searchName")
                    jw.WriteString(searchName)
                    jw.WriteMember("areaCode")
                    jw.WriteString(areaCode)
                    jw.WriteMember("areaName")
                    jw.WriteString(areaName)
                    jw.WriteEndObject()
                Next
                jw.WriteEndObject()
                Using xmlReader As Settings = New Settings(Config.GetFile(Config.Dir.Config, "MediaPortal.xml"))
                    xmlReader.SetValue("BBCWeather", "tempUnit", IIf(rbnDegF.Checked, "degF", "degC"))
                    xmlReader.SetValue("BBCWeather", "windUnit", IIf(rbnKph.Checked, "kph", "mph"))
                    xmlReader.SetValue("BBCWeather", "interval", numInterval.Value)
                    xmlReader.SetValueAsBool("BBCWeather", "overRide", cbxInfoService.Checked)
                    xmlReader.SetValue("BBCWeather", "locations", jw.ToString)
                    Settings.SaveCache()
                End Using
            End Using
        End If

        If cbxInfoService.Checked Then MessageBox.Show("You should open InfoService config now and remove the tick from the 'Weather information enabled' checkbox on the Weather tab. Then click Save.", "BBC Weather", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Me.Close()
    End Sub

    Private Sub clbResults_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles clbResults.ItemCheck
        If e.NewValue = CheckState.Checked Then
            For i As Integer = 0 To clbResults.Items.Count - 1
                If e.Index <> i Then
                    clbResults.SetItemChecked(i, False)
                End If
            Next
        End If
    End Sub

    Private Sub clbLocations_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles clbLocations.ItemCheck
        If e.NewValue = CheckState.Checked Then
            For i As Integer = 0 To clbLocations.Items.Count - 1
                If e.Index <> i Then
                    clbLocations.SetItemChecked(i, False)
                End If
            Next
        End If
    End Sub

    Private Sub PictureBox1_Click(sender As System.Object, e As System.EventArgs) Handles PictureBox1.Click
        System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=WDNKXZFMKR4MY")
    End Sub

    Private Sub btnRemove_Click(sender As System.Object, e As System.EventArgs) Handles btnRemove.Click
        Dim itemToRemove As Integer
        For Each checkedItem In clbLocations.CheckedItems
            itemToRemove = clbLocations.Items.IndexOf(checkedItem)
        Next
        _searchNames.RemoveAt(itemToRemove)
        _areaCodes.RemoveAt(itemToRemove)
        _areaNames.RemoveAt(itemToRemove)
        clbLocations.Items.RemoveAt(itemToRemove)
    End Sub

    Private Sub btnAdd_Click(sender As System.Object, e As System.EventArgs) Handles btnAdd.Click
        Dim itemToAdd As Integer = 0
        Dim nameToAdd As String = String.Empty
        For Each checkedItem In clbResults.CheckedItems
            itemToAdd = clbResults.Items.IndexOf(checkedItem)
            nameToAdd = checkedItem
        Next
        _searchNames.Add(tbxInput.Text)
        _areaCodes.Add(_areaCodeList(itemToAdd))
        _areaNames.Add(clbResults.Items.IndexOf(itemToAdd).ToString)
        clbLocations.Items.Add(nameToAdd)
    End Sub
End Class