SELECT * FROM (
    SELECT *, 'platform' as team FROM "platform_team.csv"
    UNION 
    SELECT *, 'backend' as team FROM "backend_team.csv"
) members WHERE members.last_name != 'oh' 