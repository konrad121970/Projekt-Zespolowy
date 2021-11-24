USE PZ_DB;


CREATE TABLE Account (
    ID                  INT                 NOT NULL        PRIMARY KEY IDENTITY,

    Username            VARCHAR(64)         NOT NULL,
    Password            VARCHAR(1024)       NOT NULL
);

CREATE TABLE Conversation (
	ID					INT					NOT NULL        PRIMARY KEY IDENTITY,		
);

CREATE TABLE Message (
	ID                  INT                 NOT NULL        PRIMARY KEY IDENTITY,
	
    Account_ID          INT                 NOT NULL        FOREIGN KEY REFERENCES Account(ID),
	Conversation_ID     INT                 NOT NULL        FOREIGN KEY REFERENCES Conversation(ID),
    Content             VARCHAR(1024)       NOT NULL,
    SendDate            DATE                NOT NULL,
);

CREATE TABLE Conversation_Account (
    Account_ID          INT                 NOT NULL        FOREIGN KEY REFERENCES Account(ID),
	Conversation_ID     INT                 NOT NULL        FOREIGN KEY REFERENCES Conversation(ID)

    PRIMARY KEY (Account_ID, Conversation_ID)
);