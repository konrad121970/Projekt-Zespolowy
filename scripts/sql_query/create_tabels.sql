USE Drocsid_DB;

CREATE TABLE Account (
    ID                  INT                 NOT NULL        PRIMARY KEY IDENTITY,
    Username            VARCHAR(64)         NOT NULL,

    Email               VARCHAR(128)        NOT NULL,
    Password            VARCHAR(1024)       NOT NULL,
);

CREATE TABLE Friendship (
	ID 					INT					NOT NULL		PRIMARY KEY IDENTITY,

    UserID              INT                 NOT NULL        FOREIGN KEY REFERENCES Account(ID),
    FriendID            INT                 NOT NULL        FOREIGN KEY REFERENCES Account(ID),
);

CREATE TABLE PrivateMessage (
    ID                  INT                 NOT NULL        PRIMARY KEY IDENTITY,
    Content             VARCHAR(5000)       NOT NULL,

    SenderID            INT                 NOT NULL        FOREIGN KEY REFERENCES Account(ID),
    ReceiverID          INT                 NOT NULL        FOREIGN KEY REFERENCES Account(ID),
);