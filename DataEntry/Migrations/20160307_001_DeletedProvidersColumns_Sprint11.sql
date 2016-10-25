
ALTER TABLE ServiceProviderEdits ADD Action nvarchar(10);
ALTER TABLE ServiceProviderEdits ADD DeletedProviderName nvarchar(100);
ALTER TABLE ServiceProviderEdits ALTER COLUMN ProviderId int NULL; 
Update ServiceProviderEdits Set Action = 'Edited';