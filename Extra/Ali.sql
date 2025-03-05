
Declare @id_Execution nvarchar(50);select @id_Execution=NULL;
Declare @id_Personne nvarchar(50);select @id_Personne='0ad1ccd7-2ae3-49be-94a4-54b84f3de002';

select DateDebut, x.Execution, x.Reference, x.Id, x.Emplacement, e.Reference, x.Statut, t.Reference tache from Execution x 
	inner join Emplacement e on x.Emplacement=e.Id
	left join Tache t on x.tache=t.Id
	where	(
				(@id_Execution is null and x.Execution is NULL) or 
				(x.Execution = @id_Execution)) and 
			CAST(DateDebut as Date) = CONVERT(Date, CONVERT(DateTime, '17/03/2025 00:00:00', 103), 103) and 
			Personne='0ad1ccd7-2ae3-49be-94a4-54b84f3de002'

select @id_Execution, @id_Personne