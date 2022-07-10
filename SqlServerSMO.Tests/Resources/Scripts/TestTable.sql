create table TestTable
(
    Id   int          not null
        constraint id
            primary key,
    Name nvarchar(50) not null
)
go

INSERT INTO TestTable (Id, Name) VALUES (1, N'Jane Doe');
INSERT INTO TestTable (Id, Name) VALUES (2, N'John Doe');
