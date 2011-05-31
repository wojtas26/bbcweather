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
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.btnSave.Location = New System.Drawing.Point(183, 201)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(55, 23)
        Me.btnSave.TabIndex = 4
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.BBCWeather.My.Resources.Resources.btn_donate_SM
        Me.PictureBox1.Location = New System.Drawing.Point(16, 201)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(75, 23)
        Me.PictureBox1.TabIndex = 5
        Me.PictureBox1.TabStop = False
        '
        'PluginSetupForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(255, 236)
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
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnLookup As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tbxInput As System.Windows.Forms.TextBox
    Friend WithEvents clbResults As System.Windows.Forms.CheckedListBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
End Class
