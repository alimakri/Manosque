
Declare @id_Execution nvarchar(50);
select @id_Execution=id  from Execution where Reference='NULL';
Declare @id_Personne nvarchar(50);
select @id_Personne=id  from Personne where Reference='0ad1ccd7-2ae3-49be-94a4-54b84f3de002';