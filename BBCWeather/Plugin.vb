﻿Imports System
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms
Imports System.Runtime.CompilerServices

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

    End Enum

#End Region

#Region "Variables"

    <SkinControlAttribute(2)> Protected _button5days As GUIButtonControl = Nothing
    <SkinControlAttribute(3)> Protected _button24Hours As GUIButtonControl = Nothing
    <SkinControlAttribute(4)> Protected _buttonMonthly As GUIButtonControl = Nothing

    Private Const NUM_DAYS As Integer = 5

    Private _5DayForecast As ForeCast() = New ForeCast(NUM_DAYS) {}
    Private _24HourForecast As ForeCast() = New ForeCast(7) {}
    Private _monthly As MonthForeCast = New MonthForeCast
    Private _locationCode As String = String.Empty
    Private _locationName As String = String.Empty
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
            _locationCode = xmlReader.GetValue("BBCWeather", "areaCode")
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
        Return Load(GUIGraphicsContext.Skin & "\BBCWeather.xml")
    End Function

    Protected Overrides Sub OnPageLoad()

        AppDomain.CurrentDomain.AppendPrivatePath(String.Format("{0}\Plugins\Windows\BBCWeather", AppDomain.CurrentDomain.BaseDirectory))

        MyBase.OnPageLoad()

        LoadSettings()

        If _locationCode = "" Then
            _locationCode = "8"
            Log.Info("plugin: BBCWeather starting - areaCode is unknown, defaulting to {0}", _locationCode)
        Else
            Log.Info("plugin: BBCWeather starting - areaCode is set to {0}", _locationCode)
        End If

        HideAllControls()
        RefreshNewMode()

        _downloadLock = New Object

        If _lastRefreshTime < Now.AddMinutes(-_refreshIntervalMinutes) Then BackgroundUpdate(False)

    End Sub

    Public Overrides Sub Process()

        Dim a As DateTime = DateTime.Now

        If (DateTime.Now - _lastRefreshTime).Minutes >= _refreshIntervalMinutes AndAlso _locationCode <> String.Empty AndAlso Not Me.IsRefreshing Then
            Log.Debug("plugin: BBCWeather: autoupdating data.")
            BackgroundUpdate(True)
        End If

        MyBase.Process()

    End Sub

    Protected Overrides Sub OnClicked(controlId As Integer, control As MediaPortal.GUI.Library.GUIControl, actionType As MediaPortal.GUI.Library.Action.ActionType)

        HideAllControls()
        SetHeaderControls()
        SetCurrentWeatherControls()

        If control Is _button5days Then
            _currentMode = Mode.FiveDay
        ElseIf control Is _button24Hours Then
            _currentMode = Mode.TwentyFourHours
        ElseIf control Is _buttonMonthly Then
            _currentMode = Mode.Monthly
        ElseIf (control.GetID = 504) And actionType = MediaPortal.GUI.Library.Action.ActionType.ACTION_MOVE_UP Then
            control.NavigateUp = 1
        ElseIf (control.GetID = 504) And actionType = MediaPortal.GUI.Library.Action.ActionType.ACTION_MOVE_DOWN Then
            control.NavigateDown = 1
        End If

        RefreshNewMode()

        MyBase.OnClicked(controlId, control, actionType)

    End Sub

#End Region

#Region "Skin control methods"

    Private Sub RefreshNewMode()
        Select Case _currentMode
            Case Mode.FiveDay
                Set5DayModeControls()
                GUIControl.FocusControl(GetID, 2)
            Case Mode.TwentyFourHours
                Set24HourModeControls()
                GUIControl.FocusControl(GetID, 3)
            Case Mode.Monthly
                SetMonthlyModeControls()
                GUIControl.FocusControl(GetID, 4)
        End Select
    End Sub

    Private Sub HideAllControls()

        For Each i As Integer In [Enum].GetValues(GetType(Controls))
            GUIControl.HideControl(GetID, i)
        Next

    End Sub

    Private Sub SetHeaderControls()

        GUIControl.ShowControl(GetID, Controls.LBL_LOCATION)
        GUIControl.SetControlLabel(GetID, Controls.LBL_LOCATION, _locationName)

    End Sub

    Private Sub SetCurrentWeatherControls()

        'Current weather controls
        For i As Integer = 100 To 116
            GUIControl.ShowControl(GetID, i)
        Next

        GUIControl.SetControlLabel(GetID, Controls.LBL_CURRENT_SUMMARY, _currentSummary)
        GUIControl.SetControlLabel(GetID, Controls.LBL_CURRENT_TEMP, _currentTemp)
        GUIControl.SetControlLabel(GetID, Controls.LBL_CURRENT_WIND, _currentWind)
        GUIControl.SetControlLabel(GetID, Controls.LBL_CURRENT_HUMIDITY, _currentHumidity)
        GUIControl.SetControlLabel(GetID, Controls.LBL_CURRENT_PRESSURE, _currentPressure)
        GUIControl.SetControlLabel(GetID, Controls.LBL_CURRENT_VISIBILITY, _currentVisibility)
        GUIControl.SetControlLabel(GetID, Controls.LBL_CURRENT_OBS_TIME, _currentObsTime)
        GUIControl.SetControlLabel(GetID, Controls.LBL_CURRENT_OBS_STATION, _currentObsStation)

    End Sub

    Private Sub Set5DayModeControls()

        HideAllControls()
        SetHeaderControls()
        SetCurrentWeatherControls()

        Dim image As GUIImage = Nothing

        For dayNum As Integer = 0 To 4

            For i As Integer = 200 To 299
                GUIControl.ShowControl(GetID, i)
            Next

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

        For i As Integer = 500 To 505
            GUIControl.ShowControl(GetID, i)
        Next

        Dim txt As String
        txt = String.Format("{0} {1}", _monthly.publishedDate, _monthly.nextUpdate)

        GUIControl.SetControlLabel(GetID, Controls.LBL_MONTHLY_PUBLISHED_LABEL, txt)
        GUIControl.SetControlLabel(GetID, Controls.LBL_MONTHLY_AUTHOR_LABEL, _monthly.author)
        GUIControl.SetControlLabel(GetID, Controls.LBL_MONTHLY_HEADLINE_LABEL, _monthly.headline)

        Dim break As String = String.Format("{0}{0}{1}{0}{0}", Environment.NewLine, Replicate("=", 30))
        txt = _monthly.summary
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
        txt += _monthly.periodDetail3

        GUIControl.SetControlLabel(GetID, Controls.TBX_MONTHLY_DETAIL_TEXTBOX, txt)

    End Sub

    Private Function GetTemperatureImage(ByVal temp As String) As String

        Dim img As String = "na"

        If temp <> "" Then
            Select Case CInt(temp)
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

    Private Function GetWeatherImage(ByVal weather As String, Optional ByVal hour As Integer = 12) As String

        Dim img As Integer = 0
        Dim day As Integer = 0
        'If hour = -1 Then
        '    If Now.Hour < 16 Then day = 1
        'Else
        If hour >= 6 And hour <= 20 Then day = 1
        'End If


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
            Case "black cloud"
                img = 8
            Case "light rain shower"
                img = 9 + day
            Case "light rain"
                img = 11 + day
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

        Return (String.Format("{0}\Media\BBCWeather\weather\{1}.png", GUIGraphicsContext.Skin, img.ToString))

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

                Dim downloadSuccess As Boolean = False
                Dim parseDataSuccess As Integer = 0
                '==============================================================================================================
                ' http://news.bbc.co.uk/weather/forecast/355/Forecast.xhtml?state=fo:A = Next Five Days
                ' http://news.bbc.co.uk/weather/forecast/355/Forecast.xhtml?state=fo:B = Twenty Four Hours
                ' http://news.bbc.co.uk/weather/forecast/355/Forecast.xhtml?state=fo:C = Monthly Outlook
                ' http://news.bbc.co.uk/weather/forecast/355/ObservationsEmbed.xhtml   = Latest Observations
                ' http://news.bbc.co.uk/weather/forecast/355/Forecast.xhtml?&showDay=A = Days detail
                '==============================================================================================================

                downloadSuccess = DownloadForecast(String.Format("http://news.bbc.co.uk/weather/forecast/{0}/Forecast.xhtml?state=fo:A", _locationCode), "A") '5 days
                For i As Integer = 0 To 4
                    downloadSuccess = DownloadForecast(String.Format("http://news.bbc.co.uk/weather/forecast/{0}/Forecast.xhtml?&showDay={1}", _locationCode, Chr(65 + i)), String.Format("A_{0}", Chr(65 + i))) '5 day - day 1
                Next
                downloadSuccess = DownloadForecast(String.Format("http://news.bbc.co.uk/weather/forecast/{0}/Forecast.xhtml?state=fo:B", _locationCode), "B") '24 hours
                downloadSuccess = DownloadForecast(String.Format("http://news.bbc.co.uk/weather/forecast/{0}/Forecast.xhtml?state=fo:C", _locationCode), "C") 'Monthly
                downloadSuccess = DownloadForecast(String.Format("http://news.bbc.co.uk/weather/forecast/{0}/ObservationsEmbed.xhtml", _locationCode), "D") 'Latest observations

                If downloadSuccess Then
                    If Not ParseLocationName() AndAlso Not autoUpdate Then DisplayErrorDialog("location name")
                    If Not ParseCurrentObservation() AndAlso Not autoUpdate Then DisplayErrorDialog("current observation")
                    If Not Parse5DayWeatherInfo() AndAlso Not autoUpdate Then DisplayErrorDialog("5 day forecast")
                    If Not Parse24HourWeatherInfo() AndAlso Not autoUpdate Then DisplayErrorDialog("24 hour forecast")
                    If Not ParseMonthlyWeatherInfo() AndAlso Not autoUpdate Then DisplayErrorDialog("monthly outlook")
                End If

                RefreshNewMode()

                _lastRefreshTime = DateTime.Now

            End Using
        End SyncLock

        RefreshNewMode()

    End Sub

    Private Sub DisplayErrorDialog(ByVal forecastType As String)
        Dim errorDialog As GUIDialogOK = DirectCast(GUIWindowManager.GetWindow(CInt(Window.WINDOW_DIALOG_OK)), GUIDialogOK)
        errorDialog.SetHeading("BBC Weather parsing error")
        errorDialog.SetLine(1, String.Format("Error parsing {0}.", forecastType))
        errorDialog.SetLine(2, String.Format("Location is {0} ({1})", _locationName, _locationCode))
        errorDialog.SetLine(3, String.Empty)
        errorDialog.DoModal(GetID)
    End Sub

#End Region

#Region "Downloads"

    Private Function DownloadForecast(ByVal URL As String, ByVal forecastType As String) As Boolean

        If Not Win32API.IsConnectedToInternet() Then
            Log.Error("plugin: BBCWeather - error downloading weather, no internet connection")
            Return False
        End If

        Try
            Dim sourceString As String = New WebClient().DownloadString(URL)
            Dim writer As StreamWriter = New StreamWriter(String.Format("{0}\BBCWeather_{1}_{2}.html", Config.GetFolder(Config.Dir.Cache), _locationCode, forecastType), False)
            writer.WriteLine(sourceString)
            writer.Close()
        Catch ex As Exception
            Log.Error("plugin: BBCWeather - error downloading weather from {0}", URL)
            Return False
        End Try

        Log.Info("plugin: BBCWeather - successfully downloaded {0}", URL)

        Return True

    End Function

#End Region

#Region "Parse HTML files"

    Private Function ParseLocationName() As Boolean

        Dim doc As New HtmlAgilityPack.HtmlDocument

        Try
            doc.Load(String.Format("{0}\BBCWeather_{1}_D.html", Config.GetFolder(Config.Dir.Cache), _locationCode))
            Dim node As HtmlAgilityPack.HtmlNode = doc.DocumentNode.SelectSingleNode("//h1")
            _locationName = node.InnerText

            Log.Info("plugin: BBCWeather - completed parsing BBCWeather_{0}_D.html", _locationCode)
        Catch ex As Exception
            Log.Error("plugin: BBCWeather - error with 24 hour parse : {0}", ex.Message)
            Return False
        End Try

        Return True

    End Function

    Private Function Parse5DayWeatherInfo() As Boolean

        Dim doc As New HtmlAgilityPack.HtmlDocument
        Try

            For i As Integer = 0 To 4

                doc.Load(String.Format("{0}\BBCWeather_{1}_A.html", Config.GetFolder(Config.Dir.Cache), _locationCode))
                Dim node As HtmlAgilityPack.HtmlNode = doc.DocumentNode.SelectSingleNode(String.Format("//tr[@id = ""n5_Day{0}""]", Chr(65 + i)))
                _5DayForecast(i).DayName = node.ChildNodes(1).ChildNodes(1).ChildNodes(1).Attributes("title").Value
                _5DayForecast(i).Summary = node.ChildNodes(3).ChildNodes(1).ChildNodes(3).InnerText
                'Max temp is not given if after 4pm
                If ((i = 0) And (DateTime.Now.Hour < 16)) Or (i > 0) Then _5DayForecast(i).MaxTemp = Replace(node.ChildNodes(5).ChildNodes(1).ChildNodes(1).InnerText, "&deg;C", "") '5-2-1 for degF
                _5DayForecast(i).MinTemp = Replace(node.ChildNodes(7).ChildNodes(1).ChildNodes(1).InnerText, "&deg;C", "") '7-2-1 for degF
                _5DayForecast(i).WindDirection = node.ChildNodes(9).ChildNodes(1).ChildNodes(1).InnerText.Trim()
                _5DayForecast(i).WindSpeed = Replace(node.ChildNodes(9).ChildNodes(1).ChildNodes(2).InnerText, "mph", "") '9-1-3 for kph
                _5DayForecast(i).Humidity = node.ChildNodes(11).ChildNodes(3).InnerText.Trim()
                _5DayForecast(i).Pressure = node.ChildNodes(11).ChildNodes(7).InnerText.Trim()
                _5DayForecast(i).Visibility = node.ChildNodes(11).ChildNodes(11).InnerText.Trim()

                'Get sunrise/sunset from the day specific page
                For j As Integer = 0 To 4
                    doc.Load(String.Format("{0}\BBCWeather_{1}_A_{2}.html", Config.GetFolder(Config.Dir.Cache), _locationCode, Chr(65 + j)))
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
                Log.Info("plugin: BBCWeather - completed parsing BBCWeather_{0}_A.html", _locationCode)
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
            doc.Load(String.Format("{0}\BBCWeather_{1}_B.html", Config.GetFolder(Config.Dir.Cache), _locationCode))
            Dim node As HtmlAgilityPack.HtmlNode = doc.DocumentNode.SelectSingleNode("//tbody")

            Dim i As Integer = 0
            For Each childnode As HtmlAgilityPack.HtmlNode In node.ChildNodes
                If childnode.Name.ToLower = "tr" Then
                    If childnode.ChildNodes(1).ChildNodes.Count > 1 Then
                        _24HourForecast(i).DayName = childnode.ChildNodes(1).ChildNodes(0).InnerText.Trim() + vbCrLf
                        _24HourForecast(i).DayName += childnode.ChildNodes(1).ChildNodes(2).InnerText.Trim()
                        _24HourForecast(i).DayName = _24HourForecast(i).DayName.Trim()
                    Else
                        _24HourForecast(i).DayName = childnode.ChildNodes(1).ChildNodes(0).InnerText.Trim()
                    End If
                    _24HourForecast(i).Summary = childnode.ChildNodes(3).InnerText.Trim()
                    _24HourForecast(i).MaxTemp = Replace(childnode.ChildNodes(5).ChildNodes(1).ChildNodes(1).InnerText, "&deg;C", "") '5-1-3 degF
                    _24HourForecast(i).WindDirection = childnode.ChildNodes(7).ChildNodes(1).ChildNodes(1).InnerText.Trim()
                    _24HourForecast(i).WindSpeed = Replace(childnode.ChildNodes(7).ChildNodes(1).ChildNodes(2).InnerText, "mph", "")
                    _24HourForecast(i).Humidity = childnode.ChildNodes(9).ChildNodes(3).InnerText.Trim()
                    _24HourForecast(i).Pressure = childnode.ChildNodes(9).ChildNodes(7).InnerText.Trim()
                    _24HourForecast(i).Visibility = childnode.ChildNodes(9).ChildNodes(11).InnerText.Trim()
                    i += 1
                End If
            Next

            Log.Info("plugin: BBCWeather - completed parsing BBCWeather_{0}_B.html", _locationCode)
        Catch ex As Exception
            Log.Error("plugin: BBCWeather - error with 24 hour parse : {0}", ex.Message)
            Return False
        End Try

        Return True

    End Function

    Private Function ParseMonthlyWeatherInfo() As Boolean

        Dim doc As New HtmlAgilityPack.HtmlDocument
        Try

            doc.Load(String.Format("{0}\BBCWeather_{1}_C.html", Config.GetFolder(Config.Dir.Cache), _locationCode))
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

            Log.Info("plugin: BBCWeather - completed parsing BBCWeather_{0}_C.html", _locationCode)

        Catch ex As Exception
            Log.Error("plugin: BBCWeather - error with 24 hour parse : {0}", ex.Message)
            Return False
        End Try

        Return True

    End Function

    Private Function ParseCurrentObservation() As Boolean

        Dim doc As New HtmlAgilityPack.HtmlDocument

        Try
            doc.Load(String.Format("{0}\BBCWeather_{1}_D.html", Config.GetFolder(Config.Dir.Cache), _locationCode))
            Dim node As HtmlAgilityPack.HtmlNode = doc.DocumentNode.SelectSingleNode("//div[@id = ""ob_V_node""]")
            _currentObsTime = node.ChildNodes(1).InnerText.Trim()
            _currentObsTime = Right(_currentObsTime, Len(_currentObsTime) - InStr(_currentObsTime, "at ") - 2)
            _currentObsStation = Replace(node.ChildNodes(3).InnerText, "Observation station:", "").Trim()
            _currentSummary = node.ChildNodes(11).ChildNodes(1).InnerText
            _currentTemp = Replace(node.ChildNodes(11).ChildNodes(3).ChildNodes(1).InnerText, "&deg;", "°")
            _currentWind = Replace(node.ChildNodes(11).ChildNodes(5).ChildNodes(0).InnerText, "Wind:", "").Trim()
            _currentWind = String.Format("{0} ({1})", node.ChildNodes(11).ChildNodes(5).ChildNodes(1).InnerText, GetCompassPoint(_currentWind))
            _currentHumidity = Replace(node.ChildNodes(11).ChildNodes(7).InnerText, "Hum:", "").Trim()
            _currentPressure = Replace(node.ChildNodes(13).ChildNodes(1).InnerText, "Press:", "").Trim()
            _currentVisibility = Replace(node.ChildNodes(13).ChildNodes(3).InnerText, "Vis:", "").Trim()
            Log.Info("plugin: BBCWeather - completed parsing BBCWeather_{0}_D.html", _locationCode)
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
