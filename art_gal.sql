CREATE DATABASE ART_GAL
GO

USE ART_GAL
GO

CREATE TABLE [User] (
	UserID					INT				PRIMARY KEY			IDENTITY,
	FirstName				VARCHAR(15),
	LastName				VARCHAR(15),
	DOB						DATE,
	[Address]				VARCHAR(50),
	AccountType				VARCHAR(6),
	Gender					CHAR,
	[Password]				CHAR(128),
	Email					VARCHAR(30)
)

CREATE TABLE Artwork (
	ArtworkID				INT				PRIMARY KEY			IDENTITY,
	Creator					VARCHAR(200),
	[Name]					VARCHAR(300),
	[Description]			VARCHAR(MAX),
	Medium					VARCHAR(MAX),
	Origin					VARCHAR(100),
	[Date]					DATE,
	Price					INT,
	pictureLink				VARCHAR(MAX)
) 
 
CREATE TABLE UserArtworkPurchase (
	PurchaseID				INT				PRIMARY KEY		IDENTITY,
	UserID					INT				FOREIGN KEY REFERENCES [User](UserID)
											ON UPDATE CASCADE ON DELETE CASCADE,
	ArtworkID				INT				FOREIGN KEY REFERENCES Artwork(ArtworkID)
											ON UPDATE CASCADE ON DELETE CASCADE,
	DataSubmitted			DATE
)
 
CREATE TABLE UserArtworkSale (
	SaleID					INT				PRIMARY KEY		IDENTITY,
	UserID					INT				FOREIGN KEY REFERENCES [User](UserID),
	ArtworkID				INT				FOREIGN KEY REFERENCES Artwork(ArtworkID),
	DataSubmitted			DATE
)

CREATE TABLE Catagory (
	CatagoryID				INT				PRIMARY KEY,
	[Name]					VARCHAR(20),
	Popularity				INT
)

CREATE TABLE UserLikes (
	likeID					INT				PRIMARY KEY		IDENTITY,
	userID					INT				FOREIGN KEY REFERENCES [User](UserID)
											ON UPDATE CASCADE ON DELETE CASCADE,
	artID					INT				FOREIGN KEY REFERENCES Artwork(ArtworkID)
											ON UPDATE CASCADE ON DELETE CASCADE,
)

CREATE TABLE ArtworkRating (
	ArtworkID				INT				FOREIGN KEY REFERENCES Artwork(ArtworkID)
											ON UPDATE CASCADE ON DELETE CASCADE,
	UserID					INT				FOREIGN KEY REFERENCES [User](UserID)
											ON UPDATE CASCADE ON DELETE CASCADE,
	Rating					INT				NOT NULL,
	Additional				VARCHAR(30)		NULL,

	PRIMARY KEY(ArtworkID, UserID)
)
GO


CREATE PROCEDURE signUp (

	@firstName			VARCHAR(15),
	@lastName			VARCHAR(15),
	@dob				DATE,
	@address			VARCHAR(50),
	@accountType		VARCHAR(6),
	@gender				CHAR(1),
	@password			NVARCHAR(64),
	@email				VARCHAR(30)
)
AS 
BEGIN

	SELECT @password
	DECLARE @hash CHAR(128) = HASHBYTES('SHA2_512', @password)
	SELECT @hash
	
	INSERT INTO [User]
	VALUES (
		@firstName,
		@lastName,
		@DOB,
		@address,
		@accountType,
		@gender,
		@hash,
		@email
	)
END
GO

CREATE PROCEDURE logOn (

	@email				VARCHAR(MAX),
	@password			NVARCHAR(64),
	@returnValue		INT OUTPUT
)
AS 
BEGIN
	
	SET @returnValue = 2	--wrong password
	
	IF NOT EXISTS(SELECT * FROM [User] WHERE [User].Email = @email)
	BEGIN
		SET @returnValue = 1 --user does not exist
	END

	ELSE IF EXISTS(SELECT * FROM [User] WHERE [User].Email = @email AND [User].[Password] = HASHBYTES('SHA2_512', @password))
	BEGIN
		SET @returnValue =  0 --login
	END
END
GO

CREATE PROCEDURE addArtwork (
	@creator			VARCHAR(15),
	@name				VARCHAR(15),
	@description		VARCHAR(250),
	@medium				VARCHAR(25),
	@origin				VARCHAR(20),
	@date				DATE,
	@price				INT,
	@link				VARCHAR(MAX)
)
AS 
BEGIN
	INSERT INTO Artwork
	VALUES (
		@creator,
		@name,
		@description,
		@medium,
		@origin,
		@date,
		@price,
		@link)
END
GO

CREATE PROCEDURE rateArt (
	@userID			INT,
	@artID			INT, 
	@rating			INT,
	@additional		VARCHAR(30),
	@out			INT			OUTPUT
)
AS 
BEGIN
	SET @out = -1	--rating already exists
	IF (NOT EXISTS (SELECT * FROM ArtworkRating WHERE userID = @userID AND ArtworkID = @artID))
	BEGIN
		INSERT INTO artworkRating
		VALUES (
				@artID,
				@userID,
				@rating,
				@additional)
		SET @out = 0
	END
END
GO

CREATE FUNCTION get_sim(
	@origin VARCHAR(100), 
	@id INT
)
RETURNS TABLE
AS
	RETURN (SELECT *
			FROM Artwork
			WHERE Origin = @origin AND ArtworkID != @id)
GO
								

CREATE FUNCTION searchArtOrigin(
	@identifier VARCHAR(500)
)
RETURNS TABLE 
AS
	RETURN (SELECT * 
			FROM Artwork
			WHERE Artwork.Origin = @identifier)
GO


CREATE FUNCTION searchArt(
	@identifier VARCHAR(500)
)
RETURNS TABLE 
AS
	RETURN (SELECT * 
			FROM Artwork
			WHERE Artwork.Name = @identifier)
GO

CREATE PROCEDURE send_mail(
	@user_id INT,
	@sub_type VARCHAR(30)
)
AS
BEGIN
	-- show advanced options
	EXEC sp_configure 'show advanced options', 1
	RECONFIGURE
	
	-- enable Database Mail XPs
	EXEC sp_configure 'Database Mail XPs', 1
	RECONFIGURE
	 
	-- check if it has been changed
	EXEC sp_configure 'Database Mail XPs'
	
	-- hide advanced options
	EXEC sp_configure 'show advanced options', 0
	RECONFIGURE
	
	DECLARE @email VARCHAR(30), @user_Name VARCHAR(15)
	SELECT @email = Email, @user_Name = [User].FirstName
	FROM [User]
	WHERE [User].UserID = @user_id

	DECLARE @body VARCHAR(100)

	SET @body = 'Hi' + @user_Name + ',\n\tThank you for subscribing to our service! Your subscription Plan is ' + @sub_type
	            + '.\nHave a nice day!'
	EXEC msdb.dbo.sp_send_dbmail
						@profile_name = 'gallerie d_art',
                        @recipients = @email,
						@subject = 'Purchase confirmation',
                        @body = @body,
                        @body_format = 'TEXT'
END
GO

CREATE PROCEDURE likeArt (
	@userID		INT,
	@artID		INT,
	@out		INT		OUTPUT
)
AS
BEGIN
	SET @out = -1	--already liked
	IF (NOT EXISTS (SELECT * FROM UserLikes WHERE userID = @userID AND artID = @artID))
	BEGIN
		INSERT INTO UserLikes
		VALUES (@userID, @artID)
		
		SET @out = 0
	END
END
GO

CREATE PROCEDURE removeArt (
	@artID		INT,
	@out		INT			OUTPUT
)
AS
BEGIN
	SET @out = -1	--already liked
	IF (EXISTS (SELECT * FROM Artwork WHERE artworkID = @artID))
	BEGIN
		DELETE Artwork
		WHERE ArtworkID = @artID
		
		SET @out = 0
	END
END
GO

CREATE PROCEDURE sellArt (
	@userID			INT,
	@name			VARCHAR(15),
	@creator		VARCHAR(15),
	@price			INT,
	@date			DATE,
	@origin			VARCHAR(20),
	@medium			VARCHAR(MAX),
	@description	VARCHAR(MAX),
	@link			VARCHAR(MAX)
)
AS
BEGIN
	EXEC addArtwork @name, @creator, @price, @date, @origin, @medium, @description, @link

	DECLARE @artID	INT
	SELECT TOP 1 @artID = artworkID
	FROM Artwork
	ORDER BY ArtworkID DESC

	INSERT INTO UserArtworkSale
	VALUES (@userID, @artID, CAST(GETDATE() AS DATE))
END
GO

CREATE PROCEDURE purchaseArt (
	@userID			INT,
	@sub_type		VARCHAR(30),
	@artID			INT
)
AS
BEGIN
	IF(NOT EXISTS (SELECT * FROM UserArtworkPurchase WHERE ArtworkID = @artID AND userID = @userID))
	BEGIN
		INSERT INTO UserArtworkPurchase
		VALUES (@userID, @artID, CAST(GETDATE() AS DATE))
	END
END
GO

CREATE FUNCTION get_liked (
	@userID		INT
)
RETURNS TABLE
AS
	RETURN ( SELECT B.ArtworkID, B.Creator, B.Date, B.Description, B.Medium, B.Name, B.Origin, B.pictureLink, B.Price
			 FROM UserLikes AS A JOIN Artwork AS B ON A.artID = B.ArtworkID
			 WHERE A.userID = @userID)
GO

CREATE FUNCTION get_sale (
	@userID		INT
)
RETURNS TABLE
AS
	RETURN ( SELECT B.ArtworkID, B.Creator, B.Date, B.Description, B.Medium, B.Name, B.Origin, B.pictureLink, B.Price
			 FROM UserArtworkSale AS A JOIN Artwork AS B ON A.artworkID = B.ArtworkID
			 WHERE A.userID = @userID)
GO

CREATE FUNCTION get_pur (
	@userID		INT
)
RETURNS TABLE
AS
	RETURN ( SELECT B.ArtworkID, B.Creator, B.Date, B.Description, B.Medium, B.Name, B.Origin, B.pictureLink, B.Price
			 FROM UserArtworkPurchase AS A JOIN Artwork AS B ON A.artworkID = B.ArtworkID
			 WHERE A.userID = @userID)
GO

CREATE TABLE #T(ArtworkID				INT,
				Creator					VARCHAR(200),
				[Name]					VARCHAR(300),
				[Description]			VARCHAR(MAX),
				Medium					VARCHAR(MAX),
				Origin					VARCHAR(100),
				[Date]					DATE,
				Price					INT,
				pictureLink				VARCHAR(MAX))

BULK INSERT #T
FROM 'C:\Users\ather\Desktop\Test\Gallerie-d-Art\data.txt'
WITH (
	ROWTERMINATOR = '\n',
	FIELDTERMINATOR = '=!=!='
)
GO

INSERT INTO Artwork(Creator, [Name], [Description], Medium, Origin, [Date], Price, pictureLink)
SELECT Creator, [Name], [Description], Medium, Origin, [Date], Price, pictureLink FROM #T


-----------
USE master
GO

DROP TABLE #T
GO
