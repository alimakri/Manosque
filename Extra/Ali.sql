Declare @id_Emplacement nvarchar(50);
select @id_Emplacement=id  from Emplacement where Reference='Site1';
Declare @id_Personne nvarchar(50);
select @id_Personne=id  from Personne where Reference='mohamed';
DECLARE @IDs TABLE(ID uniqueidentifier, Reference nvarchar(50));
insert Execution (Id, Reference, Emplacement, DateDebut, Personne) output inserted.Id, inserted.reference into @IDs(ID, Reference) values ('50FE19B9-0F82-4ED5-8961-0E49B7B00001', 'EXE_1', @id_Emplacement, '17/02/2025 08:00', @id_Personne);
select * from Execution p inner join @IDs t on p.id=t.Id