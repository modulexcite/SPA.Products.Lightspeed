CREATE TABLE [dbo].[Color](
 [Id] [uniqueidentifier] NOT NULL,
 [Name] [nvarchar](max) NOT NULL,
 [CreatedOn] [datetime] NOT NULL,
 [UpdatedOn] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
 [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
GO
ALTER TABLE [dbo].[Color] ADD  DEFAULT ('') FOR [Name]