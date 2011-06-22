<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PluginSetupForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PluginSetupForm))
        Me.btnLookup = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tbxInput = New System.Windows.Forms.TextBox()
        Me.clbResults = New System.Windows.Forms.CheckedListBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.cbxInfoService = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.rbnMph = New System.Windows.Forms.RadioButton()
        Me.rbnKph = New System.Windows.Forms.RadioButton()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.rbnDegC = New System.Windows.Forms.RadioButton()
        Me.rbnDegF = New System.Windows.Forms.RadioButton()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnLookup
        '
        Me.btnLookup.Location = New System.Drawing.Point(183, 38)
        Me.btnLookup.Name = "btnLookup"
        Me.btnLookup.Size = New System.Drawing.Size(55, 23)
        Me.btnLookup.TabIndex = 0
        Me.btnLookup.Text = "Lookup"
        Me.btnLookup.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(225, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Enter a town, city, UK county or UK postcode:"
        '
        'tbxInput
        '
        Me.tbxInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbxInput.Location = New System.Drawing.Point(16, 40)
        Me.tbxInput.Name = "tbxInput"
        Me.tbxInput.Size = New System.Drawing.Size(161, 20)
        Me.tbxInput.TabIndex = 2
        '
        'clbResults
        '
        Me.clbResults.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.clbResults.CheckOnClick = True
        Me.clbResults.FormattingEnabled = True
        Me.clbResults.HorizontalScrollbar = True
        Me.clbResults.Location = New System.Drawing.Point(16, 77)
        Me.clbResults.Name = "clbResults"
        Me.clbResults.Size = New System.Drawing.Size(222, 107)
        Me.clbResults.TabIndex = 3
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(183, 322)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(55, 23)
        Me.btnSave.TabIndex = 4
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.BBCWeather.My.Resources.Resources.btn_donate_SM
        Me.PictureBox1.Location = New System.Drawing.Point(16, 322)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(75, 23)
        Me.PictureBox1.TabIndex = 5
        Me.PictureBox1.TabStop = False
        '
        'cbxInfoService
        '
        Me.cbxInfoService.AutoSize = True
        Me.cbxInfoService.Location = New System.Drawing.Point(16, 198)
        Me.cbxInfoService.Name = "cbxInfoService"
        Me.cbxInfoService.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.cbxInfoService.Size = New System.Drawing.Size(188, 17)
        Me.cbxInfoService.TabIndex = 6
        Me.cbxInfoService.Text = "Override InfoService weather data"
        Me.cbxInfoService.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.rbnKph)
        Me.GroupBox1.Controls.Add(Me.rbnMph)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 221)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(222, 40)
        Me.GroupBox1.TabIndex = 7
        Me.GroupBox1.TabStop = False
        '
        'rbnMph
        '
        Me.rbnMph.AutoSize = True
        Me.rbnMph.Location = New System.Drawing.Point(115, 14)
        Me.rbnMph.Name = "rbnMph"
        Me.rbnMph.Size = New System.Drawing.Size(46, 17)
        Me.rbnMph.TabIndex = 0
        Me.rbnMph.TabStop = True
        Me.rbnMph.Text = "Mph"
        Me.rbnMph.UseVisualStyleBackColor = True
        '
        'rbnKph
        '
        Me.rbnKph.AutoSize = True
        Me.rbnKph.Location = New System.Drawing.Point(172, 14)
        Me.rbnKph.Name = "rbnKph"
        Me.rbnKph.Size = New System.Drawing.Size(44, 17)
        Me.rbnKph.TabIndex = 1
        Me.rbnKph.TabStop = True
        Me.rbnKph.Text = "Kph"
        Me.rbnKph.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(57, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Wind units"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 16)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(92, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Temperature units"
        '
        'rbnDegC
        '
        Me.rbnDegC.AutoSize = True
        Me.rbnDegC.Location = New System.Drawing.Point(115, 14)
        Me.rbnDegC.Name = "rbnDegC"
        Me.rbnDegC.Size = New System.Drawing.Size(36, 17)
        Me.rbnDegC.TabIndex = 0
        Me.rbnDegC.TabStop = True
        Me.rbnDegC.Text = "°C"
        Me.rbnDegC.UseVisualStyleBackColor = True
        '
        'rbnDegF
        '
        Me.rbnDegF.AutoSize = True
        Me.rbnDegF.Location = New System.Drawing.Point(172, 14)
        Me.rbnDegF.Name = "rbnDegF"
        Me.rbnDegF.Size = New System.Drawing.Size(35, 17)
        Me.rbnDegF.TabIndex = 1
        Me.rbnDegF.TabStop = True
        Me.rbnDegF.Text = "°F"
        Me.rbnDegF.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.rbnDegF)
        Me.GroupBox2.Controls.Add(Me.rbnDegC)
        Me.GroupBox2.Location = New System.Drawing.Point(16, 267)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(222, 40)
        Me.GroupBox2.TabIndex = 8
        Me.GroupBox2.TabStop = False
        '
        'PluginSetupForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(255, 357)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.cbxInfoService)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.btnLookup)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.clbResults)
        Me.Controls.Add(Me.tbxInput)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "PluginSetupForm"
        Me.Text = "BBC Weather config"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnLookup As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tbxInput As System.Windows.Forms.TextBox
    Friend WithEvents clbResults As System.Windows.Forms.CheckedListBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents cbxInfoService As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents rbnKph As System.Windows.Forms.RadioButton
    Friend WithEvents rbnMph As System.Windows.Forms.RadioButton
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents rbnDegC As System.Windows.Forms.RadioButton
    Friend WithEvents rbnDegF As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
End Class
