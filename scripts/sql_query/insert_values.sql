USE PZ_DB;

-- Account
SET IDENTITY_INSERT [dbo].[Account] ON
INSERT INTO [dbo].[Account] ([ID], [Username], [Password]) VALUES (1, N'pudzian028', N'AQAAAAEAACcQAAAAEDjkXNQrudtioHccMbA5NwT4rBS9hzUb/EEFaTMXslGm4vUUQWfU06kroUH9UHfyMQ==')
INSERT INTO [dbo].[Account] ([ID], [Username], [Password]) VALUES (2, N'karmelek17', N'AQAAAAEAACcQAAAAEHFMN1Lwea+APi4wHk2Tu4yGy7WSk4Gyy6uAdmS/2KwOZkVnZ74D+geZpGDg8QQvmg==')
SET IDENTITY_INSERT [dbo].[Account] OFF

-- Conversation
SET IDENTITY_INSERT [dbo].[Conversation] ON
INSERT INTO [dbo].[Conversation] ([ID], [Name]) VALUES (1, N'...')
SET IDENTITY_INSERT [dbo].[Conversation] OFF

-- Account_Conversation
INSERT INTO [dbo].[Account_Conversation] ([Member_ID], [Conversation_ID]) VALUES (1, 1)
INSERT INTO [dbo].[Account_Conversation] ([Member_ID], [Conversation_ID]) VALUES (2, 1)