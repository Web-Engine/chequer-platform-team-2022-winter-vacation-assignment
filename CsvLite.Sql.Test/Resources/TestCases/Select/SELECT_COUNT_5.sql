SELECT COUNT(*), members.last_name, members.team FROM (
  SELECT *, 'platform' as team FROM "platform_team.csv"
  UNION
  SELECT *, 'backend' as team FROM "backend_team.csv"
) members GROUP BY members.last_name, members.team