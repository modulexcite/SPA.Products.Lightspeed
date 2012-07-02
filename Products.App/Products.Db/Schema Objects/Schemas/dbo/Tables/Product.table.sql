CREATE TABLE [dbo].[Product](
 [Id] [uniqueidentifier] NOT NULL,
 [Name] [nvarchar](max) NOT NULL,
 [Price] [float] NOT NULL,
 [Quantity] [int] NOT NULL,
 [CreatedOn] [datetime] NOT NULL,
 [UpdatedOn] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
 [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT ('') FOR [Name]
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT ((0)) FOR [Price]
GO
ALTER TABLE [dbo].[Product] ADD  DEFAULT ((0)) FOR [Quantity]
GO