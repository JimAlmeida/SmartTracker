SELECT * from UserInfo;

ALTER TABLE UserInfo 
    ADD
        "Username" NVARCHAR(100),
        "DateOfBirth" DATE,
        "Address" NVARCHAR(300),
        "City" NVARCHAR(100),
        "State/Province" NVARCHAR(100),
        "Country" NVARCHAR(100),
        "AboutMe" NVARCHAR(400) DEFAULT 'Insert here your biography or personal statement.',
        "JobRole" NVARCHAR(100),
        "JobDescription" NVARCHAR(300),
        "ImageURL" NVARCHAR(1500),
        "ImageData" VARBINARY(MAX);

SELECT * from UserInfo;