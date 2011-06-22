Imports System
Imports System.IO
Imports System.Net
Imports System.Xml
Imports System.Text
Imports System.Drawing
Imports System.Drawing.Drawing2D

Imports System.Threading
Imports System.Windows.Forms
Imports System.Runtime.CompilerServices

Imports Jayrock.Json

Imports MediaPortal.GUI.Library
Imports MediaPortal.Dialogs
Imports MediaPortal.Configuration
Imports MediaPortal.Profile
Imports MediaPortal.Util

<PluginIcons("BBCWeather.bbc_on.png", "BBCWeather.bbc_off.png")>
Public Class BBCWeatherPlugin
    Inherits GUIWindow
    Implements ISetupForm
    Implements IShowPlugin

#Region "Structures"

    Private Structure ForeCast
        Public DayName As String
        Public Summary As String
        Public MaxTemp As String
        Public MinTemp As String
        Public WindDirection As String
        Public WindSpeed As String
        Public Humidity As String
        Public Pressure As String
        Public Visibility As String
        Public SunRise As String
        Public SunSet As String
    End Structure

    Private Structure MonthForeCast
        Public publishedDate As String
        Public nextUpdate As String
        Public author As String
        Public headline As String
        Public summary As String
        Public period1 As String
        Public periodSummary1 As String
        Public periodDetail1 As String
        Public period2 As String
        Public periodSummary2 As String
        Public periodDetail2 As String
        Public period3 As String
        Public periodSummary3 As String
        Public periodDetail3 As String
    End Structure

#End Region

#Region "Enums"

    Private Enum Mode
        FiveDay
        TwentyFourHours
        Monthly
        Maps
    End Enum

    Private Enum Controls

        LBL_LOCATION = 51

        LBL_CURRENT_SUMMARY_STATIC = 100
        LBL_CURRENT_SUMMARY = 101
        LBL_CURRENT_TEMP_STATIC = 102
        LBL_CURRENT_TEMP = 103
        LBL_CURRENT_WIND_STATIC = 104
        LBL_CURRENT_WIND = 105
        LBL_CURRENT_HUMIDITY_STATIC = 106
        LBL_CURRENT_HUMIDITY = 107
        LBL_CURRENT_PRESSURE_STATIC = 108
        LBL_CURRENT_PRESSURE = 109
        LBL_CURRENT_VISIBILITY_STATIC = 110
        LBL_CURRENT_VISIBILITY = 111
        LBL_CURRENT_OBS_TIME_STATIC = 112
        LBL_CURRENT_OBS_TIME = 113
        LBL_CURRENT_OBS_STATION_STATIC = 114
        LBL_CURRENT_OBS_STATION = 115
        LBL_CURRENT_OBS_HEADING = 116

        LBL_DAY0_DAYNAME = 206
        IMG_DAY0_SUMMARY_IMAGE = 207
        IMG_DAY0_SUMMARY_FRAME = 219
        LBL_DAY0_SUMMARY_LABEL = 208
        IMG_DAY0_MAXTEMP_IMAGE = 209
        LBL_DAY0_MAXTEMP_LABEL = 210
        IMG_DAY0_MINTEMP_IMAGE = 211
        LBL_DAY0_MINTEMP_LABEL = 212
        IMG_DAY0_WIND_IMAGE = 213
        LBL_DAY0_WIND_LABEL = 214
        LBL_DAY0_HUMIDITY_LABEL = 215
        LBL_DAY0_PRESSURE_LABEL = 216
        LBL_DAY0_VISIBILITY_LABEL = 217
        IMG_DAY0_BACKGROUND = 218

        LBL_DAY1_DAYNAME = 226
        IMG_DAY1_SUMMARY_IMAGE = 227
        IMG_DAY1_SUMMARY_FRAME = 239
        LBL_DAY1_SUMMARY_LABEL = 228
        IMG_DAY1_MAXTEMP_IMAGE = 229
        LBL_DAY1_MAXTEMP_LABEL = 230
        IMG_DAY1_MINTEMP_IMAGE = 231
        LBL_DAY1_MINTEMP_LABEL = 232
        IMG_DAY1_WIND_IMAGE = 233
        LBL_DAY1_WIND_LABEL = 234
        LBL_DAY1_HUMIDITY_LABEL = 235
        LBL_DAY1_PRESSURE_LABEL = 236
        LBL_DAY1_VISIBILITY_LABEL = 237
        IMG_DAY1_BACKGROUND = 238

        LBL_DAY2_DAYNAME = 246
        IMG_DAY2_SUMMARY_IMAGE = 247
        IMG_DAY2_SUMMARY_FRAME = 259
        LBL_DAY2_SUMMARY_LABEL = 248
        IMG_DAY2_MAXTEMP_IMAGE = 249
        LBL_DAY2_MAXTEMP_LABEL = 250
        IMG_DAY2_MINTEMP_IMAGE = 251
        LBL_DAY2_MINTEMP_LABEL = 252
        IMG_DAY2_WIND_IMAGE = 253
        LBL_DAY2_WIND_LABEL = 254
        LBL_DAY2_HUMIDITY_LABEL = 255
        LBL_DAY2_PRESSURE_LABEL = 256
        LBL_DAY2_VISIBILITY_LABEL = 257
        IMG_DAY2_BACKGROUND = 258

        LBL_DAY3_DAYNAME = 266
        IMG_DAY3_SUMMARY_IMAGE = 267
        IMG_DAY3_SUMMARY_FRAME = 279
        LBL_DAY3_SUMMARY_LABEL = 268
        IMG_DAY3_MAXTEMP_IMAGE = 269
        LBL_DAY3_MAXTEMP_LABEL = 270
        IMG_DAY3_MINTEMP_IMAGE = 271
        LBL_DAY3_MINTEMP_LABEL = 272
        IMG_DAY3_WIND_IMAGE = 273
        LBL_DAY3_WIND_LABEL = 274
        LBL_DAY3_HUMIDITY_LABEL = 275
        LBL_DAY3_PRESSURE_LABEL = 276
        LBL_DAY3_VISIBILITY_LABEL = 277
        IMG_DAY3_BACKGROUND = 278

        LBL_DAY4_DAYNAME = 286
        IMG_DAY4_SUMMARY_IMAGE = 287
        IMG_DAY4_SUMMARY_FRAME = 299
        LBL_DAY4_SUMMARY_LABEL = 288
        IMG_DAY4_MAXTEMP_IMAGE = 289
        LBL_DAY4_MAXTEMP_LABEL = 290
        IMG_DAY4_MINTEMP_IMAGE = 291
        LBL_DAY4_MINTEMP_LABEL = 292
        IMG_DAY4_WIND_IMAGE = 293
        LBL_DAY4_WIND_LABEL = 294
        LBL_DAY4_HUMIDITY_LABEL = 295
        LBL_DAY4_PRESSURE_LABEL = 296
        LBL_DAY4_VISIBILITY_LABEL = 297
        IMG_DAY4_BACKGROUND = 298

        IMG_HOUR0_BACKGROUND = 300
        LBL_HOUR0_HOURNAME = 301
        IMG_HOUR0_SUMMARY_IMAGE = 302
        IMG_HOUR0_SUMMARY_FRAME = 311
        LBL_HOUR0_SUMMARY_LABEL = 303
        IMG_HOUR0_MAXTEMP_IMAGE = 304
        LBL_HOUR0_MAXTEMP_LABEL = 305
        IMG_HOUR0_WIND_IMAGE = 306
        LBL_HOUR0_WIND_LABEL = 307
        LBL_HOUR0_HUMIDITY_LABEL = 308
        LBL_HOUR0_PRESSURE_LABEL = 309
        LBL_HOUR0_VISIBILITY_LABEL = 310

        IMG_HOUR1_BACKGROUND = 320
        LBL_HOUR1_HOURNAME = 321
        IMG_HOUR1_SUMMARY_IMAGE = 322
        IMG_HOUR1_SUMMARY_FRAME = 331
        LBL_HOUR1_SUMMARY_LABEL = 323
        IMG_HOUR1_MAXTEMP_IMAGE = 324
        LBL_HOUR1_MAXTEMP_LABEL = 325
        IMG_HOUR1_WIND_IMAGE = 326
        LBL_HOUR1_WIND_LABEL = 327
        LBL_HOUR1_HUMIDITY_LABEL = 328
        LBL_HOUR1_PRESSURE_LABEL = 329
        LBL_HOUR1_VISIBILITY_LABEL = 330

        IMG_HOUR2_BACKGROUND = 340
        LBL_HOUR2_HOURNAME = 341
        IMG_HOUR2_SUMMARY_IMAGE = 342
        IMG_HOUR2_SUMMARY_FRAME = 351
        LBL_HOUR2_SUMMARY_LABEL = 343
        IMG_HOUR2_MAXTEMP_IMAGE = 344
        LBL_HOUR2_MAXTEMP_LABEL = 345
        IMG_HOUR2_WIND_IMAGE = 346
        LBL_HOUR2_WIND_LABEL = 347
        LBL_HOUR2_HUMIDITY_LABEL = 348
        LBL_HOUR2_PRESSURE_LABEL = 349
        LBL_HOUR2_VISIBILITY_LABEL = 350

        IMG_HOUR3_BACKGROUND = 360
        LBL_HOUR3_HOURNAME = 361
        IMG_HOUR3_SUMMARY_IMAGE = 362
        IMG_HOUR3_SUMMARY_FRAME = 371
        LBL_HOUR3_SUMMARY_LABEL = 363
        IMG_HOUR3_MAXTEMP_IMAGE = 364
        LBL_HOUR3_MAXTEMP_LABEL = 365
        IMG_HOUR3_WIND_IMAGE = 366
        LBL_HOUR3_WIND_LABEL = 367
        LBL_HOUR3_HUMIDITY_LABEL = 368
        LBL_HOUR3_PRESSURE_LABEL = 369
        LBL_HOUR3_VISIBILITY_LABEL = 370

        IMG_HOUR4_BACKGROUND = 380
        LBL_HOUR4_HOURNAME = 381
        IMG_HOUR4_SUMMARY_IMAGE = 382
        IMG_HOUR4_SUMMARY_FRAME = 391
        LBL_HOUR4_SUMMARY_LABEL = 383
        IMG_HOUR4_MAXTEMP_IMAGE = 384
        LBL_HOUR4_MAXTEMP_LABEL = 385
        IMG_HOUR4_WIND_IMAGE = 386
        LBL_HOUR4_WIND_LABEL = 387
        LBL_HOUR4_HUMIDITY_LABEL = 388
        LBL_HOUR4_PRESSURE_LABEL = 389
        LBL_HOUR4_VISIBILITY_LABEL = 390

        IMG_HOUR5_BACKGROUND = 400
        LBL_HOUR5_HOURNAME = 401
        IMG_HOUR5_SUMMARY_IMAGE = 402
        IMG_HOUR5_SUMMARY_FRAME = 411
        LBL_HOUR5_SUMMARY_LABEL = 403
        IMG_HOUR5_MAXTEMP_IMAGE = 404
        LBL_HOUR5_MAXTEMP_LABEL = 405
        IMG_HOUR5_WIND_IMAGE = 406
        LBL_HOUR5_WIND_LABEL = 407
        LBL_HOUR5_HUMIDITY_LABEL = 408
        LBL_HOUR5_PRESSURE_LABEL = 409
        LBL_HOUR5_VISIBILITY_LABEL = 410

        IMG_HOUR6_BACKGROUND = 420
        LBL_HOUR6_HOURNAME = 421
        IMG_HOUR6_SUMMARY_IMAGE = 422
        IMG_HOUR6_SUMMARY_FRAME = 431
        LBL_HOUR6_SUMMARY_LABEL = 423
        IMG_HOUR6_MAXTEMP_IMAGE = 424
        LBL_HOUR6_MAXTEMP_LABEL = 425
        IMG_HOUR6_WIND_IMAGE = 426
        LBL_HOUR6_WIND_LABEL = 427
        LBL_HOUR6_HUMIDITY_LABEL = 428
        LBL_HOUR6_PRESSURE_LABEL = 429
        LBL_HOUR6_VISIBILITY_LABEL = 430

        IMG_HOUR7_BACKGROUND = 440
        LBL_HOUR7_HOURNAME = 441
        IMG_HOUR7_SUMMARY_IMAGE = 442
        IMG_HOUR7_SUMMARY_FRAME = 451
        LBL_HOUR7_SUMMARY_LABEL = 443
        IMG_HOUR7_MAXTEMP_IMAGE = 444
        LBL_HOUR7_MAXTEMP_LABEL = 445
        IMG_HOUR7_WIND_IMAGE = 446
        LBL_HOUR7_WIND_LABEL = 447
        LBL_HOUR7_HUMIDITY_LABEL = 448
        LBL_HOUR7_PRESSURE_LABEL = 449
        LBL_HOUR7_VISIBILITY_LABEL = 450

        IMG_HOUR_SEPARATOR = 499

        LBL_MONTHLY_HEADING_LABEL = 500
        LBL_MONTHLY_PUBLISHED_LABEL = 501
        LBL_MONTHLY_AUTHOR_LABEL = 502
        LBL_MONTHLY_HEADLINE_LABEL = 503
        TBX_MONTHLY_DETAIL_TEXTBOX = 504
        IMG_MONTHLY_HORIZ_RULE = 505

        IMG_MAPS_ANIMATIONS_BACKGROUND = 600
        IMG_MAPS_ANIMATIONS = 601
        IMG_MAPS_ANIMATIONS_OVERLAY = 602

    End Enum

#End Region

#Region "Variables"

    <SkinControlAttribute(2)> Protected _button5days As GUIButtonControl = Nothing
    <SkinControlAttribute(3)> Protected _button24Hours As GUIButtonControl = Nothing
    <SkinControlAttribute(4)> Protected _buttonMonthly As GUIButtonControl = Nothing
    <SkinControlAttribute(5)> Protected _buttonMaps As GUIButtonControl = Nothing

    Private Const NUM_DAYS As Integer = 5

    Private _5DayForecast As ForeCast() = New ForeCast(NUM_DAYS) {}
    Private _24HourForecast As ForeCast() = New ForeCast(7) {}
    Private _monthly As MonthForeCast = New MonthForeCast
    Private _areaCode As String = String.Empty
    Private _areaName As String = String.Empty
    Private _tempUnit As String = String.Empty
    Private _windUnit As String = String.Empty
    Private _overRide As Boolean = False
    Private _regionName As String = String.Empty
    Private _workerActive As Boolean = False
    Private _downloadLock As Object = Nothing
    Private _currentMode As String = Mode.FiveDay           'Start with the 5 day forecast
    Private _refreshIntervalMinutes As Integer = 15         'Refresh every 15 minutes
    Private _lastRefreshTime As DateTime = Now.AddHours(-1) 'Set to -1 to force a refresh on init 

    Private _currentImage As GUIImage = Nothing
    Private _currentSummary As String = String.Empty
    Private _currentTemp As String = String.Empty
    Private _currentWind As String = String.Empty
    Private _currentHumidity As String = String.Empty
    Private _currentPressure As String = String.Empty
    Private _currentVisibility As String = String.Empty
    Private _currentObsTime As String = String.Empty
    Private _currentObsStation As String = String.Empty

#End Region

#Region "Constructor"

    Public Sub New()

        For i As Integer = 0 To NUM_DAYS - 1
            _5DayForecast(i).DayName = String.Empty
            _5DayForecast(i).Summary = String.Empty
            _5DayForecast(i).MaxTemp = String.Empty
            _5DayForecast(i).MinTemp = String.Empty
            _5DayForecast(i).WindDirection = String.Empty
            _5DayForecast(i).WindSpeed = String.Empty
            _5DayForecast(i).Humidity = String.Empty
            _5DayForecast(i).Pressure = String.Empty
            _5DayForecast(i).Visibility = String.Empty
            _5DayForecast(i).SunRise = String.Empty
            _5DayForecast(i).SunSet = String.Empty
            _5DayForecast(i).WindDirection = String.Empty
        Next

    End Sub

#End Region

#Region "Serialisation"

    Private Sub LoadSettings()

        Using xmlReader As Settings = New MPSettings
            _areaCode = xmlReader.GetValueAsString("BBCWeather", "areaCode", "8")
            _areaName = xmlReader.GetValueAsString("BBCWeather", "areaName", "London")
            _tempUnit = xmlReader.GetValueAsString("BBCWeather", "tempUnit", "degC")
            _windUnit = xmlReader.GetValueAsString("BBCWeather", "windUnit", "mph")
            _overRide = xmlReader.GetValueAsBool("BBCWeather", "overRide", False)
        End Using

    End Sub

#End Region

#Region "Properties"

    Public Overrides Property GetID As Integer
        Get
            Return 8192
        End Get
        Set(value As Integer)

        End Set
    End Property

    Public Property IsRefreshing() As Boolean
        Get
            Return _workerActive
        End Get
        <MethodImpl(MethodImplOptions.Synchronized)> _
        Private Set(value As Boolean)
            _workerActive = value
        End Set
    End Property

#End Region

#Region "Overrides"

    Public Overloads Overrides Function Init() As Boolean
        AppDomain.CurrentDomain.AppendPrivatePath(String.Format("{0}\Plugins\Windows\BBCWeather", AppDomain.CurrentDomain.BaseDirectory))
        Try
            LoadSettings()
            DownloadAll()
            ParseCurrentObservation()
            Parse5DayWeatherInfo()
            Parse24HourWeatherInfo()
            SetInfoServiceProperties()
        Catch ex As Exception
            Log.Error("plugin: BBCWeather: error on plugin init.")
        End Try
        Return Load(GUIGraphicsContext.Skin & "\BBCWeather.xml")
    End Function

    Protected Overrides Sub OnPageLoad()

        MyBase.OnPageLoad()
        RefreshNewMode()
        _downloadLock = New Object
        If _lastRefreshTime < Now.AddMinutes(-_refreshIntervalMinutes) Then BackgroundUpdate(False)

    End Sub

    Public Overrides Sub Process()

        Dim a As DateTime = DateTime.Now

        If (DateTime.Now - _lastRefreshTime).Minutes >= _refreshIntervalMinutes AndAlso _areaCode <> String.Empty AndAlso Not Me.IsRefreshing Then
            Log.Debug("plugin: BBCWeather: autoupdating data.")
            BackgroundUpdate(True)
        End If

        MyBase.Process()

    End Sub

    Protected Overrides Sub OnClicked(controlId As Integer, control As MediaPortal.GUI.Library.GUIControl, actionType As MediaPortal.GUI.Library.Action.ActionType)

        If control Is _button5days Then
            _currentMode = Mode.FiveDay
        ElseIf control Is _button24Hours Then
            _currentMode = Mode.TwentyFourHours
        ElseIf control Is _buttonMonthly Then
            _currentMode = Mode.Monthly
        ElseIf control Is _buttonMaps Then
            _currentMode = Mode.Maps
        End If

        RefreshNewMode()
        MyBase.OnClicked(controlId, control, actionType)

    End Sub

#End Region

#Region "Skin control methods"

    Private Sub RefreshNewMode()
        Select Case _currentMode
            Case Mode.FiveDay
                GUIControl.FocusControl(GetID, 2)
                Set5DayModeControls()
            Case Mode.TwentyFourHours
                GUIControl.FocusControl(GetID, 3)
                Set24HourModeControls()
            Case Mode.Monthly
                GUIControl.FocusControl(GetID, 4)
                SetMonthlyModeControls()
            Case Mode.Maps
                GUIControl.FocusControl(GetID, 5)
                SetMapsModeControls()
        End Select
        GUIControl.FocusControl(GetID, 9999)
    End Sub

    Private Sub HideAllControls()
        For i As Integer = 100 To 999
            GUIControl.HideControl(GetID, i)
        Next
    End Sub

    Private Sub SetHeaderControls()

        GUIControl.ShowControl(GetID, Controls.LBL_LOCATION)
        GUIControl.SetControlLabel(GetID, Controls.LBL_LOCATION, _areaName)

    End Sub

    Private Sub SetCurrentWeatherControls()

        For i As Integer = 100 To 199
            GUIControl.ShowControl(GetID, i)
        Next

        GUIControl.SetControlLabel(GetID, Controls.LBL_CURRENT_SUMMARY, _currentSummary)
        GUIControl.SetControlLabel(GetID, Controls.LBL_CURRENT_TEMP, _currentTemp)
        GUIControl.SetControlLabel(GetID, Controls.LBL_CURRENT_WIND, _currentWind)
        GUIControl.SetControlLabel(GetID, Controls.LBL_CURRENT_HUMIDITY, _currentHumidity)
        GUIControl.SetControlLabel(GetID, Controls.LBL_CURRENT_PRESSURE, _currentPressure)
        GUIControl.SetControlLabel(GetID, Controls.LBL_CURRENT_VISIBILITY, _currentVisibility)
        GUIControl.SetControlLabel(GetID, Controls.LBL_CURRENT_OBS_TIME, _currentObsTime)
        GUIControl.SetControlLabel(GetID, Controls.LBL_CURRENT_OBS_STATION, _currentObsTime)

    End Sub

    Private Sub Set5DayModeControls()

        HideAllControls()
        SetHeaderControls()
        SetCurrentWeatherControls()

        For i As Integer = 200 To 299
            GUIControl.ShowControl(GetID, i)
        Next

        Dim image As GUIImage = Nothing
        For dayNum As Integer = 0 To 4

            GUIControl.SetControlLabel(GetID, Controls.LBL_DAY0_DAYNAME + (dayNum * 20), Left(DateTime.Now.AddDays(dayNum).ToString("dddd"), 3))
            image = DirectCast(GetControl(Controls.IMG_DAY0_SUMMARY_IMAGE + (dayNum * 20)), GUIImage)
            image.SetFileName(GetWeatherImage(_5DayForecast(dayNum).Summary))
            GUIControl.SetControlLabel(GetID, Controls.LBL_DAY0_SUMMARY_LABEL + (dayNum * 20), _5DayForecast(dayNum).Summary)
            image = DirectCast(GetControl(Controls.IMG_DAY0_MAXTEMP_IMAGE + (dayNum * 20)), GUIImage)
            image.SetFileName(GetTemperatureImage(_5DayForecast(dayNum).MaxTemp))
            GUIControl.SetControlLabel(GetID, Controls.LBL_DAY0_MAXTEMP_LABEL + (dayNum * 20), _5DayForecast(dayNum).MaxTemp)
            image = DirectCast(GetControl(Controls.IMG_DAY0_MINTEMP_IMAGE + (dayNum * 20)), GUIImage)
            image.SetFileName(GetTemperatureImage(_5DayForecast(dayNum).MinTemp))
            GUIControl.SetControlLabel(GetID, Controls.LBL_DAY0_MINTEMP_LABEL + (dayNum * 20), _5DayForecast(dayNum).MinTemp)
            image = DirectCast(GetControl(Controls.IMG_DAY0_WIND_IMAGE + (dayNum * 20)), GUIImage)
            image.SetFileName(GetWindImage(_5DayForecast(dayNum).WindDirection))
            GUIControl.SetControlLabel(GetID, Controls.LBL_DAY0_WIND_LABEL + (dayNum * 20), _5DayForecast(dayNum).WindSpeed)
            GUIControl.SetControlLabel(GetID, Controls.LBL_DAY0_HUMIDITY_LABEL + (dayNum * 20), _5DayForecast(dayNum).Humidity)
            GUIControl.SetControlLabel(GetID, Controls.LBL_DAY0_PRESSURE_LABEL + (dayNum * 20), _5DayForecast(dayNum).Pressure)
            GUIControl.SetControlLabel(GetID, Controls.LBL_DAY0_VISIBILITY_LABEL + (dayNum * 20), _5DayForecast(dayNum).Visibility)

        Next

    End Sub

    Private Sub Set24HourModeControls()

        HideAllControls()
        SetHeaderControls()
        SetCurrentWeatherControls()

        For i As Integer = 300 To 499
            GUIControl.ShowControl(GetID, i)
        Next

        Dim image As GUIImage = Nothing

        For hourNum As Integer = 0 To 7

            GUIControl.SetControlLabel(GetID, Controls.LBL_HOUR0_HOURNAME + (hourNum * 20), _24HourForecast(hourNum).DayName)
            image = DirectCast(GetControl(Controls.IMG_HOUR0_SUMMARY_IMAGE + (hourNum * 20)), GUIImage)
            image.SetFileName(GetWeatherImage(_24HourForecast(hourNum).Summary, CInt(Split(_24HourForecast(hourNum).DayName, ":")(0))))
            GUIControl.SetControlLabel(GetID, Controls.LBL_HOUR0_SUMMARY_LABEL + (hourNum * 20), _24HourForecast(hourNum).Summary)
            image = DirectCast(GetControl(Controls.IMG_HOUR0_MAXTEMP_IMAGE + (hourNum * 20)), GUIImage)
            image.SetFileName(GetTemperatureImage(_24HourForecast(hourNum).MaxTemp))
            GUIControl.SetControlLabel(GetID, Controls.LBL_HOUR0_MAXTEMP_LABEL + (hourNum * 20), _24HourForecast(hourNum).MaxTemp)
            image = DirectCast(GetControl(Controls.IMG_HOUR0_WIND_IMAGE + (hourNum * 20)), GUIImage)
            image.SetFileName(GetWindImage(_24HourForecast(hourNum).WindDirection))
            GUIControl.SetControlLabel(GetID, Controls.LBL_HOUR0_WIND_LABEL + (hourNum * 20), _24HourForecast(hourNum).WindSpeed)
            GUIControl.SetControlLabel(GetID, Controls.LBL_HOUR0_HUMIDITY_LABEL + (hourNum * 20), _24HourForecast(hourNum).Humidity)
            GUIControl.SetControlLabel(GetID, Controls.LBL_HOUR0_PRESSURE_LABEL + (hourNum * 20), _24HourForecast(hourNum).Pressure)
            GUIControl.SetControlLabel(GetID, Controls.LBL_HOUR0_VISIBILITY_LABEL + (hourNum * 20), _24HourForecast(hourNum).Visibility)

        Next

    End Sub

    Private Sub SetMonthlyModeControls()

        HideAllControls()
        SetHeaderControls()
        SetCurrentWeatherControls()

        For i As Integer = 500 To 599
            GUIControl.ShowControl(GetID, i)
        Next

        Dim txt As String
        txt = String.Format("{0} {1}", _monthly.publishedDate, _monthly.nextUpdate)

        GUIControl.SetControlLabel(GetID, Controls.LBL_MONTHLY_PUBLISHED_LABEL, txt)
        GUIControl.SetControlLabel(GetID, Controls.LBL_MONTHLY_AUTHOR_LABEL, _monthly.author)
        GUIControl.SetControlLabel(GetID, Controls.LBL_MONTHLY_HEADLINE_LABEL, _monthly.headline)

        Dim break As String = String.Format("{0}{1}{0}", Environment.NewLine, Replicate("=", 30))
        txt = Environment.NewLine + _monthly.summary
        txt += break
        txt += _monthly.period1 + Environment.NewLine
        txt += _monthly.periodSummary1 + Environment.NewLine
        txt += _monthly.periodDetail1 + Environment.NewLine
        txt += break
        txt += _monthly.period2 + Environment.NewLine
        txt += _monthly.periodSummary2 + Environment.NewLine
        txt += _monthly.periodDetail2 + Environment.NewLine
        txt += break
        txt += _monthly.period3 + Environment.NewLine
        txt += _monthly.periodSummary3 + Environment.NewLine
        txt += _monthly.periodDetail3 + Environment.NewLine

        GUIControl.SetControlLabel(GetID, Controls.TBX_MONTHLY_DETAIL_TEXTBOX, txt)

    End Sub

    Private Sub SetMapsModeControls()

        HideAllControls()
        SetHeaderControls()
        SetCurrentWeatherControls()

        For i As Integer = 600 To 699
            GUIControl.ShowControl(GetID, i)
        Next

        Dim image As GUIImage = DirectCast(GetControl(Controls.IMG_MAPS_ANIMATIONS_OVERLAY), GUIImage)
        image.SetFileName(String.Format("{0}\Media\BBCWeather\overlay.png", GUIGraphicsContext.Skin))

        Dim mImage As GUIMultiImage = DirectCast(GetControl(Controls.IMG_MAPS_ANIMATIONS), GUIMultiImage)
        mImage.AllocResources()

    End Sub

    Private Function GetTemperatureImage(ByVal temp As String) As String

        Dim img As String = "na"

        If temp <> "" Then
            Dim iTemp As Integer = CInt(temp)
            If _tempUnit.ToLower = "degf" Then iTemp = (iTemp - 32) / 1.8
            Select Case iTemp
                Case Is >= 25
                    img = "25"
                Case 22, 23, 24
                    img = "22-24"
                Case 19, 20, 21
                    img = "19-21"
                Case 16, 17, 18
                    img = "16-18"
                Case 13, 14, 15
                    img = "13-15"
                Case 10, 11, 12
                    img = "10-12"
                Case 7, 8, 9
                    img = "7-9"
                Case 4, 5, 6
                    img = "4-6"
                Case 1, 2, 3
                    img = "1-3"
                Case 0
                    img = "0"
                Case -1, -2, -3
                    img = "-1-3"
                Case -4, -5, -6
                    img = "-4-6"
                Case -7, -8, -9
                    img = "-7-9"
                Case -10, -11, -12
                    img = "-10-12"
                Case -13, -14, -15
                    img = "-13-15"
                Case -16, -17, -18
                    img = "-16-18"
                Case -19, -20, -21
                    img = "-19-21"
                Case -22, -23, -24
                    img = "22-24"
                Case -25, -26, -27
                    img = "-25-27"
                Case -28, -29, -30
                    img = "-28-30"
                Case -31, -32, -33
                    img = "-31-33"
                Case -34, -35, -36
                    img = "-34-36"
                Case -37, -38, -39
                    img = "-37-39"
                Case Is <= -40
                    img = "-40"
                Case Else
                    img = "na"
            End Select
        End If

        Return (String.Format("{0}\Media\BBCWeather\temp\{1}.png", GUIGraphicsContext.Skin, img))

    End Function

    Private Function GetWindImage(ByVal wind As String) As String

        Dim dir As String = "na"

        Select Case wind.ToLower
            Case "easterly"
                dir = "e"
            Case "east north easterly"
                dir = "ene"
            Case "east south easterly"
                dir = "ese"
            Case "northerly"
                dir = "n"
            Case "north easterly"
                dir = "ne"
            Case "north north easterly"
                dir = "nne"
            Case "north north westerly"
                dir = "nnw"
            Case "north westerly"
                dir = "nw"
            Case "southerly"
                dir = "s"
            Case "south easterly"
                dir = "se"
            Case "south south easterly"
                dir = "sse"
            Case "south south westerly"
                dir = "ssw"
            Case "south westerly"
                dir = "sw"
            Case "westerly"
                dir = "w"
            Case "west north westerly"
                dir = "wnw"
            Case "west south westerly"
                dir = "wsw"
            Case Else
                dir = "na"
        End Select

        Return (String.Format("{0}\Media\BBCWeather\wind\{1}.png", GUIGraphicsContext.Skin, dir))

    End Function

    Private Function GetWeatherImage(ByVal weather As String, Optional ByVal hour As Integer = 12, Optional ByVal fullPath As Boolean = True, Optional ByVal infoService As Boolean = False) As String

        Dim img As Integer = 0
        Dim day As Integer = 0
        If hour >= 6 And hour <= 20 Then day = 1

        If InStr(weather, "with") > 0 Then
            weather = Left(weather, InStr(weather, "with") - 2)
        End If

        Select Case weather.ToLower
            Case "clear sky", "sunny"
                img = 0 + day
            Case "partly cloudy", "sunny intervals"
                img = 2 + day
            Case "mist"
                img = 5
            Case "fog"
                img = 6
            Case "white cloud"
                img = 7
            Case "black cloud", "grey cloud"
                img = 8
            Case "light rain shower"
                img = 9 + day
            Case "drizzle"
                img = 11
            Case "light rain"
                img = 12
            Case "heavy rain shower"
                img = 13 + day
            Case "heavy rain"
                img = 15
            Case "sleet shower"
                img = 16 + day
            Case "sleet"
                img = 18
            Case "hail shower"
                img = 19 + day
            Case "hail"
                img = 21
            Case "light snow shower"
                img = 22 + day
            Case "light snow"
                img = 24
            Case "heavy snow shower"
                img = 25 + day
            Case "heavy snow"
                img = 27
            Case "thundery shower"
                img = 28 + day
            Case "thunder storm"
                img = 30 + day
            Case Else
                img = 32
        End Select

        If infoService Then
            Select Case img
                Case 0
                    img = 31
                Case 1
                    img = 32
                Case 2
                    img = 33
                Case 3
                    img = 34
                Case 5, 6
                    img = 20
                Case 7
                    img = 30
                Case 8
                    img = 28
                Case 9
                    img = 45
                Case 10
                    img = 39
                Case 11
                    img = 11
                Case 12, 13, 14, 15
                    img = 10
                Case 16, 17, 18
                    img = 5
                Case 19, 20, 21
                    img = 6
                Case 22, 23, 24, 25, 26, 27
                    img = 13
                Case 28, 30
                    img = 47
                Case 29, 31
                    img = 37
                Case 32
                    img = 99
                Case Else
                    img = 99
            End Select
        End If

        If fullPath Then
            Return (String.Format("{0}\Media\BBCWeather\weather\{1}.png", GUIGraphicsContext.Skin, img.ToString))
        Else
            Return img.ToString
        End If

    End Function

    Public Function Replicate(ByVal item As String, ByVal repeat As Integer) As String
        If item Is Nothing Then Return String.Empty
        If repeat <= 0 Then Return String.Empty
        Dim work As StringBuilder = New StringBuilder(repeat * item.Length)
        For i As Integer = 0 To repeat - 1
            work.Append(item)
        Next
        Return work.ToString()
    End Function

#End Region

#Region "Background updaters"

    Private Sub BackgroundUpdate(isAuto As Boolean)
        Dim updateThread As New Thread(New ParameterizedThreadStart(AddressOf DownloadWorker))
        updateThread.IsBackground = True
        updateThread.Name = "BBC Weather updater"
        IsRefreshing = True
        updateThread.Start(isAuto)

        While IsRefreshing
            GUIWindowManager.Process()
        End While

    End Sub

    <MethodImpl(MethodImplOptions.Synchronized)> _
    Private Sub DownloadWorker(data As Object)
        RefreshMe(CBool(data))
        'do an autoUpdate refresh
        IsRefreshing = False
    End Sub

#End Region

#Region "Main refresh method"

    Private Sub RefreshMe(autoUpdate As Boolean)

        HideAllControls()

        SyncLock _downloadLock
            Using cursor As New WaitCursor()

                If DownloadAll() Then
                    If Not ParseCurrentObservation() AndAlso Not autoUpdate Then DisplayErrorDialog("current observation")
                    If Not Parse5DayWeatherInfo() AndAlso Not autoUpdate Then DisplayErrorDialog("5 day forecast")
                    If Not Parse24HourWeatherInfo() AndAlso Not autoUpdate Then DisplayErrorDialog("24 hour forecast")
                    If Not ParseMonthlyWeatherInfo() AndAlso Not autoUpdate Then DisplayErrorDialog("monthly outlook")
                    If Not ParseMapOverlay() AndAlso Not autoUpdate Then DisplayErrorDialog("monthly outlook")
                    If Not SetInfoServiceProperties() AndAlso Not autoUpdate Then DisplayErrorDialog("monthly outlook")
                End If

                _lastRefreshTime = DateTime.Now

            End Using
        End SyncLock

        RefreshNewMode()

    End Sub

    Private Sub DisplayErrorDialog(ByVal forecastType As String)
        Dim errorDialog As GUIDialogOK = DirectCast(GUIWindowManager.GetWindow(CInt(Window.WINDOW_DIALOG_OK)), GUIDialogOK)
        errorDialog.SetHeading("BBC Weather parsing error")
        errorDialog.SetLine(1, String.Format("Error parsing {0}.", forecastType))
        errorDialog.SetLine(2, String.Format("Location is {0} ({1})", _areaName, _areaCode))
        errorDialog.SetLine(3, String.Empty)
        errorDialog.DoModal(GetID)
    End Sub

#End Region

#Region "Downloads"

    Private Function DownloadAll() As Boolean

        If Not Directory.Exists(String.Format("{0}\BBCWeather\", Config.GetFolder(Config.Dir.Cache))) Then
            Directory.CreateDirectory(String.Format("{0}\BBCWeather\", Config.GetFolder(Config.Dir.Cache)))
        End If

        '==============================================================================================================
        ' http://news.bbc.co.uk/weather/forecast/355/Forecast.xhtml?state=fo:A = Next Five Days
        ' http://news.bbc.co.uk/weather/forecast/355/Forecast.xhtml?state=fo:B = Twenty Four Hours
        ' http://news.bbc.co.uk/weather/forecast/355/Forecast.xhtml?state=fo:C = Monthly Outlook
        ' http://news.bbc.co.uk/weather/forecast/355/ObservationsEmbed.xhtml   = Latest Observations
        ' http://news.bbc.co.uk/weather/forecast/355/Forecast.xhtml?&showDay=A = Days detail
        '==============================================================================================================

        Dim downloadSuccess As Boolean = False
        downloadSuccess = DownloadForecast(String.Format("http://news.bbc.co.uk/weather/forecast/{0}/Forecast.xhtml?state=fo:A", _areaCode), "A") '5 days
        For i As Integer = 0 To 4
            downloadSuccess = DownloadForecast(String.Format("http://news.bbc.co.uk/weather/forecast/{0}/Forecast.xhtml?&showDay={1}", _areaCode, Chr(65 + i)), String.Format("A_{0}", Chr(65 + i))) '5 day - day 1
        Next
        downloadSuccess = DownloadForecast(String.Format("http://news.bbc.co.uk/weather/forecast/{0}/Forecast.xhtml?state=fo:B", _areaCode), "B") '24 hours
        downloadSuccess = DownloadForecast(String.Format("http://news.bbc.co.uk/weather/forecast/{0}/Forecast.xhtml?state=fo:C", _areaCode), "C") 'Monthly
        downloadSuccess = DownloadForecast(String.Format("http://news.bbc.co.uk/weather/forecast/{0}/ObservationsEmbed.xhtml", _areaCode), "D") 'Latest observations

        ParseLocationName()
        ParseRegionName()

        downloadSuccess = DownloadMaps()
        downloadSuccess = DownloadMapOverlay()

        Return downloadSuccess

    End Function

    Private Function DownloadForecast(ByVal URL As String, ByVal forecastType As String) As Boolean

        If Not Win32API.IsConnectedToInternet() Then
            Log.Error("plugin: BBCWeather - error downloading weather, no internet connection")
            Return False
        End If

        Try
            Dim sourceString As String = New WebClient().DownloadString(URL)
            Dim writer As StreamWriter = New StreamWriter(String.Format("{0}\BBCWeather\BBCWeather_{1}_{2}.html", Config.GetFolder(Config.Dir.Cache), _areaCode, forecastType), False)
            writer.WriteLine(sourceString)
            writer.Close()
        Catch ex As Exception
            Log.Error("plugin: BBCWeather - error downloading weather from {0}", URL)
            Return False
        End Try

        Log.Info("plugin: BBCWeather - successfully downloaded {0}", URL)

        Return True

    End Function

    Private Function DownloadMaps() As Boolean

        If Not Win32API.IsConnectedToInternet() Then
            Log.Error("plugin: BBCWeather - error downloading weather maps, no internet connection")
            Return False
        End If

        ClearAnimationFiles()

        Dim URL As String = String.Empty
        Dim fileName As String = String.Empty
        Dim wClient As New WebClient

        Dim hour As Integer = Now.Hour
        Dim day As Integer = Now.Day
        Dim daysInMonth As Integer = Date.DaysInMonth(Now.Year, Now.Month)
        Dim dayCount As Integer = 0
        Dim sDay As String = String.Empty

        Dim sHour As String = GetHour(hour)
        If CInt(sHour) = hour Then hour = GetHour(hour - 1)

        hour = CInt(sHour)

        '3 hourly images for the first 2 days
        Dim numImages As Integer = 14
        If hour = 6 Or hour = 12 Or hour = 18 Or hour = 0 Then numImages += 1

        For i As Integer = 1 To numImages
            sHour = GetHour(hour)
            sDay = GetDay(day)
            URL = String.Format("http://newsimg.bbc.co.uk/weather/map_presenter/{0}/{1}/forecast/{2}.jpg", sDay, sHour, _regionName)
            fileName = String.Format("{0}\Media\animations\BBCWeather\{1}_{2}_{3}.jpg", GUIGraphicsContext.Skin, IIf(i < 10, "0" + i.ToString, i.ToString), sDay, sHour)
            Try
                wClient.DownloadFile(URL, fileName)
                AddTimeInfo(fileName, sHour, Now.AddDays(dayCount).ToString("dddd").ToUpper)
            Catch ex As Exception
                Log.Error("plugin: BBCWeather - error downloading weather from {0}", URL)
                Return False
            End Try

            hour += +3
            If hour > 23 Then
                day += 1
                dayCount += 1
                hour -= 24
            End If
            If day > daysInMonth Then day = day - daysInMonth
        Next

        '12 hourly images for the next 3 days
        If hour < 12 Then
            hour = 12
        Else
            day += 1
            dayCount += 1
            hour = 0
        End If

        For i As Integer = numImages + 1 To numImages + 6
            sHour = GetHour(hour)
            sDay = GetDay(day)
            URL = String.Format("http://newsimg.bbc.co.uk/weather/map_presenter/{0}/{1}/forecast/{2}.jpg", sDay, sHour, _regionName)
            fileName = String.Format("{0}\Media\animations\BBCWeather\{1}_{2}_{3}.jpg", GUIGraphicsContext.Skin, IIf(i < 10, "0" + i.ToString, i.ToString), sDay, sHour)
            Try
                wClient.DownloadFile(URL, fileName)
                AddTimeInfo(fileName, "", String.Format("{0}{1}", IIf(sHour = "00", Now.AddDays(dayCount - 1).ToString("dddd").ToUpper, Now.AddDays(dayCount).ToString("dddd").ToUpper), IIf(sHour = "00", " NIGHT", "")))
            Catch ex As Exception
                Log.Error("plugin: BBCWeather - error downloading weather from {0}", URL)
                Return False
            End Try

            hour += 12
            If hour > 23 Then
                day += 1
                dayCount += 1
                hour -= 24
            End If

            If day > daysInMonth Then day = day - daysInMonth
        Next

        Return True

    End Function

    Private Function DownloadMapOverlay() As Boolean

        If Not Win32API.IsConnectedToInternet() Then
            Log.Error("plugin: BBCWeather - error downloading weather, no internet connection")
            Return False
        End If

        Dim URL As String = String.Format("http://news.bbc.co.uk/weather/map_presenter/{0}/MapAreaNode.xml", _regionName)

        Try
            Dim sourceString As String = New WebClient().DownloadString(URL)
            Dim writer As StreamWriter = New StreamWriter(String.Format("{0}\BBCWeather\BBCWeather_{1}.xml", Config.GetFolder(Config.Dir.Cache), _areaCode), False)
            writer.WriteLine(sourceString)
            writer.Close()
        Catch ex As Exception
            Log.Error("plugin: BBCWeather - error downloading weather from {0}", URL)
            Return False
        End Try

        Log.Info("plugin: BBCWeather - successfully downloaded {0}", URL)

        Return True

    End Function

    Private Function GetHour(ByVal hh As Integer) As String

        Do Until hh < 24
            If hh >= 24 Then hh = hh - 24
        Loop

        Select Case hh
            Case 0, 1, 2
                Return "00"
            Case 3, 4, 5
                Return "03"
            Case 6, 7, 8
                Return "06"
            Case 9, 10, 11
                Return "09"
            Case 12, 13, 14
                Return "12"
            Case 15, 16, 17
                Return "15"
            Case 18, 19, 20
                Return "18"
            Case 21, 22, 23
                Return "21"
            Case Else
                Return "00"
        End Select

    End Function

    Private Function GetDay(ByVal dd As Integer) As String

        If dd < 10 Then Return String.Format("0{0}", dd.ToString)
        Return dd.ToString

    End Function

    Private Shared Sub ClearAnimationFiles()

        Dim path As String = String.Format("{0}\\Media\\animations\\BBCWeather\\", GUIGraphicsContext.Skin)

        If Not Directory.Exists(path) Then Directory.CreateDirectory(path)

        Dim dir As DirectoryInfo = New DirectoryInfo(path)
        For Each f As FileInfo In dir.GetFiles
            Try
                f.Delete()
            Catch ex As Exception
                Log.Error("plugin: BBCWeather - error deleting weather image, {0}", ex.Message)
            End Try
        Next

    End Sub

    Private Sub AddTimeInfo(ByVal fileName As String, ByVal hour As String, ByVal day As String)

        Dim oldImage As Image = Image.FromFile(fileName)
        Dim imageSize As New Size(oldImage.Width, oldImage.Height)
        Dim newBitmap As New Bitmap(oldImage, imageSize)
        oldImage.Dispose()

        Dim graphic As Graphics = Graphics.FromImage(newBitmap)
        Dim hourText As String = String.Format("{0}:00", hour)
        Dim font As New Font("Verdana", 8, FontStyle.Bold)

        Dim extGraphic As ExtendedGraphics = New ExtendedGraphics(graphic)
        Dim pen As Pen = New Pen(Brushes.White, 2.5F)

        If hour <> "" Then
            extGraphic.FillRoundRectangle(Brushes.Black, 390, 430, 50, 18, 5)
            extGraphic.DrawRoundRectangle(pen, 390, 430, 50, 18, 5)
            graphic.DrawString(hourText, font, Brushes.White, 394, 432)
        End If

        graphic.PageUnit = GraphicsUnit.Pixel
        Dim stringSize As SizeF = graphic.MeasureString(day, font)
        extGraphic.FillRoundRectangle(Brushes.DarkRed, 430 - stringSize.Width, 455, stringSize.Width + 10, 18, 5)
        extGraphic.DrawRoundRectangle(pen, 430 - stringSize.Width, 455, stringSize.Width + 10, 18, 5)
        graphic.DrawString(day, font, Brushes.White, 434 - stringSize.Width, 457)

        File.Delete(fileName)
        newBitmap.Save(fileName, Imaging.ImageFormat.Jpeg)
        newBitmap.Dispose()

    End Sub

#End Region

#Region "Parse files"

    Private Function ParseLocationName() As Boolean

        Dim doc As New HtmlAgilityPack.HtmlDocument

        Try
            doc.Load(String.Format("{0}\BBCWeather\BBCWeather_{1}_D.html", Config.GetFolder(Config.Dir.Cache), _areaCode))
            Dim node As HtmlAgilityPack.HtmlNode = doc.DocumentNode.SelectSingleNode("//h1")
            _areaName = node.InnerText
            Log.Info("plugin: BBCWeather - completed parsing BBCWeather_{0}_D.html", _areaCode)
        Catch ex As Exception
            Log.Error("plugin: BBCWeather - error with 24 hour parse : {0}", ex.Message)
            Return False
        End Try

        Return True

    End Function

    Private Function ParseRegionName() As Boolean

        Dim URL As String = String.Format("http://news.bbc.co.uk/weather/forecast/{0}/MapPresenterInner.json", _areaCode)
        Dim response As String = New WebClient().DownloadString(URL)
        Dim jo1 As JsonObject = CType(Conversion.JsonConvert.Import(response), JsonObject)
        Dim jo2 As JsonObject = jo1.Item("MapPresenter")
        _regionName = jo2.Item("showLocation")

        Return True

    End Function

    Private Function Parse5DayWeatherInfo() As Boolean

        Dim doc As New HtmlAgilityPack.HtmlDocument
        Try

            For i As Integer = 0 To 4

                doc.Load(String.Format("{0}\BBCWeather\BBCWeather_{1}_A.html", Config.GetFolder(Config.Dir.Cache), _areaCode))
                Dim node As HtmlAgilityPack.HtmlNode = doc.DocumentNode.SelectSingleNode(String.Format("//tr[@id = ""n5_Day{0}""]", Chr(65 + i)))
                _5DayForecast(i).DayName = node.ChildNodes(1).ChildNodes(1).ChildNodes(1).Attributes("title").Value
                _5DayForecast(i).Summary = node.ChildNodes(3).ChildNodes(1).ChildNodes(3).InnerText
                If ((i = 0) And (DateTime.Now.Hour < 16)) Or (i > 0) Then 'Max temp is not given if after 4pm
                    If _tempUnit.ToLower = "degc" Then
                        _5DayForecast(i).MaxTemp = Replace(node.ChildNodes(5).ChildNodes(1).ChildNodes(1).InnerText, "&deg;C", "")
                    Else
                        _5DayForecast(i).MaxTemp = Replace(node.ChildNodes(5).ChildNodes(1).ChildNodes(3).InnerText, "&deg;F", "")
                    End If
                End If
                If _tempUnit.ToLower = "degc" Then
                    _5DayForecast(i).MinTemp = Replace(node.ChildNodes(7).ChildNodes(1).ChildNodes(1).InnerText, "&deg;C", "")
                Else
                    _5DayForecast(i).MinTemp = Replace(node.ChildNodes(7).ChildNodes(1).ChildNodes(3).InnerText, "&deg;F", "")
                End If
                _5DayForecast(i).WindDirection = node.ChildNodes(9).ChildNodes(1).ChildNodes(1).InnerText.Trim()
                If _windUnit.ToLower = "mph" Then
                    _5DayForecast(i).WindSpeed = Replace(node.ChildNodes(9).ChildNodes(1).ChildNodes(2).InnerText, "mph", "")
                Else
                    _5DayForecast(i).WindSpeed = Replace(node.ChildNodes(9).ChildNodes(1).ChildNodes(4).InnerText, "km/h", "")
                End If
                _5DayForecast(i).Humidity = node.ChildNodes(11).ChildNodes(3).InnerText.Trim()
                _5DayForecast(i).Pressure = node.ChildNodes(11).ChildNodes(7).InnerText.Trim()
                _5DayForecast(i).Visibility = node.ChildNodes(11).ChildNodes(11).InnerText.Trim()

                'Get sunrise/sunset from the day specific page
                For j As Integer = 0 To 4
                    doc.Load(String.Format("{0}\BBCWeather\BBCWeather_{1}_A_{2}.html", Config.GetFolder(Config.Dir.Cache), _areaCode, Chr(65 + j)))
                    node = doc.DocumentNode.SelectSingleNode("//div[@id = ""summary-info""]")
                    If (DateTime.Now.Hour >= 16) AndAlso (j = 0) Then
                        'Sunrise is not given if after 4pm on the day
                        _5DayForecast(i).SunSet = node.ChildNodes(1).ChildNodes(1).ChildNodes(7).InnerText
                    ElseIf j < 2 Then
                        'Sunrise & sunset are shown along with other data on days 1 & 2
                        _5DayForecast(i).SunRise = node.ChildNodes(1).ChildNodes(1).ChildNodes(7).InnerText
                        _5DayForecast(i).SunSet = node.ChildNodes(1).ChildNodes(3).ChildNodes(7).InnerText
                    Else
                        'Sunrise & sunset are shown alone days 3, 4 & 5
                        _5DayForecast(i).SunRise = node.ChildNodes(1).ChildNodes(1).InnerText
                        _5DayForecast(i).SunSet = node.ChildNodes(1).ChildNodes(3).InnerText
                    End If
                Next
                Log.Info("plugin: BBCWeather - completed parsing BBCWeather_{0}_A.html", _areaCode)
            Next

        Catch ex As Exception
            Log.Error("plugin: BBCWeather - error with 5 day parse : {0}", ex.Message)
            Return False
        End Try

        Return True

    End Function

    Private Function Parse24HourWeatherInfo() As Boolean

        Dim doc As New HtmlAgilityPack.HtmlDocument

        Try
            doc.Load(String.Format("{0}\BBCWeather\BBCWeather_{1}_B.html", Config.GetFolder(Config.Dir.Cache), _areaCode))
            Dim node As HtmlAgilityPack.HtmlNode = doc.DocumentNode.SelectSingleNode("//tbody")

            Dim i As Integer = 0
            For Each childnode As HtmlAgilityPack.HtmlNode In node.ChildNodes
                If childnode.Name.ToLower = "tr" Then
                    _24HourForecast(i).DayName = childnode.ChildNodes(1).ChildNodes(0).InnerText.Trim()
                    _24HourForecast(i).Summary = childnode.ChildNodes(3).InnerText.Trim()
                    If _tempUnit.ToLower = "degc" Then
                        _24HourForecast(i).MaxTemp = Replace(childnode.ChildNodes(5).ChildNodes(1).ChildNodes(1).InnerText, "&deg;C", "")
                    Else
                        _24HourForecast(i).MaxTemp = Replace(childnode.ChildNodes(5).ChildNodes(1).ChildNodes(3).InnerText, "&deg;F", "")
                    End If
                    _24HourForecast(i).WindDirection = childnode.ChildNodes(7).ChildNodes(1).ChildNodes(1).InnerText.Trim()
                    If _windUnit.ToLower = "mph" Then
                        _24HourForecast(i).WindSpeed = Replace(childnode.ChildNodes(7).ChildNodes(1).ChildNodes(2).InnerText, "mph", "")
                    Else
                        _24HourForecast(i).WindSpeed = Replace(childnode.ChildNodes(7).ChildNodes(1).ChildNodes(4).InnerText, "km/h", "")
                    End If
                    _24HourForecast(i).Humidity = childnode.ChildNodes(9).ChildNodes(3).InnerText.Trim()
                    _24HourForecast(i).Pressure = childnode.ChildNodes(9).ChildNodes(7).InnerText.Trim()
                    _24HourForecast(i).Visibility = childnode.ChildNodes(9).ChildNodes(11).InnerText.Trim()
                    i += 1
                End If
            Next

            Log.Info("plugin: BBCWeather - completed parsing BBCWeather_{0}_B.html", _areaCode)
        Catch ex As Exception
            Log.Error("plugin: BBCWeather - error with 24 hour parse : {0}", ex.Message)
            Return False
        End Try

        Return True

    End Function

    Private Function ParseMonthlyWeatherInfo() As Boolean

        Dim doc As New HtmlAgilityPack.HtmlDocument
        Try

            doc.Load(String.Format("{0}\BBCWeather\BBCWeather_{1}_C.html", Config.GetFolder(Config.Dir.Cache), _areaCode))
            Dim node As HtmlAgilityPack.HtmlNode = doc.DocumentNode.SelectSingleNode("//div[@class = ""updated""]")
            _monthly.publishedDate = node.ChildNodes(3).InnerText
            _monthly.nextUpdate = node.ChildNodes(5).InnerText
            _monthly.author = node.ChildNodes(7).InnerText

            node = doc.DocumentNode.SelectSingleNode("//div[@class = ""outlook_content""]")
            _monthly.headline = node.ChildNodes(3).InnerText
            _monthly.summary = node.ChildNodes(5).InnerText
            For i As Integer = 0 To 4
                If node.ChildNodes(5 + i).NextSibling.Name.ToLower = "p" Then
                    _monthly.summary += Environment.NewLine + Environment.NewLine
                    _monthly.summary += node.ChildNodes(6 + i).InnerText
                Else
                    Exit For
                End If
            Next

            Dim nodes As HtmlAgilityPack.HtmlNodeCollection = doc.DocumentNode.SelectNodes("//div[@class = ""outlookitem""]")

            _monthly.period1 = nodes(0).ChildNodes(1).InnerText
            _monthly.periodSummary1 = nodes(0).ChildNodes(3).InnerText
            _monthly.periodDetail1 = nodes(0).ChildNodes(5).InnerText
            For i As Integer = 0 To 4
                If nodes(0).ChildNodes(5 + i).NextSibling.Name.ToLower = "p" Then
                    _monthly.periodDetail1 += Environment.NewLine + Environment.NewLine
                    _monthly.periodDetail1 += nodes(0).ChildNodes(6 + i).InnerText
                Else
                    Exit For
                End If
            Next

            _monthly.period2 = nodes(1).ChildNodes(1).InnerText
            _monthly.periodSummary2 = nodes(1).ChildNodes(3).InnerText
            _monthly.periodDetail2 = nodes(1).ChildNodes(5).InnerText
            For i As Integer = 0 To 4
                If nodes(1).ChildNodes(5 + i).NextSibling.Name.ToLower = "p" Then
                    _monthly.periodDetail2 += Environment.NewLine + Environment.NewLine
                    _monthly.periodDetail2 += nodes(1).ChildNodes(6 + i).InnerText
                Else
                    Exit For
                End If
            Next

            _monthly.period3 = nodes(2).ChildNodes(1).InnerText
            _monthly.periodSummary3 = nodes(2).ChildNodes(3).InnerText
            _monthly.periodDetail3 = nodes(2).ChildNodes(5).InnerText
            For i As Integer = 0 To 4
                If nodes(2).ChildNodes(5 + i).NextSibling.Name.ToLower = "p" Then
                    _monthly.periodDetail3 += Environment.NewLine + Environment.NewLine
                    _monthly.periodDetail3 += nodes(2).ChildNodes(6 + i).InnerText
                Else
                    Exit For
                End If
            Next

            Log.Info("plugin: BBCWeather - completed parsing BBCWeather_{0}_C.html", _areaCode)

        Catch ex As Exception
            Log.Error("plugin: BBCWeather - error with 24 hour parse : {0}", ex.Message)
            Return False
        End Try

        Return True

    End Function

    Private Function ParseCurrentObservation() As Boolean

        Dim doc As New HtmlAgilityPack.HtmlDocument

        Try
            doc.Load(String.Format("{0}\BBCWeather\BBCWeather_{1}_D.html", Config.GetFolder(Config.Dir.Cache), _areaCode))
            Dim node As HtmlAgilityPack.HtmlNode = doc.DocumentNode.SelectSingleNode("//div[@id = ""ob_V_node""]")
            _currentObsTime = node.ChildNodes(1).InnerText.Trim()
            _currentObsTime = Right(_currentObsTime, Len(_currentObsTime) - InStr(_currentObsTime, "at ") - 2)
            _currentObsStation = Replace(node.ChildNodes(3).InnerText, "Observation station:", "").Trim()
            _currentSummary = node.ChildNodes(11).ChildNodes(1).InnerText
            If _tempUnit.ToLower = "degc" Then
                _currentTemp = Replace(node.ChildNodes(11).ChildNodes(3).ChildNodes(1).InnerText, "&deg;", "°")
            Else
                _currentTemp = Replace(node.ChildNodes(11).ChildNodes(3).ChildNodes(2).InnerText, "&deg;", "°")
            End If
            If _windUnit.ToLower = "mph" Then
                _currentWind = node.ChildNodes(11).ChildNodes(5).ChildNodes(1).InnerText
            Else
                _currentWind = node.ChildNodes(11).ChildNodes(5).ChildNodes(3).InnerText
            End If
            _currentWind = String.Format("{0} ({1})", _currentWind, GetCompassPoint(Replace(node.ChildNodes(11).ChildNodes(5).ChildNodes(0).InnerText, "Wind:", "").Trim()))
            _currentHumidity = Replace(node.ChildNodes(11).ChildNodes(7).InnerText, "Hum:", "").Trim()
            _currentPressure = Replace(node.ChildNodes(13).ChildNodes(1).InnerText, "Press:", "").Trim()
            _currentVisibility = Replace(node.ChildNodes(13).ChildNodes(3).InnerText, "Vis:", "").Trim()
            Log.Info("plugin: BBCWeather - completed parsing BBCWeather_{0}_D.html", _areaCode)
        Catch ex As Exception
            Log.Error("plugin: BBCWeather - error with current observation parse : {0}", ex.Message)
            Return False
        End Try

        Return True

    End Function

    Private Function GetCompassPoint(ByVal abbr As String) As String

        Dim compassPoint As String = String.Empty

        For Each c As Char In abbr
            If c = "N" Then
                compassPoint += "North "
            ElseIf c = "E" Then
                compassPoint += "East "
            ElseIf c = "S" Then
                compassPoint += "South "
            ElseIf c = "W" Then
                compassPoint += "West "
            End If
        Next

        Return compassPoint.TrimEnd()

    End Function

    Private Function ParseMapOverlay() As Boolean

        Dim doc As New XmlDocument
        doc.Load(String.Format("{0}\BBCWeather\BBCWeather_{1}.xml", Config.GetFolder(Config.Dir.Cache), _areaCode))

        Dim nodes As XmlNodeList = doc.SelectNodes("//placename")

        Dim name As String = String.Empty
        Dim x As Integer = 0
        Dim y As Integer = 0
        Dim align As String = String.Empty

        Dim bitmap As New Bitmap(453, 500)
        Dim graphic As Graphics = Graphics.FromImage(bitmap)
        Dim font As New Font("Verdana", 10, FontStyle.Bold)

        For Each node As XmlNode In nodes
            name = node.Attributes("name").Value
            x = node.Attributes("x").Value
            y = node.Attributes("y").Value
            align = node.Attributes("align").Value
            Dim rect As Rectangle = New Rectangle(x - 2, y - 2, 5, 5)
            graphic.DrawRectangle(Pens.White, rect)
            graphic.FillRectangle(Brushes.White, rect)

            If align.ToLower = "left" Then
                Dim offset As SizeF = graphic.MeasureString(name, font)
                x = x - offset.Width + 2
            Else
                x += 7
            End If

            Dim point1 As New Point(x - 3, y - 7)
            graphic.DrawString(name, font, Brushes.Black, point1)
            Dim point2 As New Point(x - 2, y - 8)
            graphic.DrawString(name, font, Brushes.White, point2)
            graphic.Save()

        Next

        bitmap.Save(String.Format("{0}\Media\BBCWeather\overlay.png", GUIGraphicsContext.Skin), Imaging.ImageFormat.Png)
        graphic.Dispose()
        bitmap.Dispose()


        Return True

    End Function

    Private Function SetInfoServiceProperties() As Boolean

        Try
            If Not _areaName Is Nothing Then SetInfoServiceProperty("#infoservice.weather.location", _areaName)
            If Not _currentTemp Is Nothing Then SetInfoServiceProperty("#infoservice.weather.today.temp", _currentTemp)
            SetInfoServiceProperty("#infoservice.weather.today.feelsliketemp", "N/A")
            If Not _currentHumidity Is Nothing Then SetInfoServiceProperty("#infoservice.weather.today.humidity", _currentHumidity)
            SetInfoServiceProperty("#infoservice.weather.today.sunrise", "N/A")
            SetInfoServiceProperty("#infoservice.weather.today.sunset", "N/A")
            SetInfoServiceProperty("#infoservice.weather.today.uvindex", "N/A")
            If Not _currentWind Is Nothing Then SetInfoServiceProperty("#infoservice.weather.today.wind", _currentWind)
            If Not _currentSummary Is Nothing Then SetInfoServiceProperty("#infoservice.weather.today.condition", _currentSummary)
            If Not _currentSummary Is Nothing Then SetInfoServiceProperty("#infoservice.weather.today.img.small.fullpath", Config.GetFile(Config.Dir.Weather, String.Format("64x64\{0}.png", GetWeatherImage(_currentSummary, , False, True))))
            If Not _currentSummary Is Nothing Then SetInfoServiceProperty("#infoservice.weather.today.img.small.filenamewithext", Path.GetFileName(Config.GetFile(Config.Dir.Weather, String.Format("64x64\{0}.png", GetWeatherImage(_currentSummary, , False, True)))))
            If Not _currentSummary Is Nothing Then SetInfoServiceProperty("#infoservice.weather.today.img.small.filenamewithoutext", Path.GetFileNameWithoutExtension(Config.GetFile(Config.Dir.Weather, String.Format("64x64\{0}.png", GetWeatherImage(_currentSummary, , False, True)))))
            If Not _currentSummary Is Nothing Then SetInfoServiceProperty("#infoservice.weather.today.img.big.fullpath", Config.GetFile(Config.Dir.Weather, String.Format("128x128\{0}.png", GetWeatherImage(_currentSummary, , False, True))))
            If Not _currentSummary Is Nothing Then SetInfoServiceProperty("#infoservice.weather.today.img.big.filenamewithext", Path.GetFileName(Config.GetFile(Config.Dir.Weather, String.Format("128x128\{0}.png", GetWeatherImage(_currentSummary, , False, True)))))
            If Not _currentSummary Is Nothing Then SetInfoServiceProperty("#infoservice.weather.today.img.big.filenamewithoutext", Path.GetFileNameWithoutExtension(Config.GetFile(Config.Dir.Weather, String.Format("128x128\{0}.png", GetWeatherImage(_currentSummary, , False, True)))))
            SetInfoServiceProperty("#infoservice.weather.today.weekday", Now.ToString("dddd"))
            If Not _currentObsStation Is Nothing Then SetInfoServiceProperty("#infoservice.weather.lastupdated.message", String.Format("Observation station is {0}.", _currentObsStation))
            If Not _currentObsTime Is Nothing Then SetInfoServiceProperty("#infoservice.weather.lastupdated.datetime", _currentObsTime)

            Dim num As Integer = 1
            Dim forecast As ForeCast
            For Each forecast In _5DayForecast

                If Not forecast.MinTemp Is Nothing Then SetInfoServiceProperty(String.Format("#infoservice.weather.forecast{0}.mintemp", num), String.Format("{0}°C", forecast.MinTemp))
                If Not forecast.MaxTemp Is Nothing Then SetInfoServiceProperty(String.Format("#infoservice.weather.forecast{0}.maxtemp", num), String.Format("{0}°C", forecast.MaxTemp))
                If Not forecast.SunRise Is Nothing Then SetInfoServiceProperty(String.Format("#infoservice.weather.forecast{0}.sunrise", num), Replace(forecast.SunRise, "sunrise", "").Trim)
                If Not forecast.SunSet Is Nothing Then SetInfoServiceProperty(String.Format("#infoservice.weather.forecast{0}.sunset", num), Replace(forecast.SunSet, "sunset", "").Trim)
                If Not forecast.Summary Is Nothing Then SetInfoServiceProperty(String.Format("#infoservice.weather.forecast{0}.day.condition", num), forecast.Summary)
                SetInfoServiceProperty(String.Format("#infoservice.weather.forecast{0}.night.condition", num), "N/A")
                If Not forecast.WindSpeed Is Nothing Then SetInfoServiceProperty(String.Format("#infoservice.weather.forecast{0}.day.wind", num), forecast.WindSpeed)
                SetInfoServiceProperty(String.Format("#infoservice.weather.forecast{0}.night.wind", num), "N/A")
                If Not forecast.Humidity Is Nothing Then SetInfoServiceProperty(String.Format("#infoservice.weather.forecast{0}.day.humidity", num), forecast.Humidity)
                SetInfoServiceProperty(String.Format("#infoservice.weather.forecast{0}.night.humidity", num), "N/A")
                If Not forecast.Summary Is Nothing Then SetInfoServiceProperty(String.Format("#infoservice.weather.forecast{0}.day.img.small.fullpath", num), Config.GetFile(Config.Dir.Weather, String.Format("64x64\{0}.png", GetWeatherImage(forecast.Summary, , False, True))))
                If Not forecast.Summary Is Nothing Then SetInfoServiceProperty(String.Format("#infoservice.weather.forecast{0}.day.img.small.filenamewithext", num), Path.GetFileName(Config.GetFile(Config.Dir.Weather, String.Format("64x64\{0}.png", GetWeatherImage(forecast.Summary, , False, True)))))
                If Not forecast.Summary Is Nothing Then SetInfoServiceProperty(String.Format("#infoservice.weather.forecast{0}.day.img.small.filenamewithoutext", num), Path.GetFileNameWithoutExtension(Config.GetFile(Config.Dir.Weather, String.Format("64x64\{0}.png", GetWeatherImage(forecast.Summary, , False, True)))))
                If Not forecast.Summary Is Nothing Then SetInfoServiceProperty(String.Format("#infoservice.weather.forecast{0}.day.img.big.fullpath", num), Config.GetFile(Config.Dir.Weather, String.Format("128x128\{0}.png", GetWeatherImage(forecast.Summary, , False, True))))
                If Not forecast.Summary Is Nothing Then SetInfoServiceProperty(String.Format("#infoservice.weather.forecast{0}.day.img.big.filenamewithext", num), Path.GetFileName(Config.GetFile(Config.Dir.Weather, String.Format("128x128\{0}.png", GetWeatherImage(forecast.Summary, , False, True)))))
                If Not forecast.Summary Is Nothing Then SetInfoServiceProperty(String.Format("#infoservice.weather.forecast{0}.day.img.big.filenamewithoutext", num), Path.GetFileNameWithoutExtension(Config.GetFile(Config.Dir.Weather, String.Format("128x128\{0}.png", GetWeatherImage(forecast.Summary, , False, True)))))
                SetInfoServiceProperty(String.Format("#infoservice.weather.forecast{0}.weekday", num), Now.AddDays(num - 1).ToString("dddd"))
                num += 1
            Next
            Log.Info("plugin: BBCWeather - completed setting Infoservice props.")
        Catch ex As Exception
            Log.Error("plugin: BBCWeather - error with setting Infoservice props : {0}", ex.Message)
            Return False
        End Try

        GUIPropertyManager.Changed = True
        Return True

    End Function

    Private Sub SetInfoServiceProperty(ByVal tag As String, tagValue As String)
        GUIPropertyManager.SetProperty(tag, tagValue)
        Log.Debug("plugin: BBCWeather - set {0} to {1}", tag, tagValue)
    End Sub


#End Region

#Region "Plugin ISetupForm members"

    Public Function PluginName() As String Implements MediaPortal.GUI.Library.ISetupForm.PluginName
        Return "BBC Weather"
    End Function

    Public Function Description() As String Implements MediaPortal.GUI.Library.ISetupForm.Description
        Return String.Format("Displays weather forecasts using BBC weather data and images.{0}24 hour, 5 day and monthly outlook forecasts are displayed.", vbCrLf)
    End Function

    Public Function Author() As String Implements MediaPortal.GUI.Library.ISetupForm.Author
        Return "Cheezey"
    End Function

    Public Sub ShowPlugin() Implements MediaPortal.GUI.Library.ISetupForm.ShowPlugin
        Using setupForm As Form = New Global.BBCWeather.PluginSetupForm
            setupForm.ShowDialog()
        End Using
    End Sub

    Public Function CanEnable() As Boolean Implements MediaPortal.GUI.Library.ISetupForm.CanEnable
        Return True
    End Function

    Public Function GetWindowId() As Integer Implements MediaPortal.GUI.Library.ISetupForm.GetWindowId
        Return GetID
    End Function

    Public Function DefaultEnabled() As Boolean Implements MediaPortal.GUI.Library.ISetupForm.DefaultEnabled
        Return False
    End Function

    Public Function HasSetup() As Boolean Implements MediaPortal.GUI.Library.ISetupForm.HasSetup
        Return True
    End Function

    Public Function GetHome(ByRef strButtonText As String, ByRef strButtonImage As String, ByRef strButtonImageFocus As String, ByRef strPictureImage As String) As Boolean Implements MediaPortal.GUI.Library.ISetupForm.GetHome
        strButtonText = PluginName()
        strButtonImage = String.Empty
        strButtonImageFocus = String.Empty
        strPictureImage = "hover_BBCWeather.png"
        Return True
    End Function

#End Region

#Region "IShowPlugin members"

    Public Function ShowDefaultHome() As Boolean Implements MediaPortal.GUI.Library.IShowPlugin.ShowDefaultHome
        Return True
    End Function

#End Region

End Class

Public Class ExtendedGraphics

    Private mGraphics As Graphics
    Public Property Graphics() As Graphics
        Get
            Return Me.mGraphics
        End Get
        Set(value As Graphics)
            Me.mGraphics = value
        End Set
    End Property

    Public Sub New(graphics As Graphics)
        Me.Graphics = graphics
    End Sub


#Region "Fills a Rounded Rectangle with integers."
    Public Sub FillRoundRectangle(brush As Global.System.Drawing.Brush, x As Integer, y As Integer, width As Integer, height As Integer, radius As Integer)

        Dim fx As Single = Convert.ToSingle(x)
        Dim fy As Single = Convert.ToSingle(y)
        Dim fwidth As Single = Convert.ToSingle(width)
        Dim fheight As Single = Convert.ToSingle(height)
        Dim fradius As Single = Convert.ToSingle(radius)
        Me.FillRoundRectangle(brush, fx, fy, fwidth, fheight, fradius)

    End Sub
#End Region


#Region "Fills a Rounded Rectangle with continuous numbers."
    Public Sub FillRoundRectangle(brush As Global.System.Drawing.Brush, x As Single, y As Single, width As Single, height As Single, radius As Single)
        Dim rectangle As New RectangleF(x, y, width, height)
        Dim path As GraphicsPath = Me.GetRoundedRect(rectangle, radius)
        Me.Graphics.FillPath(brush, path)
    End Sub
#End Region


#Region "Draws a Rounded Rectangle border with integers."
    Public Sub DrawRoundRectangle(pen As Global.System.Drawing.Pen, x As Integer, y As Integer, width As Integer, height As Integer, radius As Integer)
        Dim fx As Single = Convert.ToSingle(x)
        Dim fy As Single = Convert.ToSingle(y)
        Dim fwidth As Single = Convert.ToSingle(width)
        Dim fheight As Single = Convert.ToSingle(height)
        Dim fradius As Single = Convert.ToSingle(radius)
        Me.DrawRoundRectangle(pen, fx, fy, fwidth, fheight, fradius)
    End Sub
#End Region


#Region "Draws a Rounded Rectangle border with continuous numbers."
    Public Sub DrawRoundRectangle(pen As Global.System.Drawing.Pen, x As Single, y As Single, width As Single, height As Single, radius As Single)
        Dim rectangle As New RectangleF(x, y, width, height)
        Dim path As GraphicsPath = Me.GetRoundedRect(rectangle, radius)
        Me.Graphics.DrawPath(pen, path)
    End Sub
#End Region


#Region "Get the desired Rounded Rectangle path."
    Private Function GetRoundedRect(baseRect As RectangleF, radius As Single) As GraphicsPath

        ' if corner radius is less than or equal to zero, 
        ' return the original rectangle 
        If radius <= 0.0F Then
            Dim mPath As New GraphicsPath()
            mPath.AddRectangle(baseRect)
            mPath.CloseFigure()
            Return mPath
        End If

        ' if the corner radius is greater than or equal to 
        ' half the width, or height (whichever is shorter) 
        ' then return a capsule instead of a lozenge 
        If radius >= (Math.Min(baseRect.Width, baseRect.Height)) / 2.0 Then
            Return GetCapsule(baseRect)
        End If

        ' create the arc for the rectangle sides and declare 
        ' a graphics path object for the drawing 
        Dim diameter As Single = radius * 2.0F
        Dim sizeF As New SizeF(diameter, diameter)
        Dim arc As New RectangleF(baseRect.Location, sizeF)
        Dim path As GraphicsPath = New Global.System.Drawing.Drawing2D.GraphicsPath()

        ' top left arc 
        path.AddArc(arc, 180, 90)

        ' top right arc 
        arc.X = baseRect.Right - diameter
        path.AddArc(arc, 270, 90)

        ' bottom right arc 
        arc.Y = baseRect.Bottom - diameter
        path.AddArc(arc, 0, 90)

        ' bottom left arc
        arc.X = baseRect.Left
        path.AddArc(arc, 90, 90)

        path.CloseFigure()
        Return path
    End Function
#End Region

#Region "Gets the desired Capsular path."
    Private Function GetCapsule(baseRect As RectangleF) As GraphicsPath
        Dim diameter As Single
        Dim arc As RectangleF
        Dim path As GraphicsPath = New Global.System.Drawing.Drawing2D.GraphicsPath()
        Try
            If baseRect.Width > baseRect.Height Then
                ' return horizontal capsule 

                diameter = baseRect.Height
                Dim sizeF As New SizeF(diameter, diameter)
                arc = New RectangleF(baseRect.Location, sizeF)
                path.AddArc(arc, 90, 180)
                arc.X = baseRect.Right - diameter
                path.AddArc(arc, 270, 180)
            ElseIf baseRect.Width < baseRect.Height Then
                ' return vertical capsule 

                diameter = baseRect.Width
                Dim sizeF As New SizeF(diameter, diameter)
                arc = New RectangleF(baseRect.Location, sizeF)
                path.AddArc(arc, 180, 180)
                arc.Y = baseRect.Bottom - diameter
                path.AddArc(arc, 0, 180)
            Else
                ' return circle 

                path.AddEllipse(baseRect)
            End If
        Catch ex As Exception
            path.AddEllipse(baseRect)
        Finally
            path.CloseFigure()
        End Try
        Return path
    End Function
#End Region
End Class
