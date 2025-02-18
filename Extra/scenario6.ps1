Set-Option -DisplayMode Normal
Set-Option -Service Data

# Nettoyage de la BD
Delete-All

# Enum Type Personne ---------------------------------------------------------------------------------------------
New-Enum -Reference "Executant" -Valeur 1 -Commentaire "Type Personne" -Groupe 1
New-Enum -Reference "Superviseur" -Valeur 2 -Commentaire "Type Personne" -Groupe 1
New-Enum -Reference "Syndic" -Valeur 3 -Commentaire "Type Personne" -Groupe 1
New-Enum -Reference "Locataire" -Valeur 4 -Commentaire "Type Personne" -Groupe 1
# Enum Type Emplacement
New-Enum -Reference "Site" -Valeur 1 -Commentaire "Type Emplacement" -Groupe 6
New-Enum -Reference "Toilettes" -Valeur 2 -Commentaire "Type Emplacement" -Groupe 6
# Enum Statut Tache
New-Enum -Reference "AFaire" -Valeur 1 -Commentaire "Statut Tache" -Groupe 14
New-Enum -Reference "Fait" -Valeur 2 -Commentaire "Statut Tache" -Groupe 14
New-Enum -Reference "Empechement" -Valeur -1000 -Commentaire "Statut Tache" -Groupe 14
# Enum Type Tache
New-Enum -Reference "Nettoyage" -Valeur 1 -Commentaire "Type Tache" -Groupe 4
New-Enum -Reference "Electricite" -Valeur 2 -Commentaire "Type Tache" -Groupe 4
New-Enum -Reference "Plomberie" -Valeur 3 -Commentaire "Type Tache" -Groupe 4
New-Enum -Reference "Vitre" -Valeur 4 -Commentaire "Type Tache" -Groupe 4
New-Enum -Reference "VitreHauteur" -Valeur 5 -Commentaire "Type Tache" -Groupe 4
# Enum Type Empechement
New-Enum -Reference "PasAcces" -Valeur 1 -Commentaire "Type Empechement" -Groupe 5
# Enum Type Produit
New-Enum -Reference "Consommable" -Valeur 1 -Commentaire "Type Produit" -Groupe 2
New-Enum -Reference "Materiel" -Valeur 2 -Commentaire "Type Produit" -Groupe 2
New-Enum -Reference "ProduitNettoyage" -Valeur 3 -Commentaire "Type Produit" -Groupe 2
# Enum Divers
New-Enum -Reference "AjoutStock" -Valeur 1 -Commentaire "Type Stock" -Groupe 3
# Enum Type Retour
New-Enum -Reference "Bool" -Valeur 1 -Commentaire "Type Produit" -Groupe 7
New-Enum -Reference "Texte" -Valeur 2 -Commentaire "Type Produit" -Groupe 7
New-Enum -Reference "Numerique" -Valeur 2 -Commentaire "Type Produit" -Groupe 7
New-Enum -Reference "Photo" -Valeur 3 -Commentaire "Type Produit" -Groupe 7

# Personne -----------------------------------------------------------------------------------------------------
New-Personne -Id "0AD1CCD7-2AE3-49BE-94A4-54B84F3DE62F" -Nom "Makri" -Prenom "Sarah" -Email "sarah@makri.fr" -Telephone "" -Reference "samakri" -Type @Executant -Localisation "Saint-Priest"
New-Personne -Id "3C990AA6-B9BF-43ED-9877-DB7158B52C10" -Nom "Makri" -Prenom "Fatiha" -Email "Fatiha@makri.fr" -Telephone "" -Reference "famakri" -Type @Executant -Localisation "Genas"
New-Personne -Nom "Makri" -Prenom "Nadir" -Email "Nadir@makri.fr" -Telephone "" -Reference "namakri" -Type @Executant -Localisation "Villeurbanne"
New-Personne -Nom "Makri" -Prenom "Yamin" -Email "Yamin@makri.fr" -Telephone "" -Reference "yamakri" -Type @Executant -Localisation "Bron"
New-Personne -Nom "Makri" -Prenom "Asma" -Email "Asma@makri.fr" -Telephone "" -Reference "asmakri" -Type @Executant -Localisation "Vénissieux"

# Compétences --------------------------------------------------------------------------------------------------
New-Competence -Reference "Entretien et nettoyage"
New-Competence -Reference "Habilité auto-laveuse"
New-Competence -Reference "Travaux acrobatiques"
New-Competence -Reference "Permis B"

# samakri
Get-Personne -Reference "samakri"
Add-Competence -Reference "Entretien et nettoyage" -Personne ^

Add-Competence -Reference "Habilité auto-laveuse" -Personne ^
Add-Competence -Reference "Travaux acrobatiques" -Personne ^
# famakri
Get-Personne -Reference "famakri"
Add-Competence -Reference "Entretien et nettoyage" -Personne ^
Add-Competence -Reference "Habilité auto-laveuse" -Personne ^
Add-Competence -Reference "Permis B" -Personne ^
# namakri
Get-Personne -Reference "namakri"
Add-Competence -Reference "Entretien et nettoyage" -Personne ^
Add-Competence -Reference "Habilité auto-laveuse" -Personne ^
Add-Competence -Reference "Travaux acrobatiques" -Personne ^
# yamakri
Get-Personne -Reference "yamakri"
Add-Competence -Reference "Entretien et nettoyage" -Personne ^

Get-Competence -Reference "Permis B"
Add-Competence -Reference "Permis B" -Personne ^
# asmakri
Get-Personne -Reference "asmakri"
Add-Competence -Reference "Entretien et nettoyage" -Personne ^
Add-Competence -Reference "Habilité auto-laveuse" -Personne ^
Add-Competence -Reference "Travaux acrobatiques" -Personne ^

# Emplacement 1 -----------------------------------------------------------------------------------------------------
New-Emplacement -Reference "Berte Morisot B1" -Type @Site
New-Emplacement -Reference "Berte Morisot B2" -Type @Site
New-Emplacement -Reference "Berte Morisot A3" -Type @Site
New-Emplacement -Reference "Berte Morisot A1" -Type @Site
New-Emplacement -Reference "Eklaa" -Type @Site
New-Emplacement -Reference "Le Vilette" -Type @Site
New-Emplacement -Reference "Welink" -Type @Site
New-Emplacement -Reference "Angle Sud" -Type @Site
New-Emplacement -Reference "Grand angle" -Type @Site
New-Emplacement -Reference "Progrès Bellecour" -Type @Site
New-Emplacement -Reference "Sun 7 A" -Type @Site
New-Emplacement -Reference "Sun 7 B" -Type @Site
New-Emplacement -Reference "Sun 7 C" -Type @Site

New-Emplacement -Reference "Berte Morisot B1/RDC" -Emplacement "Berte Morisot B1"
New-Emplacement -Reference "Berte Morisot B1/RDC/WC" -Emplacement "Berte Morisot B1/RDC"
New-Emplacement -Id "89A16088-0355-4602-B777-0E49B7BF5F7F" -Reference "Berte Morisot B1/RDC/WC/WC Homme" -Emplacement "Berte Morisot B1/RDC/WC"
New-Emplacement -Reference "Berte Morisot B1/RDC/WC/WC Femme" -Emplacement "Berte Morisot B1/RDC/WC"

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

# Tache 2 ---------------------------------------------------------------------------------------------------------
New-Tache -Id "89A16088-0355-4602-B777-0E49B7B10000" -Reference "Berte Morisot B1/RDC" 
New-Tache -Id "89A16088-0355-4602-B777-0E49B7B11000" -Reference "Berte Morisot B1/RDC/WC" -Tache "Berte Morisot B1/RDC"
New-Tache -Id "89A16088-0355-4602-B777-0E49B7B11100" -Reference "Berte Morisot B1/RDC/WC/WC Homme" -Tache "Berte Morisot B1/RDC/WC"
New-Tache -Id "89A16088-0355-4602-B777-0E49B7B11200" -Reference "Berte Morisot B1/RDC/WC/WC Femme" -Tache "Berte Morisot B1/RDC/WC"

New-Tache -Id "89A16088-0355-4602-B777-0E49B7B11110" -Reference "Fourbissage des éléments"  -Type @EntretienNettoyage -Ordre 1
New-Tache -Id "89A16088-0355-4602-B777-0E49B7B11111" -Reference "Fourbissage du miroir" -Tache "Fourbissage des éléments" -Ordre 1
New-Tache -Id "89A16088-0355-4602-B777-0E49B7B11112" -Reference "Fourbissage robinetterie" -Tache "Fourbissage des éléments" -Ordre 2
New-Tache -Id "89A16088-0355-4602-B777-0E49B7B11120" -Reference "Balayage sol"  -Ordre 2

# Execution 2 -----------------------------------------------------------------------------------------------------
New-Execution -Id "50FE19B9-0F82-4ED5-8961-0E49B7B11110" -Tache "Fourbissage des éléments" -Emplacement "Berte Morisot B1" -DateDebut "29/01/2025 08:00" -Personne "samakri"
New-Execution -Id "50FE19B9-0F82-4ED5-8961-0E49B7B11111" -Tache "Fourbissage du miroir"    -Emplacement "Berte Morisot B1" -DateDebut "29/01/2025 09:00" -Personne "samakri" -Type @EntretienNettoyage -Statut @AFaire -Avancement 60 -Execution "50FE19B9-0F82-4ED5-8961-0E49B7B11110"
New-Execution -Id "50FE19B9-0F82-4ED5-8961-0E49B7B11112" -Tache "Fourbissage robinetterie" -Emplacement "Berte Morisot B1" -DateDebut "29/01/2025 10:00" -Personne "samakri" -Type @EntretienNettoyage -Statut @AFaire -Avancement 30 -Execution "50FE19B9-0F82-4ED5-8961-0E49B7B11110"
New-Execution -Id "50FE19B9-0F82-4ED5-8961-0E49B7B11120" -Tache "Balayage sol"             -Emplacement "Berte Morisot B1" -DateDebut "29/01/2025 11:00" -Personne "samakri" -Type @TravauxAcrobat 

# Action Question
New-Action -Id "003663BB-287B-484E-9E1A-ECD8A0C70001" -Question "Combien de m² ?"       -Tache "Fourbissage du miroir" -Type @Texte
New-Action -Id "003663BB-287B-484E-9E1A-ECD8A0C70002" -Question "Quelle couleur ?"      -Tache "Fourbissage du miroir" -Type @Texte

# ####################################################################################
# Login ok si Count==1
Get-Personne -Reference "samakri" -Password "P@ssw0rd" -Return "OnlyCount"

# Liste des sites pour exécutions de samakri le 29/01/2025 quelque soit l'heure
Get-Execution -Personne ^ -DateDebut "29/01/2025" -Filter "ListeSites"

# Selection du site Berte Morisot B1
Get-Emplacement -Reference "Berte Morisot B1"

# Liste des tâches de premier niveau
Get-Execution -Filter TopTache -Personne ^ -Emplacement ^ -Select "Tache.Id, Tache.Reference"

# Selection de la tâche Fourbissage des éléments
Get-Tache -Reference "Fourbissage des éléments"

# Liste des exécutions de cette Tache
Get-Execution -Filter SousTaches -Personne ^ -Emplacement ^ -Tache ^ -Select "Execution.Id, Tache.Id as Tache, Tache.Reference"

# Selection de l'exécution Fourbissage du miroir
Get-Execution -Id "50fe19b9-0f82-4ed5-8961-0e49b7b11111"

# Selection d'une tâche
Get-Tache -Id "89a16088-0355-4602-b777-0e49b7b11111"

# Liste des actions de cette Tache
Get-Action -Tache ^

# Selection d'une Action
Get-Action -Id "003663bb-287b-484e-9e1a-ecd8a0c70001"

# Suppression des réponses de l'action
Delete-Reponse -Action ^ -Execution ^

# Réponse à Combien de m²
New-Reponse -Action ^ -Execution ^ -Libelle "150 m²"

# Selection d'une autre Action
Get-Action -Id "003663bb-287b-484e-9e1a-ecd8a0c70002"

# Réponse à Quelle couleur
New-Reponse -Action ^ -Execution ^ -Libelle "Rouge"

# Affichage des réponses
Get-Reponse -Select "Action.Question, Libelle, Execution.DateDebut, Execution.Avancement"