Param (

    [Parameter(Mandatory=$True)]
    [ValidateNotNullOrEmpty()]
    [string]$ProjectName,

    [Parameter(Mandatory=$True)]
    [ValidateSet('Dev', 'Test', 'UAT', 'Prod', 'Sandbox', 'NonProd')]
    [string]$Environment,

    [Parameter(Mandatory=$True)]
    [ValidateNotNullOrEmpty()]
    [string]$AWSAccountNo,

    [Parameter(Mandatory=$False)]
    [ValidateNotNullOrEmpty()]
    [string[]]$Roles = @('Admin', 'ReadOnly', 'DevOps', 'Developer'),

    [Parameter(Mandatory=$False)]
    [ValidateNotNullOrEmpty()]
    [string]$TeamGroupName

)

<#
.\Create-ADGroups.ps1 -ProjectName 'TeamTorpedo' -Environment NonProd -AWSAccountNo '499349583757'
.\Create-ADGroups.ps1 -ProjectName 'TeamPlatform' -Environment NonProd -AWSAccountNo '499349583757' -TeamGroupName 'R IT BuildSource Team Platform'
#>

Write-Warning "Work in progress / sandbox!"


# Variables
$DomainName = 'dfds.root'
$GroupOU = 'OU=AWS,OU=Permissions,OU=Groups,DC=dfds,DC=root'

Try {$RootDC = Get-ADDomainController -DomainName $DomainName -Discover | Select -Expand HostName -First 1}
Catch {Throw "Could not find domain controller for domain $DomainName"}

Try {$DKDC = Get-ADDomainController -DomainName "dk.$($DomainName)" -Discover | Select -Expand HostName -First 1}
Catch {Throw "Could not find domain controller for domain dk.$($DomainName)"}

$GlobalAdminGroup = Get-ADGroup -Identity 'R AWS Global Admins' -Server $RootDC
$GlobalDevOpsGroup = Get-ADGroup -Identity 'R IT BuildSource Development Excellence Team' -Server $DKDC
If ($TeamGroupName) {
    $TeamGroup = Get-ADGroup -Identity $TeamGroupName -Server $DKDC
}


<#
# Prompt for AWS account details
[string]$BaseNameInput = Read-Host "Enter base account name (without ""dfds-"", separate tokens with hyphens, no spaces or funny characters), e.g. ""Team-Torpedo"""
[string]$Environment = Read-Host "Enter environment name (e.g. "Prod", base account name (without ""dfds-"", separate tokens with hyphens, no spaces or funny characters), e.g. ""Team-Torpedo"""
[string]$AWSAccountNo = Read-Host "Enter AWS account number (12 digits), e.g. ""012345678901"""
#>


# Sanitise AWS account name
$TextInfo = (Get-Culture).TextInfo
#$AWSBaseAccountName = $TextInfo.ToTitleCase("$ProjectName-$Environment")
$AWSBaseAccountName = "$ProjectName-$Environment"
$AWSAccountName = "dfds-$($AWSBaseAccountName.ToLower())"
$AWSAccountEmail = "aws.$($ProjectName.ToLower()).$($Environment.ToLower())@dfds.com"


# Basic check of AWS account number format
If ($AWSAccountNo -notmatch '\d{12}') {
    Throw "Invalid AWS account number format"
}


# Output some helpful info
Write-Host "Email address for shared mailbox: $AWSAccountEmail" -ForegroundColor Green


# Build hashtable of groups to build
$ADGroups = ForEach ($Role in $Roles) {

    Switch ($Environment) {
        "Prod" {
            Switch ($Role) {
                "Admin"     {$GroupMember = @($GlobalAdminGroup)}
                "DevOps"    {$GroupMember = @($GlobalDevOpsGroup)}
                "Developer" {$GroupMember = @()}
                "ReadOnly"  {$GroupMember = @($TeamGroup)}
                Default     {$GroupMember = @()}
            }
        }

        Default {
            Switch ($Role) {
                "Admin"     {$GroupMember = @($GlobalAdminGroup, $TeamGroup)}
                "DevOps"    {$GroupMember = @($GlobalDevOpsGroup)}
                Default     {$GroupMember = @()}
            }
        }

    }

    [pscustomobject]@{
        Name = "P AWS $AWSBaseAccountName $Role"
        Email = "ADFS-$Role@$AWSAccountNo"
        Description = "Grants access to the $Role role of AWS $AWSBaseAccountName account"
        GroupMember = $GroupMember
    }
}


# Generate confirmation prompt
$ADGroupOverview = @()
$ADGroups | ForEach {
    $ADGroupOverview += "$($_.Name) ($($_.Email))`n[$($_.GroupMember.Name -join ', ')]"
}
$ConfirmationPrompt = @"
Create the following AD groups in "$GroupOU"?

$($ADGroupOverview -join "`n`n")
"@


# Prompt for confirmation
$Choices = @()
$Choices += [System.Management.Automation.Host.ChoiceDescription]::New("&Yes")
$Choices += [System.Management.Automation.Host.ChoiceDescription]::New("&No")

$Response = $Host.UI.PromptForChoice("Continue?",$ConfirmationPrompt,$Choices,0)

If ($Response -ne 0) {
    Write-Warning "Aborted"
    Return
}


# Create groups
ForEach ($ADGroup in $ADGroups) {

    Write-Host "Creating group ""$($ADGroup.Name)"" ($($ADGroup.Email))"

    # Create group
    $Group = New-ADGroup -Name $ADGroup.Name -Description $ADGroup.Description -GroupCategory Security -GroupScope Universal -Server $RootDC -OtherAttributes @{'mail'=$ADGroup.Email} -Path $GroupOU -PassThru

    # Add members
    If ($ADGroup.GroupMember) {
        $Group | Add-ADGroupMember -Members $ADGroup.GroupMember
    }

}