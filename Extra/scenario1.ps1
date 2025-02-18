Set-Option -Service Data

# Enum Remove
Remove-Enum
Remove-Tache
Remove-Emplacement
Remove-Calendrier
Remove-Personne
Remove-Empechement

# Enum --------------------------------------
# Type Personne
New-Enum -Reference "Executant" -Valeur 1 -Commentaire 		 "Type Personne"
New-Enum -Reference "Superviseur" -Valeur 2 -Commentaire 	 "Type Personne"
New-Enum -Reference "Syndic" -Valeur 3 -Commentaire 		 "Type Personne"
New-Enum -Reference "Locataire" -Valeur 4 -Commentaire 		 "Type Personne"
# Type Emplacement											 
New-Enum -Reference "Site" -Valeur 1 -Commentaire 			 "Type Emplacement"
New-Enum -Reference "Toilettes" -Valeur 2 -Commentaire 		 "Type Emplacement"
# Statut Tache												 
New-Enum -Reference "AFaire" -Valeur 1 -Commentaire 		 "Statut Tache"
New-Enum -Reference "Fait" -Valeur 2 -Commentaire 			 "Statut Tache"
New-Enum -Reference "Empechement" -Valeur 3 -Commentaire 	 "Statut Tache"
# Type Tache												 
New-Enum -Reference "Nettoyage" -Valeur 1 -Commentaire 		 "Type Tache"
New-Enum -Reference "Electricite" -Valeur 2 -Commentaire 	 "Type Tache"
New-Enum -Reference "Plomberie" -Valeur 3 -Commentaire 		 "Type Tache"
New-Enum -Reference "Vitre" -Valeur 4 -Commentaire 			 "Type Tache"
New-Enum -Reference "VitreHauteur" -Valeur 5 -Commentaire 	 "Type Tache"
# Type Empechement											 
New-Enum -Reference "PasAcces" -Valeur 1 -Commentaire 		 "Type Empechement"
New-Enum -Reference "PasMateriel" -Valeur 2 -Commentaire 	"Type Empechement"
New-Enum -Reference "PasConsom" -Valeur 3 -Commentaire 		 "Type Empechement"

# Personne
New-Personne -Nom "Aber" -Prenom "Axel" -Email "aaber@gmail.com" -Telephone "" -Reference "axaber" -Type @Executant

# Calendrier
New-Calendrier -Reference "axaberCal1" -Personne "axaber"

# Emplacement
New-Emplacement -Reference "Batiment B1" -Type @Site
New-Emplacement -Reference "R+2" -Emplacement "Batiment B1"

# Tache
New-Tache -Reference "Nettoyage accueil" -Emplacement "R+2" -Statut "EnCours" -DateDebut "27/01/2025 08:00" -DateFin "27/01/2025 10:00" -Personne "axaber" -Type @Nettoyage
New-Tache -Reference "Nettoyage Toilettes" -Tache "Nettoyage accueil" -Emplacement "R+2" -Statut "EnCours" -DateDebut "27/01/2025 09:00" -DateFin "27/01/2025 10:00" -Personne "axaber" -Type @Nettoyage

# Empechement
New-Empechement -Reference "Pas accès au local de ménage" -Type @PasAcces 
