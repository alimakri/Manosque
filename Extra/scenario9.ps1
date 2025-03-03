local
Set-Option -DisplayMode Normal
Connect-Service -Name Data

# Nettoyage de la BD
Delete-All

# Enum Type Personne ---------------------------------------------------------------------------------------------
New-Enum -Reference "ChefEntreprise" -Valeur 1 -Commentaire "Type Personne" -Groupe 1
New-Enum -Reference "ChefChantier" -Valeur 2 -Commentaire "Type Personne" -Groupe 1
New-Enum -Reference "Ouvrier" -Valeur 3 -Commentaire "Type Personne" -Groupe 1
# Enum Type Emplacement
New-Enum -Reference "Site" -Valeur 1 -Commentaire "Type Emplacement" -Groupe 6
# Enum Statut Tache
New-Enum -Reference "Chiffrage en cours" -Valeur 1 -Commentaire "Statut Tache" -Groupe 14
New-Enum -Reference "Chiffrage transmis" -Valeur 2 -Commentaire "Statut Tache" -Groupe 14
New-Enum -Reference "Chiffrage refusée" -Valeur -1000 -Commentaire "Statut Tache" -Groupe 14
New-Enum -Reference "Chantier en cours" -Valeur -1000 -Commentaire "Statut Tache" -Groupe 14
New-Enum -Reference "Chantier finalisé" -Valeur -1000 -Commentaire "Statut Tache" -Groupe 14
# Enum Type Retour
New-Enum -Reference "Bool" -Valeur 1 -Commentaire "Type Produit" -Groupe 7
New-Enum -Reference "Texte" -Valeur 2 -Commentaire "Type Produit" -Groupe 7
New-Enum -Reference "Numerique" -Valeur 2 -Commentaire "Type Produit" -Groupe 7
New-Enum -Reference "Photo" -Valeur 3 -Commentaire "Type Produit" -Groupe 7
# Enum Type Produit
New-Enum -Reference "Consommable" -Valeur 1 -Commentaire "Type Produit" -Groupe 2
New-Enum -Reference "Materiel" -Valeur 2 -Commentaire "Type Produit" -Groupe 2
New-Enum -Reference "ProduitNettoyage" -Valeur 3 -Commentaire "Type Produit" -Groupe 2

# Personne ----------------------------------------------------------------------------------------------------------------
New-Personne -Id "0AD1CCD7-2AE3-49BE-94A4-54B84F3DE001" -Nom "Dupont" -Prenom "LeBoss" -Email "dupont@nexity.fr" -Telephone "" -Reference "leboss" -Type @ChefEntreprise 
New-Personne -Id "0AD1CCD7-2AE3-49BE-94A4-54B84F3DE002" -Nom "louvrier" -Prenom "Mohamed" -Email "mohamed@nexity.fr" -Telephone "" -Reference "mohamed" -Type @Ouvrier 

# Sites -------------------------------------------------------------------------------------------------------------------
New-Emplacement -Id "310F51DB-BD8A-44D4-B672-B68B6EE9F001" -Reference "Site1" -Type @Site
New-Emplacement -Id "310F51DB-BD8A-44D4-B672-B68B6EE9F002" -Reference "Site2" -Type @Site
New-Emplacement -Id "310F51DB-BD8A-44D4-B672-B68B6EE9F003" -Reference "Site3" -Type @Site

# Taches
New-Tache -Id "89A16088-0355-4602-B777-0E49B7B0001A" -Reference "Tache 1.A" 
New-Tache -Id "89A16088-0355-4602-B777-0E49B7B0013A" -Reference "Tache 1.3.A" 
New-Tache -Id "89A16088-0355-4602-B777-0E49B7B1111A" -Reference "Tache 1.1.1.A" 
New-Tache -Id "89A16088-0355-4602-B777-0E49B7B1111B" -Reference "Tache 1.1.1.B" 
New-Tache -Id "89A16088-0355-4602-B777-0E49B7B1111C" -Reference "Tache 1.1.1.C" 

# Executions Niveau 0 -----------------------------------------------------------------------------------------------------
New-Execution -Id "50FE19B9-0F82-4ED5-8961-0E49B7B00001" -Reference "EXE_1" -Emplacement "Site1" -DateDebut "17/02/2025 08:00" -Personne "mohamed" 
New-Execution -Id "50FE19B9-0F82-4ED5-8961-0E49B7B00002" -Reference "EXE_2" -Emplacement "Site2" -DateDebut "17/02/2025 08:00" -Personne "mohamed"
New-Execution -Id "50FE19B9-0F82-4ED5-8961-0E49B7B00003" -Reference "EXE_3" -Emplacement "Site3" -DateDebut "17/02/2025 08:00" -Personne "mohamed"

# Executions Niveau 1 -----------------------------------------------------------------------------------------------------
New-Execution -Id "50FE19B9-0F82-4ED5-8961-0E49B7B00110" -Reference "EXE_1.1"						 -Execution "EXE_1" -Emplacement "Site1" -DateDebut "17/02/2025 08:00" -Personne "mohamed"
New-Execution -Id "50FE19B9-0F82-4ED5-8961-0E49B7B00120" -Reference "EXE_1.2"						 -Execution "EXE_1" -Emplacement "Site1" -DateDebut "17/02/2025 08:00" -Personne "mohamed"
New-Execution -Id "50FE19B9-0F82-4ED5-8961-0E49B7B0013A" -Reference "EXE_1.3.A" -Tache "Tache 1.3.A" -Execution "EXE_1" -Emplacement "Site1" -DateDebut "17/02/2025 08:00" -Personne "mohamed"

# Executions Niveau 2 -----------------------------------------------------------------------------------------------------
New-Execution -Id "50FE19B9-0F82-4ED5-8961-0E49B7B0111A" -Reference "EXE_1.1.1.A" -Tache "Tache 1.1.1.A" -Execution "EXE_1.1" -Emplacement "310F51DB-BD8A-44D4-B672-B68B6EE9F001" -DateDebut "17/02/2025 08:00" -Personne "0AD1CCD7-2AE3-49BE-94A4-54B84F3DE002"
New-Execution -Id "50FE19B9-0F82-4ED5-8961-0E49B7B0111B" -Reference "EXE_1.1.1.B" -Tache "Tache 1.1.1.B" -Execution "EXE_1.1" -Emplacement "310F51DB-BD8A-44D4-B672-B68B6EE9F001" -DateDebut "17/02/2025 08:00" -Personne "0AD1CCD7-2AE3-49BE-94A4-54B84F3DE002"
New-Execution -Id "50FE19B9-0F82-4ED5-8961-0E49B7B0111C" -Reference "EXE_1.1.1.C" -Tache "Tache 1.1.1.C" -Execution "EXE_1.1" -Emplacement "310F51DB-BD8A-44D4-B672-B68B6EE9F001" -DateDebut "17/02/2025 08:00" -Personne "0AD1CCD7-2AE3-49BE-94A4-54B84F3DE002"
New-Execution -Id "50FE19B9-0F82-4ED5-8961-0E49B7B01120" -Reference "EXE_1.1.2"                          -Execution "EXE_1.1" -Emplacement "310F51DB-BD8A-44D4-B672-B68B6EE9F001" -DateDebut "17/02/2025 08:00" -Personne "0AD1CCD7-2AE3-49BE-94A4-54B84F3DE002"
New-Execution -Id "50FE19B9-0F82-4ED5-8961-0E49B7B01130" -Reference "EXE_1.1.3"                          -Execution "EXE_1.1" -Emplacement "310F51DB-BD8A-44D4-B672-B68B6EE9F001" -DateDebut "17/02/2025 08:00" -Personne "0AD1CCD7-2AE3-49BE-94A4-54B84F3DE002"

# Tache
New-Action -Id "003663BB-287B-484E-9E1A-ECD8A0C70001" -Question "Tache 1.3.A Action 1" -Tache "Tache 1.3.A" -Type @Texte
New-Action -Id "003663BB-287B-484E-9E1A-ECD8A0C70002" -Question "Tache 1.3.A Action 2" -Tache "Tache 1.3.A" -Type @Texte
New-Action -Id "003663BB-287B-484E-9E1A-ECD8A0C70003" -Question "Tache 1.3.A Action 3" -Tache "Tache 1.3.A" -Type @Texte

# ##################################
# Authentification
Get-Personne -Reference "mohamed" -Password "P@ssw0rd" -Select "Id"

# Liste des sites ******************
# Get-Execution -Reference NULL
# Get-Execution -Execution ^ -Personne "mohamed" -DateDebut "17/02/2025" -Filter "ListeSites"

# WHILE
# Get-Execution -Reference "EXE_1"
# 
# Get-Execution -Execution ^ -Personne "mohamed" -DateDebut "17/02/2025" -Filter "ListeSites"
# 
# Get-Execution -Reference "EXE_1.1"
# 
# Get-Execution -Execution ^ -Personne "mohamed" -DateDebut "17/02/2025" -Filter "ListeSites"
# 
# Get-Tache -Reference "Tache 1.3.A"
# 
# Get-Action -Tache ^
# 
# Connect-Service -Name Api
# Connect-Service -Name Data
# 
# # Connect-Server -Name local -Login "mohamed" -Password "P@ssw0rd"
# # Get-Info
# # Connect-Service -Name Api
# # Connect-Service -Name Api
# 
# Connect-Server -Name ionos -Login "mohamed" -Password "P@ssw0rd"
# Connect-Service -Name Data
# Get-Info

# Connect-Service -Name Api -Login "mohamed" -Password "P@ssw0rd"
# Connect-Service -Name Data
# Get-Personne
# Connect-Service -Name Api
Connect-Service -Name Api -Login "mohamed" -Password "P@ssw0rd"