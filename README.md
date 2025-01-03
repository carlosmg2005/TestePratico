Query executada no Azure Studio

CREATE TABLE Pessoas (
    Id INT PRIMARY KEY IDENTITY,
    NomeFantasia NVARCHAR(100) NOT NULL,
    CnpjCpf NVARCHAR(14) NOT NULL
);
