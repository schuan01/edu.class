USE [dbEduClass]
GO

/*PROFESORES*/
INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('jarias','??p????p???x4??E''????????X','Juan','Arias','19860910','jarias@gmail.com','31256268','20160508',NULL,1,0)
INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('mariapinto','??p????p???x4??E''????????X','Maria','Pinto','20020205','mariapinto@gmail.com','72047230','20160508',NULL,1,0)
GO

INSERT INTO [dbo].[Groups]([Name], [Description], [CreatedAt], [UpdatedAt], [Enabled], [Key], [Teacher_Id]) VALUES('Liceo 1 Biología 2B', 'Liceo 1 Biología 2B - Programa 5542', GETDATE(), NULL, 1, '6y4esf83', 1);
INSERT INTO [dbo].[Groups]([Name], [Description], [CreatedAt], [UpdatedAt], [Enabled], [Key], [Teacher_Id]) VALUES('Liceo 4 Literatura 4C', 'Liceo 4 Literatura 4C - Programa 5543', GETDATE(), NULL, 1, '6y4esf83', 2);
INSERT INTO [dbo].[Groups]([Name], [Description], [CreatedAt], [UpdatedAt], [Enabled], [Key], [Teacher_Id]) VALUES('Liceo 36 Biología 1A', 'Liceo 36 Biología 1A - Programa 5543', GETDATE(), NULL, 1, '6y4esf83', 1);
GO

INSERT INTO [dbo].[Person_Teacher] ([GroupId],[Id]) VALUES(1, 1)
INSERT INTO [dbo].[Person_Teacher] ([GroupId],[Id]) VALUES(2, 2)
INSERT INTO [dbo].[Person_Teacher] ([GroupId],[Id]) VALUES(3, 1)
GO

INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 1)
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 2)
GO

INSERT INTO [dbo].[Calendars] ([Description],[CreatedAt],[UpdatedAt],[Enabled],[Group_Id])VALUES ('Calendario Biologia 2B', GETDATE(), NULL, 1, 1)
INSERT INTO [dbo].[Calendars] ([Description],[CreatedAt],[UpdatedAt],[Enabled],[Group_Id])VALUES ('Calendario Literatura 4C', GETDATE(), NULL, 1, 2)
INSERT INTO [dbo].[Calendars] ([Description],[CreatedAt],[UpdatedAt],[Enabled],[Group_Id])VALUES ('Calendario Astronomis 1A', GETDATE(), NULL, 1, 3)
GO

USE [dbEduClass]
GO

INSERT INTO [dbo].[Events]([Name],[Description],[EventType],[StartDate],[EndDate],[CreatedAt],[UpdateAt],[CalendarId],[Enabled]) VALUES ('', '', 1, GETDATE(), GETDATE(), GETDATE(), NULL, 1, 1)
GO



/*ALUMNOS*/
INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('carlitosperez','??p????p???x4??E''????????X','Carlos','Pérez','20011219','carlitosperez@hotmail.com','28089662','20160508',NULL,1,0)
GO
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (3)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(1, 3)
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 3)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('enriques','??p????p???x4??E''????????X','Enrique','Sánchez','19870627','enriques@hotmail.com','17858951','20160508',NULL,1,0)
GO
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (4)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(1, 4)
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 4)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('jorherre','??p????p???x4??E''????????X','Jorge','Herrera','20030415','jorherre@gmail.com','67188172','20160508',NULL,1,0)
GO
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (5)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(1, 5)
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 5)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('lrojas','??p????p???x4??E''????????X','Luis','Rojas','19920903','lrojas@outlook.com','54267521','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 6)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (6)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(1, 6)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('roma','??p????p???x4??E''????????X','Raúl','Poma','19890215','roma@gmail.com','49885465','20160508',NULL,1,0)
GO
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (7)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(1, 7)
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 7)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('sofiredondo','??p????p???x4??E''????????X','Sofia','Redondo','20030608','sofiredondo@outlook.com','11353121','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 8)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (8)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(1, 8)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('yisustapia','??p????p???x4??E''????????X','Jesús','Tapia','19850218','yisustapia@gmail.com','69975783','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 9)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (9)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(1, 9)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('alexgil','??p????p???x4??E''????????X','Alexis','Gil','19970304','alexgil@gmail.com','15700499','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 10)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (10)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(1, 10)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('gonzabril','??p????p???x4??E''????????X','Gonzalo','Abril','19880115','gonzabril@hotmail.com','36736320','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 11)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (11)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(1, 11)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('anaacero','??p????p???x4??E''????????X','Ana','Acero','20001013','anaacero@gmail.com','51346320','20160508',NULL,1,0)
GO
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (12)
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 12)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(1, 12)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('luadame','??p????p???x4??E''????????X','Lucía','Adame','19981104','luadame@gmail.com','35716179','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 13)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (13)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(1, 13)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('camiadel','??p????p???x4??E''????????X','Camila','Adel','19950730','camiadel@hotmail.com','92343985','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 14)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (14)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(1, 14)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('rabaeza','??p????p???x4??E''????????X','Ramón','Baeza','19960917','rabaeza@outlook.com','37954470','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 15)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (15)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(1, 15)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('carmenbanda','??p????p???x4??E''????????X','Carmen','Banda','20000930','carmenbanda@hotmail.com','12899093','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 16)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (16)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(1, 16)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('abarbo','??p????p???x4??E''????????X','Alvaro','Barbo','19901016','abarbo@gmail.com','17002809','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 17)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (17)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(1, 17)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('solbarra','??p????p???x4??E''????????X','Solange','Barra','19990114','solbarra@outlook.com','70390015','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 18)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (18)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(1, 18)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('edubazan','??p????p???x4??E''????????X','Eduardo','Bazán','19890608','edubazan@hotmail.com','90768761','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 19)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (19)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(1, 19)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('eugecalvo','??p????p???x4??E''????????X','Eugenia','Calvo','19890806','eugecalvo@gmail.com','64085448','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 20)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (20)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(1, 20)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('emicamus','??p????p???x4??E''????????X','Emilio','Camus','19901104','emicamus@gmail.com','95407508','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 21)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (21)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(1, 21)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('acanal','??p????p???x4??E''????????X','Adán','Canal','19860922','acanal@outlook.com','44363397','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 22)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (22)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(1, 22)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('alecobos','??p????p???x4??E''????????X','Alejandro','Cobos','19870413','alecobos@gmail.com','97339549','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 23)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (23)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(1, 23)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('alejandraolmo','??p????p???x4??E''????????X','Alejandra','Olmo','19860922','alejandraolmo@hotmail.com','49244677','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 24)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (24)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(1, 24)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('arios','??p????p???x4??E''????????X','Álvaro','Ríos','19870413','arios@gmail.com','68041375','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 25)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (25)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(1, 25)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('amandaroca','??p????p???x4??E''????????X','Amanda','Roca','19940826','amandaroca@outlook.com','98094796','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 26)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (26)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(1, 26)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('amparo','??p????p???x4??E''????????X','Amparo','Elío','19940118','amparo@gmail.com','12316984','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 27)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (27)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 27)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('barbiegoya','??p????p???x4??E''????????X','Bárbara','Goya','19870527','barbiegoya@gmail.com','84443561','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 28)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (28)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 28)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('belehera','??p????p???x4??E''????????X','Belén','Hera','19930718','belehera@hotmail.com','67794591','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 29)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (29)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 29)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('elbenjakent','??p????p???x4??E''????????X','Benjamín','Kent','19980629','elbenjakent@gmail.com','38116625','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 30)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (30)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 30)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('bleon','??p????p???x4??E''????????X','Bruno','León','20040321','bleon@outlook.com','72356893','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 31)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (31)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 31)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('rikyara','??p????p???x4??E''????????X','Ricardo','Araciel','19981104','rikyara@gmail.com','53107075','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 32)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (32)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 32)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('juancaarbolea','??p????p???x4??E''????????X','Juan Camilo','Arbolea','19990709','juancaarbolea@hotmail.com','83732694','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 33)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (33)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 33)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('majoazorero','??p????p???x4??E''????????X','María José','Azorero','19901126','majoazorero@gmail.com','77745087','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 34)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (34)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 34)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('cecibarbera','??p????p???x4??E''????????X','Cecilia','Barberá','20010306','cecibarbera@outlook.com','83855309','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 35)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (35)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 35)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('carmenbarreto','??p????p???x4??E''????????X','Carmen','Barreto','20010601','carmenbarreto@outlook.com','72925303','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 36)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (36)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 36)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('carlanegrete','??p????p???x4??E''????????X','Carla','Negrete','19871124','carlanegrete@gmail.com','54816546','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 37)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (37)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 37)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('crisquiroga','??p????p???x4??E''????????X','Cristian','Quiroga','19850412','crisquiroga@outlook.com','79718120','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 38)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (38)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 38)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('claudiosegui','??p????p???x4??E''????????X','Claudio','Seguí','19980524','claudiosegui@gmail.com','32251883','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 39)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (39)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 39)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('diegonoguera','??p????p???x4??E''????????X','Diego','Noguera','20010707','diegonoguera@outlook.com','91228237','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 40)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (40)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 40)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('srosales','??p????p???x4??E''????????X','Santiago','Rosales','19990328','srosales@hotmail.com','49196800','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 41)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (41)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 41)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('dariosalazar','??p????p???x4??E''????????X','Darío','Salazar','19981223','dariosalazar@gmail.com','56486870','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 42)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (42)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 42)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('dobregon','??p????p???x4??E''????????X','Dieana','Obregón','20030916','dobregon@outlook.com','17090405','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 43)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (43)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 43)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('doranavarro','??p????p???x4??E''????????X','Dora','Navarro','19940331','doranavarro@gmail.com','65906293','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 44)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (44)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 44)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('fabilatorre','??p????p???x4??E''????????X','Fabián','Latorre','19920426','fabilatorre@outlook.com','73813464','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 45)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (45)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 45)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('fatiledesma','??p????p???x4??E''????????X','Fátima','Ledesma','19990208','fatiledesma@gmail.com','85139761','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 46)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (46)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 46)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('fedeescobar','??p????p???x4??E''????????X','Federico','Escobar','19871012','fedeescobar@outlook.com','52295035','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 47)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (47)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 47)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('feliencinas','??p????p???x4??E''????????X','Félix','Encinas','19920129','feliencinas@hotmail.com','41192135','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 48)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (48)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 48)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('feliserna','??p????p???x4??E''????????X','Felipe','Serna','19890905','feliserna@outlook.com','34923361','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 49)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (49)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 49)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('fernovoa','??p????p???x4??E''????????X','Fernando','Novoa','19861216','fernovoa@gmail.com','45571228','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 50)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (50)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 50)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('fromay','??p????p???x4??E''????????X','Flavio','Romay','19990723','fromay@hotmail.com','87609833','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 51)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (51)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 51)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('flaviaripol','??p????p???x4??E''????????X','Flavia','Ripol','19870919','flaviaripol@hotmail.com','84255790','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 52)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (52)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 52)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('francosalas','??p????p???x4??E''????????X','Franco','Salas','19850313','francosalas@outlook.com','66937883','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 53)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (53)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(2, 53)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('monirivas','??p????p???x4??E''????????X','Mónica','Rivas','19870408','monirivas@hotmail.com','80494958','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 54)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (54)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(3, 54)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('gustavorosas','??p????p???x4??E''????????X','Gustavo','Rosas','20010306','gustavorosas@gmail.com','48272019','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 55)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (55)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(3, 55)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('gabioliva','??p????p???x4??E''????????X','Grabriela','Oliva','20021206','gabioliva@outlook.com','42892685','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 56)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (56)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(3, 56)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('elguilleochoa','??p????p???x4??E''????????X','Guillermo','Ochoa','19940602','elguilleochoa@gmail.com','68984292','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 57)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (57)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(3, 57)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('griselojeda','??p????p???x4??E''????????X','Grisel','Ojeda','19950424','griselojeda@outlook.com','42824303','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 58)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (58)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(3, 58)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('launieto','??p????p???x4??E''????????X','Laura','Nieto','19980309','launieto@hotmail.com','66578740','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 59)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (59)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(3, 59)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('leandroneira','??p????p???x4??E''????????X','Leandro','Neira','19931113','leandroneira@gmail.com','37412630','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 60)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (60)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(3, 60)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('guilinadal','??p????p???x4??E''????????X','Giuliana','Nadal','19951017','guilinadal@outlook.com','86036940','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 61)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (61)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(3, 61)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('leticialemos','??p????p???x4??E''????????X','Leticia','Lemos','19850616','leticialemos@gmail.com','58965822','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 62)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (62)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(3, 62)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('lulamas','??p????p???x4??E''????????X','Luciano','Lamas','19930107','lulamas@outlook.com','83138248','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 63)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (63)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(3, 63)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('lucashereas','??p????p???x4??E''????????X','Lucas','Hereas','19920922','lucashereas@hotmail.com','14632030','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 64)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (64)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(3, 64)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('lugodoy','??p????p???x4??E''????????X','Lucrecia','Godoy','19910329','lugodoy@gmail.com','46806917','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 65)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (65)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(3, 65)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('luisgijon','??p????p???x4??E''????????X','Luis','Gijón','19850412','luisgijon@hotmail.com','67594694','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 66)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (66)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(3, 66)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('walterrea','??p????p???x4??E''????????X','Walter','Errea','19881120','walterrea@outlook.com','96443618','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 67)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (67)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(3, 67)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('wendydoria','??p????p???x4??E''????????X','Wendy','Doría','20010707','wendydoria@gmail.com','90222486','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 68)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (68)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(3, 68)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('elbraian','??p????p???x4??E''????????X','Braian','Doris','19990802','elbraian@hotmail.com','38778316','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 69)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (69)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(3, 69)

INSERT INTO [dbo].[Person]([UserName],[Password],[FirstName],[LastName],[Birthday],[Email],[IdentificationCard],[CreatedAt],[UpdatedAt],[Enabled],[Silenced]) VALUES ('walbertodonat','??p????p???x4??E''????????X','Waldo','Donat','20040222','walbertodonat@gmail.com','65730084','20160508',NULL,1,0)
GO
INSERT [dbo].[Avatars] ([UrlPhoto], [UpdatedAt], [Enabled], [Person_Id]) VALUES (N'~\Content\images\default.png', CAST(N'2016-05-08 20:17:43.597' AS DateTime), 1, 70)
INSERT INTO [dbo].[Person_Student] ([Id]) VALUES (70)
INSERT INTO [dbo].[GroupStudent] ([Groups_Id] ,[Students_Id]) VALUES(3, 70)