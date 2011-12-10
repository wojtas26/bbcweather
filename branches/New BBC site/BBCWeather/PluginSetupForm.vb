Imports Jayrock.Json
Imports MediaPortal.Configuration
Imports MediaPortal.Profile

Imports System.Net
Imports System.Web
Imports System.Windows.Forms
Imports System.Text.RegularExpressions

Public Class PluginSetupForm

    Private Class BBCLocation
        Public LocationID As String
        Public LocationName As String
        Public ForecastID As String
    End Class

    Private _SelectedBBCLocations As New List(Of BBCLocation)
    Private _AllBBCLocations As New List(Of BBCLocation)
    Private _jsonLocations As String = String.Empty
    Private _tempUnit As String = String.Empty
    Private _windUnit As String = String.Empty
    Private _overRide As Boolean = False
    Private _interval As Integer = 0

    Private Sub PluginSetupForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        AppDomain.CurrentDomain.AppendPrivatePath(String.Format("{0}\Plugins\Windows\BBCWeather", AppDomain.CurrentDomain.BaseDirectory))

        Using xmlReader As Settings = New Settings(Config.GetFile(Config.Dir.Config, "MediaPortal.xml"))
            _tempUnit = xmlReader.GetValueAsString("BBCWeather", "tempUnit", "degC")
            _windUnit = xmlReader.GetValueAsString("BBCWeather", "windUnit", "mph")
            _interval = xmlReader.GetValueAsInt("BBCWeather", "interval", 15)
            _overRide = xmlReader.GetValueAsBool("BBCWeather", "overRide", False)
            _jsonLocations = xmlReader.GetValueAsString("BBCWeather", "locations", "")
        End Using

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

        Dim jo As JsonObject
        jo = CType(Conversion.JsonConvert.Import(_jsonLocations), JsonObject)
        If jo.Count = 0 Then Exit Sub

        Dim BBCLocation As New BBCLocation
        For i As Integer = 0 To jo.Count - 1
            Dim location As JsonObject = jo.Item(jo.Names(i))
            BBCLocation.LocationID = location.Item("LocationID")
            BBCLocation.LocationName = location.Item("LocationName")
            BBCLocation.ForecastID = location.Item("ForecastID")
            _SelectedBBCLocations.Add(BBCLocation)
            clbSelectedBBCLocations.Items.Add(BBCLocation.LocationName)
        Next

    End Sub

    Private Sub btnLookup_Click(sender As System.Object, e As System.EventArgs) Handles btnLookup.Click
        AreaLookup(tbxInput.Text)
    End Sub

    Private Sub AreaLookup(ByVal area As String)

        If Not MediaPortal.Util.Win32API.IsConnectedToInternet() Then
            MessageBox.Show("Cannot detect an Internet connection.", "BBC Weather", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        ElseIf area.Length = 0 Then
            MessageBox.Show("Please supply a location.", "BBC Weather", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
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

        clbAllBBCLocations.Items.Clear()
        _AllBBCLocations.Clear()

        Dim locationString As String = CType(jo("results"), String)
        Dim doc As New HtmlAgilityPack.HtmlDocument
        doc.LoadHtml(locationString)

        Dim nodes As HtmlAgilityPack.HtmlNodeCollection = doc.DocumentNode.SelectNodes("//li")
        For Each node As HtmlAgilityPack.HtmlNode In nodes
            Dim BBCLocation As New BBCLocation
            BBCLocation.LocationID = Split(node.ChildNodes(0).Attributes("href").Value, "/")(2)
            BBCLocation.LocationName = node.InnerText
            BBCLocation.ForecastID = ""
            _AllBBCLocations.Add(BBCLocation)
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
                    Dim BBCLocation As New BBCLocation
                    BBCLocation.LocationID = Split(node.ChildNodes(0).Attributes("href").Value, "/")(2)
                    BBCLocation.LocationName = node.InnerText
                    BBCLocation.ForecastID = ""
                    _AllBBCLocations.Add(BBCLocation)
                Next
            Next
        End If

        For Each BBCLocation In _AllBBCLocations
            clbAllBBCLocations.Items.Add(BBCLocation.LocationName)
        Next

        Cursor = Windows.Forms.Cursors.Arrow
    End Sub

    Private Function GetForecastID(ByVal locationID As String) As String

        Dim forecastID As String = String.Empty

        If Not MediaPortal.Util.Win32API.IsConnectedToInternet() Then
            MessageBox.Show("Cannot detect an Internet connection.", "BBC Weather", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return forecastID
        End If

        Dim URL As String = String.Format("http://www.bbc.co.uk/weather/{0}", HttpUtility.UrlEncode(locationID))
        Dim response As String = New WebClient().DownloadString(URL)

        Dim r As New Regex("Forecast ID: (?<locid>\d+) -->")
        Dim m As Match = r.Match(response)

        If m.Success Then forecastID = r.Match(response).Result("${locid}")

        Return forecastID

    End Function

    Private Sub CheckSelectedListItem(areaName)
        Dim itemToBeChecked As Integer = 0

        For Each areaItem In clbAllBBCLocations.Items
            If areaName = areaItem Then
                itemToBeChecked = clbAllBBCLocations.Items.IndexOf(areaItem)
            End If
        Next
        clbAllBBCLocations.SetItemChecked(itemToBeChecked, True)
        clbAllBBCLocations.TopIndex = itemToBeChecked
    End Sub

    Private Sub clbAllBBCLocations_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles clbAllBBCLocations.ItemCheck
        If e.NewValue = CheckState.Checked Then
            For i As Integer = 0 To clbAllBBCLocations.Items.Count - 1
                If e.Index <> i Then
                    clbAllBBCLocations.SetItemChecked(i, False)
                End If
            Next
        End If
    End Sub

    Private Sub clbSelectedBBCLocations_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles clbSelectedBBCLocations.ItemCheck
        If e.NewValue = CheckState.Checked Then
            For i As Integer = 0 To clbSelectedBBCLocations.Items.Count - 1
                If e.Index <> i Then
                    clbSelectedBBCLocations.SetItemChecked(i, False)
                End If
            Next
        End If
    End Sub

    Private Sub btnRemove_Click(sender As System.Object, e As System.EventArgs) Handles btnRemove.Click
        Dim nameToRemove As String = String.Empty
        For Each checkedItem In clbSelectedBBCLocations.CheckedItems
            nameToRemove = checkedItem
        Next
        If nameToRemove = String.Empty Then Exit Sub

        For Each BBCLocation As BBCLocation In _SelectedBBCLocations
            If BBCLocation.LocationName = nameToRemove Then
                _SelectedBBCLocations.Remove(BBCLocation)
                clbSelectedBBCLocations.Items.Remove(nameToRemove)
                Exit Sub
            End If
        Next

    End Sub

    Private Sub btnAdd_Click(sender As System.Object, e As System.EventArgs) Handles btnAdd.Click
        Dim nameToAdd As String = String.Empty
        For Each checkedItem In clbAllBBCLocations.CheckedItems
            nameToAdd = checkedItem
        Next
        If nameToAdd = String.Empty Then Exit Sub

        For Each item In clbSelectedBBCLocations.Items
            If nameToAdd = item Then
                MessageBox.Show("Location aready added.", "BBC Weather", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
        Next
        Dim BBCLocation As BBCLocation = Nothing
        For Each BBCLocation In _AllBBCLocations
            If BBCLocation.LocationName = nameToAdd Then
                _SelectedBBCLocations.Add(BBCLocation)
                clbSelectedBBCLocations.Items.Add(nameToAdd)
                Exit Sub
            End If
        Next

    End Sub

    Private Sub btnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click

        Cursor = Windows.Forms.Cursors.WaitCursor

        Using xmlReader As Settings = New Settings(Config.GetFile(Config.Dir.Config, "MediaPortal.xml"))
            xmlReader.SetValue("BBCWeather", "tempUnit", IIf(rbnDegF.Checked, "degF", "degC"))
            xmlReader.SetValue("BBCWeather", "windUnit", IIf(rbnKph.Checked, "kph", "mph"))
            xmlReader.SetValue("BBCWeather", "interval", numInterval.Value)
            xmlReader.SetValueAsBool("BBCWeather", "overRide", cbxInfoService.Checked)
        End Using

        Using jw As New JsonTextWriter
            jw.PrettyPrint = False
            jw.WriteStartObject()

            For Each BBCLocation In _SelectedBBCLocations
                jw.WriteMember(_SelectedBBCLocations.IndexOf(BBCLocation))
                jw.WriteStartObject()
                jw.WriteMember("LocationID")
                jw.WriteString(BBCLocation.LocationID)
                jw.WriteMember("LocationName")
                jw.WriteString(BBCLocation.LocationName)
                jw.WriteMember("ForecastID")
                jw.WriteString(GetForecastID(BBCLocation.LocationID))
                jw.WriteEndObject()
            Next
            jw.WriteEndObject()
            Using xmlReader As Settings = New Settings(Config.GetFile(Config.Dir.Config, "MediaPortal.xml"))
                xmlReader.SetValue("BBCWeather", "locations", jw.ToString)
                Settings.SaveCache()
            End Using
        End Using

        If cbxInfoService.Checked Then MessageBox.Show("You should open InfoService config now and remove the tick from the 'Weather information enabled' checkbox on the Weather tab. Then click Save.", "BBC Weather", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Cursor = Windows.Forms.Cursors.Arrow
        Me.Close()
    End Sub

    Private Sub PictureBox1_Click(sender As System.Object, e As System.EventArgs) Handles PictureBox1.Click
        System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=WDNKXZFMKR4MY")
    End Sub

End Class