ALTER TABLE [dbo].[ProductColor]  WITH CHECK ADD  CONSTRAINT [FK__ProductCo__Color__3493CFA7] FOREIGN KEY([ColorId])
REFERENCES [dbo].[Color] ([Id])
GO
ALTER TABLE [dbo].[ProductColor] CHECK CONSTRAINT [FK__ProductCo__Color__3493CFA7]
GO