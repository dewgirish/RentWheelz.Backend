USE [RentWheelsDb]
GO
/****** Object:  Table [dbo].[Cars]    Script Date: 3/18/2024 1:21:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cars](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CarID] [varchar](255) NOT NULL,
	[CarModel] [varchar](255) NOT NULL,
	[RegistrationNumber] [varchar](255) NOT NULL,
	[CarAvailability] [varchar](255) NOT NULL,
	[Brand] [varchar](255) NOT NULL,
	[PricePerHour] [int] NOT NULL,
	[Thumbnail] [varchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reservations]    Script Date: 3/18/2024 1:21:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reservations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BookingId] [varchar](500) NOT NULL,
	[UserEmail] [varchar](255) NOT NULL,
	[CarId] [varchar](500) NOT NULL,
	[ReservationDate] [date] NOT NULL,
	[PickupDate] [date] NOT NULL,
	[ReturnDate] [date] NOT NULL,
	[NumOfTravellers] [int] NOT NULL,
	[Status] [varchar](500) NOT NULL,
	[Car] [varchar](500) NOT NULL,
	[Img] [varchar](max) NOT NULL,
	[Total] [float] NOT NULL,
 CONSTRAINT [PK_Reservations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 3/18/2024 1:21:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Value] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 3/18/2024 1:21:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 3/18/2024 1:21:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](255) NOT NULL,
	[UserEmail] [varchar](255) NOT NULL,
	[UserPassword] [varchar](255) NOT NULL,
	[ProofId] [varchar](255) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Cars] ON 
GO
INSERT [dbo].[Cars] ([Id], [CarID], [CarModel], [RegistrationNumber], [CarAvailability], [Brand], [PricePerHour], [Thumbnail]) VALUES (1, N'5f899fe3-561b-4f24-9933-b87c8f03612e', N'Audi A4', N'ABC123', N'1', N'Audi', 100, N'/white-convertible-car-isolated-white-vector_53876-66815.jpg')
GO
INSERT [dbo].[Cars] ([Id], [CarID], [CarModel], [RegistrationNumber], [CarAvailability], [Brand], [PricePerHour], [Thumbnail]) VALUES (2, N'c2c3c7', N'M100', N'12ABC', N'YES', N'BMW', 110, N'/Car.png')
GO
SET IDENTITY_INSERT [dbo].[Cars] OFF
GO
SET IDENTITY_INSERT [dbo].[Reservations] ON 
GO
INSERT [dbo].[Reservations] ([Id], [BookingId], [UserEmail], [CarId], [ReservationDate], [PickupDate], [ReturnDate], [NumOfTravellers], [Status], [Car], [Img], [Total]) VALUES (3, N'a450b6', N'Admin@Admin.com', N'5f899fe3-561b-4f24-9933-b87c8f03612e', CAST(N'2024-03-15' AS Date), CAST(N'2024-03-17' AS Date), CAST(N'2024-03-18' AS Date), 1, N'CANCELLED', N'Audi Audi A4', N'/white-convertible-car-isolated-white-vector_53876-66815.jpg', 100)
GO
INSERT [dbo].[Reservations] ([Id], [BookingId], [UserEmail], [CarId], [ReservationDate], [PickupDate], [ReturnDate], [NumOfTravellers], [Status], [Car], [Img], [Total]) VALUES (4, N'2b3a44', N'Admin@Admin.com', N'5f899fe3-561b-4f24-9933-b87c8f03612e', CAST(N'2024-03-15' AS Date), CAST(N'2024-03-19' AS Date), CAST(N'2024-03-20' AS Date), 1, N'CONFIRMED', N'Audi Audi A4', N'/white-convertible-car-isolated-white-vector_53876-66815.jpg', 200)
GO
INSERT [dbo].[Reservations] ([Id], [BookingId], [UserEmail], [CarId], [ReservationDate], [PickupDate], [ReturnDate], [NumOfTravellers], [Status], [Car], [Img], [Total]) VALUES (8, N'b87ccc', N'Admin@Admin.com', N'5f899fe3-561b-4f24-9933-b87c8f03612e', CAST(N'2024-03-15' AS Date), CAST(N'2024-03-21' AS Date), CAST(N'2024-03-22' AS Date), 1, N'CONFIRMED', N'Audi Audi A4', N'/white-convertible-car-isolated-white-vector_53876-66815.jpg', 100)
GO
INSERT [dbo].[Reservations] ([Id], [BookingId], [UserEmail], [CarId], [ReservationDate], [PickupDate], [ReturnDate], [NumOfTravellers], [Status], [Car], [Img], [Total]) VALUES (9, N'8fa65e', N'Admin@Admin.com', N'5f899fe3-561b-4f24-9933-b87c8f03612e', CAST(N'2024-03-15' AS Date), CAST(N'2024-03-24' AS Date), CAST(N'2024-03-27' AS Date), 1, N'CONFIRMED', N'Audi Audi A4', N'/white-convertible-car-isolated-white-vector_53876-66815.jpg', 300)
GO
INSERT [dbo].[Reservations] ([Id], [BookingId], [UserEmail], [CarId], [ReservationDate], [PickupDate], [ReturnDate], [NumOfTravellers], [Status], [Car], [Img], [Total]) VALUES (10, N'eda24a', N'Admin@Admin.com', N'5f899fe3-561b-4f24-9933-b87c8f03612e', CAST(N'2024-03-15' AS Date), CAST(N'2024-03-16' AS Date), CAST(N'2024-03-17' AS Date), 1, N'CONFIRMED', N'Audi Audi A4', N'/white-convertible-car-isolated-white-vector_53876-66815.jpg', 100)
GO
INSERT [dbo].[Reservations] ([Id], [BookingId], [UserEmail], [CarId], [ReservationDate], [PickupDate], [ReturnDate], [NumOfTravellers], [Status], [Car], [Img], [Total]) VALUES (11, N'ff3e73', N'Admin@Admin.com', N'c2c3c7', CAST(N'2024-03-15' AS Date), CAST(N'2024-03-16' AS Date), CAST(N'2024-03-17' AS Date), 1, N'CONFIRMED', N'BMW M100', N'/Car.png', 110)
GO
SET IDENTITY_INSERT [dbo].[Reservations] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 
GO
INSERT [dbo].[Roles] ([Id], [Value]) VALUES (1, N'Admin')
GO
INSERT [dbo].[Roles] ([Id], [Value]) VALUES (2, N'User')
GO
INSERT [dbo].[Roles] ([Id], [Value]) VALUES (3, N'Guest')
GO
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[UserRoles] ON 
GO
INSERT [dbo].[UserRoles] ([Id], [UserId], [RoleId]) VALUES (1, 1, 1)
GO
INSERT [dbo].[UserRoles] ([Id], [UserId], [RoleId]) VALUES (2, 2, 2)
GO
INSERT [dbo].[UserRoles] ([Id], [UserId], [RoleId]) VALUES (3, 3, 3)
GO
SET IDENTITY_INSERT [dbo].[UserRoles] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([Id], [UserName], [UserEmail], [UserPassword], [ProofId]) VALUES (1, N'Admin', N'Admin@Admin.com', N'Admin', N'Admin')
GO
INSERT [dbo].[Users] ([Id], [UserName], [UserEmail], [UserPassword], [ProofId]) VALUES (2, N'User', N'User@User.com', N'User', N'User')
GO
INSERT [dbo].[Users] ([Id], [UserName], [UserEmail], [UserPassword], [ProofId]) VALUES (3, N'Guest', N'Guest@guest.com', N'Guest', N'Guest')
GO
INSERT [dbo].[Users] ([Id], [UserName], [UserEmail], [UserPassword], [ProofId]) VALUES (1002, N'string', N'me@me.com', N'string', N'string')
GO
INSERT [dbo].[Users] ([Id], [UserName], [UserEmail], [UserPassword], [ProofId]) VALUES (1003, N'string', N'user@example.com', N'string', N'string')
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
