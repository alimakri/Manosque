
                                Declare @id_Execution nvarchar(50);select @id_Execution='50fe19b9-0f82-4ed5-8961-0e49b7b00001';
                                Declare @id_Personne nvarchar(50);select @id_Personne='0ad1ccd7-2ae3-49be-94a4-54b84f3de002';
                                select x.Reference, x.Id, x.Emplacement, e.Reference, x.Statut, t.Reference tache, t.id tacheId from Execution x 
                                    inner join Emplacement e on x.Emplacement=e.Id
                                    left join Tache t on x.tache=t.Id
                                    where ((@id_Execution is null and x.Execution is NULL) or (x.Execution = @id_Execution)) and CAST(DateDebut as Date) = CONVERT(Date, '17/02/2025', 103) and Personne=@id_Personne