
Param
(
[string]$SiteURL = $null
)

write-host “Adding Snapin”
Add-PsSnapin Microsoft.SharePoint.PowerShell
write-host “Added”

$site=new-object Microsoft.SharePoint.SPSite($SiteURL);
$web=$site.RootWeb;

write-host “Installing WSP”
Add-SPSolution BJSmarts.SharePoint.wsp
Start-Sleep -s 30
Install-SPSolution -Identity BJSmarts.SharePoint.wsp -GACDeployment -force
Start-Sleep -s 30
write-host “Installtion of WSP Done.”

write-host “Removing Snapin”
Remove-PsSnapin Microsoft.SharePoint.PowerShell
write-host “Removed”
