update ServiceProvider set ServiceTypes = 1 where ServiceTypes = 3;
update CategoryTypes set ServiceTypeId = 1 where ServiceTypeId = 3
Delete from ServiceTypes where id = 3