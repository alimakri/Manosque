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

# Tache 1 ---------------------------------------------------------------------------------------------------------
New-Tache -Reference "Livraison Stock"

# Execution 1 -----------------------------------------------------------------------------------------------------
Get-Tache -Reference "Livraison Stock"
Get-Emplacement -Reference "Berte Morisot B1"
Get-Personne -Reference "yamakri"
New-Execution -Id "2970DC78-3FC2-4FDE-AD0C-884FECAB3999" -Personne ^ -Emplacement ^ -Tache ^ -DateDebut "29/01/2025 10:00" -DateFin "29/01/2025 11:00" -Type @EntretienNettoyage

# Stock -----------------------------------------------------------------------------------------------------------
Get-Execution Id "Get-Execution "2970DC78-3FC2-4FDE-AD0C-884FECAB3999""
Get-Emplacement -Reference "Berte Morisot B1"
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

Get-Emplacement -Reference "Eklaa"
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

Get-Emplacement -Reference "Progrès Bellecour"
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

Get-Emplacement -Reference "Berte Morisot B2"
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
New-Stock -Produit "Torchon" -Quantite 8 -Emplacement ^ -Execution ^
New-Stock -Produit "Produit sol" -Quantite 1 -Emplacement ^ -Execution ^
New-Stock -Produit "Produit vitres" -Quantite 1 -Emplacement ^ -Execution ^
New-Stock -Produit "Produit WC" -Quantite 1 -Emplacement ^ -Execution ^
New-Stock -Produit "Produit détartrage" -Quantite 1 -Emplacement ^ -Execution ^

# Tache 1 ---------------------------------------------------------------------------------------------------------
New-Tache -Id "DDD5E62E-6FC4-44FF-AC5D-BB19B1355795" -Reference "Fourbissage des éléments" -Ordre 1
New-Tache -Reference "Fourbissage du miroir" -Tache "Fourbissage des éléments" -Ordre 1
New-Tache -Reference "Fourbissage robinetterie" -Tache "Fourbissage des éléments" -Ordre 2
New-Tache -Reference "Balayage sol"  -Ordre 2

# Execution 2 -----------------------------------------------------------------------------------------------------
New-Execution -Id "50FE19B9-0F82-4ED5-8961-C3CBC3642910" -Tache "Fourbissage des éléments" -Emplacement "Berte Morisot B1/RDC/WC/WC Homme" -DateDebut "29/01/2025 08:00" -DateFin "29/01/2025 10:00" -Personne "samakri" -Type @EntretienNettoyage
New-Execution -Id "74FE3AF6-65FB-49DD-B1BF-C2C865799C11" -Execution "50FE19B9-0F82-4ED5-8961-C3CBC3642910" -Tache "Fourbissage du miroir" -Emplacement "Berte Morisot B1/RDC/WC/WC Homme" -DateDebut "29/01/2025 08:00" -DateFin "29/01/2025 10:00" -Personne "samakri" -Type @EntretienNettoyage -Statut @AFaire -Avancement 60
New-Execution -Id "D0061F4E-6205-4865-9ACF-87A8C765D712" -Execution "50FE19B9-0F82-4ED5-8961-C3CBC3642910" -Tache "Fourbissage robinetterie" -Emplacement "Berte Morisot B1/RDC/WC/WC Homme" -DateDebut "29/01/2025 08:00" -DateFin "29/01/2025 10:00" -Personne "samakri" -Type @EntretienNettoyage -Statut @AFaire -Avancement 30
New-Execution -Id "0A41BCD1-5F6D-423B-A571-87C27294CE20" -Tache "Balayage sol" -Emplacement "Berte Morisot B1/RDC/WC/WC Homme" -DateDebut "29/01/2025 08:00" -DateFin "29/01/2025 10:00" -Personne "samakri" -Type @TravauxAcrobat 

Get-Emplacement -Reference "Berte Morisot B1/RDC/WC/WC Homme"
Get-Personne -Reference "samakri"
Get-Execution -Tache "Fourbissage robinetterie" -DateDebut "29/01/2025 08:00" -Personne ^ -Emplacement ^

Update-Execution -Statut @AFaire -Avancement 40 

Get-Execution -Tache "Fourbissage du miroir" -DateDebut "29/01/2025 08:00" -Personne ^ -Emplacement ^
Update-Execution -Statut @AFaire -Avancement 60 

Get-Execution -Id "50FE19B9-0F82-4ED5-8961-C3CBC3642910" -Compute "Statut"
Get-Tache -Id "DDD5E62E-6FC4-44FF-AC5D-BB19B1355795"

Get-Execution -Tache "DDD5E62E-6FC4-44FF-AC5D-BB19B1355795"
Get-Execution -Emplacement "89A16088-0355-4602-B777-0E49B7BF5F7F"

# Tache 2 ---------------------------------------------------------------------------------------------------------
Update-Tache -Id "DDD5E62E-6FC4-44FF-AC5D-BB19B1355795" -Frequence "0 9 * * 1-5"

# Execution 3 : Calcul Statut et Avancement -----------------------------------------------------------------------
Get-Execution -Id "74FE3AF6-65FB-49DD-B1BF-C2C865799C11"
Update-Execution -Statut @Empechement -Avancement 80 

Get-Execution -Id "D0061F4E-6205-4865-9ACF-87A8C765D712"
Update-Execution -Statut @AFaire -Avancement 45

Get-Execution -Id "50FE19B9-0F82-4ED5-8961-C3CBC3642910" -Compute "Statut"

# La tache "Fourbissage du miroir" nécessite l'utilisation de 2 produits ------------------------------------------
Get-Tache -Reference "Fourbissage du miroir"
Add-Produit -Reference "Savon main" -Tache ^
Add-Produit -Reference "Rouleau essuie mains" -Tache ^

# Liste de ces deux produits --------------------------------------------------------------------------------------
Get-Tache -Reference "Fourbissage du miroir" -Liste Produit

# Suite à l'utilisation de ces deux produits màj du stock ---------------------------------------------------------
Get-Execution -Id "74FE3AF6-65FB-49DD-B1BF-C2C865799C11"

New-Stock -Produit "Savon main" -Quantite -2 -Emplacement "Berte Morisot B1" -Execution ^
New-Stock -Produit "Rouleau essuie mains" -Quantite -3 -Emplacement "Berte Morisot B1" -Execution ^

# Generate-Execution ----------------------------------------------------------------------------------------------
New-Absence -Jour "12/02/2025" -Motif "Jour férié"
New-Absence -Jour "20/02/2025" -Motif "Maladie"

Get-Tache -Reference "Fourbissage des éléments"
Get-Emplacement -Reference "Berte Morisot B1/RDC/WC/WC Homme"
Get-Personne -Id "0AD1CCD7-2AE3-49BE-94A4-54B84F3DE62F"
Generate-Execution -Frequence "0 9 * * 1-5" -Type @Nettoyage -DateDebut "10/02/2025" -DateFin "01/03/2025" -Personne ^ -Tache ^ -Emplacement ^

Get-Personne -Reference "yamakri"
Generate-Execution -Frequence "0 9 * * 1-5" -Type @VitreHauteur -DateDebut "10/02/2025" -Personne ^ -Tache ^ -Emplacement ^

# Message --------------------------------------------------------------------------------------------------------
New-Message -Expediteur "samakri" -Destinataire "samakri" -Objet "Objet" -Message "Message"

# Question - Reponse
New-Question -Id "003663BB-287B-484E-9E1A-ECD8A0C7B001" -Libelle "Combien de m² ?" -Tache "Fourbissage du miroir"
New-Question -Id "D1C8E1A2-586D-4436-80FC-9C25A9F47002" -Libelle "Quelle couleur ?" -Tache "Fourbissage du miroir"
New-Reponse -Execution "74FE3AF6-65FB-49DD-B1BF-C2C865799C11" -Libelle "120 m²" -Type @Texte -Question "003663BB-287B-484E-9E1A-ECD8A0C7B001"
New-Reponse -Execution "74FE3AF6-65FB-49DD-B1BF-C2C865799C11" -Libelle "Vert" -Type @Texte -Question "D1C8E1A2-586D-4436-80FC-9C25A9F47002"
New-Reponse -Execution "74FE3AF6-65FB-49DD-B1BF-C2C865799C11" -Libelle "Panne de l''ascenseur" -Type @Texte 

Get-Reponse -Execution "74FE3AF6-65FB-49DD-B1BF-C2C865799C11" -Select "Question.Libelle as question, Id as ReponseId, Libelle as reponse" 

Get-Personne -Select "Nom, Prenom"