CREATE VIEW [dbo].[RoleView]
	AS SELECT [User],CONCAT([RoleType].Name ,'_',[RoleLevel].Name) as Role FROM [UserRole] LEFT JOIN [RoleType] ON [UserRole].Role = [RoleType].Id LEFT JOIN [RoleLevel] ON [RoleLevel].Level <= [UserRole].Level
