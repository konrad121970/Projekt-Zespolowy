USE PZ_DB;


CREATE TABLE Account (
    ID                  INT                 NOT NULL        PRIMARY KEY IDENTITY,

    Username            VARCHAR(64)         NOT NULL,
    Password            VARCHAR(1024)       NOT NULL,
);

CREATE TABLE Conversation (
	ID					INT					NOT NULL        PRIMARY KEY IDENTITY,
	Name                VARCHAR(64)         NOT NULL,
);

CREATE TABLE Message (
	ID                  INT                 NOT NULL        PRIMARY KEY IDENTITY,
	
	Sender_ID           INT                 NOT NULL        FOREIGN KEY REFERENCES Account(ID),
	Conversation_ID     INT                 NOT NULL        FOREIGN KEY REFERENCES Conversation(ID),

    Content             VARCHAR(1024)       NOT NULL,
    SendDate            DATE                NOT NULL,
);

CREATE TABLE Account_Conversation (
    Member_ID           INT                 NOT NULL        FOREIGN KEY REFERENCES Account(ID),
	Conversation_ID     INT                 NOT NULL        FOREIGN KEY REFERENCES Conversation(ID),

    PRIMARY KEY (Member_ID, Conversation_ID),
);

CREATE TABLE PendingFriendInvitation (
	ID                  INT                 NOT NULL        PRIMARY KEY IDENTITY,
	
	Sender_ID           INT                 NOT NULL        FOREIGN KEY REFERENCES Account(ID),
	Receiver_ID         INT                 NOT NULL        FOREIGN KEY REFERENCES Account(ID),
);