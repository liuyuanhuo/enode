NoES �¼�ģ��
���ݿ����Version=0������: NoESһ������
���ݿ����Version>0������: �¼�����, ��Ϊversion������
���ݿ��޸�һ���Ѵ�������,��Version +=1: �¼�����, ��Ϊversion������
���ݿ��޸�һ���Ѵ�������,��Version +=2: �¼�����, ��Ϊversion������
���ݼ�¼���ص��ڴ��л����,�޸����ݿ��¼,��Version +=1: �¼�����, ��ΪVersion���ڴ滺�治һ�£��¼��������������ݿ�ʱ����id+versionƥ�䣬�����޼�¼�쳣

�ۺϱ�����δʵ��


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
		   '���ݿ��ֶ����4',	
		   '����--���ݿ��ֶ����4',	
		   '5479375dd9d5d21380fdb9c1',	
		   '5479376fd9d5d21380fdb9c3',	
		   '2014-11-23 21:00:08.130',	
		   '2014-11-24 21:59:24.390',	
		   4)
GO
