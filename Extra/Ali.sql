--select * from Execution where CAST(DateDebut as DATE)=CONVERT(Date, '17/02/2025', 103) and Personne='0AD1CCD7-2AE3-49BE-94A4-54B84F3DE002' and Emplacement='310F51DB-BD8A-44D4-B672-B68B6EE9F001'

select t.* from Execution x 
inner join Tache t on x.Tache = t.Id
where x.Id='50FE19B9-0F82-4ED5-8961-0E49B7B0013A'

select * from Execution order by Tache

select * from Action where Tache='89A16088-0355-4602-B777-0E49B7B0013A'
