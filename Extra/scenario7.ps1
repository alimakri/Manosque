Set-Option -DisplayMode Normal
Set-Option -Service Api

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

# Personne -----------------------------------------------------------------------------------------------------
New-Personne -Id "0AD1CCD7-2AE3-49BE-94A4-54B84F3DE001" -Nom "Dupont" -Prenom "LeBoss" -Email "dupont@nexity.fr" -Telephone "" -Reference "leboss" -Type @ChefEntreprise 
New-Personne -Id "0AD1CCD7-2AE3-49BE-94A4-54B84F3DE002" -Nom "louvrier" -Prenom "Mohamed" -Email "mohamed@nexity.fr" -Telephone "" -Reference "mohamed" -Type @Ouvrier 

# Emplacement 1 -----------------------------------------------------------------------------------------------------
New-Emplacement -Id "310F51DB-BD8A-44D4-B672-B68B6EE9FE6A" -Reference "Part-Dieu" -Type @Site

# Planification des tâches : Remise en état lot vacants -------------------------------------------------------------
New-Tache -Id "89A16088-0355-4602-B777-0E49B7B10000" -Reference "Remise en état lot vacants" 
# 
# New-Tache -Id "89A16088-0355-4602-B777-0E49B7B11000" -Reference "Ligne sol"						-Tache "89A16088-0355-4602-B777-0E49B7B10000"
# New-Tache -Id "89A16088-0355-4602-B777-0E49B7B12000" -Reference "Ligne mur"						-Tache "89A16088-0355-4602-B777-0E49B7B10000"
# New-Tache -Id "89A16088-0355-4602-B777-0E49B7B13000" -Reference "Ligne plafond"					-Tache "89A16088-0355-4602-B777-0E49B7B10000"
# New-Tache -Id "89A16088-0355-4602-B777-0E49B7B14000" -Reference "Ligne éclairage"				-Tache "89A16088-0355-4602-B777-0E49B7B10000"
# New-Tache -Id "89A16088-0355-4602-B777-0E49B7B15000" -Reference "Ligne équipement sanitaire "	-Tache "89A16088-0355-4602-B777-0E49B7B10000"
# # Ligne sol
# New-Tache -Id "89A16088-0355-4602-B777-0E49B7B11100" -Reference "Reprise nécessaire"			-Tache "89A16088-0355-4602-B777-0E49B7B11000"
# New-Tache -Id "89A16088-0355-4602-B777-0E49B7B11200" -Reference "Revêtement souhaité"			-Tache "89A16088-0355-4602-B777-0E49B7B11000"
# New-Tache -Id "89A16088-0355-4602-B777-0E49B7B11300" -Reference "Surface m²"					-Tache "89A16088-0355-4602-B777-0E49B7B11000"
# New-Tache -Id "89A16088-0355-4602-B777-0E49B7B11400" -Reference "Temps par unité"				-Tache "89A16088-0355-4602-B777-0E49B7B11000"
# # Reprise nécessaire
# New-Tache -Id "89A16088-0355-4602-B777-0E49B7B11110" -Reference "Reprise béton"					-Tache "89A16088-0355-4602-B777-0E49B7B11100"
# New-Tache -Id "89A16088-0355-4602-B777-0E49B7B11120" -Reference "Surface m²"					-Tache "89A16088-0355-4602-B777-0E49B7B11100"
# New-Tache -Id "89A16088-0355-4602-B777-0E49B7B11130" -Reference "Temps par unité"				-Tache "89A16088-0355-4602-B777-0E49B7B11100"
# 
# # Action 
# Get-Tache -Reference "Reprise béton" -Display Query
# New-Action -Id "003663BB-287B-484E-9E1A-ECD8A0C70001" -Question "Reprise de la planéité du sol. réagréage du sol"       -Tache ^ -Type @Texte
# New-Action -Id "003663BB-287B-484E-9E1A-ECD8A0C70002" -Question "Fourniture & pose moquette"							-Tache ^ -Type @Texte
# 
# 
# # Execute Tache ######################################################################################################################
# Get-Personne -Reference "mohamed"
# Get-Emplacement -Reference "Part-Dieu"
Get-Personne -Reference "mohamed"
Get-Tache -Reference "Remise en état lot vacants"
New-Execution -Id "50FE19B9-0F82-4ED5-8961-0E49B7B00001" -Tache ^ -Emplacement ^ -DateDebut "11/02/2025 08:00" -Personne ^
# 
# # Présentation des sous tâches -----------------------------------------------------------------------------------------------------
# Get-Execution -Filter SousTaches -Personne ^ -Emplacement ^ -Tache ^ -Select "Execution.Id, Tache.Id as Tache, Tache.Reference"
# 
# # Execute Tache -----------------------------------------------------------------------------------------------------
# Get-Tache -Reference "Ligne sol"
# New-Execution -Id "50FE19B9-0F82-4ED5-8961-0E49B7B00002" -Tache ^ -Emplacement ^ -DateDebut "11/02/2025 08:00" -Personne ^
# 
# # Présentation des sous tâches -----------------------------------------------------------------------------------------------------
# Get-Execution -Filter SousTaches -Personne ^ -Emplacement ^ -Tache ^ -Select "Execution.Id, Tache.Id as Tache, Tache.Reference"
# 
# # Execute Tache -----------------------------------------------------------------------------------------------------
# Get-Tache -Reference "Reprise nécessaire"
# New-Execution -Id "50FE19B9-0F82-4ED5-8961-0E49B7B00003" -Tache ^ -Emplacement ^ -DateDebut "11/02/2025 08:00" -Personne ^
# 
# # Présentation des sous tâches -----------------------------------------------------------------------------------------------------
# Get-Execution -Filter SousTaches -Personne ^ -Emplacement ^ -Tache ^ -Select "Execution.Id, Tache.Id as Tache, Tache.Reference" -Display Query
# 
# 
# # Execute Tache -----------------------------------------------------------------------------------------------------
# Get-Tache -Reference "Reprise béton"
# New-Execution -Id "50FE19B9-0F82-4ED5-8961-0E49B7B00004" -Tache ^ -Emplacement ^ -DateDebut "11/02/2025 08:00" -Personne ^
# 
# # Présentation des actions -----------------------------------------------------------------------------------------------------
# Get-Action -Tache ^
# 
# # Selection d'une Action
# Get-Action -Id "003663BB-287B-484E-9E1A-ECD8A0C70001"
# 
# # Suppression des réponses de l'action
# Delete-Reponse -Action ^ -Execution ^
# 
# # Réponse à Combien de m²
# New-Reponse -Action ^ -Execution ^ -Libelle "Ok béton"
# 
# # Selection d'une autre Action
# Get-Action -Id "003663bb-287b-484e-9e1a-ecd8a0c70002"
# 
# # Réponse à Quelle couleur
# New-Reponse -Action ^ -Execution ^ -Libelle "Ok moquette"
# 
# # Affichage des réponses
# Get-Reponse -Select "Action.Question, Libelle, Execution.DateDebut, Execution.Avancement"

# Produit -----------------------------------------------------------------------------------------------------
New-Produit -Reference "Savon main" -Type @Consommable
New-Produit -Reference "Rouleau essuie mains" -Type @Consommable
New-Produit -Reference "Rouleau papier WC" -Type @Consommable
New-Produit -Reference "Bombe désodorisante" -Type @Consommable
New-Produit -Reference "Sac poubelle" -Type @Consommable

New-Produit -Reference "Chariot" -Type @Materiel
New-Produit -Reference "Balai" -Type @Materiel
New-Produit -Reference "Balayette" -Type @Materiel
New-Produit -Reference "Aspirateur" -Type @Materiel
New-Produit -Reference "Serpillère et sceau" -Type @Materiel
New-Produit -Reference "Plumeau" -Type @Materiel
New-Produit -Reference "Torchon" -Type @Materiel

New-Produit -Reference "Produit sol" -Type @ProduitNettoyage
New-Produit -Reference "Produit vitres" -Type @ProduitNettoyage
New-Produit -Reference "Produit WC" -Type @ProduitNettoyage
New-Produit -Reference "Produit détartrage" -Type @ProduitNettoyage

# Stock -----------------------------------------------------------------------------------------------------------
Get-Execution -Id "50FE19B9-0F82-4ED5-8961-0E49B7B00001"

Get-Emplacement -Id "310F51DB-BD8A-44D4-B672-B68B6EE9FE6A"
New-Stock -Produit "Savon Main" -Quantite 12 -Emplacement ^ -Execution ^			
New-Stock -Produit "Rouleau essuie mains" -Quantite 21 -Emplacement ^ -Execution ^
New-Stock -Produit "Rouleau papier WC" -Quantite 35 -Emplacement ^ -Execution ^
New-Stock -Produit "Bombe désodorisante" -Quantite 1 -Emplacement ^ -Execution ^
New-Stock -Produit "Sac poubelle" -Quantite 1 -Emplacement ^ -Execution ^
New-Stock -Produit "Chariot" -Quantite 1 -Emplacement ^ -Execution ^
New-Stock -Produit "Balai" -Quantite 3 -Emplacement ^ -Execution ^
New-Stock -Produit "Balayette" -Quantite 2 -Emplacement ^ -Execution ^
New-Stock -Produit "Aspirateur" -Quantite 1 -Emplacement ^ -Execution ^
New-Stock -Produit "Serpillère et sceau" -Quantite 3 -Emplacement ^ -Execution ^
New-Stock -Produit "Plumeau" -Quantite 1 -Emplacement ^ -Execution ^
New-Stock -Produit "Torchon" -Quantite 6 -Emplacement ^ -Execution ^
New-Stock -Produit "Produit sol" -Quantite 1 -Emplacement ^ -Execution ^
New-Stock -Produit "Produit vitres" -Quantite 1 -Emplacement ^ -Execution ^
New-Stock -Produit "Produit WC" -Quantite 1 -Emplacement ^ -Execution ^
New-Stock -Produit "Produit détartrage" -Quantite 1 -Emplacement ^ -Execution ^

Get-Stock -Quantite 12 -Liste Produit -Select "Id, Produit.Reference, Quantite"