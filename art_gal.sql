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
	ArtworkID				INT				PRIMARY KEY,
	Creator					VARCHAR(200),
	[Name]					VARCHAR(300),
	[Description]			VARCHAR(MAX),
	Medium					VARCHAR(MAX),
	Origin					VARCHAR(100),
	[Date]					DATE,
	Price					INT,
	pictureLink				VARCHAR(MAX)
)

CREATE TABLE ArtworkPictures (
	PictureID				INT				PRIMARY KEY,
	ArtworkID				INT				FOREIGN KEY REFERENCES Artwork(ArtworkID),
	pictureLink				VARCHAR(50)
)
  
 
CREATE TABLE UserArtworkPurchase (
	PurchaseID				INT				PRIMARY KEY,
	UserID					INT				FOREIGN KEY REFERENCES [User](UserID),
	ArtworkID				INT				FOREIGN KEY REFERENCES Artwork(ArtworkID),
	DataSubmitted			DATE
)
 
CREATE TABLE UserArtworkSale (
	SaleID					INT				PRIMARY KEY,
	UserID					INT				FOREIGN KEY REFERENCES [User](UserID),
	ArtworkID				INT				FOREIGN KEY REFERENCES Artwork(ArtworkID),
	DataSubmitted			DATE
)

CREATE TABLE Catagory (
	CatagoryID				INT				PRIMARY KEY,
	[Name]					VARCHAR(20),
	Popularity				INT
)


CREATE TABLE Specifics (
	ContentID				INT				PRIMARY KEY,
	[Name]					VARCHAR(20),
	CatagoryID				INT				FOREIGN KEY REFERENCES Catagory(CatagoryID),
)

CREATE TABLE UserSpecifics (
	UserID					INT,
	SpecificsID				INT,
	ContentID				INT				FOREIGN KEY REFERENCES specifics(contentID),
	
	PRIMARY KEY(UserID, SpecificsID)
)

CREATE TABLE ArtworkRating (
	ArtworkID				INT				FOREIGN KEY REFERENCES Artwork(ArtworkID) ,
	UserID					INT				FOREIGN KEY REFERENCES [User](UserID),
	Rating					INT				NOT NULL,

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

CREATE PROCEDURE searchArtwork (
	@ArtworkID			INT,
	@Name				VARCHAR(15)		OUTPUT,
	@Creator			VARCHAR(15)		OUTPUT,
	@Price				INT				OUTPUT,
	@Date				DATE			OUTPUT,
	@Origin				VARCHAR(20)		OUTPUT,
	@Medium				VARCHAR(25)		OUTPUT,
	@Description		VARCHAR(250)	OUTPUT
)
AS
BEGIN

	SELECT @name = [Name], @Creator = Creator, 
		   @Price = Price, @Date = [Date], 
		   @Origin = Origin, @Medium = Medium, 
		   @Description = [Description]
	FROM Artwork 
	WHERE ArtworkID = @artworkID
END
GO

CREATE FUNCTION getPictures (
	@artworkID			INT
)
RETURNS TABLE
AS
	RETURN (SELECT * 
		    FROM ArtworkPictures
			WHERE ArtworkID = @artworkID)
GO

CREATE PROCEDURE addArtwork (
	@artworkID			INT,
	@name				VARCHAR(15),
	@creator			VARCHAR(15),
	@price				INT	,
	@date				DATE,
	@origin				VARCHAR(20),
	@medium				VARCHAR(25),
	@description		VARCHAR(250),
	@link				VARCHAR(MAX)
)
AS 
BEGIN
	INSERT INTO Artwork
	VALUES (
		@artworkID,
		@name,
		@creator,
		@price,
		@date,
		@origin,
		@medium,
		@description,
		@link)
END
GO

CREATE PROCEDURE addArtworkPictures (
	@pictureID		INT,
	@artworkID		INT,
	@pictureLink	VARCHAR(50)
)
AS 
BEGIN
	INSERT INTO ArtworkPictures
	VALUES (
		@pictureID,
		@artworkID,
		@pictureLink)
END
GO

CREATE PROCEDURE rateArtwork (
	@userID			INT,
	@artworkID		INT, 
	@rating			INT
)
AS 
BEGIN
	INSERT INTO artworkRating
	VALUES (
			@artworkID,
			@userID,
			@rating)	
END
GO

CREATE FUNCTION searchArtDescription (
	@searchTerm		VARCHAR(50)
)
RETURNS TABLE
AS 
	RETURN (SELECT * 
			FROM Artwork 
			WHERE Artwork.[Description] LIKE CONCAT('%', @searchTerm, '%'))
GO

CREATE PROCEDURE artworkMedium (
	@userID			INT,
	@artOut			INT					OUTPUT
)
AS
BEGIN

	SELECT @artOut = artworkID
	FROM (SELECT Medium 
	      FROM ArtworkRating AS R INNER JOIN Artwork AS A ON R.artworkID = A.artworkID 
		  WHERE rating >= 4 AND userID = @userID) AS Foo INNER JOIN Artwork AS Ar ON Foo.Medium = Ar.Medium
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


-- show advanced options
EXEC sp_configure 'show advanced options', 1
GO
RECONFIGURE
GO
 
-- enable Database Mail XPs
EXEC sp_configure 'Database Mail XPs', 1
GO
RECONFIGURE
GO
 
-- check if it has been changed
EXEC sp_configure 'Database Mail XPs'
GO
 
-- hide advanced options
EXEC sp_configure 'show advanced options', 0
GO
RECONFIGURE
GO

ALTER PROCEDURE send_mail(
	@user_id INT,
	@sub_type VARCHAR(30)
)
AS
BEGIN
	SET NOCOUNT OFF
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

BULK INSERT Artwork
FROM 'D:\Other\FAST\DB\Gallerie-d-Art\data.txt'
WITH (
	ROWTERMINATOR = '\n',
	FIELDTERMINATOR = '=!=!='
)