Declare @id_Execution nvarchar(50);
select @id_Execution=id  from TableId where Reference='Execution';
Declare @id_Personne nvarchar(50);
select @id_Personne=id  from Personne where Reference='mohamed';
DECLARE @id uniqueidentifier;
select @id=id from Execution  where DateDebut=CONVERT(Datetime, '17/02/2025', 103) and Execution=@id_Execution and Personne=@id_Personne;
if @@ROWCOUNT=1
BEGIN
Update TableId set id=@id where Reference= 'Execution';
if @@ROWCOUNT = 0 
insert TableId (Reference, id) values('Execution', @id);
END
else
delete TableId where Reference='Execution';

select * from Execution  where DateDebut=CONVERT(Datetime, '17/02/2025', 103) and Execution=@id_Execution and Personne=@id_Personne