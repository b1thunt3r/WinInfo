Function GetCPUArch(arch)
  If arch = 0 Then
    GetCPUArch =  "x86-32"
  ElseIf arch = 1 Then
    GetCPUArch =  "MIPS"
  ElseIf arch = 2 Then
    GetCPUArch =  "Alpha"
  ElseIf arch = 3 Then
    GetCPUArch =  "PowerPC"
  ElseIf arch = 6 Then
    GetCPUArch =  "Itanium"
  ElseIf arch = 9 Then
    GetCPUArch =  "x86-64"
  Else
    GetCPUArch =  "Unknown"
  End If
End Function

Function GetPCType(pc)
  If pc = 0 Then
    GetPCType =  "Unspecified"
  ElseIf pc = 1 Then
    GetPCType =  "Desktop"
  ElseIf pc = 2 Then
    GetPCType =  "Mobile"
  ElseIf pc = 3 Then
    GetPCType =  "Workstation"
  ElseIf pc = 4 Then
    GetPCType =  "Enterprise Server"
  ElseIf pc = 5 Then
    GetPCType =  "Small Office and Home Office (SOHO) Server"
  ElseIf pc = 6 Then
    GetPCType =  "Appliance PC"
  ElseIf pc = 7 Then
    GetPCType =  "Performance Server"
  ElseIf pc = 8 Then
    GetPCType =  "Maximum"
  Else
    GetPCType =  "Unknown"
  End If
End Function

Function TimeSpan(dt1, dt2) 
  ' Function to display the difference between
  ' 2 dates in hh:mm:ss format
  If (isDate(dt1) And IsDate(dt2)) = false Then 
    TimeSpan = "0 days" 
    Exit Function 
  End If 
 
  seconds = Abs(DateDiff("S", dt1, dt2)) 
  minutes = seconds \ 60 
  hours = minutes \ 60 
  days = hours \ 24 
  hours = hours mod 24 
  minutes = minutes mod 60 
  seconds = seconds mod 60 

  dim strDay

  if len(hours) = 1 then hours = "0" & hours 
  if days = 1 then strDay = "day" else strDay = "days"

  TimeSpan = days & " " & strDay & ", " & _
      RIGHT("00" & hours, 2) & ":" & _ 
      RIGHT("00" & minutes, 2) & ":" & _ 
      RIGHT("00" & seconds, 2)
End Function

Function UserInput( myPrompt )
  If UCase( Right( WScript.FullName, 12 ) ) = "\CSCRIPT.EXE" Then
    ' If so, use StdIn and StdOut
    WScript.StdOut.Write myPrompt & ": "
    UserInput = WScript.StdIn.ReadLine
    WScript.StdOut.Write vbNewLine
  Else
    ' If not, use InputBox( )
    UserInput = InputBox( myPrompt , "", ".")
  End If
  
  If (UserInput = "") Then
    UserInput = "."
  End if
  
End Function

Set objArgs = Wscript.Arguments

Dim strComputer
if (objArgs.Count = 1) then
  strComputer = objArgs(0)
Else
  strComputer = UserInput("Enter Compter Name")
  'strComputer = InputBox("Enter Compter Name", "Computer Name/IP", ".")
End If
Dim str

Set objWMIService = GetObject("winmgmts:" _
 & "{impersonationLevel=impersonate}!\\" & _
 strComputer & "\root\cimv2")
 
Set colSettings = objWMIService.ExecQuery _
    ("Select * from Win32_ComputerSystem")
For Each objComputer in colSettings 
    str = "System Name: " & objComputer.Name & vbNewline & _
    "Domain: " & objComputer.Domain & vbNewline & _
    "Workgroup: " & objComputer.Workgroup & vbNewline & _
    "Username: " & objComputer.Username  & vbNewline & vbNewline & _
    "Manufacturer: " & objComputer.Manufacturer & vbNewline & _
    "Model: " & objComputer.Model & vbNewline & _
    "Type: " & GetPCType(objComputer.PCSystemType) & vbNewline & vbNewline & _
    "Total Physical Memory: " & _
     Round((objComputer.TotalPhysicalMemory/(1024^3)),3) & " GB" & vbNewline & _
    "Number of Processors: " & objComputer.NumberOfProcessors & _
    " (" & objComputer.NumberOfLogicalProcessors & " logical cores)"
Next

Set colProcessors = objWMIService.ExecQuery("Select * from Win32_Processor")
For Each objProcessor in colProcessors
  str = str & vbNewline & _
  "Name: " & objProcessor.Name & vbNewline & _
  "Architecture: " & GetCPUArch(objProcessor.Architecture) & _
  " (" & objProcessor.DataWidth & "-bit)" & vbNewline & _
  "Processor ID: " & objProcessor.ProcessorID
Next

'Set colChassis = objWMIService.ExecQuery _
' ("SELECT * FROM Win32_SystemEnclosure")
'For Each objChassis in colChassis
'    str = str & vbNewline & vbNewline & _ 
'    "Name: " & objChassis.Name
'    "SerialNumber: " & objChassis.SerialNumber & vbNewline & _
'    "SMBIOSAssetTag: " & objChassis.SMBIOSAssetTag
'Next

Set colNet = objWMIService.ExecQuery _
 ("SELECT * FROM Win32_NetworkAdapter WHERE (Caption like '%Realtek%' OR Caption like '%Broadcom%') AND (Caption like '%Ethernet%' OR Caption like '%GBE%')")
For Each objNet in colNet
      'Wscript.Echo objNet.MACAddress
      str = str & vbNewline & vbNewline & _ 
      "Ethernet: " & objNet.Name & vbNewline & _
      "MAC: " & Replace(objNet.MACAddress, "-", ":")
Next

Set colOS = objWMIService.ExecQuery _
 ("SELECT * FROM Win32_OperatingSystem")
For Each objOS in colOS
    set iDate = CreateObject("WbemScripting.SWbemDateTime")
    set lDate = CreateObject("WbemScripting.SWbemDateTime")
    iDate.Value = objOS.InstallDate
    lDate.Value = objOS.LastBootUpTime
    str = str & vbNewline & vbNewline & _ 
    "OS: " & Trim(objOS.Caption) & " [" & objOS.Version & "] " & _
    "(" & objOS.OSArchitecture & ")" & vbNewline & _
    "Install Date: " & iDate.GetVarDate & " (" & TimeSpan(iDate.GetVarDate, Now()) & ")" & vbNewline & _
    "Last boot: " & lDate.GetVarDate & _
    " (" & TimeSpan(lDate.GetVarDate, Now()) & ")"
Next

Wscript.Echo str