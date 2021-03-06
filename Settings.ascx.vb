'
' Copyright (c) 2008 - 2009, interApps, Erik van Ballegoij, http://www.interapps.nl
' All rights reserved.
'
' Redistribution and use in source and binary forms, with or without modification, are permitted provided that the 
' following conditions are met:
'
' * Redistributions of source code must retain the above copyright notice, this list of conditions and the 
'   following disclaimer.
' * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the 
'   following disclaimer in the documentation and/or other materials provided with the distribution.
' * Neither the name of Apollo Software nor the names of its contributors may be used to endorse or promote products 
'  derived from this software without specific prior written permission.
'
' THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
' INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE 
' DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
' SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR 
' SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
' WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE 
' USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
'


Imports DotNetNuke
Imports DotNetNuke.Entities.Modules

Namespace interApps.DNN.Modules.IdentitySwitcher

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The Settings class manages Module Settings
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class Settings
        Inherits Entities.Modules.ModuleSettingsBase

#Region "Base Method Implementations"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' LoadSettings loads the settings from the Database and displays them
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Sub LoadSettings()
            Try
				If (Page.IsPostBack = False) Then
					rbSortBy.Items.Add(New ListItem(Localization.GetString("SortByDisplayName", LocalResourceFile), "DisplayName"))
					rbSortBy.Items.Add(New ListItem(Localization.GetString("SortByUserName", LocalResourceFile), "UserName"))
					rbSortBy.SelectedIndex = 0


					If UserInfo.IsSuperUser Then
						If TabModuleSettings.Contains("includeHost") Then
							Me.cbIncludeHostUser.Checked = Boolean.Parse(TabModuleSettings("includeHost").ToString())
						End If
					Else
						trHostSettings.Visible = False
					End If
					If TabModuleSettings.Contains("useAjax") Then
						Me.cbUseAjax.Checked = Boolean.Parse(TabModuleSettings("useAjax").ToString())
					Else
						Me.cbUseAjax.Checked = True
					End If

					If TabModuleSettings.Contains("sortBy") Then
						rbSortBy.SelectedValue = TabModuleSettings("sortBy").ToString()
					End If
				End If
            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' UpdateSettings saves the modified settings to the Database
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Sub UpdateSettings()
            Try
                Dim objModules As New Entities.Modules.ModuleController
                If UserInfo.IsSuperUser Then
                    objModules.UpdateTabModuleSetting(TabModuleId, "includeHost", Me.cbIncludeHostUser.Checked.ToString)
                End If
				objModules.UpdateTabModuleSetting(TabModuleId, "useAjax", Me.cbUseAjax.Checked.ToString)
				objModules.UpdateTabModuleSetting(TabModuleId, "sortBy", rbSortBy.SelectedValue)

				' refresh cache
                ModuleController.SynchronizeModule(ModuleId)
            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

#End Region

    End Class

End Namespace

