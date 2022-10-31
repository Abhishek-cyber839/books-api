#!/usr/bin/env bash
PG_SUPER_USER_USERNAME="postgres"
PG_DATABASE_NAME="dotndockdb"
PG_ROLE="dbowner"
PG_TABLE_NAME="blogs"
PG_GRANT_ACCESS="GRANT ALL ON ALL TABLES IN SCHEMA public TO $PG_ROLE;"
PG_SEQUENCE_GRANT="GRANT ALL ON ALL SEQUENCES IN SCHEMA public TO $PG_ROLE;"
EXIT_PSQL="\q"
TOTAL_DATA="SELECT count(*) as total_rows FROM $PG_TABLE_NAME;"
 
psql << EOF
DROP DATABASE IF EXISTS dotndockdb;
CREATE DATABASE dotndockdb;

CREATE USER abhishek WITH PASSWORD 'strong_password';
CREATE USER abhishek_1 WITH PASSWORD 'strong_password_1';

CREATE ROLE dbowner; 

GRANT dbowner TO abhishek, abhishek_1;

\connect dotndockdb;
Create table blogs(
    blogId serial PRIMARY KEY,
    authorName varchar(40) CONSTRAINT autHor_should_not_be_empty check (length(authorName) > 0) NOT null DEFAULT '',
    blogCategory int not null,
    description text null,
    title text NOT null DEFAULT '' CONSTRAINT must_be_different_text UNIQUE ,CONSTRAINT title_should_not_be_empty check (length(title) > 0),
    numberOfViewers bigint CONSTRAINT positive_numberOfViewers check (numberOfViewers > 0)
);

insert into blogs(authorName, blogCategory, description, title, numberOfViewers) values ('abhishek-1',   1, 'a random description-1', 'title 1', 1000);
insert into blogs(authorName, blogCategory, description, title, numberOfViewers) values ('abhishek-2', 1, 'a random description-2', 'title 2', 1001); 
insert into blogs(authorName, blogCategory, description, title, numberOfViewers) values ('abhishek-3', 1, 'a random description-3', 'title 3', 1002); 
insert into blogs(authorName, blogCategory, description, title, numberOfViewers) values ('abhishek-4', 1, 'a random description-4', 'title 4', 1003);
insert into blogs(authorName, blogCategory, description, title, numberOfViewers) values ('abhishek-5', 1, 'a random description-5', 'title 5', 1004);
\q;
EOF
echo $TOTAL_DATA$PG_GRANT_ACCESS$PG_SEQUENCE_GRANT$EXIT_PSQL | psql -U $PG_SUPER_USER_USERNAME -d $PG_DATABASE_NAME 