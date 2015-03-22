NoES 事件模拟
数据库插入Version=0的数据: NoES一切正常
数据库插入Version>0的数据: 事件重试, 因为version不连续
数据库修改一条已存在数据,并Version +=1: 事件重试, 因为version不连续
数据库修改一条已存在数据,并Version +=2: 事件重试, 因为version不连续
数据记录加载到内存中缓存后,修改数据库记录,并Version +=1: 事件重试, 因为Version和内存缓存不一致，事件处理函数存入数据库时依据id+version匹配，出现无记录异常

聚合保存尚未实现


USE [Forum]
GO

INSERT INTO [dbo].[Post]
           ([Id]
           ,[Subject]
           ,[Body]
           ,[AuthorId]
           ,[SectionId]
           ,[CreatedOn]
           ,[UpdatedOn]
           ,[Version])
     VALUES
           (
		   '2a71da58d9d5d21b1809b8ee',	
		   '数据库手动添加4',	
		   '内容--数据库手动添加4',	
		   '5479375dd9d5d21380fdb9c1',	
		   '5479376fd9d5d21380fdb9c3',	
		   '2014-11-23 21:00:08.130',	
		   '2014-11-24 21:59:24.390',	
		   4)
GO
