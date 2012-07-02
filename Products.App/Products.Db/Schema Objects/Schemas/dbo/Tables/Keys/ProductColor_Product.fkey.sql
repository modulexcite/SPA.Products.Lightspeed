ALTER TABLE [dbo].[ProductColor]  WITH CHECK ADD  CONSTRAINT [FK__ProductCo__Produ__339FAB6E] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[ProductColor] CHECK CONSTRAINT [FK__ProductCo__Produ__339FAB6E]
GO